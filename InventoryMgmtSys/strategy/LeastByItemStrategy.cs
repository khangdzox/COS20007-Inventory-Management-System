using InventoryMgmtSys.product;

namespace InventoryMgmtSys.strategy
{
    // Strategy to generate a summary of the inventory by the least number of items of each type
    public class LeastByItemStrategy : IStrategy
    {
        private Dictionary<Product, int> _leastByItems = new(); // product with the least number of items of each type
        private Dictionary<Type, int> _sumTypes = new(); // total number of items of each type

        public LeastByItemStrategy()
        {
            Update();
        }

        // Update the strategy
        public void Update()
        {
            _leastByItems.Clear();

            // Find the product with the least number of items of each type
            foreach (KeyValuePair<Product, int> product in Inventory.Instance.Products)
            {
                if (!_leastByItems.Where(item => item.Key.GetType() == product.Key.GetType()).Any())
                {
                    _leastByItems.Add(product.Key, product.Value);
                }
                else
                {
                    if (_leastByItems.Where(item => item.Key.GetType() == product.Key.GetType()).First().Value > product.Value)
                    {
                        _leastByItems.Remove(_leastByItems.Where(item => item.Key.GetType() == product.Key.GetType()).First().Key);
                        _leastByItems.Add(product.Key, product.Value);
                    }
                }
            }

            _sumTypes.Clear();

            // Find the total number of items of each type
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

        // Generate a summary of the inventory
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

        // Generate an alert for the inventory
        public List<string> GenerateAlert()
        {
            List<string> alert = new()
            {
                "Alert on Least product by items"
            };
            foreach (KeyValuePair<Product, int> leastByItem in _leastByItems)
            {
                alert.Add($"Lowest {leastByItem.Key.GetType().Name[..^7]} stock on");
                alert.Add($"{leastByItem.Key.Name} ({leastByItem.Value} left)!");
            }

            return alert;
        }
    }
}
