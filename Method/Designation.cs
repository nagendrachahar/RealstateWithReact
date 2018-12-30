using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Designation")]
    public partial class Designation
    {
        [Key]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
    }
}
