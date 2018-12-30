using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Relation")]
    public partial class Relation
    {
        [Key]
        public int RelationId { get; set; }
        public string RelationName { get; set; }
    }
}
