using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("PlotType")]
    public partial class PlotType
    {
        [Key]
        public int PlottypeId { get; set; }
        public string PlottypeName { get; set; }
    }
}
