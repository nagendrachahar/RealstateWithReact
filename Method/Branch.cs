using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Branch")]
    public partial class Branch
    {
        [Key]
        public int BranchID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string InchargePerson { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int PinCode { get; set; }
        public string ContactNo { get; set; }
    }
}
