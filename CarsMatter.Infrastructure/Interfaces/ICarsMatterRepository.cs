namespace CarsMatter.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICarsMatterRepository<T>
    {
        Task<List<T>> GetAll();

        Task<bool> Add(T item);

        Task<bool> Update(T item);

        Task<bool> Delete(int itemId);
    }
}
