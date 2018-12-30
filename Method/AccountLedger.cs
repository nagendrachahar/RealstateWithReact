using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("AccountLedger")]
    public partial class AccountLedger
    {
        [Key]
        public int LedgerId { get; set; }
        public int GroupId { get; set; }
        public string LedgerName { get; set; }
    }
}
