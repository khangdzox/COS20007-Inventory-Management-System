using SplashKitSDK;

namespace InventoryMgmtSys.gui.uicomponent
{
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
}
