
using System.Collections.Generic;

namespace StatData.Runtime
{
    public class ProfessorStat
    {
        private static ProfessorStat _instance;

        protected ProfessorData m_ProfessorData;

        public string m_ProfessorNameValue;
        public StudentType m_ProfessorType;
        public string m_ProfessorSet;
        public int m_ProfessorPower;
        public List<string> m_ProfessorSkills = new List<string>();
        public int m_ProfessorPay;
        public int m_ProfessorHealth;
        public int m_ProfessorPassion;

        public string professorName => m_ProfessorData.ProfessorName;
        public StudentType professorType => m_ProfessorData.ProfessorType;
        public string professorSet => m_ProfessorData.ProfessorSet;
        public List<string> professorSkills => m_ProfessorData.ProfessorSkills;
        public int professorPower => m_ProfessorData.ProfessorPower;
        public int professorPay => m_ProfessorData.ProfessorPay;
        public int professorHealth => m_ProfessorData.ProfessorHealth;
        public int professorPassion => m_ProfessorData.ProfessorPassion;

        public ProfessorStat()
        {

        }

        public ProfessorStat(ProfessorData _professorData)
        {
            m_ProfessorData = _professorData;
            m_ProfessorNameValue = professorName;
            m_ProfessorType = professorType;
            m_ProfessorSet = professorSet;
            m_ProfessorPower = professorPower;
            m_ProfessorSkills = professorSkills;
            m_ProfessorPay = professorPay;
            m_ProfessorHealth = professorHealth;
            m_ProfessorPassion = professorPassion;
        }

        public static ProfessorStat Instance()
        {
            if (_instance == null)
            {
                _instance = new ProfessorStat();
            }
            return _instance;
        }
    }
}
