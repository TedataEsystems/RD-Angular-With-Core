using RD.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RD.Domain.Entities
{
    public class Users : BaseEntity
    {
   
        public string? userName { get; set; }
        [ForeignKey("Roles")]
        public int? roleID { get; set; }
        public virtual Roles Roles { get; set; }
    }
}
