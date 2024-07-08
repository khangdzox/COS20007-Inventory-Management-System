namespace InventoryMgmtSys.strategy
{
    // Strategy to generate summary and alert on total of all products
    public class TotalAllStrategy : IStrategy
    {
        private int _countAll = 0;
        private int _sumAll = 0;

        public TotalAllStrategy()
        {
            Update();
        }

        // Update strategy
        public void Update()
        {
            _countAll = Inventory.Instance.Products.Count;
            _sumAll = Inventory.Instance.Products.Sum(product => product.Value);
        }

        // Generate summary
        public List<string> GenerateSummary()
        {
            List<string> summary = new()
            {
                $"Inventory has {_countAll} types",
                $"with {_sumAll} products in total"
            };

            return summary;
        }

        // Generate alert if total of all products is less than 10
        public List<string> GenerateAlert()
        {
            List<string> alert = new()
            {
                "Alert on Total of all products"
            };

            bool hasAlert = false;

            if (_sumAll < 10)
            {
                alert.Add($"Low stock ({_sumAll} in total)!");
                hasAlert = true;
            }

            if (!hasAlert)
            {
                alert.Add("No alert");
            }

            return alert;
        }
    }
}
