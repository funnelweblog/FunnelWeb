using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Model.Repositories.Internal
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ISession session;

        public AdminRepository(ISession session)
        {
            this.session = session;
        }

        public IQueryable<Comment> GetComments(int skip, int take)
        {
            return session.Linq<Comment>()
                .OrderByDescending(x => x.Posted)
                .Skip(skip)
                .Take(take);
        }

        public IQueryable<Comment> GetSpam()
        {
            return session.Linq<Comment>()
                .Where(x => x.Status == 0);
        }

        public Comment GetComment(int commentId)
        {
            return session.Linq<Comment>()
                .Where(x => x.Id == commentId)
                .FirstOrDefault();
        }

        public Pingback GetPingback(int pingback)
        {
            return session.Linq<Pingback>()
                .Where(x => x.Id == pingback)
                .FirstOrDefault();
        }

        public IQueryable<Setting> GetSettings()
        {
            return session.Linq<Setting>()
                .Take(1000);
        }

        public IQueryable<Redirect> GetRedirects()
        {
            return session.Linq<Redirect>()
                .Take(1000);
        }

        public IEnumerable<Pingback> GetPingbacks()
        {
            return session.Linq<Pingback>()
                .OrderByDescending(x => x.Id)
                .Take(50);
        }

        public void Save(IEnumerable<Setting> settings)
        {
            foreach (var setting in settings)
            {
                session.SaveOrUpdate(setting);
            }
        }

        public void Save(Redirect redirect)
        {
            session.SaveOrUpdate(redirect);
        }

        public void Delete(Redirect redirect)
        {
            session.Delete(redirect);
        }

        public void Delete(Pingback pingback)
        {
            session.Delete(pingback);
        }

        public void Update(Comment comment)
        {
            session.Update(comment);
        }

        public void Delete(Comment comment)
        {
            session.Delete(comment);
        }

        public void Save(Comment comment)
        {
            session.Update(comment);
        }

        public void Save(Pingback pingback)
        {
            session.Update(pingback);
        }
    }
}
