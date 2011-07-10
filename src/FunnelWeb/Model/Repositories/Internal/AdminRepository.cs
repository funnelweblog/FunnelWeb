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
            return session.Query<Comment>()
                .OrderByDescending(x => x.Posted)
                .Skip(skip)
                .Take(take);
        }

        public IQueryable<Comment> GetSpam()
        {
            return session.Query<Comment>()
                .Where(x => x.Status == 0);
        }

        public Comment GetComment(int commentId)
        {
            return session.Query<Comment>()
                .Where(x => x.Id == commentId)
                .FirstOrDefault();
        }

        public Pingback GetPingback(int pingback)
        {
            return session.Query<Pingback>()
                .Where(x => x.Id == pingback)
                .FirstOrDefault();
        }

        public IQueryable<Setting> GetSettings()
        {
            return session.Query<Setting>();
        }

        public IQueryable<Redirect> GetRedirects()
        {
            return session.Query<Redirect>()
                .Take(1000);
        }

        public IEnumerable<Pingback> GetPingbacks()
        {
            return session.Query<Pingback>()
                .OrderByDescending(x => x.Id)
                .Take(50);
        }

        public void Save(IEnumerable<Setting> settings)
        {
            foreach (var setting in settings)
            {
                session.SaveOrUpdate(setting);               
            }
            session.Flush();
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
