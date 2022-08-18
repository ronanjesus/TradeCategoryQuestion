using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Trades.Classes;
using Trades.Services;

namespace Trades.Views
{
    public class View
    {
        CategoryService categoryService;
        TradeService tradeService;
        List<Category> categories;
        public View()
        {
            categoryService = new CategoryService();
            tradeService = new TradeService();

            categories = categoryService.LoadCategories();
        }
        public void MainMenu()
        {
            int option = 0;
            while (option != 3)
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1 - Add and classifie portfolio of trades");
                Console.WriteLine("2 - Manage categories");
                Console.WriteLine("3 - Exit");
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Portfolio();
                        break;
                    case 2:
                        ManageCategories();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
                Console.WriteLine();
            }
        }

        private void Portfolio()
        {
            try
            {
                Console.WriteLine("Input the reference date (format mm/dd/aaaa) :");
                DateTime dataReferencia = DateTime.Parse(Console.ReadLine(), new CultureInfo("en-US"));

                Console.WriteLine("Input number of trade(s):");
                int.TryParse(Console.ReadLine(), out var numeroTrades);

                List<Trade> tradeList = new List<Trade>();

                Console.WriteLine("Input the " + numeroTrades.ToString() + " trade(s) in the pattern explained below");
                Console.WriteLine("Input trade amount, client’s sector and date of next pending payment. (in this order separated by a space)");
                for (int i = 1; i <= numeroTrades; i++)
                {
                    string[] tradeInput = Console.ReadLine().Split(' ');
                    tradeService.Add(tradeList, tradeInput, categories, dataReferencia);
                }

                Console.WriteLine("");
                Console.WriteLine("Categorization of trades:");
                foreach (var item in tradeList)
                {
                    if (item.Category != null)
                        Console.WriteLine(item.Category.Description);
                    else
                        Console.WriteLine("There is no category for this trade");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: "+e.Message);
            }
        }

        private void ManageCategories()
        {
            try
            {
                int option = 0;
                while (option != 6)
                {
                    Console.WriteLine("CATEGORIES MENU");
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1 - List categories");
                    Console.WriteLine("2 - Add a category");
                    Console.WriteLine("3 - Update a category");
                    Console.WriteLine("4 - Delete a category");
                    Console.WriteLine("5 - Reorganize order of precedence");
                    Console.WriteLine("6 - Back to main menu");
                    option = int.Parse(Console.ReadLine());

                    switch (option)
                    {
                        case 1:
                            ListCategories();
                            break;
                        case 2:
                            AddCategory();
                            Console.WriteLine("Added category");
                            break;
                        case 3:
                            UpdateCategory();
                            Console.WriteLine("Updated category");
                            break;
                        case 4:
                            DeleteCategory();
                            Console.WriteLine("Deleted category");
                            break;
                        case 5:
                            ReorganizeOrderOfPrecedence();
                            Console.WriteLine("Reorganized list");
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
        private void ListCategories()
        {
            foreach (var item in categories)
            {
                Console.WriteLine(item.Description);
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(item);
                    Console.WriteLine("{0} = '{1}'", name, value);
                }
                Console.WriteLine("");
            }
        }

        private void AddCategory()
        {
            Console.WriteLine("Input description, operator, value, clientSector, expiredDays. (in this order separated by a space)");
            string[] categoryInput = Console.ReadLine().Split(' ');

            categoryService.Add(categories, categoryInput);
        }

        public void ReorganizeOrderOfPrecedence()
        {
            foreach (var item in categories)
            {
                Console.WriteLine(item.Description);
                Console.WriteLine("Current order of precedence: " + item.OrderPrecedence);
                Console.WriteLine("Input new order of precedence: ");
                int newOrder = int.Parse(Console.ReadLine());

                item.OrderPrecedence = newOrder;
            }

            categories = categoryService.OrderByPrecedence(categories);
        }

        private void UpdateCategory()
        {
            ListCategories();
            Console.WriteLine("Input the description of the item you want to change:");
            string description = Console.ReadLine();

            var selectCategory = categoryService.GetByDescription(categories, description);

            Console.WriteLine("Input new values in the pattern explained below");
            Console.WriteLine("Input description, operator, value, clientSector, expiredDays. (in this order separated by a space)");
            string[] categoryNewValues = Console.ReadLine().Split(' ');

            categoryService.Update(selectCategory, categoryNewValues);
        }

        private void DeleteCategory()
        {
            ListCategories();
            Console.WriteLine("Input the description of the item you want to delete:");
            string description = Console.ReadLine();
            
            categoryService.Delete(categories, description);
        }
    }
}
