using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RD.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System;
using RD.API.ViewModels;
using SampleProject.Services;
using RD.Services;
using Microsoft.AspNetCore.Identity;

namespace RD_Angular_Core.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersServices UsersManager;
        private readonly ILogDataServices LogManager;
        private readonly IMapper Mapper;
        public UsersController(IUsersServices UsersManage, ILogDataServices LogManager, IMapper Mapper)
        {
            this.UsersManager = UsersManage;
            this.LogManager = LogManager;
            this.Mapper = Mapper;
        }
        [HttpGet]
        public IActionResult Get([FromHeader] PaginationViewModel model)
        {
            try
            {

                model.pagesize = model.pagesize == null || model.pagesize == 0 ? 25 : model.pagesize;
                model.pagenumber = model.pagenumber == null || model.pagenumber == 0 ? 1 : model.pagenumber;
                var UsersList = UsersManager.Get(p => p.Roles).ToList();

                if (!string.IsNullOrEmpty(model.attributeName))
                {
                    try
                    {
                        int requestid = Convert.ToInt32(model.attributeName);
                        UsersList = UsersList.Where(p => p.roleID == requestid).ToList();
                    }
                    catch
                    {

                        return Ok(new { status = false, error = "Invalid Roles Id" });
                    }
                }
                if (!string.IsNullOrEmpty(model.searchvalue))
                {
                    model.searchvalue = model.searchvalue.ToLower().Trim();
                    UsersList = UsersList.Where(p => p.Id.ToString().Contains(model.searchvalue)
                    || (p.userName != null && p.userName.ToLower().Contains(model.searchvalue))
                    || (p.roleID != null && p.Roles != null && p.Roles.Id.ToString().Contains(model.searchvalue))
                    || (p.CreatedBy != null && p.CreatedBy.ToLower().Contains(model.searchvalue))
                    || (p.CreationDate != null && p.CreationDate.ToString().ToLower().Contains(model.searchvalue))
                    || (p.ModifyiedBy != null && p.ModifyiedBy.ToLower().Contains(model.searchvalue))
                    || (p.ModificationDate != null && p.ModificationDate.ToString().ToLower().Contains(model.searchvalue))
                    ).ToList();
                }

                int TotalRecords = UsersList.Count();
                int TotalPages = TotalRecords / model.pagesize.Value;
                double ieee = Math.IEEERemainder(TotalRecords, model.pagesize.Value);
                if (ieee >= 1)
                {
                    TotalPages++;
                }
                if (TotalPages == 0 && TotalRecords > 1)
                {
                    TotalPages = 1;
                }

                //if (!(string.IsNullOrEmpty(model.sortcolumn) && string.IsNullOrEmpty(model.sortcolumndir)))
                //{
                //    UsersList = UsersList.AsQueryable().OrderBy(model.sortcolumn + " " + model.sortcolumndir).ToList();
                //}
                List<Users> _requests = UsersList.Skip((model.pagenumber.Value - 1) * model.pagesize.Value).Take(model.pagesize.Value).ToList();
                var allrequests = Mapper.Map<List<Users>, List<UsersViewModel>>(_requests);

                return Ok(new
                {
                    status = true,
                    result = new { data = allrequests, totalrecords = TotalRecords, totalpages = TotalPages, sortcolumn = model.sortcolumn, sortcolumndir = model.sortcolumndir }
                });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, error = ex.Message });
            }

        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Users Users = UsersManager.GetById(id);
                if (Users != null)
                {
                    var item = Mapper.Map<Users, UsersViewModel>(Users);
                    return Ok(new { status = true, data = item });
                }
                return Ok(new { status = false, error = "Invalid Id" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, error = ex.Message });
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] UsersViewModel model)
        {
            try
            {
                var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                var userGroup = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userGroup")?.Value;
                Users Users = Mapper.Map<UsersViewModel, Users>(model);
                Users.CreationDate = DateTime.Now;
                Users.CreatedBy = userName;
                Users.CreatedByTeam = userGroup;
                UsersManager.Add(Users);
                model = Mapper.Map<Users, UsersViewModel>(Users);
                //var UserLogs = UsersManager.Get(a => a.Id == Users.Id, a => a.Roles).FirstOrDefault();
                #region Log
                LogData Log = new LogData();
                Log.TableName = "Users";
                Log.UserGroup = userGroup;
                Log.UserName = userName;
                Log.ActionType = "Add";
                Log.CreationDate = DateTime.Now;
                Log.Desciption =
                    "Id: " + Users.Id + ", " +
                    "UserName: " + (Users.userName == null ? "" : Users.userName) + ", ";
                //+
                //    "Role: " + (Users.Roles.Title == null ? "" : Users.Roles.Title);
                LogManager.Add(Log);

                #endregion
                return Ok(new { status = true, data = model });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, error = ex.Message });
            }
        }

        [HttpPost("{UpdateUsers}")]
        public IActionResult UpdateUsers([FromBody] UsersViewModel model)
        {
            try
            {
                var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                var userGroup = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userGroup")?.Value;
                Users Users = UsersManager.GetById(model.Id);
                if (Users != null)
                {
                    // Users = Mapper.Map<UsersViewModel, Users>(model);
                    if (Users.userName != model.userName)
                    {
                        Users.userName = model.userName;
                    }
                    if (Users.roleID != model.roleID)
                    {
                        Users.roleID = model.roleID;
                    }

                    Users.ModifyiedByTeam = userGroup;
                    Users.ModifyiedBy = userName;
                    Users.ModificationDate = DateTime.Now;
                    UsersManager.Update(Users);
                    //var UserLogs = UsersManager.Get(a => a.Id == Users.Id, a => a.Roles).FirstOrDefault();
                    #region Log
                    LogData Log = new LogData();
                    Log.TableName = "Users";
                    Log.UserGroup = userGroup;
                    Log.UserName = userName;
                    Log.ActionType = "Edit";
                    Log.CreationDate = DateTime.Now;
                    Log.Desciption =
                     "Id: " + Users.Id + ", " +
                     "UserName: " + (Users.userName == null ? "" : Users.userName) + ", ";
                    // +
                    //"Role: " + (Users.Roles.Title == null ? "" : Users.Roles.Title);
                    LogManager.Add(Log); ;


                    #endregion
                    model = Mapper.Map<Users, UsersViewModel>(Users);
                    return Ok(new { status = true, data = model });
                }
                else
                {
                    return Ok(new { status = false, error = "Invalid Data" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, error = ex.Message });
            }

        }

        [HttpGet("{RemoveUsers}/{id}")]
        public IActionResult RemoveUsers([FromRoute] int id)
        {
            try
            {
                Users Users = UsersManager.GetById(id);
                if (Users != null)
                {
                    var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                    var userGroup = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userGroup")?.Value;
                    //var UserLogs = UsersManager.Get(a => a.Id == id, a => a.Roles).FirstOrDefault();
                    UsersManager.Delete(Users);
                    #region Log
                    LogData Log = new LogData();
                    Log.TableName = "Users";
                    Log.UserGroup = userGroup;
                    Log.UserName = userName;
                    Log.ActionType = "Delete";
                    Log.CreationDate = DateTime.Now;
                    Log.Desciption =
                       "Id: " + Users.Id + ", " +
                       "UserName: " + (Users.userName == null ? "" : Users.userName) + ", ";
                      // +
                    //"Role: " + (Users.Roles.Title == null ? "" : Users.Roles.Title);
                    LogManager.Add(Log);


                    #endregion
                    var item = Mapper.Map<Users, UsersViewModel>(Users);
                    return Ok(new { status = true, data = item });
                }
                else
                {
                    return Ok(new { status = false, error = "Invalid Data" });
                }

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, error = ex.Message });
            }
        }
    }
}
