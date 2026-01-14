using CarRental.Core.Models;

namespace CarRental.Core.Services
{
    public class RentalService
    {
        private readonly List<Rental> _rentals = new();
        private int _nextRentalId = 1;
        private readonly CarService _carService;

        public RentalService(CarService carService)
        {
            _carService = carService;
        }

        
        public Rental CreateRental(int carId, string customerName,
                                  string customerPhone, DateTime startDate,
                                  DateTime endDate)
        {
            ValidateRentalInput(customerName, startDate, endDate);

            var car = ValidateAndGetCar(carId, startDate, endDate);
            var rental = CreateRentalRecord(carId, customerName, customerPhone, startDate, endDate, car);

            UpdateCarStatus(carId, false);
            _rentals.Add(rental);

            return rental;
        }

        
        public List<Rental> GetAllRentals() => _rentals.ToList();

        public List<Rental> GetActiveRentals() =>
            _rentals.Where(r => r.Status == RentalStatus.Active).ToList();

        public List<Rental> GetCompletedRentals() =>
            _rentals.Where(r => r.Status == RentalStatus.Completed).ToList();

        public List<Rental> GetCancelledRentals() =>
            _rentals.Where(r => r.Status == RentalStatus.Cancelled).ToList();

        public Rental? GetRentalById(int rentalId) =>
            _rentals.FirstOrDefault(r => r.Id == rentalId);

        
        public bool IsCarAvailableForDates(int carId, DateTime startDate, DateTime endDate)
        {
            return !_rentals.Any(r =>
                r.CarId == carId &&
                r.Status == RentalStatus.Active &&
                DatesOverlap(r.StartDate, r.EndDate, startDate, endDate));
        }

        public List<Car> GetAvailableCarsForDates(DateTime startDate, DateTime endDate,
                                                 CarService carService)
        {
            var allCars = carService.GetAllCars();
            return allCars.Where(car =>
                IsCarAvailableForDates(car.Id, startDate, endDate)).ToList();
        }

       
        public bool CompleteRental(int rentalId)
        {
            var rental = GetRentalById(rentalId);
            if (rental == null || rental.Status != RentalStatus.Active)
                return false;

            rental.Status = RentalStatus.Completed;
            UpdateCarStatus(rental.CarId, true);

            return true;
        }

        public bool CancelRental(int rentalId)
        {
            var rental = GetRentalById(rentalId);
            if (rental == null || rental.Status != RentalStatus.Active)
                return false;

            rental.Status = RentalStatus.Cancelled;
            UpdateCarStatus(rental.CarId, true);

            return true;
        }

        
        public List<Rental> GetRentalsByCustomer(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return new List<Rental>();

            return _rentals.Where(r =>
                r.CustomerName.Contains(customerName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Rental> GetRentalsByCar(int carId)
        {
            return _rentals.Where(r => r.CarId == carId).ToList();
        }

        public List<Rental> GetRentalsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _rentals.Where(r =>
                !(r.EndDate < startDate || r.StartDate > endDate)).ToList();
        }

       
        public int GetTotalRentalsCount() => _rentals.Count;

        public int GetActiveRentalsCount() => GetActiveRentals().Count;

        public decimal GetTotalRevenue()
        {
            return _rentals.Sum(r => r.TotalPrice);
        }

        public decimal GetExpectedRevenue(DateTime startDate, DateTime endDate,
                                        CarService carService)
        {
            var availableCars = GetAvailableCarsForDates(startDate, endDate, carService);
            int days = (endDate - startDate).Days + 1;

            return availableCars.Sum(car => days * car.RentalPricePerDay);
        }

        private Car ValidateAndGetCar(int carId, DateTime startDate, DateTime endDate)
        {
            var car = _carService.GetCarById(carId);
            if (car == null)
                throw new ArgumentException("Автомобиль не найден");

            if (!car.IsAvailable)
                throw new ArgumentException("Автомобиль уже арендован");

            if (!IsCarAvailableForDates(carId, startDate, endDate))
                throw new ArgumentException("Автомобиль уже забронирован на эти даты");

            return car;
        }

        private Rental CreateRentalRecord(int carId, string customerName,
                                        string customerPhone, DateTime startDate,
                                        DateTime endDate, Car car)
        {
            int rentalDays = (endDate - startDate).Days + 1;
            decimal totalPrice = rentalDays * car.RentalPricePerDay;

            return new Rental
            {
                Id = _nextRentalId++,
                CarId = carId,
                CustomerName = customerName.Trim(),
                CustomerPhone = customerPhone.Trim(),
                StartDate = startDate,
                EndDate = endDate,
                TotalPrice = totalPrice,
                Status = RentalStatus.Active
            };
        }

        private void UpdateCarStatus(int carId, bool isAvailable)
        {
            _carService.UpdateCarAvailability(carId, isAvailable);
        }

        private void ValidateRentalInput(string customerName, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Имя клиента не может быть пустым");

            if (startDate < DateTime.Today)
                throw new ArgumentException("Дата начала не может быть в прошлом");

            if (endDate <= startDate)
                throw new ArgumentException("Дата окончания должна быть позже даты начала");

            if (endDate > DateTime.Today.AddYears(1))
                throw new ArgumentException("Бронирование возможно не более чем на год вперед");
        }

        private bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return !(end1 < start2 || start1 > end2);
        }

        
        public Dictionary<string, int> GetRentalsByMonth(int year)
        {
            var result = new Dictionary<string, int>();
            var months = Enumerable.Range(1, 12).Select(m => new DateTime(year, m, 1));

            foreach (var month in months)
            {
                var monthName = month.ToString("MMM");
                var count = _rentals.Count(r =>
                    r.StartDate.Year == year && r.StartDate.Month == month.Month);
                result[monthName] = count;
            }

            return result;
        }

        public Dictionary<int, int> GetCarRentalCounts()
        {
            return _rentals
                .GroupBy(r => r.CarId)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public decimal GetAverageRentalDuration()
        {
            if (_rentals.Count == 0)
                return 0;

            var totalDays = _rentals.Sum(r => (r.EndDate - r.StartDate).Days + 1);
            return (decimal)totalDays / _rentals.Count;
        }
    }
}