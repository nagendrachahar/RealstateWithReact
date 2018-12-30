using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
  
    [Table("SeriesCombination")]
    public partial class SeriesCombination
    {
        [Key]
        public int SeriesCombinationId { get; set; }        
        public string SeriesCombinationName { get; set; }
    }
}