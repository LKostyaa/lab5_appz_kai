using ChildrenLeisure.BLL.DTOs;
using ChildrenLeisure.BLL.Interfaces;
using ChildrenLeisure.BLL.Mapping;
using ChildrenLeisure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ChildrenLeisure.BLL.Services
{
    public class EntertainmentService : IEntertainmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EntertainmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<AttractionDto> GetAllAttractions()
        {
            return _unitOfWork.AttractionRepository
                .GetAll()
                .AsNoTracking()
                .Select(a => a.ToDto())
                .ToList();
        }

        public List<FairyCharacterDto> GetAllFairyCharacters()
        {
            return _unitOfWork.FairyCharacterRepository
                .GetAll()
                .AsNoTracking()
                .Select(c => c.ToDto())
                .ToList();
        }

        public AttractionDto GetAttractionById(int id)
        {
            var attraction = _unitOfWork.AttractionRepository.GetById(id);
            return attraction?.ToDto();
        }

        public FairyCharacterDto GetFairyCharacterById(int id)
        {
            var character = _unitOfWork.FairyCharacterRepository.GetById(id);
            return character?.ToDto();
        }
    }
}
