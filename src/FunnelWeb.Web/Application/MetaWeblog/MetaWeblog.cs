﻿using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using CookComputing.XmlRpc;
using FunnelWeb.Authentication;
using FunnelWeb.Model;
﻿using FunnelWeb.Providers.File;
﻿using FunnelWeb.Repositories;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Settings;
using FunnelWeb.Utilities;
using NHibernate;

namespace FunnelWeb.Web.Application.MetaWeblog
{
    public class MetaWeblog : XmlRpcService, IMetaWeblog
    {
        private readonly FunnelWebSettings funnelWebSettings;
        private readonly ISettingsProvider settingsProvider;
        private readonly IRepository repository;
        private readonly ISession session;
        private readonly IFileRepository fileRepository;
        private readonly IAuthenticator authenticator;

        public MetaWeblog(
            ISettingsProvider settingsProvider,
            IRepository repository,
            ISession session,
            IFileRepository fileRepository,
            IAuthenticator authenticator)
        {
            this.settingsProvider = settingsProvider;
            this.repository = repository;
            this.session = session;
            this.fileRepository = fileRepository;
            this.authenticator = authenticator;
            funnelWebSettings = this.settingsProvider.GetSettings<FunnelWebSettings>();
        }

        public string AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            var entry = CreateUpdatePost(username, password, "-1", post, publish);
            if (entry != null)
                return entry.Id.ToString();
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public bool UpdatePost(string postid, string username, string password, Post post, bool publish)
        {
            var entry = CreateUpdatePost(username, password, postid, post, publish);
            if (entry != null)
                return true;
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        private Entry CreateUpdatePost(string username, string password, string postid, Post post, bool publish)
        {
            if (ValidateUser(username, password))
            {
                using (var transaction = session.BeginTransaction(IsolationLevel.Serializable))
                {
                    var isOldPost = true;
                    var author = authenticator.GetName();
                    var entry = repository.Get<Entry>(Int32.Parse(postid));

                    if (entry == null)
                    {
                        entry = new Entry { Author = author };
                        isOldPost = false;
                    }

                    entry.Name = post.permalink;
                    entry.Title = post.title ?? string.Empty;
                    if (string.IsNullOrEmpty(post.mt_excerpt))
                        entry.Summary = entry.Summary;
                    else
                        entry.Summary = post.mt_excerpt;
                    entry.MetaTitle = post.title;
                    entry.Published = (post.dateCreated < DateTime.Today.AddYears(10) ? DateTime.Today : post.dateCreated).ToUniversalTime();
                    entry.Status = publish ? EntryStatus.PublicBlog : EntryStatus.Private;

                    var revision = entry.Revise();
                    revision.Author = author;
                    revision.Body = post.description;
                    revision.Reason = "API";
                    revision.Format = string.Equals(post.format, Formats.Markdown, StringComparison.InvariantCultureIgnoreCase) ? Formats.Markdown : Formats.Html;

                    if (string.IsNullOrWhiteSpace(entry.Name))
                        entry.Name = post.title.Replace(" ", "-");

                    // support for slug
                    if (!string.IsNullOrEmpty(post.wp_slug))
                        entry.Name = post.wp_slug;

                    entry.MetaDescription = entry.MetaDescription ?? post.mt_excerpt;

                    var editTags = post.categories;
                    var toDelete = entry.Tags.Where(t => !editTags.Contains(t.Name)).ToList();
                    var toAdd = editTags.Where(t => !entry.Tags.Any(tag => tag.Name == t)).ToList();

                    foreach (var tag in toDelete)
                        tag.Remove(entry);
                    foreach (var tag in toAdd)
                    {
                        var existingTag = repository.FindFirstOrDefault(new SearchTagsByNameQuery(tag));
                        if (existingTag == null)
                        {
                            existingTag = new Tag { Name = tag };
                            repository.Add(existingTag);
                        }
                        existingTag.Add(entry);
                    }

                    //Does it need to be added?
                    if (!isOldPost)
                    {
                        repository.Add(entry);
                    }

                    session.Flush();
                    transaction.Commit();

                    return entry;
                }
            }
            return null;
        }

        public Post GetPost(string postid, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                var entry = repository.Get<Entry>(Int32.Parse(postid));

                return entry != null ? ConvertToPost(entry) : new Post();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public CategoryInfo[] GetCategories(string blogid, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                //TODO implement a tagged rss feed, and update url

                return repository.FindAll<Tag>()
                    .Select(t => new CategoryInfo
                    {
                        categoryid = t.Id.ToString(),
                        title = t.Name,
                        description = t.Name,
                        htmlUrl =
                            new Uri(HttpContext.Current.Request.GetOriginalUrl(), "/tagged/" + t.Name).ToString(),
                        rssUrl =
                            new Uri(HttpContext.Current.Request.GetOriginalUrl(), "/tagged/" + t.Name).ToString()
                    })
                    .ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            if (ValidateUser(username, password))
            {
                var entries = repository.Find(new GetFullEntriesQuery(), 0, numberOfPosts)
                    .Select(ConvertToPost)
                    .ToArray();

                return entries;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        private Post ConvertToPost(Entry entry)
        {
            return new Post
            {
                dateCreated = entry.LatestRevision.Revised,
                categories = entry.Tags.Select(t => t.Name).ToArray(),
                description = entry.LatestRevision.Body,
                permalink = entry.Name.ToString(),
                postid = entry.Id,
                title = entry.Title,
                userid = "FunnelWeb",
                wp_slug = entry.Name.ToString(),
                mt_excerpt = entry.MetaDescription
            };
        }

        private static Post ConvertToPost(EntryRevision entry)
        {
            return new Post
            {
                dateCreated = entry.Revised,
                categories = entry.TagsCommaSeparated.Split(',').ToArray(),
                description = entry.Body,
                permalink = entry.Name.ToString(),
                postid = entry.Id,
                title = entry.Title,
                userid = "FunnelWeb",
                wp_slug = entry.Name.ToString(),
                mt_excerpt = entry.MetaDescription
            };
        }

        public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            if (ValidateUser(username, password))
            {
                // WLR sends images with the name "imge.png". Very resourcefull
                var objectInfo = new MediaObjectInfo();

                // image
                using (var memoryStream = new MemoryStream(mediaObject.bits))
                {
                    var fileName = Path.GetFileNameWithoutExtension(mediaObject.name) + "_" + DateTime.Now.Ticks + Path.GetExtension(mediaObject.name);

                    fileRepository.Save(memoryStream, fileName, false);
                    objectInfo.url = new Uri(HttpContext.Current.Request.GetOriginalUrl(), "/get/" + fileName.TrimStart('/')).ToString();
                }

                return objectInfo;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            if (ValidateUser(username, password))
            {
                using (var transaction = session.BeginTransaction(IsolationLevel.Serializable))
                {
                    repository.Remove(repository.Get<Entry>(Int32.Parse(postid)));

                    session.Flush();
                    transaction.Commit();
                }
                return true;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public BlogInfo[] GetUsersBlogs(string key, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                // FunnelWeb only has a single blog
                var homepageUri = new Uri(Context.Request.GetOriginalUrl(), "/");

                var blogInfo = new BlogInfo
                {
                    blogName = funnelWebSettings.SiteTitle,
                    url = homepageUri.ToString(),
                    blogid = "FunnelWeb"
                };

                return new[] { blogInfo };
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public UserInfo GetUserInfo(string key, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                var info = new UserInfo();

                // TODO: Implement your own logic to get user info objects and set the info

                return info;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        private bool ValidateUser(string username, string password)
        {
            return authenticator.AuthenticateAndLogin(username, password);
        }
    }
}