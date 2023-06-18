using System.Collections.Generic;
using UnityEngine;

namespace StatData.Runtime
{
    [CreateAssetMenu(fileName = "ProfessorData", menuName = "StatData/ProfessorData", order = 0)]

    public class ProfessorData : ScriptableObject
    {
        [SerializeField] private int m_ProfessorID;
        [SerializeField] private string m_ProfessorName;
        [SerializeField] private StudentType m_ProfessorType;
        [SerializeField] private string m_ProfessorSet;
        [SerializeField] private int m_ProfessorPower;
        [SerializeField] private List<string> m_ProfessorSkills = new List<string>();
        [SerializeField] private int m_ProfessorPay;
        [SerializeField] private int m_ProfessorHealth;
        [SerializeField] private int m_ProfessorPassion;
        [SerializeField] private bool m_IsUnLockProfessor;

        public int ProfessorID => m_ProfessorID;
        public string ProfessorName => m_ProfessorName;
        public StudentType ProfessorType => m_ProfessorType;
        public List<string> ProfessorSkills => m_ProfessorSkills;
        public string ProfessorSet => m_ProfessorSet;
        public int ProfessorPower => m_ProfessorPower;
        public int ProfessorPay => m_ProfessorPay;

        public bool IsUnLockProfessor => m_IsUnLockProfessor;

        public int ProfessorHealth
        {
            get => m_ProfessorHealth;
            set => m_ProfessorHealth = value;
        }

        public int ProfessorPassion
        {
            get => m_ProfessorPassion;
            set => m_ProfessorPassion = value;
        }


    }
}