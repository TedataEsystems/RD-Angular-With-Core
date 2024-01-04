using System;

namespace RD.API.ViewModels
{
    public class RDDataVM
    {
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string CreatedBy { get; set; }
        public string UserGroup { get; set; }
        public string ModifyiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }
        public string CreatedByTeam { get; set; }
        public string ModifyiedByTeam { get; set; }
        public string CustomerName { get; set; }
        public string VRFName { get; set; }
        public int? RD_Id { get; set; }
        public bool? IsFree { get; set; }
    }
}
