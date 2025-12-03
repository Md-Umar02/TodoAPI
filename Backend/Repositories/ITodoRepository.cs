using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Model;

namespace TodoAPI.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<Todo?> GetByIdAsync(int id);
        Task AddAsync(Todo todo);
        Task UpdateAsync(Todo todo);
        Task DeleteAsync(int id);
    }
}