using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace StatData.Runtime
{
    [CreateAssetMenu(fileName = "DataBase", menuName = "StatData/DataBase", order = 0)]

    /// 수업 데이터를 json으로 받은 후 여기에 넣어주는 커스텀 에디터
    /// 폴더에 있는 ScriptableObject를 다 지워주고 다시 만든 후 DataBase에 넣어준다.
    public class DataBase : ScriptableObject
    {
        public List<ProfessorData> professorDatas;
        public List<ClassData> classDatas;

#if UNITY_EDITOR
        private StudentType FindClassType(int classType)
        {
            switch (classType)
            {
                case 0:
                {
                    return StudentType.None;
                }

                case 1:
                {
                    return StudentType.GameDesigner;
                }

                case 2:
                {
                    return StudentType.Art;
                }

                case 3:
                {
                    return StudentType.Programming;
                }

            }

            return StudentType.None;
        }

        private ClassType FindClassStatType(string _statTypeName)
        {
            switch (_statTypeName)
            {
                case "공통":
                {
                    return ClassType.Commonm;
                }

                case "특수":
                {
                    return ClassType.Special;
                }

                case "기획":
                {
                    return ClassType.Class;
                }

                case "아트":
                {
                    return ClassType.Class;
                }

                case "프로그래밍":
                {
                    return ClassType.Class;
                }
            }

            return ClassType.Commonm;
        }

        [ContextMenu("Load Json")]
        public void LoadJson()
        {
            TextAsset ClassInfoTextFiel = Resources.Load<TextAsset>("Json/ClassDataInfo");
            string text = ClassInfoTextFiel.text;

            int i = 0;
            var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClassInfoData>>(text);
  
            string[] classDataFolder = { "Assets/@Game/Scripts/ScriptableObject/ClassScriptableObject" };
            classDatas.Clear();

            foreach (var item in AssetDatabase.FindAssets("", classDataFolder))
            {
                var path = AssetDatabase.GUIDToAssetPath(item);
                AssetDatabase.DeleteAsset(path);
            }

            foreach (var item in temp)
            {
                i += 1;

                ClassData instance = ClassData.CreateClassData(
                        item.ClassID, item.ClassName, FindClassType(item.ClassType_ID), FindClassStatType(item.ClassStatType),
                        item.OpenYear, item.OpenMonth,
                        item.Sense, item.Concentration, item.Wit, item.Technique, item.Insight,
                        item.Money, item.Health, item.Passion, item.Class_Unlock);

                Debug.Log(item.ClassStatType);

                AssetDatabase.CreateAsset(
                    instance,
                    "Assets/@Game/Scripts/ScriptableObject/ClassScriptableObject/" + item.ClassStatType + i + ".asset");

                classDatas.Add(instance);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}