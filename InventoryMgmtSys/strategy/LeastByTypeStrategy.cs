using InventoryMgmtSys.product;

namespace InventoryMgmtSys.strategy
{
    // Strategy to generate a summary of the inventory by product type with the least quantity
    public class LeastByTypeStrategy : IStrategy
    {
        private Dictionary<Type, int> _sumTypes = new(); // sum of each product type

        public LeastByTypeStrategy()
        {
            Update();
        }

        // Update the sum of each product type
        public void Update()
        {
            _sumTypes.Clear();

            // Sum the quantity of each product type
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

        // Generate a summary of the inventory by product type with the least quantity
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

        // Generate an alert for the product type with the least quantity
        public List<string> GenerateAlert()
        {
            List<string> alert = new()
            {
                "Alert on Least product by type",
                $"Lowest stock on { _sumTypes.MinBy(sumType => sumType.Value).Key.Name[..^7] }",
                $"(only {_sumTypes.MinBy(sumType => sumType.Value).Value} left)"
            };

            return alert;
        }
    }
}
