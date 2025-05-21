using ChildrenLeisure.DAL.Entities;
using System;

namespace ChildrenLeisure.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Attraction> AttractionRepository { get; }
        IRepository<FairyCharacter> FairyCharacterRepository { get; }
        IRepository<Order> OrderRepository { get; }

        void Save();
    }
}