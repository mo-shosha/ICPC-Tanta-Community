using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.IRepositories
{
    public interface ITrainingLevelRepository:IBaseRepository<TrainingLevel>
    {
        public Task<IEnumerable<TrainingLevel>> LevelWithContentsAync(int id);
        public Task<IEnumerable<TrainingLevel>> LevelWithContentByYearAync(int id,string year);
    }
}
