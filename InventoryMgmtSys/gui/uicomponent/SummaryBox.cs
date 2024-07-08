using InventoryMgmtSys.product;
using InventoryMgmtSys.strategy;

namespace InventoryMgmtSys.gui.uicomponent
{
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
}
