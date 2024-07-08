using InventoryMgmtSys.product;

namespace InventoryMgmtSys
{
    // Singleton class to handle automatic buying of products
    public class AutoBuyer
    {
        private bool _enabled = false;
        private Random _random = new();

        // Singleton instance
        public static AutoBuyer Instance { get; } = new AutoBuyer();
        public bool Enabled
        {
            get { return _enabled; }
        }

        private AutoBuyer() { }

        // Update the inventory by randomly buying a product
        public void Update()
        {
            if (_enabled && Inventory.Instance.Products.Count > 0 && _random.Next(500) <= 5)
            {
                KeyValuePair<Product, int> randomProduct;

                // Find a random product that has quantity > 0
                do
                {
                    int randomIndex = _random.Next(Inventory.Instance.Products.Count);
                    randomProduct = Inventory.Instance.Products.ElementAt(randomIndex);
                } while (randomProduct.Value == 0);

                int randomQuantity = _random.Next(1, 5);
                Inventory.Instance.SubtractProduct(randomProduct.Key, randomQuantity);
            }
        }

        // Toggle the auto buyer on or off
        public void Toggle()
        {
            _enabled = !_enabled;
        }
    }
}
