using System;
using System.Collections.Generic;

namespace PosApi.Models;

public partial class receiptdetail
{
    public int receiptDetailId { get; set; }

    public int receiptId { get; set; }

    public int itemId { get; set; }

    public int itemQty { get; set; }

    public decimal itemPrice { get; set; }

    public decimal itemDiscount { get; set; }

    public decimal itemDiscountPercent { get; set; }

    public decimal itemAmount { get; set; }

    public int unitId { get; set; }

    public virtual item item { get; set; } = null!;

    public virtual receipt receipt { get; set; } = null!;

    public virtual unit unit { get; set; } = null!;
}
