using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Bank")]
    public partial class Bank
    {
        [Key]
        public int BankId { get; set; }
        public string BankName { get; set; }
    }
}
