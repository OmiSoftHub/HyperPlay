﻿namespace HyperPlayServer.Models.Dtos;

public class NewReviewDto
{
    public int GameId { get; set; }

    public int UserId { get; set; }

    public string ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }
}
