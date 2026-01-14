namespace CarRental.Core.Models;

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public int Year { get; set; }
    public string LicensePlate { get; set; } = "";
    public decimal RentalPricePerDay { get; set; }
    public bool IsAvailable { get; set; } = true;

    public string FullName => $"{Brand} {Model} ({Year})";
}