using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("RateMaster")]
    public partial class RateMaster
    {
        [Key]
        public int RateId { get; set; }
        public int ProjectId { get; set; }
        public int SectorId { get; set; }
        public int BlockId { get; set; }
        public int SegmentId { get; set; }
        public int PlotTypeId { get; set; }
        public int PlotId { get; set; }
        public double Rate { get; set; }
        public string IdCollection { get; set; }
    }
}
