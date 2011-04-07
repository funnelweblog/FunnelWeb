using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using BlogML;
using BlogML.Xml;
using FunnelWeb.Authentication;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;

namespace FunnelWeb.Tasks
{
    public class BlogMLImportTask : ITask
    {
        private readonly IEntryRepository entryRepository;
        private readonly ITagRepository tagRepository;
        private readonly IAuthenticator authenticator;

        public BlogMLImportTask(IEntryRepository entryRepository, IAuthenticator authenticator, ITagRepository tagRepository)
        {
            this.entryRepository = entryRepository;
            this.authenticator = authenticator;
            this.tagRepository = tagRepository;
        }

        public IEnumerable<TaskStep> Execute(Dictionary<string, object> properties)
        {
            var inputFile = (string)properties["inputFile"];
            if (!File.Exists(inputFile))
            {
                throw new ArgumentException("The file: '{0}' does not exist.");
            }
            
            var tags = tagRepository.GetTags().ToList();

            var progress = 0;
            yield return new TaskStep(progress++, "Input file '{0}' found", inputFile);

            using (var reader = new StreamReader(inputFile))
            {
                yield return new TaskStep(progress++, "Deserializing input file into BlogML object model");

                var blog = BlogMLSerializer.Deserialize(reader);

                yield return new TaskStep(progress++, "Successfully deserialized BlogML object model from input file");
                yield return new TaskStep(progress++, "Blog posts found: {0}", blog.Posts.Count);

                var remainingProgress = 100 - progress;
                var postCount = blog.Posts.Count;
                var postIndex = 0;

                foreach (var post in blog.Posts)
                {
                    postIndex++;
                    progress = (int)(((double)postIndex / (double)postCount) * (double)remainingProgress);
                        
                    var entry = new Entry();
                    entry.Author = authenticator.GetName();
                    entry.HideChrome = false;
                    entry.IsDiscussionEnabled = true;
                    entry.Status = post.PostType == BlogPostTypes.Article ? EntryStatus.PublicPage : EntryStatus.PublicBlog;
                    entry.Title = entry.MetaTitle = NoLongerThan(200, post.Title);
                    entry.Published = post.DateCreated < DateTime.Today.AddYears(-100) ? DateTime.UtcNow : post.DateCreated;
                    entry.Summary = post.HasExcerpt ? NoLongerThan(500, StripHtml(post.Excerpt.UncodedText)) : "";
                    entry.MetaDescription = post.HasExcerpt ? NoLongerThan(200, StripHtml(post.Excerpt.UncodedText)) : NoLongerThan(200, StripHtml(post.Content.UncodedText));
                    entry.Name = NoLongerThan(100, (post.PostUrl ?? post.PostName).Trim('/'));

                    // Ensure this post wasn't already imported
                    var existing = entryRepository.GetEntry(entry.Name);
                    if (existing != null)
                    {
                        yield return new TaskStep(progress, "Did NOT import post '{0}', because a post by this name already exists", entry.Name);

                        continue;
                    }

                    var revision = entry.Revise();
                    revision.Author = authenticator.GetName();
                    revision.Body = post.Content.UncodedText;
                    revision.Format = Formats.Html;
                    revision.Reason = "Imported from BlogML";

                    foreach (BlogMLComment comment in post.Comments)
                    {
                        var newComment = entry.Comment();
                        newComment.AuthorEmail = NoLongerThan(100, comment.UserEMail);
                        newComment.AuthorName = NoLongerThan(100, comment.UserName);
                        newComment.AuthorUrl = NoLongerThan(100, comment.UserUrl);
                        newComment.IsSpam = !comment.Approved;
                        newComment.Posted = comment.DateCreated < DateTime.Today.AddYears(-100) ? DateTime.UtcNow : comment.DateCreated;
                        newComment.Body = comment.Content.UncodedText;
                    }

                    foreach (BlogMLCategoryReference categoryRef in post.Categories)
                    {
                        var category = blog.Categories.FirstOrDefault(x => x.ID == categoryRef.Ref);
                        if (category == null)
                            continue;
                        
                        var tagName = new string(
                            (category.Title?? string.Empty)
                            .ToLowerInvariant()
                            .Select(x => char.IsLetterOrDigit(x) ? x : '-')
                            .ToArray());

                        if (string.IsNullOrEmpty(tagName))
                            continue;

                        var existingTag = tagRepository.GetTag(tagName);
                        if (existingTag == null)
                        {
                            existingTag = new Tag() {Name = tagName};
                            tagRepository.Save(existingTag);
                        }

                        existingTag.Add(entry);
                    }

                    entryRepository.Save(entry);

                    yield return new TaskStep(progress, "Imported post '{0}'", entry.Name);
                }
            }
            yield break;
        }

        private static string StripHtml(string html)
        {
            html = Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
            html = html.Replace(@"&nbsp;", " ");
            html = html.Replace(@"&quot;", "\"");
            html = html.Replace(@"&amp;", "&");
            html = html.Replace(@"&lt;", "<");
            html = html.Replace(@"&gt;", ">");
            return html;
        }

        private static string NoLongerThan(int max, string text)
        {
            text = text ?? string.Empty;
            if (text.Length > max)
            {
                text = text.Substring(0, max - 4);
                text += "...";
            }
            return text;
        }
    }
}