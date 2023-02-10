using UnityEngine;

namespace StatData.Runtime
{
    [CreateAssetMenu(fileName = "ClassData", menuName = "StatData/ClassData", order = 0)]

    public class ClassData : ScriptableObject
    {
        [SerializeField] private string m_ClassName;
        [SerializeField] private StatModifier m_Stats;
        [SerializeField] private StudentType m_ClassType;

        public string ClassName => m_ClassName;
        public StatModifier Stats => m_Stats;
        public StudentType ClassType => m_ClassType;
    }


}