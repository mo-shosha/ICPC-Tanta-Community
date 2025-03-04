using Core.IRepositories;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private INewsRepository _newsRepository;
        private IEventRepository _eventRepository;
        private IScheduleRepository _scheduleRepository;
        private ITeamRepository _teamRepository;
        private IMemberRepository _memberRepository;
        private ITrainingLevelRepository _traningLevelRepository;
        private ITrainingContentRepository _trainingContentRepository;
        private IChatRepository _chatRepository;
        private IInfoRepository _infoRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public INewsRepository NewsRepository => _newsRepository ??= new NewsRepository(_context);
        public IEventRepository EventRepository=> _eventRepository ??= new EventRepository(_context);
        public IScheduleRepository ScheduleRepository => _scheduleRepository ??= new ScheduleRepository(_context);
        public ITeamRepository TeamRepository => _teamRepository ??= new TeamRepository(_context);

        public IMemberRepository MemberRepository => _memberRepository ??= new MemberRepository(_context);

        public ITrainingLevelRepository TrainingLevelRepository => _traningLevelRepository ??= new TrainingLevelRepository(_context);

        public ITrainingContentRepository TrainingContentRepository => _trainingContentRepository ??= new TrainingContentRepository(_context);

        public IChatRepository ChatRepository =>_chatRepository= new ChatRepository(_context);

        public IInfoRepository infoRepository => _infoRepository = new InfoRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
