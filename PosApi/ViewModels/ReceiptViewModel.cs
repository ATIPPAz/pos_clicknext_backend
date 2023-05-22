using PosApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PosApi.ViewModels.ReceiptViewModel
{
    public class ReceiptCreateRequest
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
    public class ReceiptAllResponse
    {

        public int receiptId { get; set; }

        public string receiptCode { get; set; } = null!;

        public DateTime receiptDate { get; set; }

        public decimal receiptGrandTotal { get; set; }

    }

    public class DeleteReceiptRequest
    {
        [Required]
        public int itemId { get; set; }
    }
    public class UpdateReceiptRequest
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


    public class receiptdetails
    {
        public int receiptDetailId { get; set; }
        public int itemQty { get; set; }

        public decimal itemPrice { get; set; }

        public decimal itemDiscount { get; set; }

        public decimal itemDiscountPercent { get; set; }

        public decimal itemAmount { get; set; }
        public string itemName { get; set; }
        public string itemCode { get; set; }
        public string unitName { get; set; }
    }



    public class ReceiptOneResponse
    {
        public int receiptId { get; set; }

        public string receiptCode { get; set; } = null!;

        public DateTime receiptDate { get; set; }

        public decimal receiptTotalBeforeDiscount { get; set; }

        public decimal receiptTotalDiscount { get; set; }

        public decimal receiptSubTotal { get; set; }

        public decimal receiptTradeDiscount { get; set; }

        public decimal receiptGrandTotal { get; set; }

        public ICollection<receiptdetails> receiptdetails { get; set; } = new List<receiptdetails>();
    }


    public class CreateReceiptDetails
    {
        [Required]
        public int receiptId { get; set; }
        [Required]
        public int itemId { get; set; }
        [Required]
        public int itemQty { get; set; }
        [Required]
        public decimal itemPrice { get; set; }
        [Required]
        public decimal itemDiscount { get; set; }
        [Required]
        public decimal itemDiscountPercent { get; set; }
        [Required]
        public decimal itemAmount { get; set; }
        [Required]
        public int unitId { get; set; }
    }

    public class CreateReceiptRequest
    {
    
        public string receiptCode { get; set; } = null!;
        [Required]
        public DateTime receiptDate { get; set; }
        [Required]
        public decimal receiptTotalBeforeDiscount { get; set; }
        [Required]
        public decimal receiptTotalDiscount { get; set; }
        [Required]
        public decimal receiptSubTotal { get; set; }
        [Required]
        public decimal receiptTradeDiscount { get; set; }
        [Required]
        public decimal receiptGrandTotal { get; set; }
        [Required]
        public List<CreateReceiptDetails> receiptdetails { get; set; } = new List<CreateReceiptDetails>();
    }


}
