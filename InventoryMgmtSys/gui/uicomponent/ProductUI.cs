using InventoryMgmtSys.gui.uistate;
using InventoryMgmtSys.product;
using SplashKitSDK;

namespace InventoryMgmtSys.gui.uicomponent
{
    // Class to represent a component to display a product
    public class ProductUI : UIComponent
    {
        private Product _product;
        private int _productQty;
        private TextDisplay _qtyText;

        public Product Product
        {
            get { return _product; }
        }
        public int ProductQty
        {
            get { return _productQty; }
            set
            {
                _productQty = value;
                _qtyText.UpdateFormatObject(_productQty.ToString()); // Update the text display
            }
        }
        public List<UIComponent> Components { get; } = new List<UIComponent>();

        public ProductUI(int x, int y, Product product, int productQty) : base(x, y, 148, 148, 15, "#FFE97F", "#e3c94b")
        {
            _product = product;
            _productQty = productQty;
            _qtyText = new TextDisplay(X, Y + 100, Width, 15, "Quantity: {0}", "#000000", center: true, format: true, formatObject: _productQty.ToString());

            TextInput textInput = new(X + 25, Y + 128, 98, 15, filter: char.IsDigit);

            Components.Add(textInput);
            Components.Add(_qtyText);
            Components.Add(new TextDisplay(X, Y + 80, Width, 20, _product.Name, "#000000", center: true));
            Components.Add(new BitmapDisplay(X + 44, Y + 20, 60, 60, _product.Icon));

            Components.Add(new Button(X + 5, Y + 128, 15, 15, "-", () =>
            {
                string result = TextInputHandler.Instance.RequestStopInputAndGetText(textInput);
                if (result.Length == 0)
                    return;
                Inventory.Instance.SubtractProduct(_product, int.Parse(result));
                _qtyText.UpdateFormatObject(_productQty.ToString());
            }));

            Components.Add(new Button(X + 128, Y + 128, 15, 15, "+", () =>
            {
                string result = TextInputHandler.Instance.RequestStopInputAndGetText(textInput);
                if (result.Length == 0)
                    return;
                Inventory.Instance.AddProduct(_product, int.Parse(result));
                _qtyText.UpdateFormatObject(_productQty.ToString());
            }));
        }

        // Draw the component
        public override void Draw()
        {
            DrawTexture();
            Components.ForEach(component => component.Draw());
        }

        // Handle input
        public override void HandleInput()
        {
            Components.ForEach(component => component.HandleInput());

            // If the user clicks on the component and the click is not on a button or text input, change the UI state to the product UI state
            if (IsMouseClick() && !Components.Any(component => (new List<Type>() { typeof(Button), typeof(TextInput) }.Contains(component.GetType())) && component.IsMouseClick()))
            {
                GUI.Instance.ChangeState(new ProductUIState(_product));
            }
        }

        // Draw the texture of the component
        public override void DrawTexture()
        {
            SplashKit.FillTriangle(Color.White, X, Y, X + Width - 1, Y, X, Y + Height - 1);
            SplashKit.FillTriangle(Color.RGBColor(160, 160, 160), X + Width - 1, Y, X + Width - 1, Y + Height - 1, X, Y + Height - 1);
            SplashKit.FillRectangle(Color, X + 2, Y + 2, Width - 4, Height - 4);
        }
    }
}
