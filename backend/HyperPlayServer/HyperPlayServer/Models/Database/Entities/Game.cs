﻿using System.Text.Json.Serialization;
using HyperPlayServer.Models.Database.Entities.Enum;

namespace HyperPlayServer.Models.Database.Entities;

public class Game
{
    public int Id { get; set; }
    public int GameRequirementsId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Sinopsis { get; set; } 
    public Genre Genre { get; set; }
    public Drm DrmFree {  get; set; }
    public DateTime ReleaseDate {  get; set; }
    public int Price { get; set; }
    public int Stock {  get; set; }
    public int? AvgRating { get; set; }
    public List<ImageGame> ImageGames { get; set; }
    public GameRequirements GameRequirements { get; set; }
    public List<Review> Reviews { get; set; }

    public Game()
    {
        AvgRating = null;
        ImageGames = new List<ImageGame>();
        Reviews = new List<Review>();
    }
}
