
namespace StatData.Runtime
{
    public class Class
    {
        private static Class _instance;

        protected ClassData m_ClassData;

        public string m_ClassName => m_ClassData.ClassName;
        
        public int m_ClassSystemValue => m_ClassData.Stats.StatsModifierInfo[ModifierStatType.System];
        public int m_ClassContentsValue => m_ClassData.Stats.StatsModifierInfo[ModifierStatType.Contents];
        public int m_ClassBalanceValue => m_ClassData.Stats.StatsModifierInfo[ModifierStatType.Balance];
            
        public Type m_ClassType => m_ClassData.ClassType;

        public Class()
        {
        }

        public Class(ClassData _classData)
        {
            m_ClassData = _classData;
        }

        public static Class Instance()
        {
            if(_instance == null)
            {
                _instance = new Class();
            }
            return _instance;
        }
    }
}
