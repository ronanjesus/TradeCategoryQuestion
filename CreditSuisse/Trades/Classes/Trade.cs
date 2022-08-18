using System;
using Trades.Classes;

namespace Trades
{
    public class Trade : ITrade
    {
        public double Value { get; set; }

        public string ClientSector { get; set; }

        public DateTime NextPaymentDate { get; set; }

        public Category Category { get; set; }
    }
}
