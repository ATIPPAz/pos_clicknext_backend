using PosApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PosApi.ViewModels.UnitViewModel
{
    public class unitCreateRequest
    {
        [Required]
        public string unitName { get; set; } = null!;
    }
    public class unitUpdateRequest
    {
        [Required]
        public int unitId { get; set; }
        [Required]
        public string unitName { get; set; } = null!;
    }
    public class unitDeleteRequest
    {
        [Required]
        public int unitId { get; set; }
    }
    public class unitResponse
    {
        public int unitId { get; set; }
        public string unitName { get; set; } = null!;
    }
}
    