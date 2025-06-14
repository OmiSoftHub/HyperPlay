﻿namespace HyperPlayServer.Models.Dtos;

public class ReviewDto
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; }

    public string ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }

    public int Rating { get; set; }
}
