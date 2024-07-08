using InventoryMgmtSys.product;

namespace InventoryMgmtSys
{
    // Inventory model class
    public class Inventory
    {
        private Dictionary<Product, int> _inventory;
        private List<IObserver> _observers; // list of observers to notify when inventory is updated

        // Singleton instance
        public static Inventory Instance { get; } = new Inventory();
        public Dictionary<Product, int> Products
        {
            get { return _inventory; }
        }

        private Inventory()
        {
            _inventory = new Dictionary<Product, int>();
            _observers = new List<IObserver>();
        }

        // Attach an observer to the inventory
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        // Detach an observer from the inventory
        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        // Notify all observers of an update
        private void Notify(KeyValuePair<Product, int> updatedProduct)
        {
            foreach (IObserver observer in _observers)
            {
                observer.Update(updatedProduct);
            }
        }

        // Add a number of items of a product to the inventory
        public void AddProduct(Product product, int quantity)
        {
            if (_inventory.ContainsKey(product))
            {
                _inventory[product] += quantity;
            }
            else if (_inventory.Count < 9)
            {
                _inventory.Add(product, quantity);
            }
            else
            {
                return;
            }
            Notify(new KeyValuePair<Product, int>(product, _inventory[product]));
        }

        // Subtract a number of items of a product from the inventory
        public void SubtractProduct(Product product, int quantity)
        {
            if (_inventory.ContainsKey(product))
            {
                if (_inventory[product] > quantity)
                {
                    _inventory[product] -= quantity;
                }
                else
                {
                    _inventory[product] = 0;
                }
                Notify(new KeyValuePair<Product, int>(product, _inventory[product]));
            }
        }

        // Remove a product from the inventory
        public void RemoveProduct(Product product)
        {
            if (_inventory.ContainsKey(product))
            {
                _inventory.Remove(product);
                Notify(new KeyValuePair<Product, int>(product, -1));
            }
        }

        // Replace a product in the inventory with another product
        public void ReplaceProduct(Product oldProduct, Product newProduct)
        {
            if (_inventory.ContainsKey(oldProduct))
            {
                _inventory.Add(newProduct, _inventory[oldProduct]);
                _inventory.Remove(oldProduct);
                Notify(new KeyValuePair<Product, int>(newProduct, _inventory[newProduct]));
            }
        }

        // Check if the inventory contains a product
        public bool HasProduct(Product product)
        {
            return _inventory.ContainsKey(product);
        }

        // Save the inventory to a file
        public void Save(string filename)
        {
            using StreamWriter file = new(filename);

            foreach (KeyValuePair<Product, int> product in _inventory)
            {
                file.WriteLine($"{product.Key.GetType().Name} - {product.Value}");
                file.WriteLine(product.Key.ToString());
            }
        }
    }
}
