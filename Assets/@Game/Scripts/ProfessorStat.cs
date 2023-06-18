
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatData.Runtime
{
    public class ProfessorStat
    {
        protected ProfessorData m_ProfessorData;

        //강사를 담는 리스트...?

        public int m_ProfessorID;               // 강사 고유 ID
        public string m_ProfessorNameValue;     // 강사 이름
        public StudentType m_ProfessorType;     // 강사 학과 구별
        public string m_ProfessorSet;           // 외래, 전임강사 구별
        public int m_ProfessorPower;            // 강의력
        public int m_ProfessorExperience;       // 경험치
        
        public List<string> m_ProfessorSkills = new List<string>(); // 스킬
        public int m_ProfessorPay;              // 월급
        public int m_ProfessorHealth;           // 체력
        public int m_ProfessorPassion;          // 열정

        // 임시 강사 이미지 데이터 넣기 위한 변수
        public Sprite m_TeacherProfileImg;

        public bool m_IsUnLockProfessor;

        // 필요한 변수들
        // 강사 레벨
        // 강사 기분
        // 0517 woodpie9 데이터 추가시 알려주세요.

        public int professorID => m_ProfessorData.ProfessorID;
        public string professorName => m_ProfessorData.ProfessorName;
        public StudentType professorType => m_ProfessorData.ProfessorType;
        public string professorSet => m_ProfessorData.ProfessorSet;
        public List<string> professorSkills => m_ProfessorData.ProfessorSkills;
        public int professorPower => m_ProfessorData.ProfessorPower;
        public int professorPay => m_ProfessorData.ProfessorPay;

        public Sprite TeacherProfileImg => m_TeacherProfileImg;

        public bool IsUnLockProfessor => m_ProfessorData.IsUnLockProfessor;

        public int professorHealth
        {
            get => m_ProfessorData.ProfessorHealth;
            set => m_ProfessorData.ProfessorHealth = value;
        }

        public int professorPassion
        {
            get => m_ProfessorData.ProfessorPassion;
            set => m_ProfessorData.ProfessorPassion = value;
        }

        public ProfessorStat()
        {

        }

        public ProfessorStat(ProfessorData _professorData)
        {
            // m_ProfessorID = professorID;
            m_ProfessorData = _professorData;
            m_ProfessorNameValue = professorName;
            m_ProfessorType = professorType;
            m_ProfessorSet = professorSet;
            m_ProfessorPower = professorPower;
            m_ProfessorSkills = professorSkills;
            m_ProfessorPay = professorPay;
            m_ProfessorHealth = professorHealth;
            m_ProfessorPassion = professorPassion;
            m_ProfessorExperience = 0;

            foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
            {
                if (m_ProfessorNameValue == temp.ToString())
                {
                    m_TeacherProfileImg = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                }
            }
        }
    }
}
