using InventoryMgmtSys.gui.uicomponent;
using SplashKitSDK;

namespace InventoryMgmtSys.gui.uistate
{
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
}
