using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleEShop.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public int ItemsCount => Items?.Sum(p => p.Quantity) ?? 0;
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
