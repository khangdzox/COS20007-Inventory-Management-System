namespace InventoryMgmtSys.strategy
{
    // Class to hold the strategy types
    public static class Strategy
    {
        // List of strategy types
        public static List<Type> StrategyType { get; } = new()
        {
            typeof(TotalAllStrategy),
            typeof(TotalByTypeStrategy),
            typeof(LeastByTypeStrategy),
            typeof(LeastByItemStrategy)
        };
    }
}
