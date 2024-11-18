using NoteApp.Models;
using System.Threading.Tasks;

namespace NoteApp.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User> SaveAsync(User user);
        Task<User> FindByIdAsync(long id);
        Task<IEnumerable<User>> FindAllAsync();
        Task DeleteByIdAsync(long id);
    }
}