using InventoryMgmtSys.gui.uicomponent;
using SplashKitSDK;

namespace InventoryMgmtSys.gui
{
    // Class to communicate between multiple TextInput and Button objects, similar to Mediator pattern
    public class TextInputHandler
    {
        //private readonly List<TextInput> _textInputs = new List<TextInput>();
        private TextInput? _activeTextInput;

        public static TextInputHandler Instance { get; } = new TextInputHandler();

        private TextInputHandler() { }

        // A text input requests to start receiving input
        public void RequestStartInput(TextInput textInput)
        {
            RequestStopActiveInput();
            _activeTextInput = textInput;
            textInput.StartInput();
        }

        // Text input handler requests the active text input to stop receiving input
        public void RequestStopActiveInput()
        {
            SplashKit.EndReadingText();
            _activeTextInput?.AcceptInput();
            _activeTextInput?.StopInput();
            _activeTextInput = null;
        }

        // Button requests to stop the active text input and get the text from a particular text input
        public string RequestStopInputAndGetText(TextInput textInput)
        {
            RequestStopActiveInput();
            return textInput.Text;
        }

        // Update the text input handler
        public void Update()
        {
            // Update the active text input
            if (_activeTextInput != null)
            {
                _activeTextInput.CollectedText = SplashKit.TextInput();
            }
            // Handle program unexpectedly stop reading text
            if (_activeTextInput != null && !SplashKit.ReadingText())
            {
                if (!SplashKit.TextEntryCancelled())
                {
                    _activeTextInput.AcceptInput();
                }
                _activeTextInput.StopInput();
                _activeTextInput = null;
            }
        }
    }
}
