using System.Collections.Generic;

namespace StatData.Runtime
{
    public enum ModifierStatType
    {
        System,
        Contents,
        Balance,
    }

    [System.Serializable]
    public class StatModifier
    {
        public int systemValue;
        public int contentsValue;
        public int balanceValue;

        public Dictionary<ModifierStatType, int> StatsModifierInfo = new Dictionary<ModifierStatType, int>();

        public void Initailize()
        {
            if (StatsModifierInfo.Count != 0)
            {
                return;
            }
            else
            {
                StatsModifierInfo.Add(ModifierStatType.System, systemValue);
                StatsModifierInfo.Add(ModifierStatType.Contents, contentsValue);
                StatsModifierInfo.Add(ModifierStatType.Balance, balanceValue);
            }
        }
    }
}