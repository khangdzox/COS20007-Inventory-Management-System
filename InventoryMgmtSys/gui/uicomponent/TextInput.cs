using SplashKitSDK;

namespace InventoryMgmtSys.gui.uicomponent
{
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
}
