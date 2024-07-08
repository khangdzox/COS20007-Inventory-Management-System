using SplashKitSDK;

namespace InventoryMgmtSys.gui.uicomponent
{
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
}
