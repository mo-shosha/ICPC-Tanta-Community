using Core.Entities;
using Core.IRepositories;
using Repository.Data;


namespace Repository.Repositories
{
    class QAndARepository : BaseRepository<QAndA>, IQAndARepository
    {
        public QAndARepository(AppDbContext db) : base(db)
        {
        }


    }
}
