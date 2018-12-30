using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{

    [Table("Series")]
    public partial class Series
    {
        [Key]
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Prefix { get; set; }
        public int NoOfDigit { get; set; }
        public int Combination1 { get; set; }
        public int Combination2  { get; set; }
        public int Combination3 { get; set; }


    }
}