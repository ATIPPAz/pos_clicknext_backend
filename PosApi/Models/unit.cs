using System;
using System.Collections.Generic;

namespace PosApi.Models;

public partial class unit
{
    public int unitId { get; set; }

    public string unitName { get; set; } = null!;

    public virtual ICollection<item> items { get; set; } = new List<item>();

    public virtual ICollection<receiptdetail> receiptdetails { get; set; } = new List<receiptdetail>();
}
