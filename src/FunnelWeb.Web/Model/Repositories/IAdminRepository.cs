using System.Linq;
using System.Collections.Generic;

namespace FunnelWeb.Web.Model.Repositories
{
    public interface IAdminRepository
    {
        IQueryable<Comment> GetComments(int skip, int take);
        Comment GetComment(int commentId);
        Pingback GetPingback(int pingback);
        IQueryable<Setting> GetSettings();
        IQueryable<Redirect> GetRedirects();

        void Save(IEnumerable<Setting> settings);
        void Save(Redirect redirect);
        void Delete(Redirect redirect);
        void Update(Comment comment);
        void Delete(Comment comment);
        void Delete(Pingback pingback);
        void Save(Comment comment);
        void Save(Pingback pingback);
        IEnumerable<Pingback> GetPingbacks();
    }
}
