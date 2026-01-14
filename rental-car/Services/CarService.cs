using CarRental.Core.Models;

namespace CarRental.Core.Services
{
    public class CarService
    {
        private readonly List<Car> _cars = new();
        private int _nextCarId = 1;

       
        public Car AddCar(string brand, string model, int year,
                         string licensePlate, decimal pricePerDay)
        {
            ValidateCarInput(brand, model, year, pricePerDay);

            var car = new Car
            {
                Id = _nextCarId++,
                Brand = brand.Trim(),
                Model = model.Trim(),
                Year = year,
                LicensePlate = licensePlate.Trim().ToUpper(),
                RentalPricePerDay = pricePerDay,
                IsAvailable = true
            };

            _cars.Add(car);
            return car;
        }

        public List<Car> GetAllCars() => _cars.ToList();

        public List<Car> GetAvailableCars() =>
            _cars.Where(c => c.IsAvailable).ToList();

        public Car? GetCarById(int id) =>
            _cars.FirstOrDefault(c => c.Id == id);

        public void UpdateCarAvailability(int carId, bool isAvailable)
        {
            var car = GetCarById(carId);
            if (car != null)
                car.IsAvailable = isAvailable;
        }

       
        public List<Car> SearchCars(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllCars();

            var term = searchTerm.ToLower();
            return _cars.Where(c =>
                c.Brand.ToLower().Contains(term) ||
                c.Model.ToLower().Contains(term) ||
                c.LicensePlate.ToLower().Contains(term)).ToList();
        }

        public List<Car> GetCarsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _cars.Where(c =>
                c.RentalPricePerDay >= minPrice &&
                c.RentalPricePerDay <= maxPrice).ToList();
        }

        
        public bool RemoveCar(int carId)
        {
            var car = GetCarById(carId);
            if (car == null)
                return false;

            if (!car.IsAvailable)
                return false;

            _cars.Remove(car);
            return true;
        }

        public bool UpdateCarPrice(int carId, decimal newPrice)
        {
            if (newPrice <= 0)
                return false;

            var car = GetCarById(carId);
            if (car == null)
                return false;

            car.RentalPricePerDay = newPrice;
            return true;
        }

        
        public int GetTotalCarsCount() => _cars.Count;

        public int GetAvailableCarsCount() => GetAvailableCars().Count;

        public decimal GetTotalDailyRevenuePotential()
        {
            return _cars.Sum(c => c.RentalPricePerDay);
        }

        public Car? GetMostExpensiveCar()
        {
            return _cars.OrderByDescending(c => c.RentalPricePerDay).FirstOrDefault();
        }

        public Car? GetMostPopularCar()
        {
            
            return GetMostExpensiveCar();
        }

       
        private void ValidateCarInput(string brand, string model, int year, decimal pricePerDay)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Марка не может быть пустой");

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Модель не может быть пустой");

            if (year < 2000 || year > DateTime.Now.Year + 1)
                throw new ArgumentException($"Год выпуска должен быть между 2000 и {DateTime.Now.Year + 1}");

            if (pricePerDay <= 0)
                throw new ArgumentException("Цена аренды должна быть больше 0");
        }
        public bool CarExists(int carId) => GetCarById(carId) != null;

        public bool IsCarAvailable(int carId)
        {
            var car = GetCarById(carId);
            return car?.IsAvailable ?? false;
        }

        public List<Car> GetCarsSortedByPrice(bool ascending = true)
        {
            return ascending
                ? _cars.OrderBy(c => c.RentalPricePerDay).ToList()
                : _cars.OrderByDescending(c => c.RentalPricePerDay).ToList();
        }

        public List<Car> GetCarsSortedByYear(bool ascending = true)
        {
            return ascending
                ? _cars.OrderBy(c => c.Year).ToList()
                : _cars.OrderByDescending(c => c.Year).ToList();
        }
    }
}