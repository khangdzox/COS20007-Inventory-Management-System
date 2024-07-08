namespace InventoryMgmtSys.strategy
{
    // Interface for Strategy classes
    public interface IStrategy
    {
        public List<string> GenerateSummary();
        public List<string> GenerateAlert();

        public void Update();
    }
}
