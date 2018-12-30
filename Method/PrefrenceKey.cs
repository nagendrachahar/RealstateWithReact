using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
  
    [Table("PrefrenceKey")]
    public partial class PrefrenceKey
    {
        [Key]
        public int PrefrenceKeyId { get; set; }
        public string PrefrenceKeyName { get; set; }
        public string KeyValue { get; set; }
        public DateTime WEF { get; set; }
    }
}