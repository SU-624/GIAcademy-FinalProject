using UnityEngine;

namespace Conditiondata.Runtime
{
    [CreateAssetMenu(fileName = "ConditionData", menuName = "ConditionData/Condition", order = 0)]

    public class ConditionData : ScriptableObject
    {
        [SerializeField] private int m_Hungry;
        [SerializeField] private int m_Tired;

        public int Hungry => m_Hungry;
        public int Tired => m_Tired;
    }
}
