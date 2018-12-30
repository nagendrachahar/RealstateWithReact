using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Block")]
    public partial class Block
    {
        [Key]
        public int BlockId { get; set; }
        public string BlockName { get; set; }
        public int SectorId { get; set; }
    }
}
