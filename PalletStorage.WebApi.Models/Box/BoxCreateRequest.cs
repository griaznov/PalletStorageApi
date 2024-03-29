﻿namespace PalletStorage.WebApi.Models.Box;

public class BoxCreateRequest
{
    public DateTime ProductionDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public double Length { get; set; }
    public double Weight { get; set; }
}
