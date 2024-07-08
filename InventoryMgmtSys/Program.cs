using InventoryMgmtSys.gui;
using InventoryMgmtSys.product;

namespace InventoryMgmtSys
{
    public class Program
    {
        public static void Main()
        {
            // Add some products to the inventory
            Inventory.Instance.AddProduct(new BookProduct("Alphabets", "A book for kids to learn ABC", 1.99, "Nguyen Van A", "Education Publising House", "2023"), 20);
            Inventory.Instance.AddProduct(new ElectronicProduct("iPhone 14 Pro Max", "Newest model from Apple", 1099.0, "Apple", "2 years"), 5);

            // Run the GUI
            GUI.Instance.Run();
        }
    }
}
