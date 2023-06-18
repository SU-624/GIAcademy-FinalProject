
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatData.Runtime
{
    public class ProfessorStat
    {
        protected ProfessorData m_ProfessorData;

        //���縦 ��� ����Ʈ...?

        public int m_ProfessorID;               // ���� ���� ID
        public string m_ProfessorNameValue;     // ���� �̸�
        public StudentType m_ProfessorType;     // ���� �а� ����
        public string m_ProfessorSet;           // �ܷ�, ���Ӱ��� ����
        public int m_ProfessorPower;            // ���Ƿ�
        public int m_ProfessorExperience;       // ����ġ
        
        public List<string> m_ProfessorSkills = new List<string>(); // ��ų
        public int m_ProfessorPay;              // ����
        public int m_ProfessorHealth;           // ü��
        public int m_ProfessorPassion;          // ����

        // �ӽ� ���� �̹��� ������ �ֱ� ���� ����
        public Sprite m_TeacherProfileImg;

        public bool m_IsUnLockProfessor;

        // �ʿ��� ������
        // ���� ����
        // ���� ���
        // 0517 woodpie9 ������ �߰��� �˷��ּ���.

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
