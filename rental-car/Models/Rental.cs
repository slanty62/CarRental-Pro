namespace CarRental.Core.Models;

public class Rental
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerPhone { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; } = RentalStatus.Active;

    public int RentalDays => (EndDate - StartDate).Days + 1;
}