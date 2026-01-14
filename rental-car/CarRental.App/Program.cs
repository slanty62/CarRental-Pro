using System;
using CarRental.Core.Models;
using CarRental.Core.Services;

namespace rental_car.CarRental.App
{
    class Program
    {
        private static CarService _carService = new CarService();
        private static RentalService _rentalService = new RentalService(_carService);

        static void Main(string[] args)
        {

            InitializeTestData();

            while (true)
            {
                ShowMainMenu();

                Console.Write("\nВыберите пункт меню: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        ShowAvailableCars();
                        WaitForContinue();
                        break;
                    case "2":
                        SearchCars();
                        WaitForContinue();
                        break;
                    case "3":
                        BookCar();
                        WaitForContinue();
                        break;
                    case "4":
                        ShowMyBookings();
                        WaitForContinue();
                        break;
                    case "5":
                        AdminPanel();
                        WaitForContinue();
                        break;
                    case "0":
                        Console.WriteLine("╔══════════════════════════════════════╗");
                        Console.WriteLine("║      Спасибо за использование!       ║");
                        Console.WriteLine("║            До свидания!              ║");
                        Console.WriteLine("╚══════════════════════════════════════╝");
                        return;
                    default:
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        WaitForContinue();
                        break;
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║     СИСТЕМА АРЕНДЫ АВТОМОБИЛЕЙ       ║");
            Console.WriteLine("║            CarRent Pro v1.0          ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine("ГЛАВНОЕ МЕНЮ:");
            Console.WriteLine("════════════════════════════════════════");
            Console.WriteLine("1.  Просмотреть доступные автомобили");
            Console.WriteLine("2.  Найти автомобиль по параметрам");
            Console.WriteLine("3.  Забронировать автомобиль");
            Console.WriteLine("4.  Мои бронирования");
            Console.WriteLine("5.  Панель администратора");
            Console.WriteLine("0.  Выход");
            Console.WriteLine("════════════════════════════════════════");
        }

        static void InitializeTestData()
        {
            try
            {
                _carService.AddCar("Toyota", "Camry", 2022, "У811ВМ62", 2500);
                _carService.AddCar("Hyundai", "Solaris", 2023, "У505УЕ62", 1800);
                _carService.AddCar("BMW", "X5", 2021, "М777ММ62", 5000);
                _carService.AddCar("Kia", "Rio", 2023, "С652КР62", 1900);
                _carService.AddCar("Lada", "Vesta", 2022, "С605ОМ62", 1200);
                _carService.AddCar("Mercedes", "E-Class", 2020, "Е777КХ62", 4500);
                _carService.AddCar("Skoda", "Octavia", 2021, "О777ОО62", 2200);
            }
            catch
            {

            }
        }

        static void ShowAvailableCars()
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║     ДОСТУПНЫЕ АВТОМОБИЛИ             ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();

            var cars = _carService.GetAllCars();

            if (cars.Count == 0)
            {
                Console.WriteLine("🚫 В парке нет автомобилей.");
                return;
            }

            Console.WriteLine("┌────┬────────────────────────┬────────────┬──────────┬────────────┬──────────┐");
            Console.WriteLine("│ ID │ Автомобиль             │ Госномер   │ Год      │ Цена/день  │ Статус   │");
            Console.WriteLine("├────┼────────────────────────┼────────────┼──────────┼────────────┼──────────┤");

            foreach (var car in cars)
            {
                var status = car.IsAvailable ? "✅ Свободен" : "⛔ Занят";
                Console.WriteLine($"│ {car.Id,-2} │ {car.FullName,-22} │ {car.LicensePlate,-10} │ {car.Year,-8} │ {car.RentalPricePerDay,10}Руб │ {status,-8} │");
            }

            Console.WriteLine("└────┴────────────────────────┴────────────┴──────────┴────────────┴──────────┘");
            Console.WriteLine($"\n📊 Всего автомобилей: {cars.Count}");
            Console.WriteLine($"✅ Свободно: {_carService.GetAvailableCars().Count}");
        }

        static void SearchCars()
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║       ПОИСК АВТОМОБИЛЯ               ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine("Выберите тип поиска:");
            Console.WriteLine("════════════════════════════════════════");
            Console.WriteLine("1. Показать только свободные автомобили");
            Console.WriteLine("2. Поиск по марке/модели");
            Console.WriteLine("3. Фильтр по цене");
            Console.WriteLine("4. Проверить доступность на даты");
            Console.WriteLine("0. Назад в главное меню");
            Console.WriteLine("════════════════════════════════════════");

            Console.Write("\nВыберите: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAvailableCarsOnly();
                    break;
                case "2":
                    SearchByBrandModel();
                    break;
                case "3":
                    FilterByPrice();
                    break;
                case "4":
                    CheckAvailabilityByDates();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Неверный выбор.");
                    break;
            }
        }

        static void ShowAvailableCarsOnly()
        {
            var availableCars = _carService.GetAvailableCars();

            if (availableCars.Count == 0)
            {
                Console.WriteLine("\n🚫 Нет свободных автомобилей в данный момент.");
                return;
            }

            Console.WriteLine("\n┌────┬────────────────────────┬────────────┬──────────┬────────────┐");
            Console.WriteLine("  │ ID │ Автомобиль             │ Госномер   │ Год      │ Цена/день  │");
            Console.WriteLine("  ├────┼────────────────────────┼────────────┼──────────┼────────────┤");

            foreach (var car in availableCars)
            {
                Console.WriteLine($"│ {car.Id,-2} │ {car.FullName,-22} │ {car.LicensePlate,-10} │ {car.Year,-8} │ {car.RentalPricePerDay,10}Руб │");
            }

            Console.WriteLine(" └────┴────────────────────────┴────────────┴──────────┴────────────┘");
        }

        static void SearchByBrandModel()
        {
            Console.Write("\n🔍 Введите марку или модель: ");
            string searchTerm = Console.ReadLine()?.Trim() ?? "";

            var cars = _carService.GetAllCars();
            var results = cars.Where(c =>
                c.Brand.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Model.Contains(searchTerm, StringComparison.
                OrdinalIgnoreCase)).ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("\n🚫 Автомобили не найдены.");
                return;
            }

            Console.WriteLine($"\n📋 Найдено {results.Count} автомобилей:");
            Console.WriteLine("┌────┬────────────────────────┬────────────┬──────────┬────────────┬──────────┐");
            Console.WriteLine("│ ID │ Автомобиль             │ Госномер   │ Год      │ Цена/день  │ Статус   │");
            Console.WriteLine("├────┼────────────────────────┼────────────┼──────────┼────────────┼──────────┤");

            foreach (var car in results)
            {
                var status = car.IsAvailable ? "✅ Свободен" : "⛔ Занят";
                Console.WriteLine($"│ {car.Id,-2} │ {car.FullName,-22} │ {car.LicensePlate,-10} │ {car.Year,-8} │ {car.RentalPricePerDay,10}Руб {status,-8} │");
            }

            Console.WriteLine("└────┴────────────────────────┴────────────┴──────────┴────────────┴──────────┘");
        }

