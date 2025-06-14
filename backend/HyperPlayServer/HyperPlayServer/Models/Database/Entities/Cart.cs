﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HyperPlayServer.Models.Database.Entities;

public class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public List<CartDetail> CartDetails { get; set; }

    public Cart()
    {
        CartDetails = new List<CartDetail>();
    }

}
