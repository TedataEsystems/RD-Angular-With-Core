using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RD.API.ViewModels
{
    public class UsersViewModel
    {
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyiedBy { get; set; } //
        public string CreatedByTeam { get; set; }
        public string ModifyiedByTeam { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }
        public string userName { get; set; }
        public int? roleID { get; set; }
    }
}
