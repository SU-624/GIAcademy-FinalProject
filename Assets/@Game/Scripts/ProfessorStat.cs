
namespace StatData.Runtime
{
    public class ProfessorStat
    {
        protected ProfessorData m_ProfessorData;

        public int m_ProfessorSystemValue;
        public int m_ProfessorContentsValue;
        public int m_ProfessorBalanceValue;
        public Type m_ProfessorType;

        public int professorSystem => m_ProfessorData.ProfessorSystem;
        public int professorContents => m_ProfessorData.ProfessorContents;
        public int professorBalance => m_ProfessorData.ProfessorBalance;
        public Type professorType => m_ProfessorData.ProfessorType;

        public ProfessorStat(ProfessorData _professorData)
        {
            m_ProfessorData = _professorData;
            m_ProfessorSystemValue = professorSystem;
            m_ProfessorContentsValue = professorContents;
            m_ProfessorBalanceValue = professorBalance;
            m_ProfessorType = professorType;
        }
    }
}
