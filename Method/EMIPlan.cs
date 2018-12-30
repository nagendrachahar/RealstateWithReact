using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
  
    [Table("EMIPlan")]
    public partial class EMIPlan
    {
        [Key]
        public int PlanId { get; set; }
        public string PLanCode { get; set; }
        public float PlanValue { get; set; }
    }
}