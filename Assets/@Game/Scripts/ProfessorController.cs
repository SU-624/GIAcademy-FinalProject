using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatData.Runtime
{
    public class ProfessorController : MonoBehaviour
    {
        [SerializeField] private DataBase m_ProfessorDataBase;

        private Dictionary<string, ProfessorStat> m_ProfessorData = new Dictionary<string, ProfessorStat>();
        public Dictionary<string, ProfessorStat> professorData => m_ProfessorData;

        private bool m_IsInitialized = false;

        private void Awake()
        {
            if(m_IsInitialized == false)
            {
                Initialized();
                m_IsInitialized = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Initialized()
        {
            foreach(ProfessorData professorData in m_ProfessorDataBase.professorDatas)
            {
                m_ProfessorData.Add(professorData.ProfessorName, new ProfessorStat(professorData));
            }
        }


    }
}
