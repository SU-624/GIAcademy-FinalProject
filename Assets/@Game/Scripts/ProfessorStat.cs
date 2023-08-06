using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatData.Runtime
{
    public class ProfessorStat
    {
        //protected ProfessorData m_ProfessorData;

        //강사를 담는 리스트...?

        public int m_ProfessorID; // 강사 고유 ID
        public string m_ProfessorName; // 강사 이름
        public StudentType m_ProfessorType; // 강사 학과 구별
        public string m_ProfessorSet; // 외래, 전임강사 구별
        public int m_ProfessorPower; // 강의력
        public int m_ProfessorExperience; // 경험치

        public List<string> m_ProfessorSkills = new List<string>(); // 스킬
        public int m_ProfessorPay; // 월급
        public int m_ProfessorHealth; // 체력
        public int m_ProfessorPassion; // 열정

        // 임시 강사 이미지 데이터 넣기 위한 변수
        public Sprite m_TeacherProfileImg;
        public bool m_IsUnLockProfessor;


        public ProfessorStat()
        {
        }

        public ProfessorStat(ProfessorData _professorData)
        {
            //m_ProfessorData = _professorData;
            m_ProfessorID = _professorData.ProfessorID;
            m_ProfessorName = _professorData.ProfessorName;
            m_ProfessorType = _professorData.ProfessorType;
            m_ProfessorSet = _professorData.ProfessorSet;
            m_ProfessorPower = _professorData.ProfessorPower;
            m_ProfessorExperience = 0;
            
            m_ProfessorSkills = _professorData.ProfessorSkills;
            m_ProfessorPay = _professorData.ProfessorPay;
            m_ProfessorHealth = _professorData.ProfessorHealth;
            m_ProfessorPassion = _professorData.ProfessorPassion;
            m_IsUnLockProfessor = _professorData.IsUnlockProfessor;

            foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
            {
                if (m_ProfessorName == temp.ToString())
                {
                    m_TeacherProfileImg = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                }
            }
        }
    }
}