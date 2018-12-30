using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Rate")]
    public partial class Rate
    {
        [Key]
        public int RateId { get; set; }
        public double Price { get; set; }
        public DateTime WEF { get; set; }
    }
}
