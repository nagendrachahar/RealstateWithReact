using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Company")]
    public partial class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CIN { get; set; }
        public string PAN { get; set; }
        public string Website { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Pincode { get; set; }
    }
}
