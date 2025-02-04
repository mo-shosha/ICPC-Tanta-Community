using Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Repository.Data;
namespace Repository.Repositories
{
    public class TrainingContentRepository: BaseRepository<TrainingContent>, ITrainingContentRepository
    {
        public TrainingContentRepository(AppDbContext context) : base(context) { }
    }
}
