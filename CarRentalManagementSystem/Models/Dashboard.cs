namespace CarRentalManagementSystem.Models
{
    public class Dashboard
    {
        public int TotalCustomers { get; set; }
        public int TotalCars { get; set; }
        public int TotalBookings { get; set; }
        public int ActiveBookings { get; set; }
        public int AvailableCars { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

