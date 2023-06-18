
namespace StatData.Runtime
{
    public class Class
    {
        private static Class _instance;

        protected ClassData m_ClassData;

        public int m_ClassID => m_ClassData.ClassID;
        public string m_ClassName => m_ClassData.ClassName;
          
        public StudentType m_ClassType => m_ClassData.ClassType;
        public ClassType m_ClassStatType => m_ClassData.ClassStatType;
        public string m_OpenMonth => m_ClassData.OpentMonth;
        public string m_OpenYear => m_ClassData.OpenYear;

        public int m_Sense => m_ClassData.Sense;
        public int m_Concentration => m_ClassData.Concentration;
        public int m_Wit => m_ClassData.Wit;
        public int m_Technique => m_ClassData.Technique;
        public int m_Insight => m_ClassData.Insight;

        public int m_Money => m_ClassData.Money;
        public int m_Health => m_ClassData.Health;

        public bool m_UnLockClass => m_ClassData.UnLockClass;

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
