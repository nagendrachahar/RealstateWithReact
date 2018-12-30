using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("AccountGroup")]
    public partial class AccountGroup
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
