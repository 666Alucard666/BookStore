using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO_Models
{
    public class DeleteOrderRequest
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
    }
}
