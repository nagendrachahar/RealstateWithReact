using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("PlotBooking")]
    public partial class PlotBooking
    {
        [Key]
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public string BookingCode { get; set; }
        public int PlotId { get; set; }
        public int PlotTypeId { get; set; }
        public double ActualPlotAmt { get; set; }
        public double PayableAmt { get; set; }
        public double BookingAmt { get; set; }
        public DateTime BookingDate { get; set; }
        public int PayMode { get; set; }
        public string Remark { get; set; }
        public int EMIPlanId { get; set; }
        public double EMIAmt { get; set; }
        public DateTime FirstEMIDate { get; set; }
        public int isUpdate { get; set; }
    
  }
}
