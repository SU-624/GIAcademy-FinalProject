
namespace StatData.Runtime
{
    /// <summary>
    /// 에셋에 있는 학생의 스탯을 불러오기 위한 클래스
    /// 
    /// </summary>
    public class StudentStat
    {
        protected StudentData m_StudentData;
        protected ProfessorData m_ProfessorData;

        public int m_StudentSystemValue;
        public int m_StudentContentsValue;
        public int m_StudentBalanceValue;
        public Type m_StudentType;

        public int studentSystem => m_StudentData.StudentSystem;
        public int studentContents => m_StudentData.StudentContents;
        public int studentBalance => m_StudentData.StudentBalance;
        public Type studentType => m_StudentData.StudentType;

        public StudentStat(StudentData _studentData)
        {
            m_StudentData = _studentData;
            m_StudentSystemValue = studentSystem;
            m_StudentContentsValue = studentContents;
            m_StudentBalanceValue = studentBalance;
            m_StudentType = studentType;
        }

        public StudentStat(ProfessorData _professorData)
        {
            m_ProfessorData = _professorData;
        }
    }
}
