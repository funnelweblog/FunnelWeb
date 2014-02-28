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

        public IQueryable<Setting> GetSettings()
        {
            return session.Query<Setting>();
        }

        public void UpdateCommentCountFor(int entryId)
        {
            var commentCount = session
                .QueryOver<Comment>()
                .Where(c => c.Entry.Id == entryId && c.Status == 1)
                .ToRowCountQuery()
                .SingleOrDefault<int>();

            var entry = session.Get<Entry>(entryId);
            entry.CommentCount = commentCount;
            session.Flush();
        }

        public void Save(IEnumerable<Setting> settings)
        {
            using (var transaction = session.BeginTransaction())
            {
                foreach (var setting in settings)
                {
                    session.SaveOrUpdate(setting);
                }
                transaction.Commit();
                session.Flush();
            }
        }
    }
}
