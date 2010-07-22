using System;
using System.Linq;
using System.Web.Configuration;
using Joel.Net;
using FunnelWeb.Web.Application.Settings;
using FunnelWeb.Web.Model;

namespace FunnelWeb.Web.Application.Spam
{
    public class AkismetSpamChecker : ISpamChecker
    {
        private readonly ISettingsProvider _settingsProvider;

        public AkismetSpamChecker(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        private Akismet Connect()
        {
            var akismet = new Akismet(
                WebConfigurationManager.AppSettings["FunnelWeb.configuration.akismet.apiKey"],
                WebConfigurationManager.AppSettings["FunnelWeb.configuration.akismet.url"],
                WebConfigurationManager.AppSettings["FunnelWeb.configuration.akismet.userAgent"]
                );
            akismet.VerifyKey();
            return akismet;
        }

        public void Verify(Comment comment)
        {
            var akismet = Connect();
            var akismetComment = new AkismetComment();
            akismetComment.Blog = WebConfigurationManager.AppSettings["FunnelWeb.configuration.akismet.url"];
            akismetComment.CommentAuthor = comment.AuthorName;
            akismetComment.CommentAuthorEmail = comment.AuthorEmail;
            akismetComment.CommentAuthorUrl = comment.AuthorUrl;
            akismetComment.CommentContent = comment.Body;
            akismetComment.CommentType = "comment";

            comment.IsSpam = akismet.CommentCheck(akismetComment);

            if (comment.IsSpam) 
                return;

            var naughtyWords = _settingsProvider.SpamWords.Split('\n').Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            comment.IsSpam = naughtyWords.Length > 0 && naughtyWords.Any(x => comment.Body.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }

        public void Verify(Pingback pingback)
        {
            var akismet = Connect();
            var akismetComment = new AkismetComment();
            akismetComment.Blog = WebConfigurationManager.AppSettings["FunnelWeb.configuration.akismet.url"];
            akismetComment.CommentAuthorUrl = pingback.TargetUri;
            akismetComment.CommentContent = pingback.TargetUri;
            akismetComment.CommentType = "pingback";

            pingback.IsSpam = akismet.CommentCheck(akismetComment);
        }


    }
}