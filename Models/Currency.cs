namespace test.back.devises.Models
{
    internal class Currency
    {
        public string IncomingName { get; init; }
        public string ArrivalName { get; init; }
        public double ExchangeRate { get; init; }

        public Currency(string incomingName, string arrivalName, double exchangeRate)
        {
            IncomingName = incomingName;
            ArrivalName = arrivalName;
            ExchangeRate = exchangeRate;
        }
    }
}
