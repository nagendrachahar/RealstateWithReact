using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("PlotDetail")]
    public partial class PlotDetail
    {
        [Key]
        public int PlotDetailId { get; set; }
        public int ProjectId { get; set; }
        public int SectorId { get; set; }
        public int BlockId { get; set; }
        public int SegmentId { get; set; }
        public int PlotTypeId { get; set; }
        public double Rate { get; set; }
        public string Area { get; set; }
        public string PlotNo { get; set; }
        public int FromPlot { get; set; }
        public int ToPlot { get; set; }
    }
}


