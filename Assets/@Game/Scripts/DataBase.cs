using System.Collections.Generic;
using UnityEngine;
using Conditiondata.Runtime;

namespace StatData.Runtime
{
    [CreateAssetMenu(fileName = "DataBase", menuName = "StatData/DataBase", order = 0)]

    public class DataBase : ScriptableObject
    {
        public List<ProfessorData> professorDatas;
        public List<StudentData> studentDatas;
        public List<ClassData> classDatas;
        public List<ConditionData> studentCondition;
    }
}