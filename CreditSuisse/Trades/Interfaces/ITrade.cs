using System;

namespace Trades
{
    interface ITrade
    {
        double Value { get; } 
        string ClientSector { get; } 
        DateTime NextPaymentDate { get; } 
    }
}
