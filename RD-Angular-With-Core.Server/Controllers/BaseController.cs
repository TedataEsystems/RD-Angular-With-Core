using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RD.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD_Angular_Core.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public BaseController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
    }
}
