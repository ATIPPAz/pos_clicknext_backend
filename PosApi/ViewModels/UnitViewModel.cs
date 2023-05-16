using PosApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PosApi.ViewModels.UnitViewModel
{
    public class unitCreateRequest
    {
        public string unitName { get; set; } = null!;
    }
    public class unitUpdateRequest
    {
        public int unitId { get; set; }
        public string unitName { get; set; } = null!;
    }
    public class unitDeleteRequest
    {
        public int unitId { get; set; }
    }
    public class unitResponse
    {
        public int unitId { get; set; }
        public string unitName { get; set; } = null!;
    }
}
    