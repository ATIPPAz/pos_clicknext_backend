using System;
using System.Collections.Generic;

namespace PosApi.Models;

public partial class item
{
    public int itemId { get; set; }

    public string itemName { get; set; } = null!;

    public decimal itemPrice { get; set; }

    public int unitId { get; set; }

    public string itemCode { get; set; } = null!;

    public virtual ICollection<receiptdetail> receiptdetails { get; set; } = new List<receiptdetail>();

    public virtual unit unit { get; set; } = null!;
}
