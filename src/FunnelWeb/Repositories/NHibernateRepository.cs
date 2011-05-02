using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public class NHibernateRepository : IRepository
    {
        private readonly ISession session;

        public NHibernateRepository(ISession session)
        {
            this.session = session;
        }

        public TEntity Get<TEntity>(object id)
        {
            return session.Load<TEntity>(id);
        }

        public IEnumerable<TEntity> FindAll<TEntity>() where TEntity : class
        {
            return session
                .QueryOver<TEntity>()
                .List<TEntity>();
        }

        public IEnumerable<TEntity> Find<TEntity>(IQuery<TEntity> query) where TEntity : class
        {
            return query.Execute(session);
        }

        public IEnumerable<TEntity> Find<TEntity>(IQueryWithCount<TEntity> query, int skip, int take, out int totalCount) where TEntity : class
        {
            return query.Execute(session, skip, take, out totalCount);
        }

        public TEntity FindFirst<TEntity>(IQuery<TEntity> query) where TEntity : class
        {
            return One(Find(query), true);
        }

        public TEntity FindFirstOrDefault<TEntity>(IQuery<TEntity> query) where TEntity : class
        {
            return One(Find(query), false);
        }

        public void Execute(ICommand command)
        {
            command.Execute(session);
        }

        public void Add(object entity)
        {
            session.Save(entity);
        }

        public void Remove(object entity)
        {
            session.Delete(entity);
        }

        private T One<T>(IEnumerable<T> items, bool throwIfNone)
        {
            var itemsList = items.ToList();
            if (throwIfNone && itemsList.Count == 0)
            {
                throw new Exception(string.Format("Expected at least one '{0}' in the query results", typeof(T).Name));
            }
            return itemsList.Count > 0
                       ? itemsList[0]
                       : default(T);
        }
    }
}