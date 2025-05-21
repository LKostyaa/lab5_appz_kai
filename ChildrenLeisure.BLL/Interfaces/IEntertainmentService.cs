using ChildrenLeisure.BLL.DTOs;
using System.Collections.Generic;

namespace ChildrenLeisure.BLL.Interfaces
{
    public interface IEntertainmentService
    {
        List<AttractionDto> GetAllAttractions();
        List<FairyCharacterDto> GetAllFairyCharacters();
        AttractionDto GetAttractionById(int id);
        FairyCharacterDto GetFairyCharacterById(int id);
    }
}