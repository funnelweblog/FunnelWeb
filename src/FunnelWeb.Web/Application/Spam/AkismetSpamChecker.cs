using System;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Settings;
using Joel.Net;

namespace FunnelWeb.Web.Application.Spam
{
    public class AkismetSpamChecker : ISpamChecker
    {
        private readonly ISettingsProvider settingsProvider;

        public AkismetSpamChecker(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        private Akismet Connect()
        {
            var settings = settingsProvider.GetSettings<FunnelWebSettings>();
            var akismet = new Akismet(
                settings.AkismetApiKey,
                "http://www.funnelweblog.com",
                "FunnelWeb/1.0"
                );
            akismet.VerifyKey();
            return akismet;
        }

        public void Verify(Comment comment)
        {
            var settings = settingsProvider.GetSettings<FunnelWebSettings>();
            var akismet = Connect();
            var akismetComment = new AkismetComment();
            akismetComment.Blog = "http://www.funnelweblog.com";
            akismetComment.CommentAuthor = comment.AuthorName;
            akismetComment.CommentAuthorEmail = comment.AuthorEmail;
            akismetComment.CommentAuthorUrl = comment.AuthorUrl;
            akismetComment.CommentContent = comment.Body;
            akismetComment.CommentType = "comment";

            comment.IsSpam = akismet.CommentCheck(akismetComment);

            if (comment.IsSpam) 
                return;

            var naughtyWords = settings.SpamWords.Split('\n').Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            comment.IsSpam = naughtyWords.Length > 0 && naughtyWords.Any(x => comment.Body.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }

        public void Verify(Pingback pingback)
        {
            var akismet = Connect();
            var akismetComment = new AkismetComment();
            akismetComment.Blog = "http://www.funnelweblog.com";
            akismetComment.CommentAuthorUrl = pingback.TargetUri;
            akismetComment.CommentContent = pingback.TargetUri;
            akismetComment.CommentType = "pingback";

            pingback.IsSpam = akismet.CommentCheck(akismetComment);
        }
    }
}