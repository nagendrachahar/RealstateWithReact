using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realstate.Method
{
    [Table("Employee")]
    public partial class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public int TitleId { get; set; }
        public string EmployeeName { get; set; }
        public string FatherHusbandName { get; set; }
        public string MotherName { get; set; }
        public DateTime DOJ { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string WorkLocation { get; set; }
        public int ReportingPerson { get; set; }
        public DateTime DOB { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public int MaritalStatus { get; set; }
        public string Qualification { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int Pincode { get; set; }
        public string Photo { get; set; }
        public string PersonName1 { get; set; }
        public string ContactNo1 { get; set; }
        public int RelationId1 { get; set; }
        public string PersonName2 { get; set; }
        public string ContactNo2 { get; set; }
        public int RelationId2 { get; set; }
    }
}
