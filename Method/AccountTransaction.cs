using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("AccountTransaction")]
    public partial class AccountTransaction
    {
        [Key]
        public int AccountTransactionId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public int BranchId { get; set; }
        public int ProjectId { get; set; }
        public int FromLedger { get; set; }
        public int ToLedger { get; set; }
        public string Remark { get; set; }
    }
}


