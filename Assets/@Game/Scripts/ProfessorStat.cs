using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatData.Runtime
{
    public class ProfessorStat
    {
        //protected ProfessorData m_ProfessorData;

        //���縦 ��� ����Ʈ...?

        public int m_ProfessorID; // ���� ���� ID
        public string m_ProfessorName; // ���� �̸�
        public StudentType m_ProfessorType; // ���� �а� ����
        public string m_ProfessorSet; // �ܷ�, ���Ӱ��� ����
        public int m_ProfessorPower; // ���Ƿ�
        public int m_ProfessorExperience; // ����ġ

        public List<string> m_ProfessorSkills = new List<string>(); // ��ų
        public int m_ProfessorPay; // ����
        public int m_ProfessorHealth; // ü��
        public int m_ProfessorPassion; // ����

        // �ӽ� ���� �̹��� ������ �ֱ� ���� ����
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