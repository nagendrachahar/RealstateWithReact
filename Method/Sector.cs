using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Sector")]
    public partial class Sector
    {
        [Key]
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public int ProjectId { get; set; }
    }
}
