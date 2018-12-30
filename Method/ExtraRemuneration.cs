using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("ExtraRemuneration")]
    public partial class ExtraRemuneration
    {
        [Key]
        public int RemunerationId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public decimal Value { get; set; }
    }
}
