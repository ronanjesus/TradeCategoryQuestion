using System;

namespace Trades.Classes
{
    public class Category : ITrade
    {
        public int OrderPrecedence { get; set; }
        public string Description { get; set; }
        public string Operator { get; set; }        
        public double Value { get; set; }
        public string ClientSector { get; set; }
        public int ExpiredDays { get; set; }        
        public DateTime NextPaymentDate { get; set; }        
    }
}
