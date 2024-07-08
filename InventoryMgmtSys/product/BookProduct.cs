namespace InventoryMgmtSys.product
{
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
}
