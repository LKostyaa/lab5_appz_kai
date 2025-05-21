using ChildrenLeisure.BLL.DTOs;
using ChildrenLeisure.BLL.Interfaces;
using ChildrenLeisure.DAL.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ChildrenLeisure.BLL.Services
{
    public class EntertainmentService : IEntertainmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntertainmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<AttractionDto> GetAllAttractions()
        {
            return _unitOfWork.AttractionRepository
                .GetAll()
                .AsNoTracking()
                .Select(a => _mapper.Map<AttractionDto>(a))
                .ToList();
        }

        public List<FairyCharacterDto> GetAllFairyCharacters()
        {
            return _unitOfWork.FairyCharacterRepository
                .GetAll()
                .AsNoTracking()
                .Select(c => _mapper.Map<FairyCharacterDto>(c))
                .ToList();
        }

        public AttractionDto GetAttractionById(int id)
        {
            var attraction = _unitOfWork.AttractionRepository.GetById(id);
            return attraction == null ? null : _mapper.Map<AttractionDto>(attraction);
        }

        public FairyCharacterDto GetFairyCharacterById(int id)
        {
            var character = _unitOfWork.FairyCharacterRepository.GetById(id);
            return character == null ? null : _mapper.Map<FairyCharacterDto>(character);
        }
    }
}
