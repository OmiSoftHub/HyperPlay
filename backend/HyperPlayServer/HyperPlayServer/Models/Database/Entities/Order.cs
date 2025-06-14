﻿using System.Text.Json.Serialization;
using HyperPlayServer.Models.Database.Entities.Enuml;

namespace HyperPlayServer.Models.Database.Entities;

// clase order
public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TotalPrice { get; set; }
    public PayMode ModeOfPay { get; set; }
    public DateTime BillingDate { get; set; }

    public List<OrderDetail> OrderDetails { get; set; }

    public User User { get; set; }

    public Order()
    {
        OrderDetails = new List<OrderDetail>();
    }
}
