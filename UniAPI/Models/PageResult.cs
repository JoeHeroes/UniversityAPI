using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniAPI.Models
{
    public class PageResult<T>
    {

        public List<T> Item { get; set; }
        public int TotalPages { get; set; }
        public int ItemForm { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }
        public PageResult(List<T> item, int totalCounts, int pageSize, int pageNumber)
        {
            Item = item;
            TotalItemsCount = totalCounts;
            ItemForm = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemForm + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCounts/(double)pageSize);
        }

       
    }
}
