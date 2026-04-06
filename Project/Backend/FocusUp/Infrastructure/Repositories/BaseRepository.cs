using System;

namespace FocusUp.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T>
    {
        protected DatabaseConnection _dbConnection;
        protected string _tableName;

        protected BaseRepository(DatabaseConnection dbConnection, string tableName)
        {
            _dbConnection = dbConnection;
            _tableName = tableName;
        }

        public virtual T? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual int Insert(T entity)
        {
            throw new NotImplementedException();
        }
        public virtual void Update(T entity)
        {
            throw new NotImplementedException();
        }
        public virtual void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}