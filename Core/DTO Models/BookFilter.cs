using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO_Models
{
    public class BookFilter
    {
        public string Author { get; set; }

        public string Genre { get; set; }

        public decimal StartPrice { get; set; }

        public decimal EndPrice { get; set; }
    }
}
