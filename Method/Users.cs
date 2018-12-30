using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Users")]
    public partial class Users
    {
        [Key]
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
        public string Password { get; set; }
    }
}
