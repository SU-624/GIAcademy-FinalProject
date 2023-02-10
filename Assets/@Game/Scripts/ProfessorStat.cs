
namespace StatData.Runtime
{
    public class ProfessorStat
    {
        protected ProfessorData m_ProfessorData;

        public string m_ProfessorNameValue;
        public int m_ProfessorSystemValue;
        public int m_ProfessorContentsValue;
        public int m_ProfessorBalanceValue;
        public StudentType m_ProfessorType;

        public string professorName => m_ProfessorData.ProfessorName;
        public int professorSystem => m_ProfessorData.ProfessorSystem;
        public int professorContents => m_ProfessorData.ProfessorContents;
        public int professorBalance => m_ProfessorData.ProfessorBalance;
        public StudentType professorType => m_ProfessorData.ProfessorType;

        public ProfessorStat(ProfessorData _professorData)
        {
            m_ProfessorData = _professorData;
            m_ProfessorNameValue = professorName;
            m_ProfessorSystemValue = professorSystem;
            m_ProfessorContentsValue = professorContents;
            m_ProfessorBalanceValue = professorBalance;
            m_ProfessorType = professorType;
        }
    }
}
