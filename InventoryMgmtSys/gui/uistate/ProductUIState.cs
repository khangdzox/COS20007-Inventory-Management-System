using InventoryMgmtSys.gui.uicomponent;
using InventoryMgmtSys.product;
using SplashKitSDK;

namespace InventoryMgmtSys.gui.uistate
{
    // UI state for viewing and editing a product
    public class ProductUIState : UIState
    {
        // Initialize UI state with a product to edit and an original product to be replaced in the inventory (if applicable)
        public ProductUIState(Product? editingProduct = null, Product? originProduct = null) : base()
        {
            editingProduct ??= new BookProduct("", "", 0); // Default to a book product
            originProduct ??= editingProduct; // If no origin product is specified, assume the editing product is the origin product

            ProductDetailBox productDetail = new(300, 100, 450, 350, editingProduct);

            Components.Add(new BitmapDisplay(0, 0, 800, 600, SplashKit.LoadBitmap("background", "resources/images/background.png")));
            Components.Add(new BitmapDisplay(50, 100, 200, 200, editingProduct.Icon));
            Components.Add(productDetail);

            Components.Add(new Button(50, 325, 200, 50, "Change Type", () =>
            {
                TextInputHandler.Instance.RequestStopActiveInput();

                // Create a new product instance of the next type in the list of product types
                Type productType = Product.ProductType[(Product.ProductType.IndexOf(editingProduct.GetType()) + 1) % Product.ProductType.Count];
                Product newProduct = (Product) Activator.CreateInstance(productType, args: new object[] { editingProduct.Name, editingProduct.Description, editingProduct.Price })!;

                GUI.Instance.ChangeState(new ProductUIState(newProduct, originProduct));
            }));

            Components.Add(new Button(50, 500, 200, 50, "Cancel", () =>
            {
                TextInputHandler.Instance.RequestStopActiveInput();
                GUI.Instance.ChangeState(new MainUIState());
            }));

            Components.Add(new Button(300, 500, 200, 50, "Save", () =>
            {
                TextInputHandler.Instance.RequestStopActiveInput();
                Product newProduct = (Product) Activator.CreateInstance(editingProduct.GetType(), args: new object[] { editingProduct.Name, editingProduct.Description, editingProduct.Price })!;

                foreach (KeyValuePair<string, TextInput> property in productDetail.CollectedData)
                {
                    newProduct!.SetProperty(property.Key, property.Value.Text);
                }

                if (Inventory.Instance.HasProduct(originProduct))
                    Inventory.Instance.ReplaceProduct(originProduct, newProduct!);
                else
                    Inventory.Instance.AddProduct(newProduct!, 0);

                GUI.Instance.ChangeState(new MainUIState());
            }));

            Components.Add(new Button(550, 500, 200, 50, "Delete", () =>
            {
                TextInputHandler.Instance.RequestStopActiveInput();
                Inventory.Instance.RemoveProduct(originProduct);
                GUI.Instance.ChangeState(new MainUIState());
            }));
        }

        // Handle input for the Components
        public override void HandleInput()
        {
            Components.ForEach(component => component.HandleInput());
        }

        // Draw the Components
        public override void Draw()
        {
            Components.ForEach(component => component.Draw());
        }

        // Update the UI state
        public override void Update()
        {
            TextInputHandler.Instance.Update();
        }
    }
}
