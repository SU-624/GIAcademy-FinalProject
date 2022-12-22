
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
        //public int professorType => m_ProfessorData.ProfessorSystem;

        public ProfessorStat(ProfessorData _professorData)
        {
            m_ProfessorData = _professorData;
        }
    }
}
