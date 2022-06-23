namespace test.back.devises.Models
{
    internal class TargetedCurrency
    {
        public string InitialName { get; set; }
        public double Amount { get; set; }
        public string TargetName { get; set; }

        public TargetedCurrency(string initialName, double amount, string targetName)
        {
            InitialName = initialName;
            Amount = amount;
            TargetName = targetName;
        }
    }
}
