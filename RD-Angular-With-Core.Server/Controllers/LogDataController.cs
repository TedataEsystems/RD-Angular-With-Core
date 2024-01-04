using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RD.API.ViewModels;
using RD.Domain.Entities;
using RD.Services;

namespace RD_Angular_Core.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogDataController : ControllerBase
    {
        private readonly ILogDataServices LogDataManager;
        private readonly IMapper Mapper;
        public LogDataController(ILogDataServices LogDataManage, IMapper Mapper)
        {
            this.LogDataManager = LogDataManage;
            this.Mapper = Mapper;
        }

        [HttpGet]
        public IActionResult Get([FromHeader] PaginationViewModel model)
        {
            try
            {
                model.pagesize = model.pagesize == null || model.pagesize == 0 ? 25 : model.pagesize;
                model.pagenumber = model.pagenumber == null || model.pagenumber == 0 ? 1 : model.pagenumber;
                var LogDatas = LogDataManager.Get();
                if (!string.IsNullOrEmpty(model.attributeName))
                {
                    LogDatas = LogDatas.Where(p => p.TableName.ToLower() == model.attributeName.ToLower());
                }
                if (!string.IsNullOrEmpty(model.searchvalue))
                {
                    model.searchvalue = model.searchvalue.ToLower().Trim();
                    LogDatas = LogDatas.Where(p => p.Id.ToString().Contains(model.searchvalue)
                    || (p.UserName != null && p.UserName.ToLower().Contains(model.searchvalue))
                    || (p.UserGroup != null && p.UserGroup.ToLower().Contains(model.searchvalue))
                    || (p.ActionType != null && p.ActionType.ToLower().Contains(model.searchvalue))
                    || (p.TableName != null && p.TableName.ToLower().Contains(model.searchvalue))
                    || (p.Desciption != null && p.Desciption.ToLower().Contains(model.searchvalue))
                    || (p.CreationDate != null && p.CreationDate.ToString().ToLower().Contains(model.searchvalue))
                    || (p.RD_Id != null && p.RD_Id.ToString().ToLower().Contains(model.searchvalue))
                    );
                }

                int TotalRecords = LogDatas.Count();
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
                //    LogDatas = LogDatas.AsQueryable().OrderBy(model.sortcolumn + " " + model.sortcolumndir);
                //}

                List<LogData> _LogDatas = LogDatas.ToList().Skip((model.pagenumber.Value - 1) * model.pagesize.Value).Take(model.pagesize.Value).ToList();
                var allLogDatas = Mapper.Map<List<LogData>, List<LogDataViewModel>>(_LogDatas);
                return Ok(new
                {
                    status = true,
                    result = new { data = _LogDatas, totalrecords = TotalRecords, totalpages = TotalPages, sortcolumn = model.sortcolumn, sortcolumndir = model.sortcolumndir }
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
                LogData LogData = LogDataManager.GetById(id);
                if (LogData != null)
                {
                    var item = Mapper.Map<LogData, LogDataViewModel>(LogData);
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
        public IActionResult Post([FromBody] LogDataViewModel model)
        {
            try
            {
                LogData LogData = Mapper.Map<LogDataViewModel, LogData>(model);
                LogData.CreationDate = DateTime.Now;
                LogDataManager.Add(LogData);
                model = Mapper.Map<LogData, LogDataViewModel>(LogData);

                return Ok(new { status = true, data = model });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] LogDataViewModel model)
        {
            try
            {
                LogData LogData = LogDataManager.GetById(model.Id);
                if (LogData != null)
                {
                    LogData = Mapper.Map<LogDataViewModel, LogData>(model);
                    LogData.ModificationDate = DateTime.Now;
                    LogDataManager.Update(LogData);

                    model = Mapper.Map<LogData, LogDataViewModel>(LogData);
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

        [HttpDelete]
        public IActionResult Delete([FromBody] LogDataViewModel model)
        {
            try
            {
                LogData LogData = LogDataManager.GetById(model.Id);
                if (LogData != null)
                {
                    LogDataManager.Delete(LogData);

                    var item = Mapper.Map<LogData, LogDataViewModel>(LogData);
                    return Ok(new { status = true, data = item });
                }
                return Ok(new { status = false, error = "Invalid Id" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, error = ex.Message });
            }
        }

    }

}
