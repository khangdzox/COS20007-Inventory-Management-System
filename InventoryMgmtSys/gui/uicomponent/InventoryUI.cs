using InventoryMgmtSys.product;
using SplashKitSDK;

namespace InventoryMgmtSys.gui.uicomponent
{
    // Class to represent a component to display the Inventory
    public class InventoryUI : UIComponent, IObserver
    {
        private List<ProductUI> _productUIs;

        public InventoryUI(int x, int y) : base(x, y, 450, 450, 14, "#00000000", "#00000000")
        {
            _productUIs = new List<ProductUI>();

            Inventory.Instance.Attach(this); // Attach this UI to the Inventory

            GenerateProductUIs();
        }

        // Generate the ProductUIs
        public void GenerateProductUIs()
        {
            _productUIs.Clear();

            int productX = X + 2;
            int productY = Y + 2;

            foreach (KeyValuePair<Product, int> product in Inventory.Instance.Products)
            {
                _productUIs.Add(new ProductUI(productX, productY, product.Key, product.Value));

                productX += 150;
                if (productX > X + 450)
                {
                    productX = X + 2;
                    productY += 150;
                }
            }
        }

        // Draw the component
        public override void Draw()
        {
            DrawTexture();
            _productUIs.ForEach(productUI => productUI.Draw());
        }

        // Handle input
        public override void HandleInput()
        {
            _productUIs.ForEach(productUI => productUI.HandleInput());
        }

        // Draw the texture of the Inventory
        public override void DrawTexture()
        {
            SplashKit.LoadBitmap("inventory", "resources\\images\\inventory.png").Draw(X, Y);
        }

        // Update method for the Observer pattern
        public void Update(KeyValuePair<Product, int>? updatedProduct = null)
        {
            if (updatedProduct == null || updatedProduct?.Value < 0 || !_productUIs.Exists(productUI => productUI.Product == updatedProduct?.Key))
            {
                GenerateProductUIs();
            }
            else
            {
                foreach (ProductUI productUI in _productUIs)
                {
                    if (productUI.Product == updatedProduct!.Value.Key)
                    {
                        productUI.ProductQty = updatedProduct.Value.Value;
                    }
                }
            }
        }
    }
}
