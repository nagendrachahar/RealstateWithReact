using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Segment")]
    public partial class Segment
    {
        [Key]
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
    }
}
