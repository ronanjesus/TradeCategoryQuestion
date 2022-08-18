using System;
using System.Collections.Generic;
using System.Linq;
using Trades.Classes;

namespace Trades.Services
{
    public class CategoryService
    {
        public CategoryService()
        {

        }
        public Category GetByDescription(List<Category> categories, string description)
        {
            var obj = categories.FirstOrDefault(x => x.Description.ToUpper().Equals(description.ToUpper()));
            return obj;
        }

        public void Add(List<Category> categories, string[] categoryInput)
        {            
            Category category = new Category
            {
                OrderPrecedence = GetNextOrder(categories),
                Description = categoryInput[0],
                Operator = categoryInput[1],
                Value = double.Parse(categoryInput[2]),
                ClientSector = categoryInput[3],
                ExpiredDays = int.Parse(categoryInput[4])
            };

            categories.Add(category);
        }
        public void Update(Category category, string[] newValues)
        {
            category.Description = newValues[0];
            category.Operator = newValues[1];
            category.Value = double.Parse(newValues[2]);
            category.ClientSector = newValues[3];
            category.ExpiredDays = int.Parse(newValues[4]);            
        }

        public void Delete(List<Category> categories, string description)
        {
            var selectCategory = GetByDescription(categories, description);
            categories.Remove(selectCategory);
        }

        public Category Categorize(List<Category> categories, Trade trade, DateTime referenceDate)
        {
            categories = OrderByPrecedence(categories);

            foreach (var item in categories)
            {
                if (item.ExpiredDays > 0)
                {
                    if (trade.NextPaymentDate.AddDays(item.ExpiredDays) < referenceDate)
                        return item;
                }
                else
                {
                    if (trade.ClientSector.ToUpper().Equals(item.ClientSector.ToUpper()))
                    {
                        bool result = false;
                        switch (item.Operator)
                        {
                            case ">":
                                result = (trade.Value > item.Value);
                                break;

                            case ">=":
                                result = (trade.Value >= item.Value);
                                break;

                            case "<":
                                result = (trade.Value < item.Value);
                                break;

                            case "<=":
                                result = (trade.Value <= item.Value);
                                break;

                            case "=":
                                result = (trade.Value == item.Value);
                                break;

                            default:
                                result = false;
                                break;
                        }
                        if (result)
                            return item;
                    }
                }
            }
            return null;
        }

        public List<Category> LoadCategories()
        {
            List<Category> lista = new List<Category>();

            Category expired = new Category
            {
                OrderPrecedence = 1,
                Description = "EXPIRED",
                Operator = "",
                Value = 0,
                ClientSector = "",
                ExpiredDays = 30
            };

            Category highrisk = new Category
            {
                OrderPrecedence = 2,
                Description = "HIGHRISK",
                Operator = ">",
                Value = 1000000,
                ClientSector = "Private",
                ExpiredDays = 0
            };

            Category mediumrisk = new Category
            {
                OrderPrecedence = 3,
                Description = "MEDIUMRISK",
                Operator = ">",
                Value = 1000000,
                ClientSector = "Public",
                ExpiredDays = 0
            };

            lista.Add(expired);
            lista.Add(highrisk);
            lista.Add(mediumrisk);

            return lista;
        }

        public int GetNextOrder(List<Category> categories)
        {
            return categories.Max(x => x.OrderPrecedence) + 1;
        }

        public List<Category> OrderByPrecedence(List<Category> categories)
        {
            return categories.OrderBy(x => x.OrderPrecedence).ToList();
        }
    }
}
