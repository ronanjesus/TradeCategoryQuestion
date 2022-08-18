using System;
using System.Collections.Generic;
using System.Globalization;
using Trades.Classes;

namespace Trades.Services
{
    public class TradeService
    {
        CategoryService categoryService;
        public TradeService()
        {
            categoryService = new CategoryService();
        }
        public void Add(List<Trade> trades, string[] tradeInput, List<Category> categories, DateTime dataReferencia)
        {
            Trade trade = new Trade
            {
                Value = double.Parse(tradeInput[0]),
                ClientSector = tradeInput[1],
                NextPaymentDate = DateTime.Parse(tradeInput[2], new CultureInfo("en-US"))
            };

            trade.Category = categoryService.Categorize(categories, trade, dataReferencia);
            trades.Add(trade);
        }
    }
}
