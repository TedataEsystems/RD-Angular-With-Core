using RD.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RD.Domain.Entities
{
    public class Roles : BaseEntity
    {
     
        public string Title { get; set; }
    }
}
