using System;
using System.Collections.Generic;

namespace PosApi.Models;

public partial class receipt
{
    public int receiptId { get; set; }

    public string receiptCode { get; set; } = null!;

    public DateTime receiptDate { get; set; }

    public decimal receiptTotalBeforeDiscount { get; set; }

    public decimal receiptTotalDiscount { get; set; }

    public decimal receiptSubTotal { get; set; }

    public decimal receiptTradeDiscount { get; set; }

    public decimal receiptGrandTotal { get; set; }

    public virtual ICollection<receiptdetail> receiptdetails { get; set; } = new List<receiptdetail>();
}
