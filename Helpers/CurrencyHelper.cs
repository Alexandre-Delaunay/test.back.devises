using System;
using System.Collections.Generic;
using System.Linq;
using test.back.devises.Models;

namespace test.back.devises.Helpers
{
    internal static class CurrencyHelper
    {
        public static double CalculateAmount(TargetedCurrency targetedCurrency, IEnumerable<Currency> currencies, LinkedList<TreeNode> path)
        {
            var result = targetedCurrency.Amount;
            int i = 0;
            while (path.Count != 1)
            {
                var firstNode = path.First();
                path.RemoveFirst();
                var secondNode = path.First();

                Currency currency;
                if (i % 2 == 1)
                    currency = currencies.First(x => string.Equals(x.ArrivalName, firstNode.Value) && string.Equals(x.IncomingName, secondNode.Value));
                else
                    currency = currencies.First(x => string.Equals(x.IncomingName, firstNode.Value) && string.Equals(x.ArrivalName, secondNode.Value));

                if (i % 2 == 1)
                    result = Math.Round(result * Math.Round(1 / currency.ExchangeRate, 4), 4);
                else
                    result = Math.Round(result * currency.ExchangeRate, 4);

                i++;
            }

            return result;
        }
    }
}
