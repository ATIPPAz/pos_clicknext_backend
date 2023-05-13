using System.ComponentModel.DataAnnotations;

namespace PosApi.ViewModels.UnitViewModel
{
    public class UnitCreateRequest
    {

        [Required]
        public string itemName { get; set; } = null!;
        [Required]
        public decimal itemPrice { get; set; }
        [Required]
        public int unitId { get; set; }
        [Required]
        public string itemCode { get; set; } = null!;

    }
    public class UnitResponse
    {

        public int itemId { get; set; }
        public string itemName { get; set; } = null!;
        public decimal itemPrice { get; set; }
        public int unitId { get; set; }
        public string unitName { get; set; } = null!;
        public string itemCode { get; set; } = null!;
    }

    public class deleteUnitRequest
    {
        [Required]
        public int itemId { get; set; }
    }
    public class updateUnitRequest
    {
        [Required]
        public int itemId { get; set; }
        [Required]
        public string itemName { get; set; } = null!;
        [Required]
        public decimal itemPrice { get; set; }
        [Required]
        public int unitId { get; set; }
  
    }
}
