using InventoryMgmtSys.product;

namespace InventoryMgmtSys
{
    // Interface for Observer classes
    public interface IObserver
    {
        public void Update(KeyValuePair<Product, int>? updatedProduct = null);
    }
}