        static void FilterByPrice()
        {
            try
            {
                Console.Write("\n💰 Минимальная цена (Руб): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal minPrice))
                    minPrice = 0;

                Console.Write("💰 Максимальная цена (Руб): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
                    maxPrice = decimal.MaxValue;

                var cars = _carService.GetAllCars();
                var results = cars.Where(c => c.RentalPricePerDay >= minPrice && c.RentalPricePerDay <= maxPrice).ToList();

                Console.WriteLine($"\n📊 Найдено {results.Count} автомобилей в диапазоне {minPrice}Руб{maxPrice}Руб:");

                if (results.Count > 0)
                {
                    Console.WriteLine("┌────┬────────────────────────┬────────────┬──────────┬────────────┬──────────┐");
                    Console.WriteLine("│ ID │ Автомобиль             │ Госномер   │ Год      │ Цена/день  │ Статус   │");
                    Console.WriteLine("├────┼────────────────────────┼────────────┼──────────┼────────────┼──────────┤");

                    foreach (var car in results.OrderBy(c => c.RentalPricePerDay))
                    {
                        var status = car.IsAvailable ? "✅ Свободен" : "⛔ Занят";
                        Console.WriteLine($"│ {car.Id,-2} │ {car.FullName,-22}Руб{car.LicensePlate,-10} │ {car.Year,-8} │ {car.RentalPricePerDay,10}Р │ {status,-8} │");
                    }

                    Console.WriteLine("└────┴────────────────────────┴────────────┴──────────┴────────────┴──────────┘");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        static void CheckAvailabilityByDates()
        {
            try
            {
                Console.Write("\n📅 Дата начала (дд.мм.гггг): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    Console.WriteLine("❌ Некорректная дата.");
                    return;
                }

                Console.Write("📅 Дата окончания (дд.мм.гггг): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                {
                    Console.WriteLine("❌ Некорректная дата.");
                    return;
                }

                if (endDate <= startDate)
                {
                    Console.WriteLine("❌ Дата окончания должна быть позже даты начала.");
                    return;
                }

                var availableCars = _rentalService.GetAvailableCarsForDates(startDate, endDate, _carService);
                int days = (endDate - startDate).Days + 1;

                Console.WriteLine($"\n📋 Свободные автомобили с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy} ({days} дней):");

                if (availableCars.Count == 0)
                {
                    Console.WriteLine("🚫 Нет свободных автомобилей на выбранные даты.");
                    return;
                }

                Console.WriteLine("┌────┬────────────────────────┬────────────┬────────────┬──────────────┐");
                Console.WriteLine("│ ID │ Автомобиль             │ Госномер   │ Цена/день  │ Итоговая цена│");
                Console.WriteLine("├────┼────────────────────────┼────────────┼────────────┼──────────────┤");

                foreach (var car in availableCars)
                {
                    decimal totalPrice = days * car.RentalPricePerDay;
                    Console.WriteLine($"│ {car.Id,-2} │ {car.FullName,-22} │ {car.LicensePlate,-10} │ {car.RentalPricePerDay,10}Руб │ {totalPrice,12}Руб │");
                }

                Console.WriteLine("└────┴────────────────────────┴────────────┴────────────┴──────────────┘");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        static void BookCar()
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║       БРОНИРОВАНИЕ АВТОМОБИЛЯ        ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();

            var availableCars = _carService.GetAvailableCars();

            if (availableCars.Count == 0)
            {
                Console.WriteLine("🚫 В данный момент нет доступных автомобилей для бронирования.");
                return;
            }

            try
            {

                Console.WriteLine("🚗 Доступные автомобили:");
                Console.WriteLine("┌────┬────────────────────────┬────────────┬────────────┐");
                Console.WriteLine("│ ID │ Автомобиль             │ Госномер   │ Цена/день  │");
                Console.WriteLine("├────┼────────────────────────┼────────────┼────────────┤");

                foreach (var car in availableCars)
                {
                    Console.WriteLine($"│ {car.Id,-2} │ {car.FullName,-22} │ {car.LicensePlate,-10} │ {car.RentalPricePerDay,10}Руб │");
                }

                Console.WriteLine("└────┴────────────────────────┴────────────┴────────────┘");


                Console.Write("\n✏️  Выберите ID автомобиля: ");
                if (!int.TryParse(Console.ReadLine(), out int carId))
                {
                    Console.WriteLine("❌ Некорректный ID.");
                    return;
                }

                var selectedCar = _carService.GetCarById(carId);
                if (selectedCar == null || !selectedCar.IsAvailable)
                {
                    Console.WriteLine("❌ Автомобиль не найден или недоступен.");
                    return;
                }


                Console.WriteLine("\n👤 ВВЕДИТЕ ВАШИ ДАННЫЕ:");
                Console.WriteLine("──────────────────────────");

                Console.Write("ФИО: ");
                string customerName = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(customerName))
                {
                    Console.WriteLine("❌ ФИО не может быть пустым.");
                    return;
                }

                Console.Write("Телефон: ");
                string customerPhone = Console.ReadLine()?.Trim() ?? "";


                Console.WriteLine("\n📅 ВЫБЕРИТЕ ДАТЫ АРЕНДЫ:");
                Console.WriteLine("──────────────────────────");

                Console.Write("Дата начала (дд.мм.гггг): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    Console.WriteLine("❌ Некорректная дата начала.");
                    return;
                }

                Console.Write("Дата окончания (дд.мм.гггг): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                {
                    Console.WriteLine("❌ Некорректная дата окончания.");
                    return;
                }


                var rental = _rentalService.CreateRental(carId, customerName, customerPhone, startDate, endDate);

                if (rental != null)
                {
                    Console.WriteLine("\n╔══════════════════════════════════════╗");
                    Console.WriteLine("  ║        БРОНЬ ОФОРМЛЕНА!              ║");
                    Console.WriteLine("  ╚══════════════════════════════════════╝");
                    Console.WriteLine();
                    Console.WriteLine($"📋 Номер брони: #{rental.Id}");
                    Console.WriteLine($"👤 Клиент: {rental.CustomerName}");
                    Console.WriteLine($"📞 Телефон: {rental.CustomerPhone}");
                    Console.WriteLine($"🚗 Автомобиль: {selectedCar.FullName}");
                    Console.WriteLine($"📍 Госномер: {selectedCar.LicensePlate}");
                    Console.WriteLine($"📅 Период: {rental.StartDate:dd.MM.yyyy} - {rental.EndDate:dd.MM.yyyy}");
                    Console.WriteLine($"⏱️  Дней аренды: {rental.RentalDays}");
                    Console.WriteLine($"💰 Цена за день: {selectedCar.RentalPricePerDay}Руб");
                    Console.WriteLine($"💵 ИТОГО К ОПЛАТЕ: {rental.TotalPrice}Руб");
                    Console.WriteLine("\n✅ Бронь успешно оформлена!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка при оформлении брони: {ex.Message}");
            }
        }

        static void ShowMyBookings()
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║        МОИ БРОНИРОВАНИЯ             ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();

            var activeRentals = _rentalService.GetActiveRentals();

            if (activeRentals.Count == 0)
            {
                Console.WriteLine("📭 У вас нет активных бронирований.");
                return;
            }

            Console.WriteLine($"📋 Активных бронирований: {activeRentals.Count}");
            Console.WriteLine("┌────┬────────────────────┬────────────────────┬────────────┬──────────────┐");
            Console.WriteLine("│ ID │ Клиент             │ Период аренды      │ Автомобиль │ Сумма        │");
            Console.WriteLine("├────┼────────────────────┼────────────────────┼────────────┼──────────────┤");

            foreach (var rental in activeRentals)
            {
                var car = _carService.GetCarById(rental.CarId);
                string period = $"{rental.StartDate:dd.MM} - {rental.EndDate:dd.MM}";
                Console.WriteLine($"│ {rental.Id,-2} │ {rental.CustomerName,-18} │ {period,-18} │ {car?.Model ?? "Не найден",-10} │ {rental.TotalPrice,12}Руб");
            }

            Console.WriteLine("└────┴────────────────────┴────────────────────┴────────────┴──────────────┘");
        }

        static void AdminPanel()
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║      ПАНЕЛЬ АДМИНИСТРАТОРА          ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine("Выберите действие:");
            Console.WriteLine("════════════════════════════════════════");
            Console.WriteLine("1. Добавить новый автомобиль");
            Console.WriteLine("2. Просмотреть все бронирования");
            Console.WriteLine("3. Статистика по автомобилям");
            Console.WriteLine("0. Назад в главное меню");
            Console.WriteLine("════════════════════════════════════════");

            Console.Write("\nВыберите: "); string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddNewCar();
                    break;
                case "2":
                    ShowAllBookings();
                    break;
                case "3":
                    ShowCarStatistics();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Неверный выбор.");
                    break;
            }
        }

        static void AddNewCar()
        {
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine("  ║      ДОБАВЛЕНИЕ АВТОМОБИЛЯ           ║");
            Console.WriteLine("  ╚══════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                Console.Write("Марка: ");
                string brand = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Модель: ");
                string model = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Год выпуска: ");
                if (!int.TryParse(Console.ReadLine(), out int year))
                    throw new ArgumentException("Некорректный год");

                Console.Write("Госномер: ");
                string licensePlate = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Цена аренды за день (Руб): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                    throw new ArgumentException("Некорректная цена");

                var car = _carService.AddCar(brand, model, year, licensePlate, price);

                Console.WriteLine($"\n✅ Автомобиль успешно добавлен!");
                Console.WriteLine($"📋 ID: {car.Id}");
                Console.WriteLine($"🚗 {car.FullName}");
                Console.WriteLine($"📍 {car.LicensePlate}");
                Console.WriteLine($"💰 {car.RentalPricePerDay}Руб/день");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
        }

        static void ShowAllBookings()
        {
            var allRentals = _rentalService.GetAllRentals();

            if (allRentals.Count == 0)
            {
                Console.WriteLine("\n📭 Нет зарегистрированных бронирований.");
                return;
            }

            Console.WriteLine($"\n📊 Всего бронирований: {allRentals.Count}");
            Console.WriteLine("┌────┬────────────────────┬────────────────────┬────────────┬──────────────┬──────────┐");
            Console.WriteLine("│ ID │ Клиент             │ Период аренды      │ Автомобиль │ Сумма        │ Статус   │");
            Console.WriteLine("├────┼────────────────────┼────────────────────┼────────────┼──────────────┼──────────┤");

            foreach (var rental in allRentals)
            {
                var car = _carService.GetCarById(rental.CarId);
                string period = $"{rental.StartDate:dd.MM.yy} - {rental.EndDate:dd.MM.yy}";
                string status = rental.Status switch
                {
                    RentalStatus.Active => "✅ Активна",
                    RentalStatus.Completed => "✓ Завершена",
                    RentalStatus.Cancelled => "✗ Отменена",
                    _ => "❓ Неизвестен"
                };
                Console.WriteLine($"│ {rental.Id,-2} │ {rental.CustomerName,-18} │ {period,-18} │ {car?.Model ?? "Не найден",-10} │ {rental.TotalPrice,12}Руб{status,-8} │");
            }

            Console.WriteLine("└────┴────────────────────┴────────────────────┴────────────┴──────────────┴──────────┘");
        }

        static void ShowCarStatistics()
        {
            var cars = _carService.GetAllCars();
            var rentals = _rentalService.GetAllRentals();

            Console.WriteLine("\n📈 СТАТИСТИКА ПО АВТОМОБИЛЯМ:");
            Console.WriteLine("════════════════════════════════════════");
            Console.WriteLine($"Всего автомобилей в парке: {cars.Count}");
            Console.WriteLine($"Свободно: {_carService.GetAvailableCars().Count}");
            Console.WriteLine($"Занято: {cars.Count - _carService.GetAvailableCars().Count}");
            Console.WriteLine($"Всего бронирований: {rentals.Count}");
            Console.WriteLine($"Активных бронирований: {_rentalService.GetActiveRentals().Count}");

            if (rentals.Count > 0)
            {
                decimal totalRevenue = rentals.Sum(r => r.TotalPrice);
                Console.WriteLine($"Общая выручка: {totalRevenue}Руб");

                var mostExpensiveCar = cars.OrderByDescending(c => c.RentalPricePerDay).FirstOrDefault();
                if (mostExpensiveCar != null)
                {
                    Console.WriteLine($"Самый дорогой автомобиль: {mostExpensiveCar.FullName} ({mostExpensiveCar.RentalPricePerDay}Руб/день)");
                }
            }
        }

        static void WaitForContinue()
        {
            Console.WriteLine("\n════════════════════════════════════════");
            Console.Write("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}