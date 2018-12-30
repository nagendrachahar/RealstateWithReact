using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Realstate.Method;
using Newtonsoft.Json;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Realstate.Controllers
{
    [Route("api/[controller]")]
    public class MastersController : Controller
    {
        private RealstateDb Db;
        private string Con = "";
        private IHostingEnvironment _hostingEnvironment;
        private SqlConnection con;
        public MastersController(RealstateDb context, IHostingEnvironment hostingEnvironment)
        {
            Db = context;
            _hostingEnvironment = hostingEnvironment;
        }

        //-----------                           USERS      -------------
        [HttpGet("[action]")]
        public string GetSession()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    try
                    { 
                        Query="Select 1 as IsUser, "+ HttpContext.Session.GetString("UserId") + " as UserId, '"+ HttpContext.Session.GetString("UserName") + "' as UserName, '" + HttpContext.Session.GetString("FinancialYearName") + "' as FinancialYearName, '" + HttpContext.Session.GetString("CompanyName") + "' as CompanyName,'" + HttpContext.Session.GetString("BranchName") + "' as BranchName";
                    }
                    catch
                    {
                        Query = "Select 0 as IsUser, '' as UserName";
                    }
                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("IsUser"));
                dt.Columns.Add(new DataColumn("UserName"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public IEnumerable<Users> GetUsers()
        {
            return Db.User.ToList();
        }

        [HttpGet("[action]")]
        public string FillMenu()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"select m.* from User_Det UD
                                left join Menu as m on m.MenuId=UD.MenuNo
                                where UD.UserId=" + HttpContext.Session.GetString("UserId");

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{UserId}")]
        public string FillMenuTable(int UserId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"Select M.MenuId, M.MenuName,m.ParentID, isnull(U.UserId, 0) as UserId, isnull(U.MenuNo, 0) as MenuNo, isnull(U.[Save], 0) as [Save] ,isnull(U.[Update], 0) as [Update], isnull(U.[Delete],0) as [Delete], isNull(U.[View],0) as [View] 
                         from Menu M
                        left join User_Det U on U.MenuNo=M.MenuId and U.UserId=" + UserId;

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


    [HttpGet("[action]/{UserId}")]
    public string FillDataSaurce(int UserId)
    {
      DataTable dt = new DataTable();
      string JSONresult;
      string Query;
      try
      {
        using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
        {
          Query = @"exec sp_get.DataAccessRightSource " + UserId;

          SqlCommand cmd = new SqlCommand(Query, con);
          SqlDataAdapter da = new SqlDataAdapter();
          cmd.CommandType = CommandType.Text;
          con.Open();
          da.SelectCommand = cmd;
          da.Fill(dt);
          JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
          con.Close();
        }

      }
      catch (Exception Ex)
      {
        dt.Columns.Add(new DataColumn("MessageType"));
        dt.Columns.Add(new DataColumn("Message"));
        DataRow dr = dt.NewRow();
        dr[0] = "0";
        dr[1] = Ex.Message.ToString();

        dt.Rows.Add(dr);
        JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
      }
      return JSONresult;
    }

    //-----------                           STATE      -------------
    [HttpGet("[action]")]
        public IList FillState()
        {
            var Result= (from S in Db.States
                    join C in Db.Countries on S.CountryId equals C.CountryId
                    orderby S.StateName
                    select new
                    {
                        S.CountryId,
                        S.StateId,
                        S.StateName,
                        C.CountryName
                    }).ToList();
            return Result;
        }

        [HttpGet("[action]/{id}")]
        public string GetOneState(int id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.StateDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StateId", id);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveState([FromBody] State State)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.StateDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StateId", State.StateId);
                    cmd.Parameters.AddWithValue("@CountryId", State.CountryId);
                    cmd.Parameters.AddWithValue("@StateName", State.StateName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveSegment([FromBody] Segment Segment)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.SegmentDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SegmentId", Segment.SegmentId);
                    cmd.Parameters.AddWithValue("@SegmentName", Segment.SegmentName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{StateId}")]
        public string DeleteState(int StateId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.StateDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StateId", StateId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //-----------                           COUNTRY      -------------
        [HttpGet("[action]")]
        public IEnumerable<country> FillCountry()
        {
            return Db.Countries.ToList();
        }

        //-----------                           MEMBERS      -------------


        [HttpGet("[action]")]
        public string FillTitle()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "select * from title";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }



        [HttpGet("[action]")]
        public string FillMemberExplorer()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.MemberExplorer", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{MemberId}")]
        public string GetOneMember(int MemberId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.MemberDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MemberId", MemberId);
                    cmd.Parameters.AddWithValue("@BranchId", 1);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //-----------                           CITY      -------------
        [HttpGet("[action]")]
        public IList GetCity()
        {
            var Result = (from C in Db.Cities
                          join S in Db.States on C.StateId equals S.StateId
                          orderby C.CityName
                          select new
                          {
                              C.CityId,
                              C.StateId,
                              C.CityName,
                              S.StateName
                          }).ToList();
            return Result;
        }

        [HttpGet("[action]/{CityId}")]
        public string GetOneCity(int CityId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.CityDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CityId", CityId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{StateId}")]
        public IList GetCityOfState(int StateId)
        {
            var Result = (from C in Db.Cities
                          join S in Db.States on C.StateId equals S.StateId
                          orderby C.CityName
                          select new
                          {
                              C.CityId,
                              C.StateId,
                              C.CityName,
                              S.StateName
                          }).Where(x=>x.StateId==StateId).ToList();
            return Result;
        }

        [HttpPost("[action]")]
        public string SaveCity([FromBody] City City)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.CityDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CityId", City.CityId);
                    cmd.Parameters.AddWithValue("@StateId", City.StateId);
                    cmd.Parameters.AddWithValue("@CityName", City.CityName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{CityId}")]
        public string DeleteCity(int CityId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.CityDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CityId", CityId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //-----------                           RELATION      -------------
        [HttpGet("[action]")]
        public IEnumerable<Relation> GetRelation()
        {
            return Db.Relations.ToList();
        }

        [HttpGet("[action]/{RelationId}")]
        public string GetOneRelation(int RelationId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.RelationDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RelationId", RelationId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveRelation([FromBody] Relation Relation)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.RelationDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RelationId", Relation.RelationId);
                    cmd.Parameters.AddWithValue("@RelationName", Relation.RelationName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{RelationId}")]
        public string DeleteRelation(int RelationId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.RelationDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RelationId", RelationId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //-----------                           COMPANY      -------------
        [HttpGet("[action]")]
        public IEnumerable<Company> GetCompany()
        {
            return Db.Companies.ToList();
        }

        [HttpPost("[action]")]
        public string SaveCompany([FromBody] Company company)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.CompanyDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CompanyCode", company.CompanyCode);
                    cmd.Parameters.AddWithValue("@CompanyName", company.CompanyName);
                    cmd.Parameters.AddWithValue("@CIN", company.CIN);
                    cmd.Parameters.AddWithValue("@PAN", company.PAN);
                    cmd.Parameters.AddWithValue("@Website", company.Website);
                    cmd.Parameters.AddWithValue("@EmailId", company.EmailId);
                    cmd.Parameters.AddWithValue("@ContactNo", company.ContactNo);
                    cmd.Parameters.AddWithValue("@RegistrationDate", company.RegistrationDate);
                    cmd.Parameters.AddWithValue("@Address", company.Address);
                    cmd.Parameters.AddWithValue("@StateId", company.StateId);
                    cmd.Parameters.AddWithValue("@CityId", company.CityId);
                    cmd.Parameters.AddWithValue("@Pincode", company.Pincode);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //-----------                           BRANCH      -------------
        //[HttpGet("[action]")]
        //public IEnumerable<Branch> GetBranch()
        //{
        //    return Db.Branches.ToList();
        //}

        [HttpGet("[action]")]
        public string GetBranch()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {

                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    string Query;
                    Query = @"select B.*,FORMAT(B.RegistrationDate,'yyyy-MM-dd') as ReDate, C.CityName, S.StateName from Branch B
                                left join City C on C.CityId=B.CityID
                                left join [State] S on S.StateId=B.StateID";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }


            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));

                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);

            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillBanner()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {

                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    string Query;
                    Query = @"select * from Banner";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }


            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));

                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);

            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveBranch([FromBody] Branch branch)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.BranchDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BranchID", branch.BranchID);
                    cmd.Parameters.AddWithValue("@BranchCode", branch.BranchCode);
                    cmd.Parameters.AddWithValue("@BranchName", branch.BranchName);
                    cmd.Parameters.AddWithValue("@RegistrationDate", branch.RegistrationDate);
                    cmd.Parameters.AddWithValue("@InchargePerson", branch.InchargePerson);
                    cmd.Parameters.AddWithValue("@Address", branch.Address);
                    cmd.Parameters.AddWithValue("@StateID", branch.StateId);
                    cmd.Parameters.AddWithValue("@CityID", branch.CityId);
                    cmd.Parameters.AddWithValue("@PinCode", branch.PinCode);
                    cmd.Parameters.AddWithValue("@ContactNo", branch.ContactNo);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{BranchID}")]
        public string GetOneBranch(int BranchID)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.BranchDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BranchID", BranchID);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{BranchId}")]
        public string DeleteBranch(int BranchId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.BranchDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BranchId", BranchId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //-----------                           SQL Server      -------------
        [HttpGet("[action]/{SearchQuery}")]
        public string GetSqlQueryResult(string SearchQuery)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    string Query = SearchQuery;
                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //-----------                           New Series      -------------
        [HttpGet("[action]")]
        public IEnumerable<SeriesCombination> GetCombination()
        {
            return Db.SeriesCombinations.ToList();
        }

        [HttpGet("[action]/{SeriesId}")]
        public string GetOneSeries(int SeriesId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.SeriesDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SeriesId", SeriesId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillSeries()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.SeriesExplorer", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveSeries([FromBody] Series series)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.SeriesDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SeriesId", series.SeriesId);
                    cmd.Parameters.AddWithValue("@SeriesName", series.SeriesName);
                    cmd.Parameters.AddWithValue("@TableName", series.TableName);
                    cmd.Parameters.AddWithValue("@ColumnName", series.ColumnName);
                    cmd.Parameters.AddWithValue("@Prefix", series.Prefix);
                    cmd.Parameters.AddWithValue("@NoOfDigit", series.NoOfDigit);
                    cmd.Parameters.AddWithValue("@Combination1", series.Combination1);
                    cmd.Parameters.AddWithValue("@Combination2", series.Combination2);
                    cmd.Parameters.AddWithValue("@Combination3", series.Combination3);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //-----------                           Prefrence Key      -------------
        [HttpGet("[action]")]
        public IEnumerable<PrefrenceKey> GetPrefrenceKey()
        {
            return Db.prefrenceKeys.OrderBy(s=>s.PrefrenceKeyName).ToList();
        }

        [HttpGet("[action]/{prefrenceKeyId}")]
        public string GetOnePrefrenceKey(int prefrenceKeyId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.PrefrenceKeyDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PrefrenceKeyId", prefrenceKeyId);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SavePrefrenceKey([FromBody] PrefrenceKey prefrenceKeys)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.PrefrenceKeyDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PrefrenceKeyId", prefrenceKeys.PrefrenceKeyId);
                    cmd.Parameters.AddWithValue("@PrefrenceKeyName", prefrenceKeys.PrefrenceKeyName);
                    cmd.Parameters.AddWithValue("@KeyValue", prefrenceKeys.KeyValue);
                    cmd.Parameters.AddWithValue("@WEF", prefrenceKeys.WEF);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]")]
        public string FillSegment()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "Select * from Segment";
                    
                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{SegmentId}")]
        public string GetOneSegment(string SegmentId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneSegment", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SegmentId", SegmentId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{SegmentId}")]
        public string DeleteSegment(int SegmentId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.SegmentDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SegmentId", SegmentId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        ///// Plot Type


        [HttpGet("[action]/{PlottypeId}")]
        public string GetOnePlotType(string PlottypeId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OnePlotType", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlottypeId", PlottypeId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SavePlotType([FromBody] PlotType PlotType)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.PlotTypeDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlottypeId", PlotType.PlottypeId);
                    cmd.Parameters.AddWithValue("@PlottypeName", PlotType.PlottypeName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }
        
        [HttpGet("[action]")]
        public string FillPlotType()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "Select * from PlotType";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{PlotTypeId}")]
        public string DeletePlottype(int PlotTypeId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.PlotTypeDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlotTypeId", PlotTypeId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        ///     Bank 
        
        [HttpGet("[action]/{BankId}")]
        public string GetOneBank(string BankId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneBank", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BankId", BankId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpPost("[action]")]
        public string SaveBank([FromBody] Bank Bank)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.BankDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BankId", Bank.BankId);
                    cmd.Parameters.AddWithValue("@BankName", Bank.BankName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillBank()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "Select * from Bank";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{BankId}")]
        public string DeleteBank(int BankId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.BankDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BankId", BankId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //   Visitor Purpose

        [HttpGet("[action]/{VisitorPurposeId}")]
        public string GetOneVisitorPurpose(string VisitorPurposeId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneVisitorPurpose", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VisitorPurposeId", VisitorPurposeId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpPost("[action]")]
        public string SaveVisitorPurpose([FromBody] VisitorPurpose VisitorPurpose)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.VisitorPurposeDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VisitorPurposeId", VisitorPurpose.VisitorPurposeId);
                    cmd.Parameters.AddWithValue("@VisitorPurposeName", VisitorPurpose.VisitorPurposeName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillVisitorPurpose()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "Select * from VisitorPurpose";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{VisitorPurposeId}")]
        public string DeleteVisitorPurpose(int VisitorPurposeId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.VisitorPurposeDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VisitorPurposeId", VisitorPurposeId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        // Department


        [HttpGet("[action]/{DepartmentId}")]
        public string GetOneDepartment(string DepartmentId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneDepartment", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DepartmentId", DepartmentId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpPost("[action]")]
        public string SaveDepartment([FromBody] Department Department)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.DepartmentDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DepartmentId", Department.DepartmentId);
                    cmd.Parameters.AddWithValue("@DepartmentName", Department.DepartmentName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]")]
        public string FillDepartment()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "Select * from Department";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{DepartmentId}")]
        public string DeleteDepartment(int DepartmentId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.DepartmentDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        //  Designation

        [HttpGet("[action]/{DesignationId}")]
        public string GetOneDesignation(string DesignationId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneDesignation", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DesignationId", DesignationId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }



        [HttpPost("[action]")]
        public string SaveDesignation([FromBody] Designation Designation)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.DesignationDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DesignationId", Designation.DesignationId);
                    cmd.Parameters.AddWithValue("@DesignationName", Designation.DesignationName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]")]
        public string FillDesignation()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "Select * from Designation";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillFARank()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
              using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
              {

                Query = "Select * from FARank";

                SqlCommand cmd = new SqlCommand(Query, con);
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                con.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                con.Close();
              }

            }
            catch (Exception Ex)
            {
              dt.Columns.Add(new DataColumn("MessageType"));
              dt.Columns.Add(new DataColumn("Message"));
              DataRow dr = dt.NewRow();
              dr[0] = "0";
              dr[1] = Ex.Message.ToString();

              dt.Rows.Add(dr);
              JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }



        [HttpGet("[action]/{DesignationId}")]
        public string DeleteDesignation(int DesignationId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.DesignationDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DesignationId", DesignationId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //  Project

        [HttpGet("[action]/{ProjectId}")]
        public string GetOneProject(string ProjectId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneProject", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProjectId", ProjectId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpPost("[action]")]
        public string SaveProject([FromBody] Project Project)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.ProjectDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProjectId", Project.ProjectId);
                    cmd.Parameters.AddWithValue("@ProjectName", Project.ProjectName);
                    cmd.Parameters.AddWithValue("@Address", Project.Address);
                    cmd.Parameters.AddWithValue("@Area", Project.Area);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]")]
        public string FillProject()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "Select * from Project";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        
        [HttpGet("[action]/{ProjectId}")]
        public string DeleteProject(int ProjectId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.ProjectDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProjectId", ProjectId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //  Sector


        [HttpGet("[action]/{SectorId}")]
        public string GetOneSector(string SectorId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneSector", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SectorId", SectorId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }
        
        [HttpPost("[action]")]
        public string SaveSector([FromBody] Sector Sector)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.SectorDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SectorId", Sector.SectorId);
                    cmd.Parameters.AddWithValue("@ProjectId", Sector.ProjectId);
                    cmd.Parameters.AddWithValue("@SectorName", Sector.SectorName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]")]
        public string FillSector()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"select S.SectorId, S.SectorName, P.ProjectName from Sector S
                                left join Project P on P.ProjectId = S.ProjectId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{SectorId}")]
        public string DeleteSector(int SectorId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.SectorDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SectorId", SectorId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{id}")]
        public string FillSectorByProject(int id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"select S.SectorId, S.SectorName, P.ProjectName from Sector S
                                left join Project P on P.ProjectId = S.ProjectId
                                where P.ProjectId=" + id;

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        // Block   


        [HttpGet("[action]/{BlockId}")]
        public string GetOneBlock(string BlockId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneBlock", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BlockId", BlockId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpPost("[action]")]
        public string SaveBlock([FromBody] Block Block)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.BlockDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BlockId", Block.BlockId);
                    cmd.Parameters.AddWithValue("@SectorId", Block.SectorId);
                    cmd.Parameters.AddWithValue("@BlockName", Block.BlockName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]")]
        public string FillBlock()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"Select B.BlockId, B.BlockName, S.SectorName from Block B
                                left join Sector S on B.SectorId=S.SectorId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{BlockId}")]
        public string DeleteBlock(int BlockId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.BlockDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BlockId", BlockId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{id}")]
        public string FillBlockBySector(int id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"Select B.BlockId, B.BlockName, S.SectorName from Block B
                            left join Sector S on B.SectorId=S.SectorId
                            where S.SectorId="+ id;

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        // Rate Master


        [HttpGet("[action]/{RateId}")]
        public string GetOneRate(string RateId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneRate", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RateId", RateId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpPost("[action]")]
        public string SaveRate([FromBody] Rate Rate)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.RateDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RateId", Rate.RateId);
                    cmd.Parameters.AddWithValue("@Price", Rate.Price);
                    cmd.Parameters.AddWithValue("@WEF", Rate.WEF);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillRate()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"Select DISTINCT Rate from RateMaster";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]/{RateId}")]
        public string DeleteRate(int RateId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.RateDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RateId", RateId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        //  Account Group

        [HttpGet("[action]/{GroupId}")]
        public string GetOneAccountGroup(string GroupId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneAccountGroup", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@GroupId", GroupId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpPost("[action]")]
        public string SaveAccountGroup([FromBody] AccountGroup AccountGroup)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.AccountGroupDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@GroupId", AccountGroup.GroupId);
                    cmd.Parameters.AddWithValue("@GroupName", AccountGroup.GroupName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillAccountGroup()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"select * from AccountGroup";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{GroupId}")]
        public string DeleteAccountGroup(int GroupId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.AccountGroupDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@GroupId", GroupId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        // Account Ledger

        [HttpGet("[action]/{LedgerId}")]
        public string GetOneAccountLedger(string LedgerId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneAccountLedger", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LedgerId", LedgerId);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveAccountLedger([FromBody] AccountLedger AccountLedger)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.AccountLedgerDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LedgerId", AccountLedger.LedgerId);
                    cmd.Parameters.AddWithValue("@GroupId", AccountLedger.GroupId);
                    cmd.Parameters.AddWithValue("@LedgerName", AccountLedger.LedgerName);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillAccountLedger()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"select AL.LedgerId, AL.LedgerName, AG.GroupName  from AccountLedger AL
                                left join AccountGroup AG on AG.GroupId=AL.GroupId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{LedgerId}")]
        public string DeleteAccountLedger(int LedgerId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.AccountLedgerDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LedgerId", LedgerId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        // Account Transaction


        [HttpGet("[action]/{AccountTransactionId}")]
        public string GetOneAccountTransaction(string AccountTransactionId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneAccountTransaction", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AccountTransactionId", AccountTransactionId);
                    cmd.Parameters.AddWithValue("@BranchId", HttpContext.Session.GetString("BranchId").ToString());

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveAccountTransaction([FromBody] AccountTransaction AccountTransaction)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.AccountTransactionDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AccountTransactionId", AccountTransaction.AccountTransactionId);
                    cmd.Parameters.AddWithValue("@VoucherNo", AccountTransaction.VoucherNo);
                    cmd.Parameters.AddWithValue("@VoucherDate", AccountTransaction.VoucherDate);
                    cmd.Parameters.AddWithValue("@FromLedger", AccountTransaction.FromLedger);
                    cmd.Parameters.AddWithValue("@ToLedger", AccountTransaction.ToLedger);
                    cmd.Parameters.AddWithValue("@BranchId", AccountTransaction.BranchId);
                    cmd.Parameters.AddWithValue("@ProjectId", AccountTransaction.ProjectId);
                    cmd.Parameters.AddWithValue("@Remark", AccountTransaction.Remark);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpGet("[action]")]
        public string FillAccountTransaction()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"Select A.AccountTransactionId, VoucherNo, VoucherDate, P.ProjectName, FL.LedgerName as FromLedger, 
                                TL.LedgerName as ToLedger
                                from AccountTransaction A
                                left join Project P on P.ProjectId=A.ProjectId
                                left join AccountLedger FL on FL.LedgerId=A.FromLedger
                                left join AccountLedger TL on TL.LedgerId=A.ToLedger";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{Id}")]
        public string DeleteAccountTransaction(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.AccountTransactionDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AccountTransactionId", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }
        
        // Manage Employee 

        [HttpGet("[action]/{EmployeeId}")]
        public string GetOneEmployee(int EmployeeId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneEmployee", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());
                    cmd.Parameters.AddWithValue("@BranchId", HttpContext.Session.GetString("BranchId").ToString());
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented); 
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveEmployee([FromBody] Employee Employee)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.EmployeeDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmployeeId", Employee.EmployeeId);
                    cmd.Parameters.AddWithValue("@EmployeeCode", Employee.EmployeeCode);
                    cmd.Parameters.AddWithValue("@TitleId", Employee.TitleId);
                    cmd.Parameters.AddWithValue("@EmployeeName", Employee.EmployeeName);
                    cmd.Parameters.AddWithValue("@FatherHusbandName", Employee.FatherHusbandName);
                    cmd.Parameters.AddWithValue("@MotherName", Employee.MotherName);
                    cmd.Parameters.AddWithValue("@DOJ", Employee.DOJ);
                    cmd.Parameters.AddWithValue("@BranchId", Employee.BranchId);
                    cmd.Parameters.AddWithValue("@DepartmentId", Employee.DepartmentId);
                    cmd.Parameters.AddWithValue("@DesignationId", Employee.DesignationId);
                    cmd.Parameters.AddWithValue("@WorkLocation", Employee.WorkLocation);
                    cmd.Parameters.AddWithValue("@ReportingPerson", Employee.ReportingPerson);
                    cmd.Parameters.AddWithValue("@DOB", Employee.DOB);
                    cmd.Parameters.AddWithValue("@ContactNo", Employee.ContactNo);
                    cmd.Parameters.AddWithValue("@EmailId", Employee.EmailId);
                    cmd.Parameters.AddWithValue("@MaritalStatus", Employee.MaritalStatus);
                    cmd.Parameters.AddWithValue("@Qualification", Employee.Qualification);
                    cmd.Parameters.AddWithValue("@Address", Employee.Address);
                    cmd.Parameters.AddWithValue("@StateId", Employee.StateId);
                    cmd.Parameters.AddWithValue("@CityId", Employee.CityId);
                    cmd.Parameters.AddWithValue("@Pincode", Employee.Pincode);
                    cmd.Parameters.AddWithValue("@PersonName1", Employee.PersonName1);
                    cmd.Parameters.AddWithValue("@ContactNo1", Employee.ContactNo1);
                    cmd.Parameters.AddWithValue("@RelationId1", Employee.RelationId1);
                    cmd.Parameters.AddWithValue("@PersonName2", Employee.PersonName2);
                    cmd.Parameters.AddWithValue("@ContactNo2", Employee.ContactNo2);
                    cmd.Parameters.AddWithValue("@RelationId2", Employee.RelationId2);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();


                    string image = Employee.Photo;

                    if (image != null)
                    {
                        string EmployeeCode = Employee.EmployeeCode;

                        string folderName = "ClientApp/src/assets/Upload/Employee";
                        string webRootPath = _hostingEnvironment.ContentRootPath;
                        string newPath = Path.Combine(webRootPath, folderName);
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }

                        image = image.Substring(image.IndexOf(',') + 1);

                        byte[] imageBytes = Convert.FromBase64String(image);

                        if (imageBytes.Length > 0)
                        {
                            string fullPath = Path.Combine(newPath, EmployeeCode + ".jpg");

                            System.IO.File.WriteAllBytes(fullPath, imageBytes);
                        }
                    }


                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{Code}")]
        public string GetEmployeeId(string Code)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneEmployeeId", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Code", Code);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillEmployee()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"Select * from Employee";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpDelete("[action]/{Id}")]
        public string DeleteEmployee(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.EmployeeDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmployeeId", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillDocumentType()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = "Select * from DocumentType";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{Id}")]
        public string GetOneDocument(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneDocument", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DocumentId", Id);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }
        
        [HttpPost("[action]")]
        public string SaveDocument([FromBody] Document Document)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.ProcEmployeeDocumentTmp", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DocumentId", Document.DocumentId);
                    cmd.Parameters.AddWithValue("@Name", Document.Name);
                    cmd.Parameters.AddWithValue("@SerialNo", Document.SerialNo);
                    cmd.Parameters.AddWithValue("@DocumentType", Document.DocumentTypeId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                    string image = Document.Photo;

                    if (image != null)
                    {
                        //string DocumentPath = dt.Rows[0]["PhotoPath"].ToString();
                        string DocumentPath = Document.SerialNo;
                        string folderName = "ClientApp/src/assets/Upload/Employee/Document";
                        string webRootPath = _hostingEnvironment.ContentRootPath;
                        string newPath = Path.Combine(webRootPath, folderName);
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }


                        image = image.Substring(image.IndexOf(',') + 1);

                        byte[] imageBytes = Convert.FromBase64String(image);

                        if (imageBytes.Length > 0)
                        {
                            string fullPath = Path.Combine(newPath, DocumentPath + ".jpg");

                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }

                            //System.IO.File.WriteAllBytes(fullPath, imageBytes);
                        }
                    }


                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillEmployeeDocument()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = "select * from EmployeeDocumentTmp where UserId=" + HttpContext.Session.GetString("UserId").ToString();

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpDelete("[action]/{Id}")]
        public string DeleteEmployeeDocument(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.ProcEmployeeDocumentTmp", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DocumentId", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        // Manage User 

        [HttpGet("[action]/{Id}")]
        public string GetOneUser(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneUser", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", Id);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }
        
        [HttpPost("[action]")]
        public string SaveUser([FromBody] Users Users)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.UserDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId2", Users.UserId);
                    cmd.Parameters.AddWithValue("@UserCode", Users.UserCode);
                    cmd.Parameters.AddWithValue("@UserName", Users.UserName);
                    cmd.Parameters.AddWithValue("@BranchId", Users.BranchId);
                    cmd.Parameters.AddWithValue("@Password", Users.Password);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillUsers()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"Select U.UserId, U.UserCode, U.UserName, U.Password ,B.BranchName
                                from Users U
                                left join Branch B on B.BranchID=U.BranchId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{Id}")]
        public string DeleteUser(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.UserDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId2", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{MenuId}/{UserId}")]
        public string Save_SavePermission(int MenuId, int UserId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Save.SavePermission", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MenuId", MenuId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


    [HttpGet("[action]/{TableId}/{ColumnId}/{UserId}")]
    public string Save_SaveAccessData(int TableId, int ColumnId, int UserId)
    {
      DataTable dt = new DataTable();
      string JSONresult;
      try
      {
        using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
        {
          SqlCommand cmd = new SqlCommand("sp_Save.SaveAccessData", con);
          SqlDataAdapter da = new SqlDataAdapter();
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.AddWithValue("@TableId", TableId);
          cmd.Parameters.AddWithValue("@ColumnId", ColumnId);
          cmd.Parameters.AddWithValue("@UserId", UserId);

          con.Open();
          cmd.CommandType = CommandType.StoredProcedure;
          da.SelectCommand = cmd;
          da.Fill(dt);
          JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
          con.Close();
        }

      }
      catch (Exception Ex)
      {
        dt.Columns.Add(new DataColumn("MessageType"));
        dt.Columns.Add(new DataColumn("Message"));
        DataRow dr = dt.NewRow();
        dr[0] = "0";
        dr[1] = Ex.Message.ToString();

        dt.Rows.Add(dr);
        JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
      }
      return JSONresult;
    }

    [HttpGet("[action]/{TableId}/{UserId}/{isSave}")]
    public string Save_AllSaveAccessdata(int TableId, int UserId, bool isSave)
    {
      DataTable dt = new DataTable();
      string JSONresult;
      try
      {
        using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
        {
          SqlCommand cmd = new SqlCommand("sp_Save.SaveAllAccessData", con);
          SqlDataAdapter da = new SqlDataAdapter();
          cmd.CommandType = CommandType.StoredProcedure;

          cmd.Parameters.AddWithValue("@TableId", TableId);
          cmd.Parameters.AddWithValue("@isSave", isSave);
          cmd.Parameters.AddWithValue("@UserId", UserId);

          con.Open();
          cmd.CommandType = CommandType.StoredProcedure;
          da.SelectCommand = cmd;
          da.Fill(dt);
          JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
          con.Close();
        }

      }
      catch (Exception Ex)
      {
        dt.Columns.Add(new DataColumn("MessageType"));
        dt.Columns.Add(new DataColumn("Message"));
        DataRow dr = dt.NewRow();
        dr[0] = "0";
        dr[1] = Ex.Message.ToString();

        dt.Rows.Add(dr);
        JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
      }
      return JSONresult;
    }


    [HttpGet("[action]/{MenuId}/{UserId}")]
        public string Save_UpdatePermission(int MenuId, int UserId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Save.UpdatePermission", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MenuId", MenuId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{MenuId}/{UserId}")]
        public string Save_DeletePermission(int MenuId, int UserId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Save.DeletePermission", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MenuId", MenuId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{MenuId}/{UserId}")]
        public string Save_ViewPermission(int MenuId, int UserId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Save.ViewPermission", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MenuId", MenuId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        // Remuneration Structure

        [HttpPost("[action]")]
        public string SaveRemunerationStructure([FromBody] RemunerationStructure RemunerationStructure)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.RemunerationStructureDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RemunerationId", RemunerationStructure.RemunerationId);
                    cmd.Parameters.AddWithValue("@DepartmentId", RemunerationStructure.DepartmentId);
                    cmd.Parameters.AddWithValue("@DesignationId", RemunerationStructure.DesignationId);
                    cmd.Parameters.AddWithValue("@RemunerationPer", RemunerationStructure.RemunerationPer);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }
        
        [HttpGet("[action]/{RemunerationId}")]
        public string GetOneRemunerationStructure(int RemunerationId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneRemunerationStructure", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RemunerationId", RemunerationId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillRemunerationStructure()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"Select R.RemunerationId, R.RemunerationPer ,D.DepartmentName, DS.DesignationName 
                                from RemunerationStructure as R
                                left join Department D on D.DepartmentId=R.DepartmentId
                                left join Designation DS on DS.DesignationId=R.DesignationId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{Id}")]
        public string DeleteRemunerationStructure(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.RemunerationStructureDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RemunerationId", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        // Extra Remuneration

        [HttpPost("[action]")]
        public string SaveExtraRemuneration([FromBody] ExtraRemuneration ExtraRemuneration)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.ExtraRemunerationDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RemunerationId", ExtraRemuneration.RemunerationId);
                    cmd.Parameters.AddWithValue("@DepartmentId", ExtraRemuneration.DepartmentId);
                    cmd.Parameters.AddWithValue("@DesignationId", ExtraRemuneration.DesignationId);
                    cmd.Parameters.AddWithValue("@Value", ExtraRemuneration.Value);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{RemunerationId}")]
        public string GetOneExtraRemuneration(int RemunerationId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneExtraRemuneration", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RemunerationId", RemunerationId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillExtraRemuneration()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"Select R.RemunerationId, R.Value ,D.DepartmentName, DS.DesignationName 
                                from ExtraRemuneration as R
                                left join Department D on D.DepartmentId=R.DepartmentId
                                left join Designation DS on DS.DesignationId=R.DesignationId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{Id}")]
        public string DeleteExtraRemuneration(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.ExtraRemunerationDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RemunerationId", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        // Plot Detail

        [HttpPost("[action]")]
        public string SavePlotDetail([FromBody] PlotDetail PlotDetail)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.PlotDetailDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlotDetailId", PlotDetail.PlotDetailId);
                    cmd.Parameters.AddWithValue("@ProjectId", PlotDetail.ProjectId);
                    cmd.Parameters.AddWithValue("@SectorId", PlotDetail.SectorId);
                    cmd.Parameters.AddWithValue("@BlockId", PlotDetail.BlockId);
                    cmd.Parameters.AddWithValue("@SegmentId", PlotDetail.SegmentId);
                    cmd.Parameters.AddWithValue("@PlotTypeId", PlotDetail.PlotTypeId);
                    cmd.Parameters.AddWithValue("@Rate", PlotDetail.Rate);
                    cmd.Parameters.AddWithValue("@Area", PlotDetail.Area);
                    cmd.Parameters.AddWithValue("@PlotNo", PlotDetail.PlotNo);
                    cmd.Parameters.AddWithValue("@FromPlot", PlotDetail.FromPlot);
                    cmd.Parameters.AddWithValue("@ToPlot", PlotDetail.ToPlot);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }
        

        [HttpPost("[action]")]
        public string vrifyPlotDetail([FromBody] PlotDetail PlotDetail)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_get.verifyPlotDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlotDetailId", PlotDetail.PlotDetailId);
                    cmd.Parameters.AddWithValue("@ProjectId", PlotDetail.ProjectId);
                    cmd.Parameters.AddWithValue("@SectorId", PlotDetail.SectorId);
                    cmd.Parameters.AddWithValue("@BlockId", PlotDetail.BlockId);
                    cmd.Parameters.AddWithValue("@SegmentId", PlotDetail.SegmentId);
                    cmd.Parameters.AddWithValue("@PlotTypeId", PlotDetail.PlotTypeId);
                    cmd.Parameters.AddWithValue("@Rate", PlotDetail.Rate);
                    cmd.Parameters.AddWithValue("@Area", PlotDetail.Area);
                    cmd.Parameters.AddWithValue("@PlotNo", PlotDetail.PlotNo);
                    cmd.Parameters.AddWithValue("@FromPlot", PlotDetail.FromPlot);
                    cmd.Parameters.AddWithValue("@ToPlot", PlotDetail.ToPlot);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        
        [HttpGet("[action]/{PlotId}")]
        public string GetOnePlotDetail(int PlotId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OnePlotDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlotId", PlotId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillPlotDetail()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"Select PD.PlotDetailId, P.ProjectName, S.SectorName, B.BlockName, Sg.SegmentName, PT.PlottypeName
                                ,R.Rate, PD.Area, PD.PlotNo from PlotDetail PD 
                                left join Project P on P.ProjectId=PD.ProjectId
                                left join Sector S on S.SectorId=PD.SectorId
                                left join Block B on B.BlockId=PD.BlockId
                                left join Segment Sg on Sg.SegmentId=PD.SegmentId
                                left join PlotType PT on PT.PlottypeId=PD.PlotTypeId
                                left join RateMaster R on R.PlotId=PD.PlotDetailId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{BlockId}")]
        public string FillPlotByBlock(int BlockId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"select PlotDetailId, PlotNo from PlotDetail where BlockId="+ BlockId;

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{BlockId}/{isBooked}")]
        public string FillActivePlotByBlock(int BlockId, int isBooked)
        {
          DataTable dt = new DataTable();
          string JSONresult;
          string Query;
          try
          {
            using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
            {
              Query = @"select PlotDetailId, PlotNo from PlotDetail where isnull(isBooked,0)="+ isBooked + " and BlockId=" + BlockId;

              SqlCommand cmd = new SqlCommand(Query, con);
              SqlDataAdapter da = new SqlDataAdapter();
              cmd.CommandType = CommandType.Text;
              con.Open();
              da.SelectCommand = cmd;
              da.Fill(dt);
              JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
              con.Close();
            }

          }
          catch (Exception Ex)
          {
            dt.Columns.Add(new DataColumn("MessageType"));
            dt.Columns.Add(new DataColumn("Message"));
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = Ex.Message.ToString();

            dt.Rows.Add(dr);
            JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
          }
          return JSONresult;
        }

        [HttpGet("[action]/{PlotDetailId}")]
        public string DeletePlotDetail(int PlotDetailId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.PlotDetailDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlotDetailId", PlotDetailId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        //  Visitor Entry

        [HttpGet("[action]/{VisitorId}")]
        public string GetOneVisitorEntry(int VisitorId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneVisitorEntry", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VisitorId", VisitorId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveVisitorEntry([FromBody] VisitorEntry visitorEntry) {

            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.VisitorEntryDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VisitorId", visitorEntry.VisitorId);
                    cmd.Parameters.AddWithValue("@ProjectId", visitorEntry.ProjectId);
                    cmd.Parameters.AddWithValue("@VisitorName", visitorEntry.VisitorName);
                    cmd.Parameters.AddWithValue("@ContactNo", visitorEntry.ContactNo);
                    cmd.Parameters.AddWithValue("@EmailId", visitorEntry.EmailId);
                    cmd.Parameters.AddWithValue("@PurposeId", visitorEntry.PurposeId);
                    cmd.Parameters.AddWithValue("@VisitDate", visitorEntry.VisitDate);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;

        }

        [HttpGet("[action]")]
        public string FillVisitorEntry()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"select V.*, VP.VisitorPurposeName, P.ProjectName from VisitorEntry V
                                left join VisitorPurpose VP on VP.VisitorPurposeId=V.PurposeId
                                left join Project P on P.ProjectId=V.ProjectId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpDelete("[action]/{Id}")]
        public string DeleteVisitorEntry(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.VisitorEntryDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VisitorId", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        //  Rate Master

        [HttpGet("[action]/{RateId}")]
        public string GetOneRateMaster(int RateId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneRateMaster", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RateId", RateId);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string vrifyRateDetail([FromBody] RateMaster RateMaster)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_get.verifyRateDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RateId", RateMaster.RateId);
                    cmd.Parameters.AddWithValue("@ProjectId", RateMaster.ProjectId);
                    cmd.Parameters.AddWithValue("@SectorId", RateMaster.SectorId);
                    cmd.Parameters.AddWithValue("@BlockId", RateMaster.BlockId);
                    cmd.Parameters.AddWithValue("@SegmentId", RateMaster.SegmentId);
                    cmd.Parameters.AddWithValue("@PlotTypeId", RateMaster.PlotTypeId);
                    cmd.Parameters.AddWithValue("@PlotId", RateMaster.PlotId);
                    cmd.Parameters.AddWithValue("@Rate", RateMaster.Rate);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        [HttpPost("[action]")]
        public string SaveRateDetail([FromBody] RateMaster RateMaster)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.RateMasterDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RateId", RateMaster.RateId);
                    cmd.Parameters.AddWithValue("@ProjectId", RateMaster.ProjectId);
                    cmd.Parameters.AddWithValue("@SectorId", RateMaster.SectorId);
                    cmd.Parameters.AddWithValue("@BlockId", RateMaster.BlockId);
                    cmd.Parameters.AddWithValue("@SegmentId", RateMaster.SegmentId);
                    cmd.Parameters.AddWithValue("@PlotTypeId", RateMaster.PlotTypeId);
                    cmd.Parameters.AddWithValue("@Rate", RateMaster.Rate);
                    cmd.Parameters.AddWithValue("@IdCollection", RateMaster.IdCollection);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();

                }
            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillRateDetail()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    Query = @"Select R.RateId, P.ProjectName, S.SectorName, B.BlockName, Sg.SegmentName, PT.PlottypeName
                            ,R.Rate, PD.Area, PD.PlotNo from RateMaster R 
                            left join Project P on P.ProjectId=R.ProjectId
                            left join Sector S on S.SectorId=R.SectorId
                            left join Block B on B.BlockId=R.BlockId
                            left join Segment Sg on Sg.SegmentId=R.SegmentId
                            left join PlotType PT on PT.PlottypeId=R.PlotTypeId
                            left join PlotDetail PD on PD.PlotDetailId=R.PlotId";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }


        // Customer registration

        [HttpGet("[action]/{Code}")]
        public string GetCustomerId(string Code)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneCustomerId", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Code", Code);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{CustomerId}")]
        public string GetOneCustomer(int CustomerId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OneCustomer", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());
                    cmd.Parameters.AddWithValue("@BranchId", HttpContext.Session.GetString("BranchId").ToString());
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveCustomer([FromBody] Customer Customer)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.CustomerDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CustomerId", Customer.CustomerId);
                    cmd.Parameters.AddWithValue("@CustomerCode", Customer.CustomerCode);
                    cmd.Parameters.AddWithValue("@Title", Customer.Title);
                    cmd.Parameters.AddWithValue("@CustomerName", Customer.CustomerName);
                    cmd.Parameters.AddWithValue("@isFather", Customer.isFather);
                    cmd.Parameters.AddWithValue("@FatherHusband", Customer.FatherHusband);
                    cmd.Parameters.AddWithValue("@DOB", Customer.DOB);
                    cmd.Parameters.AddWithValue("@ContactNo", Customer.ContactNo);
                    cmd.Parameters.AddWithValue("@EmailId", Customer.EmailId);
                    cmd.Parameters.AddWithValue("@DOJ", Customer.DOJ);
                    cmd.Parameters.AddWithValue("@MaritalStatus", Customer.MaritalStatus);
                    cmd.Parameters.AddWithValue("@StateId", Customer.StateId);
                    cmd.Parameters.AddWithValue("@CityId", Customer.CityId);
                    cmd.Parameters.AddWithValue("@Address", Customer.Address);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();


                    string image = Customer.Photo;

                    if (image != null)
                    {
                        string EmployeeCode = Customer.CustomerCode;

                        string folderName = "ClientApp/src/assets/Upload/Customer";
                        string webRootPath = _hostingEnvironment.ContentRootPath;
                        string newPath = Path.Combine(webRootPath, folderName);
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }

                        image = image.Substring(image.IndexOf(',') + 1);

                        byte[] imageBytes = Convert.FromBase64String(image);

                        if (imageBytes.Length > 0)
                        {
                            string fullPath = Path.Combine(newPath, EmployeeCode + ".jpg");

                            System.IO.File.WriteAllBytes(fullPath, imageBytes);
                        }
                    }


                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillCustomer()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"Select * from Customer";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpDelete("[action]/{Id}")]
        public string DeleteCustomer(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.CustomerDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CustomerId", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        // EMI Plan

        [HttpGet("[action]/{Id}")]
        public string GetOneEMIPlan(string Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("SP_Get.OneEMIPlan", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlanId", Id);

                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillEMIPlan()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"select * from EMIPlan";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpPost("[action]")]
        public string SaveEMIPlan([FromBody] EMIPlan EMIPlan)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Save.EMIPlanDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlanId", EMIPlan.PlanId);
                    cmd.Parameters.AddWithValue("@PLanCode", EMIPlan.PLanCode);
                    cmd.Parameters.AddWithValue("@PlanValue", EMIPlan.PlanValue);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{Id}")]
        public string DeleteEMIPlan(int Id)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Delete.EMIPlanDetail", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PlanId", Id);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillPayMode()
        {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    Query = @"Select * from PayMode";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }

            }
            catch (Exception Ex)
            {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{EmiId}")]
        public string getEmiValue(int EmiId)
        {
          DataTable dt = new DataTable();
          string JSONresult;
          string Query;
          try
          {
            using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
            {

              Query = @"select isnull(PlanValue,0) as PlanValue, isnull(Duration, 0) as Duration from EMIPlan where PlanId=" + EmiId;

              SqlCommand cmd = new SqlCommand(Query, con);
              SqlDataAdapter da = new SqlDataAdapter();
              cmd.CommandType = CommandType.Text;
              con.Open();
              da.SelectCommand = cmd;
              da.Fill(dt);
              JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
              con.Close();
            }

          }
          catch (Exception Ex)
          {
            dt.Columns.Add(new DataColumn("MessageType"));
            dt.Columns.Add(new DataColumn("Message"));
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = Ex.Message.ToString();

            dt.Rows.Add(dr);
            JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
          }
          return JSONresult;
        }

        // Plot Booking

        [HttpGet("[action]/{BookingId}")]
        public string GetOnePlotBooking(int BookingId)
        {
            DataTable dt = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Get.OnePlotBooking", con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookingId", BookingId);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());
                    cmd.Parameters.AddWithValue("@BranchId", HttpContext.Session.GetString("BranchId").ToString());

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    con.Close();
                }
            }
            catch (Exception Ex)
            {

                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]")]
        public string FillPlotBooking()
        {
          DataTable dt = new DataTable();
          string JSONresult;
          string Query;
          try
          {
            using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
            {

              Query = @"Select * from PlotBooking";

              SqlCommand cmd = new SqlCommand(Query, con);
              SqlDataAdapter da = new SqlDataAdapter();
              cmd.CommandType = CommandType.Text;
              con.Open();
              da.SelectCommand = cmd;
              da.Fill(dt);
              JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
              con.Close();
            }

          }
          catch (Exception Ex)
          {
            dt.Columns.Add(new DataColumn("MessageType"));
            dt.Columns.Add(new DataColumn("Message"));
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = Ex.Message.ToString();

            dt.Rows.Add(dr);
            JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
          }
          return JSONresult;
        }

        [HttpGet("[action]/{PlotId}")]
        public string getActualPlotAmt(int PlotId)
        {
          DataTable dt = new DataTable();
          string JSONresult;
          string Query;
          try
          {
            using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
            {

              Query = @"Select cast(PD.Area*isnull(RM.Rate,0) as Numeric(18,2)) as ActualAmt from RateMaster RM
                        left join PlotDetail PD on PD.PlotDetailId=RM.PlotId where RM.PlotId=" + PlotId;

              SqlCommand cmd = new SqlCommand(Query, con);
              SqlDataAdapter da = new SqlDataAdapter();
              cmd.CommandType = CommandType.Text;
              con.Open();
              da.SelectCommand = cmd;
              da.Fill(dt);
              JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
              con.Close();
            }

          }
          catch (Exception Ex)
          {
            dt.Columns.Add(new DataColumn("MessageType"));
            dt.Columns.Add(new DataColumn("Message"));
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = Ex.Message.ToString();

            dt.Rows.Add(dr);
            JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
          }
          return JSONresult;
        }

        [HttpPost("[action]")]
        public string SavePlotBookingDetail([FromBody] PlotBooking Booking)
            {
              DataTable dt = new DataTable();
              string JSONresult;
              try
              {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {
                  SqlCommand cmd = new SqlCommand("SP_Save.PlotBookingDetail", con);
                  SqlDataAdapter da = new SqlDataAdapter();
                  cmd.CommandType = CommandType.StoredProcedure;

                  cmd.Parameters.AddWithValue("@BookingId", Booking.BookingId);
                  cmd.Parameters.AddWithValue("@CustomerId", Booking.CustomerId);
                  cmd.Parameters.AddWithValue("@EmployeeId", Booking.EmployeeId);
                  cmd.Parameters.AddWithValue("@BookingCode", Booking.BookingCode);
                  cmd.Parameters.AddWithValue("@PlotId", Booking.PlotId);
                  cmd.Parameters.AddWithValue("@ActualPlotAmt", Booking.ActualPlotAmt);
                  cmd.Parameters.AddWithValue("@PayableAmt", Booking.PayableAmt);
                  cmd.Parameters.AddWithValue("@BookingAmt", Booking.BookingAmt);
                  cmd.Parameters.AddWithValue("@BookingDate", Booking.BookingDate);
                  cmd.Parameters.AddWithValue("@PayMode", Booking.PayMode);
                  cmd.Parameters.AddWithValue("@Remark", Booking.Remark);
                  cmd.Parameters.AddWithValue("@EMIPlanId", Booking.EMIPlanId);
                  cmd.Parameters.AddWithValue("@EMIAmt", Booking.EMIAmt);
                  cmd.Parameters.AddWithValue("@FirstEMIDate", Booking.FirstEMIDate);
                  cmd.Parameters.AddWithValue("@isUpdate", Booking.isUpdate);
                  cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserId").ToString());

                    
                  con.Open();
                  cmd.CommandType = CommandType.StoredProcedure;
                  da.SelectCommand = cmd;
                  da.Fill(dt);

                    if (dt.Rows[0]["MessageType"].ToString() == "1")
                    {

                        JSONresult = getBookingReceipt(Booking.BookingCode, dt.Rows[0]["Message"].ToString());

                    }
                    else {
                        JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                    }
                    
                  con.Close();
                }

              }
              catch (Exception Ex)
              {
                dt.Columns.Add(new DataColumn("MessageType"));
                dt.Columns.Add(new DataColumn("Message"));
                DataRow dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                dt.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
              }
              return JSONresult;
            }

        public string getBookingReceipt(string BookingCode, string Message) {
            DataTable dt = new DataTable();
            DataTable DT = new DataTable();
            string JSONresult;
            try
            {
                using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
                {

                    string Query = "select * from BookingReceipt where BookingCode='"+ BookingCode +"'";

                    SqlCommand cmd = new SqlCommand(Query, con);
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt);

                    ReportDocument repdoctempledger = new ReportDocument();

                    string folderName = "Real/LoanReceiptRpt.rpt";
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);

                    repdoctempledger.Load(newPath);

                    repdoctempledger.SetDataSource(dt);
                    repdoctempledger.SetDatabaseLogon("sa", "master", "lenovo", "Bank");

                    string tmpPath = Path.Combine(webRootPath, "TempPdf");

                    DirectoryInfo dirInfo = new DirectoryInfo(tmpPath);
                    if (!dirInfo.Exists)
                    {
                        dirInfo.Create();
                    }
                    string FileName = "PolicyReceipt.pdf";
                    string FileFullName = dirInfo.FullName + "/" + FileName;
                    repdoctempledger.ExportToDisk(ExportFormatType.PortableDocFormat, FileFullName);
                    FileInfo PdfFile = new FileInfo(FileFullName);
                    repdoctempledger.Close();
                    repdoctempledger.Dispose();

                    if (PdfFile.Exists)
                    {
                        DT.Columns.Add(new DataColumn("MessageType"));
                        DT.Columns.Add(new DataColumn("Message"));
                        DT.Columns.Add(new DataColumn("FilePath"));
                        DataRow dr = DT.NewRow();
                        dr[0] = "1";
                        dr[1] = Message;
                        dr[2] = "TempPdf/" + PdfFile.Name;


                        DT.Rows.Add(dr);
                        JSONresult = JsonConvert.SerializeObject(DT, Formatting.Indented);
                    }
                    else
                    {
                        DT.Columns.Add(new DataColumn("MessageType"));
                        DT.Columns.Add(new DataColumn("Message"));
                        DataRow dr = DT.NewRow();
                        dr[0] = "0";
                        dr[1] = "File Not Exist";

                        DT.Rows.Add(dr);
                        JSONresult = JsonConvert.SerializeObject(DT, Formatting.Indented);
                    }
                    
                    con.Close();
                }
            }
            catch (Exception Ex)
            {
                DT.Columns.Add(new DataColumn("MessageType"));
                DT.Columns.Add(new DataColumn("Message"));
                DataRow dr = DT.NewRow();
                dr[0] = "0";
                dr[1] = Ex.Message.ToString();

                DT.Rows.Add(dr);
                JSONresult = JsonConvert.SerializeObject(DT, Formatting.Indented);
            }
            return JSONresult;
        }

        [HttpGet("[action]/{BlockId}/{isBooked}/{isUpdate}")]
        public string FillActivePlotAndSelf(int BlockId, int isBooked, int isUpdate)
          {
            DataTable dt = new DataTable();
            string JSONresult;
            string Query;
            try
            {
              using (SqlConnection con = new SqlConnection(Db.Database.GetDbConnection().ConnectionString))
              {
                Query = @"select PlotDetailId, PlotNo from PlotDetail where isnull(isBooked,0)=" + isBooked + " and BlockId=" + BlockId + @" UNION select PlotDetailId, PlotNo from PlotDetail where PlotDetailId="+ isUpdate;

                SqlCommand cmd = new SqlCommand(Query, con);
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                con.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
                JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
                con.Close();
              }

            }
            catch (Exception Ex)
            {
              dt.Columns.Add(new DataColumn("MessageType"));
              dt.Columns.Add(new DataColumn("Message"));
              DataRow dr = dt.NewRow();
              dr[0] = "0";
              dr[1] = Ex.Message.ToString();

              dt.Rows.Add(dr);
              JSONresult = JsonConvert.SerializeObject(dt, Formatting.Indented);
            }
            return JSONresult;
          }

        
  }
}
