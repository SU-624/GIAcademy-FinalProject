using UnityEngine;

namespace StatData.Runtime
{
    [CreateAssetMenu(fileName = "ClassData", menuName = "StatData/ClassData", order = 0)]

    public class ClassData : ScriptableObject
    {
        [SerializeField] private string m_ClassName;
        [SerializeField] private StudentType m_ClassType;
        [SerializeField] private ClassType m_ClassStatType;
        [SerializeField] private int m_OpenMonth;
        [SerializeField] private int m_Sense;
        [SerializeField] private int m_Concentration;
        [SerializeField] private int m_Wit;
        [SerializeField] private int m_Technique;
        [SerializeField] private int m_Insight;

        [SerializeField] private int m_Money;
        [SerializeField] private int m_Health;


        public string ClassName => m_ClassName;
        public StudentType ClassType => m_ClassType;
        public ClassType ClassStatType => m_ClassStatType;
        public int OpentMonth => m_OpenMonth;
        public int Sense => m_Sense;
        public int Concentration => m_Concentration;
        public int Wit => m_Wit;
        public int Technique => m_Technique;
        public int Insight => m_Insight;

        public int Money => m_Money;
        public int Health => m_Health;
    }

    public enum ClassType
    {
        Commonm,
        Class,
        Special,
    }

}