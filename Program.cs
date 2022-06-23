using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using test.back.devises.Helpers;
using test.back.devises.Models;
using test.back.devises.Resources;

namespace test.back.devises
{
    internal class Program
    {
        public static void Main(string[] args)
        {            
            try
            {
                if (args.Length == 0)
                    throw new ArgumentNullException(nameof(args), Errors.FilePath_ArgumentNullException);

                var filePath = args.First();

                if (!File.Exists(filePath))
                    throw new FileNotFoundException(Errors.FilePath_FileNotFoundException);

                // Parcours du fichier et récupération des devises
                var currencyFile = new CurrencyFile(filePath);
                IEnumerable<Currency> currencies = currencyFile.GetCurrencies();
                TargetedCurrency targetedCurrency = currencyFile.GetTargetedCurrency();

                if (targetedCurrency == null)
                    throw new Exception(Errors.TargetedCurrency_NotFound);

                if (!currencies.Any())
                    throw new Exception(Errors.Currencies_NullOrEmpty);

                // Creation de l'arbre et de ses chemins
                var tree = new Tree(targetedCurrency.InitialName);
                tree.Build(currencies);

                // Parcours de l'arbre pour récupérer le chemin le plus court vers la devise cible
                var shortestPath = tree.GetShortestPath(targetedCurrency.TargetName);

                if (!shortestPath.Any())
                    throw new Exception(Errors.ShortestPath_NotFound);

                // Calcul du montant
                var result = CurrencyHelper.CalculateAmount(targetedCurrency, currencies, shortestPath);

                // Sortie du montant en console
                Console.WriteLine(Convert.ToInt32(result));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
