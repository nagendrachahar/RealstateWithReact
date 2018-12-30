using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("FinancialYear")]
    public partial class FinancialYear
    {
        [Key]
        public int FinancialYearId { get; set; }
        public string FinancialYearName { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public SelectList FinancialYearList { get; set; }
    }
}
