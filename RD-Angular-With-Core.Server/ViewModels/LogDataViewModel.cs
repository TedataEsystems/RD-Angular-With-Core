using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RD.API.ViewModels
{
    public class LogDataViewModel
    {
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ActionType { get; set; }
        public string Desciption { get; set; }
        public string TableName { get; set; }
        public string UserName { get; set; }
        public string UserGroup { get; set; }
        public int? RD_Id { get; set; }
    }
}
