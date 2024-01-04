using RD.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RD.Domain.Entities
{
    public class RDData : BaseEntity
    {
        public string CustomerName { get; set; }
        public string VRFName { get; set; }
        public int? RD_Id { get; set; }
        public bool? IsFree { get; set; }
    }
}
