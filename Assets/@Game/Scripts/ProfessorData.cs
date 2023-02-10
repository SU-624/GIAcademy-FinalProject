using UnityEngine;

namespace StatData.Runtime
{
    [CreateAssetMenu(fileName = "ProfessorData", menuName = "StatData/ProfessorData", order = 0)]

    public class ProfessorData : ScriptableObject
    {
        [SerializeField] private string m_ProfessorName;
        [SerializeField] private int m_ProfessorSystem;
        [SerializeField] private int m_ProfessorContents;
        [SerializeField] private int m_ProfessorBalance;
        [SerializeField] private StudentType m_ProfessorType;


        public string ProfessorName => m_ProfessorName;
        public int ProfessorSystem => m_ProfessorSystem;
        public int ProfessorContents => m_ProfessorContents;
        public int ProfessorBalance => m_ProfessorBalance;

        public StudentType ProfessorType => m_ProfessorType;
    }
}