using System;
using System.Collections.Generic;
using System.Text;
using ConsoleEShop.Models;

namespace ConsoleEShop
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public List<CartItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        New=1,
        CanceledByAdministrator=2,
        PaymentRecieved=3,
        Sent=4,
        Recieved=5,
        Finished=6,
        CanceledByUser=7
    }
}
