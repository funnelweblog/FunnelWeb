using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.DbProviders;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public class NHibernateRepository : IRepository
    {
        private readonly ISession session;
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly IIndex<string, IDatabaseProvider> databaseProviderLookup;

        public NHibernateRepository(
            ISession session, 
            IConnectionStringProvider connectionStringProvider,
            IIndex<string, IDatabaseProvider> databaseProviderLookup)
        {
            this.session = session;
            this.connectionStringProvider = connectionStringProvider;
            this.databaseProviderLookup = databaseProviderLookup;
        }

        public TEntity Get<TEntity>(object id)
        {
            return session.Get<TEntity>(id);
        }

        public IEnumerable<TEntity> FindAll<TEntity>() where TEntity : class
        {
            return session
                .QueryOver<TEntity>()
                .List<TEntity>();
        }

        public IEnumerable<TEntity> Find<TEntity>(IQuery<TEntity> query) where TEntity : class
        {
            return query.Execute(session, databaseProviderLookup[connectionStringProvider.DatabaseProvider]);
        }

        public PagedResult<TEntity> Find<TEntity>(IPagedQuery<TEntity> query, int pageNumber, int itemsPerPage) where TEntity : class
        {
            var skip = pageNumber*itemsPerPage;
            return query.Execute(session, 
                databaseProviderLookup[connectionStringProvider.DatabaseProvider], 
                skip, itemsPerPage);
        }

        public TEntity FindFirst<TEntity>(IQuery<TEntity> query) where TEntity : class
        {
            return One(Find(query), true);
        }

        public TEntity FindFirstOrDefault<TEntity>(IQuery<TEntity> query) where TEntity : class
        {
            return One(Find(query), false);
        }

        public TEntity FindFirstOrDefault<TEntity>(IPagedQuery<TEntity> query) where TEntity : class
        {
            return One(Find(query, 0, 1), false);
        }

        public void Execute(ICommand command)
        {
            command.Execute(session);
        }

        public void Add(object entity)
        {
            session.Persist(entity);
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