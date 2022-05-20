using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO_Models
{
    public class BookBasket
    {
        public Book Book { get; set; }
        public int Count { get; set; }
    }
}
