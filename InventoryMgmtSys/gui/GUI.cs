using InventoryMgmtSys.gui.uistate;
using SplashKitSDK;

namespace InventoryMgmtSys.gui
{
    // Class to represent the GUI and manage the UI states
    public class GUI
    {
        private Window _window;
        private UIState _currentState;

        // Singleton instance
        public static GUI Instance { get; } = new GUI();
        // Static properties for fonts
        public static Font Font
        {
            get
            {
                if (!SplashKit.HasFont("inter"))
                {
                    SplashKit.LoadFont("inter", "resources\\font\\Inter.ttf");
                }
                return SplashKit.FontNamed("inter");
            }
        }
        public static Font FontBold
        {
            get
            {
                if (!SplashKit.HasFont("inter-bold"))
                {
                    Font font = SplashKit.LoadFont("inter-bold", "resources\\font\\Inter.ttf");
                    font.Style = FontStyle.BoldFont;
                }
                return SplashKit.FontNamed("inter-bold");
            }
        }

        private GUI()
        {
            _window = new Window("Inventory Management System", 800, 600);
            _currentState = new MainUIState();
        }

        // Run the GUI
        public void Run()
        {
            while (!SplashKit.QuitRequested())
            {
                SplashKit.ProcessEvents();

                _window.Clear(Color.White);

                _currentState.HandleInput();
                _currentState.Update();
                _currentState.Draw();

                _window.Refresh(60);
            }
        }

        // Change the current UI state
        public void ChangeState(UIState newState)
        {
            _currentState = newState;
        }
    }
}
