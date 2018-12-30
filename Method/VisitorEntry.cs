using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("VisitorEntry")]
    public partial class VisitorEntry
    {
        [Key]
        public int VisitorId { get; set; }
        public int ProjectId { get; set; }
        public string VisitorName { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public int PurposeId { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
