using System;
using System.Collections.Generic;
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

        public IList<TEntity> FindAll<TEntity>() where TEntity : class
        {
            return session
                .CreateCriteria<TEntity>()
                .List<TEntity>();
        }

        public IList<TEntity> Find<TEntity>(IQuery<TEntity> query) where TEntity : class
        {
            return query.Execute(session);
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

        private T One<T>(IList<T> items, bool throwIfNone)
        {
            if (throwIfNone && items.Count == 0)
            {
                throw new Exception(string.Format("Expected at least one '{0}' in the query results", typeof(T).Name));
            }
            return items.Count > 0 
                       ? items[0] 
                       : default(T);
        }
    }
}