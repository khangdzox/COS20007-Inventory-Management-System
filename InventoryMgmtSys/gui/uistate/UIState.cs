using InventoryMgmtSys.gui.uicomponent;

namespace InventoryMgmtSys.gui.uistate
{
    // Abstract class to represent a UI state
    public abstract class UIState
    {
        private List<UIComponent> _components;

        protected List<UIComponent> Components
        {
            get { return _components; }
        }

        public UIState()
        {
            _components = new List<UIComponent>();
        }

        // Abstract methods to be implemented by subclasses
        public abstract void HandleInput();
        public abstract void Draw();
        public abstract void Update();
    }
}
