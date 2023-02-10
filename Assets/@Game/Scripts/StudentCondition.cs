
namespace Conditiondata.Runtime
{
    /// <summary>
    /// 각 학생들이 들고있을 컨디션 데이터를 관리해주는 클래스
    /// 
    /// 2022.11.05
    /// </summary>
    public class StudentCondition
    {
        private ConditionData m_ConditionData;
        
        public int m_StudentHungryValue;
        public int m_StudentTiredValue;

        public int StudentHungry => m_ConditionData.Hungry;
        public int StudentTired => m_ConditionData.Tired;

        public StudentCondition(ConditionData _conditionData)
        {
            m_ConditionData = _conditionData;
            m_StudentHungryValue = StudentHungry;
            m_StudentTiredValue = StudentTired;
        }

        // 현재는 시간에 따라 컨디션이 줄어들 수 있게 해주는 함수
        //public void AutomaticCondition(int _hungry, int _tired)
        //{
        //    if (m_StudentHungryValue > 0 && m_StudentTiredValue > 0)
        //    {
        //        m_StudentHungryValue -= _hungry;
        //        m_StudentTiredValue -= _tired;

        //        InGameTest.Instance.CheckStudentInfo();
        //        InGameTest.Instance.SelectClassAndStudent();
        //    }
        //}
    }
}


