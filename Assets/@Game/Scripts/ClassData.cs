using UnityEngine;

namespace StatData.Runtime
{
    [CreateAssetMenu(fileName = "ClassData", menuName = "StatData/ClassData", order = 0)]

    public class ClassData : ScriptableObject
    {
        [SerializeField] public int m_ClassID;
        [SerializeField] public string m_ClassName;
        [SerializeField] public StudentType m_ClassType;
        [SerializeField] public ClassType m_ClassStatType;
        [SerializeField] public string m_OpenYear;
        [SerializeField] public string m_OpenMonth;
        [SerializeField] public int m_Sense;
        [SerializeField] public int m_Concentration;
        [SerializeField] public int m_Wit;
        [SerializeField] public int m_Technique;
        [SerializeField] public int m_Insight;
        [SerializeField] public int m_Money;
        [SerializeField] public int m_Health;
        [SerializeField] public int m_Passion;
        [SerializeField] public bool m_UnLockClass;

        public static ClassData CreateClassData(int classID, string className, StudentType classType, ClassType classStatType,
        string openyear, string openMonth, int sense, int concentration, int wit,
        int tecnique, int insight, int money, int health, int passion, bool unLock)
        {
            var data = ScriptableObject.CreateInstance<ClassData>();

            data.m_ClassID = classID;
            data.m_ClassName = className;
            data.m_ClassType = classType;
            data.m_ClassStatType = classStatType;
            data.m_OpenYear = openyear;
            data.m_OpenMonth = openMonth;
            data.m_Sense = sense;
            data.m_Concentration = concentration;
            data.m_Wit = wit;
            data.m_Technique = tecnique;
            data.m_Insight = insight;
            data.m_Money = money;
            data.m_Health = health;
            data.m_Passion = passion;
            data.m_UnLockClass = unLock;

            return data;
        }

        public int ClassID => m_ClassID;
        public string ClassName => m_ClassName;
        public StudentType ClassType => m_ClassType;
        public ClassType ClassStatType => m_ClassStatType;
        public string OpenYear => m_OpenYear;
        public string OpentMonth => m_OpenMonth;
        public int Sense => m_Sense;
        public int Concentration => m_Concentration;
        public int Wit => m_Wit;
        public int Technique => m_Technique;
        public int Insight => m_Insight;

        public int Money => m_Money;
        public int Health => m_Health;

        public bool UnLockClass => m_UnLockClass;
    }

    public enum ClassType
    {
        Commonm,
        Class,
        Special,
    }

}