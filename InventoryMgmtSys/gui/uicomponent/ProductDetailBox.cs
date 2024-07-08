using InventoryMgmtSys.product;
using SplashKitSDK;

namespace InventoryMgmtSys.gui.uicomponent
{
    // Class to represent a component to display a product's details
    public class ProductDetailBox : UIComponent
    {
        private Dictionary<string, TextInput> _textInputs = new(); // Dictionary to store the labels and their corresponding text inputs for each property of the product

        private List<UIComponent> Components { get; } = new List<UIComponent>();
        public Dictionary<string, TextInput> CollectedData { get { return _textInputs; } }

        public ProductDetailBox(int x, int y, int width, int height, Product product) : base(x, y, width, height, 20, "#00000000", "#00000000")
        {
            int textY = y;
            int keyMaxLength = product.Describe().Keys.Select(str => SplashKit.TextWidth(str, GUI.Font, FontSize)).Max() + 10; // Get the length of the longest key plus padding

            // Create the label and text input for each property of the product
            foreach (KeyValuePair<string, string> property in product.Describe())
            {
                Components.Add(new TextDisplay(x, textY, keyMaxLength, 25, property.Key, "#000000"));

                TextInput textInput = new(x + keyMaxLength, textY, width - keyMaxLength, 25, property.Value);
                Components.Add(textInput);

                _textInputs.Add(property.Key, textInput);

                textY += 50;
            }
        }

        // Draw the component
        public override void Draw()
        {
            Components.ForEach(component => component.Draw());
        }

        // Handle input
        public override void HandleInput()
        {
            Components.ForEach(component => component.HandleInput());
        }
    }
}