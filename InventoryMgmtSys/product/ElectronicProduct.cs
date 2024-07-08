namespace InventoryMgmtSys.product
{
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
}
