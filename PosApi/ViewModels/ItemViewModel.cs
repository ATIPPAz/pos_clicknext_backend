using System.ComponentModel.DataAnnotations;

namespace PosApi.ViewModels.ItemViewModel
{
    public class ItemCreateRequest
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
    public class ItemResponse
    {

        public int itemId { get; set; }
        public string itemName { get; set; } = null!;
        public decimal itemPrice { get; set; }
        public int unitId { get; set; }
        public string unitName { get; set; } = null!;
        public string itemCode { get; set; } = null!;
    }

    public class DeleteItemRequest
    {
        [Required]
        public int itemId { get; set; }
    }
    public class UpdateItemRequest
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
