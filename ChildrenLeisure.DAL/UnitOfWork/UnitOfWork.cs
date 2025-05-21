using ChildrenLeisure.DAL.Data;
using ChildrenLeisure.DAL.Entities;
using ChildrenLeisure.DAL.Interfaces;
using ChildrenLeisure.DAL.Repositories;
using System;

namespace ChildrenLeisure.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Attraction> _attractionRepository;
        private IRepository<FairyCharacter> _fairyCharacterRepository;
        private IRepository<Order> _orderRepository;
        private bool disposed = false;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Attraction> AttractionRepository
        {
            get
            {
                if (_attractionRepository == null)
                {
                    _attractionRepository = new BaseRepository<Attraction>(_context);
                }
                return _attractionRepository;
            }
        }

        public IRepository<FairyCharacter> FairyCharacterRepository
        {
            get
            {
                if (_fairyCharacterRepository == null)
                {
                    _fairyCharacterRepository = new BaseRepository<FairyCharacter>(_context);
                }
                return _fairyCharacterRepository;
            }
        }

        public IRepository<Order> OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new BaseRepository<Order>(_context);
                }
                return _orderRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}