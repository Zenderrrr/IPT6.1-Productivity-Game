using System;

public interface IRepository<T>
{
    T? GetById(int id);
    int Insert(T entity);
    void Update(T entity);
    void Delete(int id);
}