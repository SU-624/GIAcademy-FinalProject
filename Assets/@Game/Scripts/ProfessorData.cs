using System.Collections.Generic;
using UnityEngine;

namespace StatData.Runtime
{
    [CreateAssetMenu(fileName = "ProfessorData", menuName = "StatData/ProfessorData", order = 0)]

    public class ProfessorData : ScriptableObject
    {
        [SerializeField] private string m_ProfessorName;
        [SerializeField] private StudentType m_ProfessorType;
        [SerializeField] private string m_ProfessorSet;
        [SerializeField] private int m_ProfessorPower;
        [SerializeField] private List<string> m_ProfessorSkills = new List<string>();
        [SerializeField] private int m_ProfessorPay;
        [SerializeField] private int m_ProfessorHealth;
        [SerializeField] private int m_ProfessorPassion;

        public string ProfessorName => m_ProfessorName;
        public StudentType ProfessorType => m_ProfessorType;
        public List<string> ProfessorSkills => m_ProfessorSkills;
        public string ProfessorSet => m_ProfessorSet;
        public int ProfessorPower => m_ProfessorPower;
        public int ProfessorPay => m_ProfessorPay;
        public int ProfessorHealth => m_ProfessorHealth;
        public int ProfessorPassion => m_ProfessorPassion;
    }
}