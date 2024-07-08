using SplashKitSDK;

namespace InventoryMgmtSys.product
{
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
}
