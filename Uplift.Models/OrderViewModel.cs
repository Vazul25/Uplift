using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.Models
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader{ get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
