using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Utils
{
    public class Pagination
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Order { get; set; }

        public bool Desc { get; set; }
    }
}
