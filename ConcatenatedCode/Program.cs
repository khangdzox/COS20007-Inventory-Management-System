using SplashKitSDK;

namespace ConcatenatedCode
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


    // Interface for Observer classes
    public interface IObserver
    {
        public void Update(KeyValuePair<Product, int>? updatedProduct = null);
    }


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


    // Abstract class to represent a product
    public abstract class Product
    {
        private string _name;
        private string _description;
        private double _price;
        private Bitmap _icon;

        // List of product types
        public static List<Type> ProductType { get; } = new List<Type>
        {
            typeof(BookProduct),
            typeof(ElectronicProduct)
        };

        public string Name
        {
            get { return _name; }
        }
        public string Description
        {
            get { return _description; }
        }
        public double Price
        {
            get { return _price; }
        }
        public Bitmap Icon
        {
            get { return _icon; }
        }

        public Product(string name, string description, double price, string iconName)
        {
            _name = name;
            _description = description;
            _price = price;
            _icon = SplashKit.LoadBitmap(iconName, "resources/images/" + iconName + ".png");
        }

        // Return a dictionary of product information  
        public virtual Dictionary<string, string> Describe()
        {
            Dictionary<string, string> productInfo = new()
            {
                { "Name", _name },
                { "Description", _description },
                { "Price", _price.ToString() }
            };
            return productInfo;
        }

        // Set a property of the product
        public virtual void SetProperty(string key, string value)
        {
            switch (key)
            {
                case "Name":
                    _name = value;
                    break;
                case "Description":
                    _description = value;
                    break;
                case "Price":
                    _price = double.Parse(value);
                    break;
                default:
                    break;
            }
        }

        // Return a string representation of the product
        public override string ToString()
        {
            string result = "";

            foreach (KeyValuePair<string, string> entry in Describe())
            {
                result += entry.Key + ": " + entry.Value + "\n";
            }

            return result;
        }
    }


    // Implementation of the Product class for books
    public class BookProduct : Product
    {
        private string _author, _publisher, _year;

        public BookProduct(string name, string description, double price, string author, string publisher, string year) : base(name, description, price, "book")
        {
            _author = author;
            _publisher = publisher;
            _year = year;
        }

        // Initialize an empty book product
        public BookProduct(string name, string description, double price) : this(name, description, price, "", "", "") { }

        // Return a dictionary of product information
        public override Dictionary<string, string> Describe()
        {
            Dictionary<string, string> productInfo = base.Describe();
            productInfo.Add("Author", _author);
            productInfo.Add("Publisher", _publisher);
            productInfo.Add("Year", _year);
            return productInfo;
        }

        // Set a property of the product
        public override void SetProperty(string key, string value)
        {
            base.SetProperty(key, value);
            switch (key)
            {
                case "Author":
                    _author = value;
                    break;
                case "Publisher":
                    _publisher = value;
                    break;
                case "Year":
                    _year = value;
                    break;
            }
        }
    }


    // Implementation of the Product class for electronic products
    public class ElectronicProduct : Product
    {
        private string _brand;
        private string _warranty;

        public ElectronicProduct(string name, string description, double price, string brand, string warranty) : base(name, description, price, "electronic")
        {
            _brand = brand;
            _warranty = warranty;
        }

        // Initialize an empty electronic product
        public ElectronicProduct(string name, string description, double price) : this(name, description, price, "", "") { }

        // Return a dictionary of product information
        public override Dictionary<string, string> Describe()
        {
            Dictionary<string, string> productInfo = base.Describe();
            productInfo.Add("Brand", _brand);
            productInfo.Add("Warranty", _warranty);
            return productInfo;
        }

        // Set a property of the product
        public override void SetProperty(string key, string value)
        {
            base.SetProperty(key, value);
            switch (key)
            {
                case "Brand":
                    _brand = value;
                    break;
                case "Warranty":
                    _warranty = value;
                    break;
            }
        }
    }


    // Class to communicate between multiple TextInput and Button objects, similar to Mediator pattern
    public class TextInputHandler
    {
        //private readonly List<TextInput> _textInputs = new List<TextInput>();
        private TextInput? _activeTextInput;

        public static TextInputHandler Instance { get; } = new TextInputHandler();

        private TextInputHandler() { }

        // A text input requests to start receiving input
        public void RequestStartInput(TextInput textInput)
        {
            RequestStopActiveInput();
            _activeTextInput = textInput;
            textInput.StartInput();
        }

        // Text input handler requests the active text input to stop receiving input
        public void RequestStopActiveInput()
        {
            SplashKit.EndReadingText();
            _activeTextInput?.AcceptInput();
            _activeTextInput?.StopInput();
            _activeTextInput = null;
        }

        // Button requests to stop the active text input and get the text from a particular text input
        public string RequestStopInputAndGetText(TextInput textInput)
        {
            RequestStopActiveInput();
            return textInput.Text;
        }

        // Update the text input handler
        public void Update()
        {
            // Update the active text input
            if (_activeTextInput != null)
            {
                _activeTextInput.CollectedText = SplashKit.TextInput();
            }
            // Handle program unexpectedly stop reading text
            if (_activeTextInput != null && !SplashKit.ReadingText())
            {
                if (!SplashKit.TextEntryCancelled())
                {
                    _activeTextInput.AcceptInput();
                }
                _activeTextInput.StopInput();
                _activeTextInput = null;
            }
        }
    }


    // Class to represent the GUI and manage the UI states
    public class GUI
    {
        private Window _window;
        private UIState _currentState;

        // Singleton instance
        public static GUI Instance { get; } = new GUI();
        // Static properties for fonts
        public static Font Font
        {
            get
            {
                if (!SplashKit.HasFont("inter"))
                {
                    SplashKit.LoadFont("inter", "resources\\font\\Inter.ttf");
                }
                return SplashKit.FontNamed("inter");
            }
        }
        public static Font FontBold
        {
            get
            {
                if (!SplashKit.HasFont("inter-bold"))
                {
                    Font font = SplashKit.LoadFont("inter-bold", "resources\\font\\Inter.ttf");
                    font.Style = FontStyle.BoldFont;
                }
                return SplashKit.FontNamed("inter-bold");
            }
        }

        private GUI()
        {
            _window = new Window("Inventory Management System", 800, 600);
            _currentState = new MainUIState();
        }

        // Run the GUI
        public void Run()
        {
            while (!SplashKit.QuitRequested())
            {
                SplashKit.ProcessEvents();

                _window.Clear(Color.White);

                _currentState.HandleInput();
                _currentState.Update();
                _currentState.Draw();

                _window.Refresh(60);
            }
        }

        // Change the current UI state
        public void ChangeState(UIState newState)
        {
            _currentState = newState;
        }
    }


    // Abstract class to represent a UI component
    public abstract class UIComponent
    {
        private int _x, _y, _width, _height, _fontSize;
        private Color _color, _primaryColor, _secondaryColor;

        protected int X
        {
            get { return _x; }
            set { _x = value; }
        }
        protected int Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        protected int Width
        {
            get { return _width; }
        }
        protected int Height
        {
            get { return _height; }
        }
        protected int FontSize
        {
            get { return _fontSize; }
        }
        protected Color PrimaryColor
        {
            get { return _primaryColor; }
        }
        protected Color SecondaryColor
        {
            get { return _secondaryColor; }
        }

        public UIComponent(int x, int y, int width, int height, int fontSize, string primaryColorHex, string secondaryColorHex)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _fontSize = fontSize;

            if (primaryColorHex.Length == 7)
                primaryColorHex += "FF";
            if (secondaryColorHex.Length == 7)
                secondaryColorHex += "FF";

            _color = _primaryColor = SplashKit.StringToColor(primaryColorHex);
            _secondaryColor = SplashKit.StringToColor(secondaryColorHex);
        }

        // Abstract methods to be implemented by subclasses
        public abstract void Draw();

        // Virtual methods to be overridden by subclasses to draw texture
        public virtual void HandleInput() { }
        public virtual void DrawTexture() { }

        // Methods to check if mouse is hovering over the component
        public bool IsMouseHover()
        {
            Rectangle rect = SplashKit.RectangleFrom(_x, _y, _width, _height);
            return SplashKit.PointInRectangle(SplashKit.MousePosition(), rect);
        }

        // Methods to check if mouse is clicking on the component
        public bool IsMouseClick()
        {
            return SplashKit.MouseClicked(MouseButton.LeftButton) && IsMouseHover();
        }
    }


    // Class to represent a component to display a bitmap
    public class BitmapDisplay : UIComponent
    {
        private Bitmap _bitmap;
        private DrawingOptions _drawingOptions;

        public BitmapDisplay(int x, int y, int width, int height, Bitmap bitmap) : base(x, y, width, height, 0, "#00000000", "#00000000")
        {
            _bitmap = bitmap;
            _drawingOptions = SplashKit.OptionScaleBmp(Width / bitmap.Width, Height / bitmap.Height); // Scale bitmap to fill the width and height
            X = X + Width / 2 - bitmap.Width / 2; // Center the bitmap
            Y = Y + Height / 2 - bitmap.Height / 2; // Center the bitmap
        }

        // Draw the component
        public override void Draw()
        {
            _bitmap.Draw(X, Y, _drawingOptions);
        }
    }


    // Class to represent a button
    public class Button : UIComponent
    {
        private Action _action; // Action to be performed when the button is clicked
        private string _text;

        public Button(int x, int y, int width, int height, string text, Action action) : base(x, y, width, height, (int) ((height > 30) ? 20 : height * 0.7), "#99D4FF", "#5fb1ed")
        {
            _action = action;
            _text = text;
        }

        // Handle input
        public override void HandleInput()
        {
            if (IsMouseClick())
            {
                _action();
            }
            if (IsMouseHover())
            {
                Color = SecondaryColor;
            }
            else
            {
                Color = PrimaryColor;
            }
        }

        // Draw the component
        public override void Draw()
        {
            DrawTexture();
            SplashKit.DrawText(_text, Color.Black, GUI.FontBold, FontSize, X + Width / 2 - SplashKit.TextWidth(_text, GUI.FontBold, FontSize) / 2, Y + Height / 2 - SplashKit.TextHeight(_text, GUI.FontBold, FontSize) / 2);
        }

        // Draw the texture of the button
        public override void DrawTexture()
        {
            SplashKit.DrawRectangle(Color.Black, X, Y, Width, Height);
            SplashKit.FillRectangle(Color, X + 1, Y + 1, Width - 2, Height - 2);
        }
    }


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


    // Class to represent a component to display a summary of the inventory
    public class SummaryBox : UIComponent, IObserver
    {
        private IStrategy _strategy = new TotalAllStrategy(); // Default strategy to summarize

        private List<UIComponent> Components { get; } = new List<UIComponent>();

        public SummaryBox(int x, int y, int width, int height) : base(x, y, width, height, 0, "#000000", "#FF0000")
        {
            Inventory.Instance.Attach(this); // Attach this UI to the Inventory
            GenerateTextDisplay();
        }

        // Draw the component
        public override void Draw()
        {
            Components.ForEach(component => component.Draw());
        }

        // Change the strategy to the next strategy
        public void NextStrategy()
        {
            // Get the next strategy type in the list of strategy types
            Type newStrategyType = Strategy.StrategyType[(Strategy.StrategyType.IndexOf(_strategy.GetType()) + 1) % Strategy.StrategyType.Count];

            // Create a new instance of the strategy
            _strategy = (IStrategy) Activator.CreateInstance(newStrategyType)!;

            GenerateTextDisplay();
        }

        // Update method for the Observer pattern
        public void Update(KeyValuePair<Product, int>? updatedProduct = null)
        {
            _strategy.Update();
            GenerateTextDisplay();
        }

        // Generate the text displays
        public void GenerateTextDisplay()
        {
            Components.Clear();

            int textY = Y;
            int height = 18;

            Components.Add(new TextDisplay(X, textY, Width, height, "Auto Buyer " + (AutoBuyer.Instance.Enabled ? "enabled" : "disabled"), "#0000FF", format: true));
            textY += height;

            _strategy.GenerateSummary().ForEach(summary =>
            {
                Components.Add(new TextDisplay(X, textY, Width, height, summary, "#000000"));
                textY += height;
            });
            _strategy.GenerateAlert().ForEach(alert =>
            {
                Components.Add(new TextDisplay(X, textY, Width, height, alert, "#FF0000"));
                textY += height;
            });
        }
    }


    // Class to represent a component to display text
    public class TextDisplay : UIComponent
    {
        private string _text;
        private Font _font;
        private bool _center, _format;
        private object? _formatObject;

        public TextDisplay(int x, int y, int width, int height, string text, string colorHex, bool bold = false, bool center = false, bool format = false, object? formatObject = null) : base(x, y, width, height, (int) ((height > 50) ? 32 : height * 0.7), colorHex, "#00000000")
        {
            _text = text;
            _font = bold ? GUI.FontBold : GUI.Font;
            _center = center;
            _format = format;
            _formatObject = formatObject;
        }

        // Draw the text with format (if any) in the specified position (horizontally centered or not)
        public override void Draw()
        {
            string text = _format ? string.Format(_text, _formatObject) : _text;

            double drawX = _center ? X + Width / 2 - SplashKit.TextWidth(text, _font, FontSize) / 2 : X;
            double drawY = Y + Height / 2 - SplashKit.TextHeight(text, _font, FontSize) / 2;

            SplashKit.DrawText(text, Color, _font, FontSize, drawX, drawY);
        }

        // Update the format object for the text
        public void UpdateFormatObject(object? formatObject)
        {
            _formatObject = formatObject;
        }
    }


    // Class to represent a component to for user to input text
    public class TextInput : UIComponent
    {
        private string _text = "";
        private string _collectedText = "";
        private bool _isReading = false;
        private readonly Func<char, bool>? _filter;

        public string Text
        {
            get { return _text; }
        }
        public string CollectedText
        {
            get { return _collectedText; }
            set { _collectedText = value; }
        }

        public TextInput(int x, int y, int width, int height, string defaultText = "", Func<char, bool>? filter = null) : base(x, y, width, height, (int) ((height > 30) ? 20 : height * 0.7), "#F0F0F0", "#FFFFFF")
        {
            _text = defaultText;
            _filter = filter;
        }

        // Handle input for the text input
        public override void HandleInput()
        {
            // If the text input is being click, request the text input handler to start reading the input
            if (IsMouseClick())
            {
                TextInputHandler.Instance.RequestStartInput(this);
            }

            // If the text input is being hovered, change the color of the text input
            if (IsMouseHover() || _isReading)
            {
                Color = SecondaryColor;
            }
            else
            {
                Color = PrimaryColor;
            }
        }

        // Draw the text input
        public override void Draw()
        {
            DrawTexture();

            // If the text input is being read, draw the collected text, otherwise draw the set text
            if (_isReading)
                SplashKit.DrawText(_collectedText, Color.Black, GUI.Font, FontSize, X + Width / 2 - SplashKit.TextWidth(_collectedText, GUI.Font, FontSize) / 2, Y + Height / 2 - SplashKit.TextHeight(_collectedText, GUI.Font, FontSize) / 2);
            else
                SplashKit.DrawText(_text, Color.Black, GUI.Font, FontSize, X + Width / 2 - SplashKit.TextWidth(_text, GUI.Font, FontSize) / 2, Y + Height / 2 - SplashKit.TextHeight(_text, GUI.Font, FontSize) / 2);
        }

        // Draw the texture of the text input
        public override void DrawTexture()
        {
            SplashKit.DrawRectangle(Color.Black, X, Y, Width, Height);
            SplashKit.FillRectangle(Color, X + 1, Y + 1, Width - 2, Height - 2);
        }

        // Start reading the input
        public void StartInput()
        {
            SplashKit.StartReadingText(SplashKit.RectangleFrom(X, Y, Width, Height), _text);
            _isReading = true;
        }

        // Stop reading the input
        public void StopInput()
        {
            _collectedText = "";
            _isReading = false;
        }

        // Filter the collected text and set it to the text
        public void AcceptInput()
        {
            if (_filter != null)
            {
                _collectedText = new string(_collectedText.Where(_filter).ToArray());
            }
            _text = _collectedText;
        }
    }


    // Abstract class to represent a UI state
    public abstract class UIState
    {
        private List<UIComponent> _components;

        protected List<UIComponent> Components
        {
            get { return _components; }
        }

        public UIState()
        {
            _components = new List<UIComponent>();
        }

        // Abstract methods to be implemented by subclasses
        public abstract void HandleInput();
        public abstract void Draw();
        public abstract void Update();
    }


    // UI state for the main menu
    public class MainUIState : UIState
    {
        // Initialize the UI state and create the UI components
        public MainUIState() : base()
        {
            SummaryBox _summaryBox = new(50, 100, 200, 150);

            Components.Add(new BitmapDisplay(0, 0, 800, 600, SplashKit.LoadBitmap("background", "resources/images/background.png")));
            Components.Add(new TextDisplay(50, 0, 200, 100, "Summary", "#000000", bold: true, center: true));
            Components.Add(new TextDisplay(300, 0, 200, 100, "Inventory", "#000000", bold: true));
            Components.Add(_summaryBox);
            Components.Add(new InventoryUI(300, 100));

            Components.Add(new Button(50, 275, 200, 50, "Change Strategy", () =>
            {
                _summaryBox.NextStrategy();
            }));

            Components.Add(new Button(50, 350, 200, 50, "Enable Auto-buy", () =>
            {
                AutoBuyer.Instance.Toggle();
                _summaryBox.Update();
            }));

            Components.Add(new Button(50, 425, 200, 50, "Add Product", () =>
            {
                GUI.Instance.ChangeState(new ProductUIState());
            }));

            Components.Add(new Button(50, 500, 200, 50, "Save", () =>
            {
                Inventory.Instance.Save("inventory.txt");
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
            AutoBuyer.Instance.Update();
        }
    }


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


    // Interface for Strategy classes
    public interface IStrategy
    {
        public List<string> GenerateSummary();
        public List<string> GenerateAlert();

        public void Update();
    }


    // Class to hold the strategy types
    public static class Strategy
    {
        // List of strategy types
        public static List<Type> StrategyType { get; } = new()
        {
            typeof(TotalAllStrategy),
            typeof(TotalByTypeStrategy),
            typeof(LeastByTypeStrategy),
            typeof(LeastByItemStrategy)
        };
    }


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


    // Strategy to generate summary and alert on total of all products
    public class TotalAllStrategy : IStrategy
    {
        private int _countAll = 0;
        private int _sumAll = 0;

        public TotalAllStrategy()
        {
            Update();
        }

        // Update strategy
        public void Update()
        {
            _countAll = Inventory.Instance.Products.Count;
            _sumAll = Inventory.Instance.Products.Sum(product => product.Value);
        }

        // Generate summary
        public List<string> GenerateSummary()
        {
            List<string> summary = new()
            {
                $"Inventory has {_countAll} types",
                $"with {_sumAll} products in total"
            };

            return summary;
        }

        // Generate alert if total of all products is less than 10
        public List<string> GenerateAlert()
        {
            List<string> alert = new()
            {
                "Alert on Total of all products"
            };

            bool hasAlert = false;

            if (_sumAll < 10)
            {
                alert.Add($"Low stock ({_sumAll} in total)!");
                hasAlert = true;
            }

            if (!hasAlert)
            {
                alert.Add("No alert");
            }

            return alert;
        }
    }


    // Strategy to generate a summary of the total number of products by type
    public class TotalByTypeStrategy : IStrategy
    {
        private Dictionary<Type, int> _sumTypes = new();

        public TotalByTypeStrategy()
        {
            Update();
        }

        // Update the summary of total products by type
        public void Update()
        {
            _sumTypes.Clear();

            // sum up the total number of products by type
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

        // Generate a summary of the total number of products by type
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

        // Generate an alert if any product type has less than 10 items
        public List<string> GenerateAlert()
        {
            List<string> alert = new()
            {
                "Alert on Total products by type"
            };

            bool hasAlert = false;

            foreach (KeyValuePair<Type, int> sumType in _sumTypes)
            {
                if (sumType.Value < 10)
                {
                    alert.Add($"Low stock for {sumType.Key.Name[..^7]}!");
                    hasAlert = true;
                }
            }

            if (!hasAlert)
            {
                alert.Add("No alert");
            }

            return alert;
        }
    }
}
