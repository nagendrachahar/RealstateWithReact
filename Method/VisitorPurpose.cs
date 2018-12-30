using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("VisitorPurpose")]
    public partial class VisitorPurpose
    {
        [Key]
        public int VisitorPurposeId { get; set; }
        public string VisitorPurposeName { get; set; }
    }
}
