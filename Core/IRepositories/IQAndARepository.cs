
using Core.Entities;

namespace Core.IRepositories
{
    public interface IQAndARepository:IBaseRepository<QAndA>
    {
        Task<IEnumerable<QAndA>> GetLastAsync();

    }
}
