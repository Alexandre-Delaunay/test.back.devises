using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using test.back.devises.Extensions;
using test.back.devises.Resources;

namespace test.back.devises.Models
{
    internal class CurrencyFile
    {
        private const char SEPARATOR = ';';

        private string _filePath { get; set; }
        private IEnumerable<Currency> _currencies { get; set; }
        private TargetedCurrency _targetedCurrency { get; set; }

        public CurrencyFile(string filePath)
        {
            _filePath = filePath;

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), Errors.FilePath_ArgumentNullException);

            ReadFile();
        }

        #region Private Methods

        private void ReadFile()
        {
            _currencies = new List<Currency>();
            _targetedCurrency = null;

            int i = 0;
            foreach (var line in File.ReadLines(_filePath))
            {
                switch (i)
                {
                    case 0: // Récupération de la devise cible                
                        var firstLineSplitted = line.Split(SEPARATOR);

                        if (firstLineSplitted != null && firstLineSplitted.IsValidTargetCurrency())
                            _targetedCurrency = new TargetedCurrency(firstLineSplitted[0], double.Parse(firstLineSplitted[1]), firstLineSplitted[2]);

                        break;
                    case 1: // Le nombre de lignes n'est pas utile dans notre implémentation                        
                        break;
                    default: // Récupération de la liste de devises
                        var lineSplitted = line.Split(SEPARATOR);

                        if (lineSplitted != null && lineSplitted.IsValidCurrency())
                        {
                            var currentCurrency = new Currency(lineSplitted[0], lineSplitted[1], double.Parse(lineSplitted[2], CultureInfo.InvariantCulture));

                            _currencies = _currencies.Append(currentCurrency);
                        }

                        break;
                }
                i++;
            }
        }

        #endregion

        #region Public Methods

        public IEnumerable<Currency> GetCurrencies()
        {
            return _currencies;
        }

        public TargetedCurrency GetTargetedCurrency()
        {
            return _targetedCurrency;
        }

        #endregion
    }
}
