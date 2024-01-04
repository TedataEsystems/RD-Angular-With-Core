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
using ClosedXML.Excel;
using RD.Services;
using Microsoft.AspNetCore.Authorization;

namespace RD_Angular_Core.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RDDataController : Controller
    {
      

            private readonly IRDDataServices RDDataManager;
            private readonly ILogDataServices LogManager;
            private readonly IMapper Mapper;
            public RDDataController(IRDDataServices RDDataManage, ILogDataServices LogManager, IMapper Mapper)
            {
                this.RDDataManager = RDDataManage;
                this.LogManager = LogManager;
                this.Mapper = Mapper;
            }

            [HttpGet]
            public IActionResult Get([FromHeader] PaginationViewModel model)
            {
                try
                {


                    model.pagenumber = model.pagenumber == null || model.pagenumber == 0 ? 1 : model.pagenumber;
                    var requests = RDDataManager.Get(x => x.IsFree == false);

                    if (!string.IsNullOrEmpty(model.searchvalue))
                    {
                        model.searchvalue = model.searchvalue.ToLower().Trim();
                        requests = requests.Where(p => p.Id.ToString().Contains(model.searchvalue)

                        || (p.VRFName != null && p.VRFName.ToLower().Contains(model.searchvalue))
                        || (p.CustomerName != null && p.CustomerName.ToLower().Contains(model.searchvalue))
                        || (p.RD_Id != null && p.RD_Id.ToString().ToLower().Contains(model.searchvalue))
                        || (p.CreatedBy != null && p.CreatedBy.ToLower().Contains(model.searchvalue))
                        || (p.CreationDate != null && p.CreationDate.ToString().ToLower().Contains(model.searchvalue))
                        || (p.ModifyiedBy != null && p.ModifyiedBy.ToLower().Contains(model.searchvalue))
                        || (p.ModificationDate != null && p.ModificationDate.ToString().ToLower().Contains(model.searchvalue))
                        );
                    }


                    int TotalRecords = requests.Count();
                    if (TotalRecords == 0)
                        TotalRecords = 1;
                    model.pagesize = model.pagesize == null || model.pagesize == 0 ? TotalRecords : model.pagesize;
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
                    requests = requests.OrderBy(x => x.RD_Id);
                //    if (!(string.IsNullOrEmpty(model.sortcolumn) && string.IsNullOrEmpty(model.sortcolumndir)))
                //    {
                //    requests = requests.AsQueryable().OrderBy(model.sortcolumn + " " + model.sortcolumndir);
                //}

                List<RDData> _requests = requests.ToList().Skip((model.pagenumber.Value - 1) * model.pagesize.Value).Take(model.pagesize.Value).ToList();
                    var allrequests = Mapper.Map<List<RDData>, List<RDDataVM>>(_requests);
                    //allrequests = allrequests.OrderBy(p => p.RD_Id).ToList();

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
                    RDData RDData = RDDataManager.GetById(id);
                    if (RDData != null)
                    {
                        var item = Mapper.Map<RDData, RDDataVM>(RDData);
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
            public IActionResult Post([FromBody] RDDataVM model)
            {
                try
                {
                    var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                    var userGroup = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userGroup")?.Value;
                RDData RDData = Mapper.Map<RDDataVM, RDData>(model);
                RDData.CreationDate = DateTime.Now;
                RDData.CreatedBy = userName;
                RDData.CreatedByTeam = userGroup;
                var data = RDDataManager.Get(x => x.IsFree == true).OrderBy(p => p.RD_Id).FirstOrDefault();
                    var vrfData = RDDataManager.Get(x => x.VRFName == model.VRFName).ToList();
                    foreach (var item in vrfData)
                    {
                        if (item.VRFName == model.VRFName)
                        {
                            return Ok(new { status = false, error = "This VRF Name already exists.Please try with a different VRF Name" });
                        }

                    }

                    if (data != null)
                    {
                        data.CreationDate = DateTime.Now;
                        data.CreatedBy = userName;
                        data.CreatedByTeam = userGroup;
                        data.IsFree = false;
                        data.CustomerName = model.CustomerName;
                        data.VRFName = model.VRFName;
                        RDDataManager.Update(data);
                    }
                    else
                    {
                        return Ok(new { status = false, error = "There are no free ports. Please add new range" });
                    }
                    //RDDataManager.Add(RDData);
                    model = Mapper.Map<RDData, RDDataVM>(data);
                    #region Log
                    LogData Log = new LogData();
                    Log.TableName = "RDData";
                    Log.UserGroup = userGroup;
                    Log.UserName = userName;
                    Log.ActionType = "Add";
                    Log.CreationDate = DateTime.Now;
                    Log.RD_Id = data.RD_Id;
                    Log.Desciption =
                    "Id: " + data.Id + ", " +
                    "Customer Name: " + (data.CustomerName == null ? "" : data.CustomerName) + ", " +
                    "VRF Name: " + (data.VRFName == null ? "" : data.VRFName) + ", " +
                    "RD_Id: " + (data.RD_Id == null ? "" : data.RD_Id.Value.ToString()) + ", " +
                    "IsFree: " + (data.IsFree == null ? "" : data.IsFree.Value.ToString());
                    LogManager.Add(Log);

                    #endregion
                    return Ok(new { status = true, data = model });
                }
                catch (Exception ex)
                {
                    return Ok(new { status = false, error = ex.Message });
                }
            }
            [HttpPost("addrange")]
            public IActionResult PostRange([FromBody] RangeVM model)
            {
                try
                {
                    int count = model.max - model.min;
                    IEnumerable<int> squares = Enumerable.Range(model.min, count + 1);
                    //RDData RDData = new RDData();
                    List<RDData> dataList = new List<RDData>();
                    bool isAvailable = false;
                    int rdID = 0;
                    var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                    var userGroup = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userGroup")?.Value;
                    foreach (int num in squares)
                    {
                        var RDData = new RDData();
                        RDData.CreationDate = DateTime.Now;
                        RDData.CreatedBy = userName;
                        RDData.CreatedByTeam = userGroup;
                        RDData.IsFree = true;
                        RDData.CustomerName = "";
                        RDData.VRFName = "";
                        RDData.RD_Id = num;
                        dataList.Add(RDData);
                        var availableRD = RDDataManager.Get(x => x.RD_Id == num).ToList();
                        if (availableRD.Count > 0)
                        {
                            isAvailable = true;
                            rdID = num;

                        }
                        //RDDataManager.Add(RDData);
                    }
                    if (isAvailable)
                    {
                        return Ok(new { status = false, error = "The RD_ID " + rdID + "  already exists " });
                    }
                    else
                    {
                        RDDataManager.AddRange(dataList);
                    }
                    #region Log
                    LogData Log = new LogData();
                    Log.TableName = "RDData";
                    Log.UserGroup = userGroup;
                    Log.UserName = userName;
                    Log.ActionType = "Add";
                    Log.CreationDate = DateTime.Now;
                    Log.RD_Id = 0;
                    Log.Desciption =
                    "Range RD ID From: " + model.min + ", " +
                    "To: " + model.max;


                    LogManager.Add(Log);

                    #endregion
                    return Ok(new { status = true, data = "Added Successfully" });
                }
                catch (Exception ex)
                {
                    return Ok(new { status = false, error = ex.Message });
                }
            }
            [HttpPost("CreateCustomerRange")]
            public IActionResult CreateCustomerRange([FromBody] RangeVM model)
            {
                try
                {
                    int count = model.max - model.min;
                    IEnumerable<int> squares = Enumerable.Range(model.min, count + 1);
                    //RDData RDData = new RDData();
                    List<RDData> dataList = new List<RDData>();

                    var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                    var userGroup = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userGroup")?.Value;
                    var vrfData = RDDataManager.Get(x => x.VRFName == model.VRFName).ToList();
                    foreach (var item in vrfData)
                    {
                        if (item.VRFName == model.VRFName)
                        {
                            return Ok(new { status = false, error = "This VRF Name already exists.Please try with a different VRF Name" });
                        }

                    }
                    foreach (int num in squares)
                    {
                        var data = RDDataManager.Get(x => x.RD_Id == num && x.IsFree == true).FirstOrDefault();

                        if (data != null)
                        {
                            data.CreationDate = DateTime.Now;
                            data.CreatedBy = userName;
                            data.CreatedByTeam = userGroup;
                            data.IsFree = false;
                            data.CustomerName = model.CustomerName;
                            data.VRFName = model.VRFName;
                            RDDataManager.Update(data);
                        }
                        else
                        {
                            return Ok(new { status = false, error = "RD ID is not free or is wrong" });
                        }

                    }

                    #region Log
                    LogData Log = new LogData();
                    Log.TableName = "RDData";
                    Log.UserGroup = userGroup;
                    Log.UserName = userName;
                    Log.ActionType = "Add";
                    Log.CreationDate = DateTime.Now;
                    Log.RD_Id = 0;
                    Log.Desciption =
                    "Range RD ID From: " + model.min + ", " +
                    "To: " + model.max + ", " +
                    "Customer Name: " + (model.CustomerName == null ? "" : model.CustomerName) + ", " +
                    "VRF Name: " + (model.VRFName == null ? "" : model.VRFName);


                    LogManager.Add(Log);

                    #endregion
                    return Ok(new { status = true, data = "Added Successfully" });
                }
                catch (Exception ex)
                {
                    return Ok(new { status = false, error = ex.Message });
                }
            }
            [HttpPost("UpdateRDData")]
            public IActionResult UpdateRDData([FromBody] RDDataVM model)
            {
                try
                {
                    var RDData = RDDataManager.GetById(model.Id);
                    if (RDData != null)
                    {
                        var sermodel = Mapper.Map<RDDataVM, RDData>(model);
                        var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                        var userGroup = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userGroup")?.Value;
                        //RDDataManager.GetEditRDData(RDData, sermodel);

                        RDData.ModificationDate = DateTime.Now;
                        RDData.ModifyiedBy = userName;
                        RDData.ModifyiedByTeam = userGroup;
                        RDDataManager.Update(RDData);
                        #region Log
                        LogData Log = new LogData();
                        Log.TableName = "RDData";
                        Log.UserGroup = userGroup;
                        Log.UserName = userName;
                        Log.ActionType = "Update";
                        Log.CreationDate = DateTime.Now;
                        Log.RD_Id = RDData.RD_Id;
                        Log.Desciption =
                        "Id: " + RDData.Id + ", " +
                        "Customer Name: " + (RDData.CustomerName == null ? "" : RDData.CustomerName) + ", " +
                        "VRF Name: " + (RDData.VRFName == null ? "" : RDData.VRFName) + ", " +
                        "RD_Id: " + (RDData.RD_Id == null ? "" : RDData.RD_Id.Value.ToString()) + ", " +
                        "IsFree: " + (RDData.IsFree == null ? "" : RDData.IsFree.Value.ToString());
                        LogManager.Add(Log);

                        #endregion
                        model = Mapper.Map<RDData, RDDataVM>(RDData);
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
            [HttpGet("Export")]

            public IActionResult Export()
            {
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[8] { new DataColumn("RD_Id"),
                                        new DataColumn("CustomerName"),
                                        new DataColumn("VRFName"),
                                         new DataColumn("IsFree"),
                                        new DataColumn("CreationDate"),
                                        new DataColumn("CreatedBy"),
                                       new DataColumn("ModifiedDate"),
                                        new DataColumn("ModifiedBy")
            });

                var DataList = RDDataManager.Get(b => b.IsFree == false).OrderBy(p => p.RD_Id).ToList();

                foreach (var data in DataList)
                {
                    dt.Rows.Add(data.RD_Id, data.CustomerName, data.VRFName, data.IsFree, data.CreationDate.HasValue ? data.CreationDate.Value.ToString("dd MMMM yyyy h:mm tt") : " ", data.CreatedBy, data.ModificationDate.HasValue ? data.ModificationDate.Value.ToString("dd MMMM yyyy h:mm tt") : " ", data.ModifyiedBy);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                    }
                }
            }
            [HttpGet("ExportFree")]
            public IActionResult ExportFree()
            {
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[8] { new DataColumn("RD_Id"),
                                        new DataColumn("CustomerName"),
                                        new DataColumn("VRFName"),
                                         new DataColumn("IsFree"),
                                        new DataColumn("CreationDate"),
                                        new DataColumn("CreatedBy"),
                                       new DataColumn("ModifiedDate"),
                                        new DataColumn("ModifiedBy")
            });

                var DataList = RDDataManager.Get(b => b.IsFree == true).OrderBy(p => p.RD_Id).ToList();

                foreach (var data in DataList)
                {
                    dt.Rows.Add(data.RD_Id, data.CustomerName, data.VRFName, data.IsFree, data.CreationDate.HasValue ? data.CreationDate.Value.ToString("dd MMMM yyyy h:mm tt") : " ", data.CreatedBy, data.ModificationDate.HasValue ? data.ModificationDate.Value.ToString("dd MMMM yyyy h:mm tt") : " ", data.ModifyiedBy);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                    }
                }
            }

            [HttpGet("RemoveRDData/{id}")]
            public IActionResult RemoveRDData([FromRoute] int id)
            {
                try
                {

                    RDData RDData = RDDataManager.GetById(id);
                    if (RDData != null)
                    {
                        var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
                        var userGroup = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userGroup")?.Value;
                        RDData.IsFree = true;
                        RDData.CustomerName = "";
                        RDData.VRFName = "";
                        RDData.ModificationDate = null;
                        RDData.ModifyiedBy = "";
                        RDDataManager.Update(RDData);
                        #region Log
                        LogData Log = new LogData();
                        Log.TableName = "RDData";
                        Log.UserGroup = userGroup;
                        Log.UserName = userName;
                        Log.ActionType = "Delete";
                        Log.CreationDate = DateTime.Now;
                        Log.RD_Id = RDData.RD_Id;
                        Log.Desciption =
                        "Id: " + RDData.Id + ", " +
                        "Customer Name: " + (RDData.CustomerName == null ? "" : RDData.CustomerName) + ", " +
                        "VRF Name: " + (RDData.VRFName == null ? "" : RDData.VRFName) + ", " +
                        "RD_Id: " + (RDData.RD_Id == null ? "" : RDData.RD_Id.Value.ToString()) + ", " +
                        "IsFree: " + (RDData.IsFree == null ? "" : RDData.IsFree.Value.ToString());
                        LogManager.Add(Log);

                        #endregion
                        var item = Mapper.Map<RDData, RDDataVM>(RDData);
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

