using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Interfaces
{
    public interface IAzureTable<T>
    {
        Task<List<T>> GetList();

        Task<List<T>> GetList(string propertyName, string propertyValue);

        Task<T> GetItem(string partitionKey, string rowKey);

        Task Insert(T item);

        Task Update(T item);

        Task Delete(string partitionKey, string rowKey);

        Task CreateTableIfNotExist();
    }
}
