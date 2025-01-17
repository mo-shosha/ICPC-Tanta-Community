﻿using Core.IRepositories;
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
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public INewsRepository NewsRepository => _newsRepository ??= new NewsRepository(_context);
        public IEventRepository EventRepository=> _eventRepository ??= new EventRepository(_context);
        public IScheduleRepository ScheduleRepository => _scheduleRepository ??= new ScheduleRepository(_context);

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
