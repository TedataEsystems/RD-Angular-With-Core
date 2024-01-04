using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RD.Domain.Identity;

namespace RD.Domain.Base
{
    public abstract class BaseEntity 
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifyiedBy { get; set; }
        public string? CreatedByTeam { get; set; }
        public string? ModifyiedByTeam { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        protected BaseEntity()
        {
            CreationDate = DateTime.Now.ToLocalTime();
            IsEnabled = true;
            IsDeleted = false;
        }
    }
}
