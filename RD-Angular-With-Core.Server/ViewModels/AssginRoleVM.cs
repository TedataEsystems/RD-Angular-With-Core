using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RD.API.ViewModels
{
    public class AssginRoleVM
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        public string RoleId { get; set; }


    }
}
