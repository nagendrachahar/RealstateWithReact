using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
  
    [Table("City")]
    public partial class City
    {
        [Key]
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
    }
}