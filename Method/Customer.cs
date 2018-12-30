using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public int Title { get; set; }
        public string CustomerName { get; set; }
        public int isFather { get; set; }
        public string FatherHusband { get; set; }
        public DateTime DOB { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public DateTime DOJ { get; set; }
        public int MaritalStatus { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }

        
    }
}
