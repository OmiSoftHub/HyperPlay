﻿using HyperPlayServer.Models.Database.Entities;
using HyperPlayServer.Models.Database.Entities.Enuml;

namespace HyperPlayServer.Models.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime BillingDate { get; set; }
    public List<OrderDetailDto> OrderGames { get; set; }
    public PayMode ModeOfPay { get; set; }
    public int TotalPrice { get; set; }
}
