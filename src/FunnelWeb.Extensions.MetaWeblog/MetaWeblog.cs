using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using CookComputing.XmlRpc;
using FunnelWeb.Authentication;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Settings;
using NHibernate;

namespace FunnelWeb.Extensions.MetaWeblog
{
    public class MetaWeblog : XmlRpcService, IMetaWeblog
    {
        private readonly FunnelWebSettings funnelWebSettings;
        private readonly ISettingsProvider settingsProvider;
        private readonly IEntryRepository entryRepository;
        private readonly ISession session;
        private readonly IFileRepository fileRepository;
        private readonly IAuthenticator authenticator;
        private readonly ITagRepository tagRepository;

        public MetaWeblog(ISettingsProvider settingsProvider, IEntryRepository entryRepository, 
            ISession session, IFileRepository fileRepository, IAuthenticator authenticator,
            ITagRepository tagRepository)
        {
            this.settingsProvider = settingsProvider;
            this.entryRepository = entryRepository;
            this.session = session;
            this.fileRepository = fileRepository;
            this.authenticator = authenticator;
            this.tagRepository = tagRepository;
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
                    var author = authenticator.GetName();
                    var entry = entryRepository.GetEntry(Int32.Parse(postid)) ?? new Entry { Author = author};
                    entry.Name = post.permalink;
                    entry.Title = post.title ?? string.Empty;
                    entry.Summary = post.mt_excerpt ?? string.Empty;
                    entry.MetaTitle = post.title;
                    entry.Published = (post.dateCreated < DateTime.Today.AddYears(10) ? DateTime.Today : post.dateCreated).ToUniversalTime();
                    entry.Status = publish ? EntryStatus.PublicBlog : EntryStatus.Private;

                    var revision = entry.Revise();
                    revision.Author = author;
                    revision.Body = post.description;
                    revision.Reason = "API";
                    revision.Format = Formats.Html;

                    if (string.IsNullOrWhiteSpace(entry.Name))
                        entry.Name = post.title.Replace(" ", "-");

                    // support for slug
                    if (!string.IsNullOrEmpty(post.wp_slug))
                        entry.Name = post.wp_slug;

                    entry.MetaDescription = entry.Summary;

                    var editTags = post.categories;
                    var toDelete = entry.Tags.Where(t => !editTags.Contains(t.Name)).ToList();
                    var toAdd = editTags.Where(t => !entry.Tags.Any(tag=>tag.Name == t)).ToList();

                    foreach (var tag in toDelete)
                        tag.Remove(entry);
                    foreach (var tag in toAdd)
                    {
                        var existingTag = tagRepository.GetTag(tag);
                        if (existingTag == null)
                        {
                            existingTag = new Tag { Name = tag };
                            tagRepository.Save(existingTag);
                        }
                        existingTag.Add(entry);
                    }

                    entryRepository.Save(entry);

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
                var entry = entryRepository.GetEntry(Int32.Parse(postid));

                return entry != null ? ConvertToPost(entry) : new Post();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public CategoryInfo[] GetCategories(string blogid, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                //TODO implement a tagged rss feed, and update url

                return tagRepository.GetTags()
                    .ToList()
                    .Select(t => new CategoryInfo
                                     {
                                         categoryid = t.Id.ToString(),
                                         title = t.Name,
                                         description = t.Name,
                                         htmlUrl =
                                             new Uri(HttpContext.Current.Request.Url, "/tagged/" + t.Name).ToString(),
                                         rssUrl =
                                             new Uri(HttpContext.Current.Request.Url, "/tagged/" + t.Name).ToString()
                                     })
                    .ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            if (ValidateUser(username, password))
            {
                var entries = entryRepository.GetEntries(0, numberOfPosts)
                    .Item1
                    .Select(p=>ConvertToPost(p.Entry.Value))
                    .ToArray();

                return entries;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        public MediaObjectInfo NewMediaObject(string blogid, string username, string password,
            MediaObject mediaObject)
        {
            if (ValidateUser(username, password))
            {	// WLR sends images with the name "imge.png". Very resourcefull
                var objectInfo = new MediaObjectInfo();

                // image
                using (var memoryStream = new MemoryStream(mediaObject.bits))
                {
                    var fileName = Path.GetFileNameWithoutExtension(mediaObject.name) + "_" + DateTime.Now.Ticks + Path.GetExtension(mediaObject.name);
                    var fullPath = fileRepository.MapPath(fileName);

                    objectInfo.url = VirtualPathUtility.ToAbsolute(funnelWebSettings.UploadPath + "/" + fileName);

                    fileRepository.Save(memoryStream, fullPath, false);
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
                    entryRepository.Delete(Int32.Parse(postid));

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
                var homepageUri = new Uri(Context.Request.Url, "/");

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

        private static Post ConvertToPost(Entry entry)
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
    }
}