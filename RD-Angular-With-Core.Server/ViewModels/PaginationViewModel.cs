using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD.API.ViewModels
{
    public class PaginationViewModel
    {
        public int? pagesize { get; set; }
        public int? pagenumber { get; set; }
        public string searchvalue { get; set; }
        public string sortcolumn { get; set; }
        public int? servicetype { get; set; }
        public string sortcolumndir { get; set; }
        public string attributeName { get; set; }

    }
}
