using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("RemunerationStructure")]
    public partial class RemunerationStructure
    {
        [Key]
        public int RemunerationId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public decimal RemunerationPer { get; set; }
    }
}
