using SplashKitSDK;

namespace InventoryMgmtSys.gui.uicomponent
{
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
}
