using RD.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RD.Domain.Entities
{
    public class LogData : BaseEntity
    {
        public string? ActionType { get; set; }
        public string? Desciption { get; set; }
        public string? TableName { get; set; }
        public string? UserName { get; set; }
        public string? UserGroup { get; set; }
        public int? RD_Id { get; set; }
    }
}
