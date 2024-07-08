using InventoryMgmtSys.product;

namespace InventoryMgmtSys.strategy
{
    // Strategy to generate a summary of the total number of products by type
    public class TotalByTypeStrategy : IStrategy
    {
        private Dictionary<Type, int> _sumTypes = new();

        public TotalByTypeStrategy()
        {
            Update();
        }

        // Update the summary of total products by type
        public void Update()
        {
            _sumTypes.Clear();

            // sum up the total number of products by type
            foreach (KeyValuePair<Product, int> product in Inventory.Instance.Products)
            {
                if (_sumTypes.ContainsKey(product.Key.GetType()))
                {
                    _sumTypes[product.Key.GetType()] += product.Value;
                }
                else
                {
                    _sumTypes.Add(product.Key.GetType(), product.Value);
                }
            }
        }

        // Generate a summary of the total number of products by type
        public List<string> GenerateSummary()
        {
            List<string> summary = new()
            {
                $"Inventory has {_sumTypes.Count} types:"
            };
            foreach (KeyValuePair<Type, int> sumType in _sumTypes)
            {
                summary.Add($"{sumType.Value} {sumType.Key.Name[..^7]} products");
            }

            return summary;
        }

        // Generate an alert if any product type has less than 10 items
        public List<string> GenerateAlert()
        {
            List<string> alert = new()
            {
                "Alert on Total products by type"
            };

            bool hasAlert = false;

            foreach (KeyValuePair<Type, int> sumType in _sumTypes)
            {
                if (sumType.Value < 10)
                {
                    alert.Add($"Low stock for {sumType.Key.Name[..^7]}!");
                    hasAlert = true;
                }
            }

            if (!hasAlert)
            {
                alert.Add("No alert");
            }

            return alert;
        }
    }
}
