using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Web.Model.Repositories.Internal
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ISession _session;

        public AdminRepository(ISession session)
        {
            _session = session;
        }

        public IQueryable<Comment> GetComments(int skip, int take)
        {
            return _session.Linq<Comment>()
                .OrderByDescending(x => x.Posted)
                .Skip(skip)
                .Take(take);
        }

        public IQueryable<Comment> GetSpam()
        {
            return _session.Linq<Comment>()
                .Where(x => x.Status == 0);
        }

        public Comment GetComment(int commentId)
        {
            return _session.Linq<Comment>()
                .Where(x => x.Id == commentId)
                .FirstOrDefault();
        }

        public Pingback GetPingback(int pingback)
        {
            return _session.Linq<Pingback>()
                .Where(x => x.Id == pingback)
                .FirstOrDefault();
        }

        public IQueryable<Setting> GetSettings()
        {
            return _session.Linq<Setting>()
                .Take(1000);
        }

        public IQueryable<Redirect> GetRedirects()
        {
            return _session.Linq<Redirect>()
                .Take(1000);
        }

        public IEnumerable<Pingback> GetPingbacks()
        {
            return _session.Linq<Pingback>()
                .OrderByDescending(x => x.Id)
                .Take(50);
        }

        public void Save(IEnumerable<Setting> settings)
        {
            foreach (var setting in settings)
            {
                _session.Update(setting);
            }
        }

        public void Save(Redirect redirect)
        {
            _session.SaveOrUpdate(redirect);
        }

        public void Delete(Redirect redirect)
        {
            _session.Delete(redirect);
        }

        public void Delete(Pingback pingback)
        {
            _session.Delete(pingback);
        }

        public void Update(Comment comment)
        {
            _session.Update(comment);
        }

        public void Delete(Comment comment)
        {
            _session.Delete(comment);
        }

        public void Save(Comment comment)
        {
            _session.Update(comment);
        }

        public void Save(Pingback pingback)
        {
            _session.Update(pingback);
        }
    }
}
