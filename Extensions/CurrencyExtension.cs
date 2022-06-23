using System.Globalization;

namespace test.back.devises.Extensions
{
    internal static class CurrencyExtension
    {
        public static bool IsValidCurrency(this string[] stringArray)
        {
            if (stringArray.Length == 3 && double.TryParse(stringArray[2], NumberStyles.Number, CultureInfo.InvariantCulture, out double _))
                return true;

            return false;
        }

        public static bool IsValidTargetCurrency(this string[] stringArray)
        {
            if (stringArray.Length == 3 && double.TryParse(stringArray[1], NumberStyles.Number, CultureInfo.InvariantCulture, out double _))
                return true;

            return false;
        }
    }
}
