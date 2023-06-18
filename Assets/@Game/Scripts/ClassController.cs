using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatData.Runtime
{
    public class ClassController : MonoBehaviour
    {
        [SerializeField] private DataBase m_ClassDataBase;

        private Dictionary<string, Class> m_ClassData = new Dictionary<string, Class>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Class> classData => m_ClassData;

        public DataBase dataBase => m_ClassDataBase;

        protected virtual void Awake()
        {
            Initialized();
        }

        private void Initialized()
        {
            foreach (ClassData classData in m_ClassDataBase.classDatas)
            {
                m_ClassData.Add(classData.ClassName, new Class(classData));
            }
        }
    }
}