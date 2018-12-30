using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Realstate.Method;
using System;

namespace Realstate.Controllers
{
    public class LoginController : Controller
    {
        private RealstateDb Db;
        public LoginController(RealstateDb context)
        {
            Db = context;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                FinancialYear MD = new FinancialYear();
                MD.FinancialYearList = new SelectList(GetFinancialYearList(), "FinancialYearId", "FinancialYearName");
                return View(MD);
            }
        }
        public IEnumerable<FinancialYear> GetFinancialYearList()
        {
            return Db.FinancialYears.Where(s=>s.IsActive==true).ToList();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public bool CheckLogin(string login, string pass)
        {
            var Result = Db.User.Where(s => s.UserCode == login && s.Password== pass).FirstOrDefault();
            if (Result!=null)
            {
                HttpContext.Session.SetString("UserId",Result.UserId.ToString());
                HttpContext.Session.SetString("UserName", Result.UserName.ToString());
                HttpContext.Session.SetString("BranchId", Result.BranchId.ToString());

                var FinancialYear = Db.FinancialYears.Where(s => s.IsActive == true).FirstOrDefault();
                HttpContext.Session.SetString("FinancialYearName", FinancialYear.FinancialYearName.ToString());

                var BranchName = Db.Branches.Where(s => s.BranchID == Convert.ToInt32(HttpContext.Session.GetString("BranchId").ToString())).FirstOrDefault();
                HttpContext.Session.SetString("BranchName", BranchName.BranchName.ToString());

                var CompanyName = Db.Companies.FirstOrDefault();
                HttpContext.Session.SetString("CompanyName", CompanyName.CompanyName.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
