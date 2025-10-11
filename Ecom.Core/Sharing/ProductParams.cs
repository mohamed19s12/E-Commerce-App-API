using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
    public class ProductParams
    {
        //int? CategoryId , int PageNumber , int PageSize, string? sort = null
        public int? CategoryId { get; set; }
        public string? sort { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int MaxPageSize { get; set; } = 6;
        private int _pageSize = 3;

        public int pagesize
        {
            get { return _pageSize = 3; }
            set { _pageSize = value>MaxPageSize ? MaxPageSize : value; }
        }

    }
}
