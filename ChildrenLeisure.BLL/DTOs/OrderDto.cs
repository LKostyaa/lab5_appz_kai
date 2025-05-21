using ChildrenLeisure.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ChildrenLeisure.BLL.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBirthdayParty { get; set; }
        public int? FairyCharacterId { get; set; }
        public FairyCharacterDto FairyCharacter { get; set; }
        public List<AttractionDto> SelectedAttractions { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
    }
}