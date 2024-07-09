using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapp.DAL.DTO;
using webapp.DAL.Models;

namespace webapp.DAL.Repositories
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync();
        Task<PagedResult<T>> GetAsyncPaged(int pageSize, int pageNumber);
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<bool> DeleteAsync(string id);
    }
}
