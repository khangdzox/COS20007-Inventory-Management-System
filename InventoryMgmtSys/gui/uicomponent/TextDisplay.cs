using SplashKitSDK;

namespace InventoryMgmtSys.gui.uicomponent
{
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
}
