using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Document")]
    public partial class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
        public int DocumentTypeId { get; set; }
        public string Photo { get; set; }
    }
}
