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

        private bool m_IsInitialized = false;
        public bool isInitialized => m_IsInitialized;
        public DataBase dataBase => m_ClassDataBase;

        public event Action initialized;
        public event Action willUnInitialized;

        protected virtual void Awake()
        {
            if(m_IsInitialized == false)
            {
                Initialized();
                m_IsInitialized = true;
                initialized?.Invoke();
            }
        }

        private void OnDestroy()
        {
            willUnInitialized?.Invoke();            
        }

        private void Initialized()
        {
            foreach(ClassData classData in m_ClassDataBase.classDatas)
            {
                classData.Stats.Initailize();
                m_ClassData.Add(classData.ClassName, new Class(classData));
            }
        }

        public void AddStudentToClass()
        {

        }
    }
}