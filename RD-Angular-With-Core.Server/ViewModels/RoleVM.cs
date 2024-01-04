using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RD.API.ViewModels
{
    public class RoleVM
    {
        [Required(ErrorMessage = "Role Name is required")]
        public string Rolename { get; set; }


    }
}
