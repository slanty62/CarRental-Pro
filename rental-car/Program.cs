using System;

namespace CarRentalSystem.App
{
    class Program
    {
        static void Main(string[] args)
        {
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

            Console.Write("\nВыберите пункт меню: ");
            string input = Console.ReadLine();

            Console.WriteLine($"\nВы выбрали: {input}");
            Console.WriteLine("\nЭто каркас системы. Функциональность будет добавлена в следующих ЛР.");

            
            if (input == "1")
            {
                ShowAvailableCarsDemo();
            }
            else if (input == "2")
            {
                SearchCarsDemo();
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void ShowAvailableCarsDemo()
        {
            Console.WriteLine("\n════════════════════════════════════════");
            Console.WriteLine("ДОСТУПНЫЕ АВТОМОБИЛИ (демо):");
            Console.WriteLine("════════════════════════════════════════");
            Console.WriteLine("1. Toyota Camry 2022 - 2500 руб/день");
            Console.WriteLine("2. Hyundai Solaris 2023 - 1800 руб/день");
            Console.WriteLine("3. BMW X5 2021 - 5000 руб/день");
            Console.WriteLine("════════════════════════════════════════");
        }

        static void SearchCarsDemo()
        {
            Console.WriteLine("\n════════════════════════════════════════");
            Console.WriteLine("ПОИСК АВТОМОБИЛЯ:");
            Console.WriteLine("════════════════════════════════════════");
            Console.WriteLine("Фильтры будут реализованы в следующих ЛР");
            Console.WriteLine("════════════════════════════════════════");
        }
    }
}