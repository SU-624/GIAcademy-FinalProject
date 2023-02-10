using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentSkills
{
    public string m_SkillID;
    public SkillType m_SkillType;
    // ũ�Ⱑ ���� �������� ���� �����̶� short�� ����
    public Int16 m_UnLockType;
    public Int16 m_UnLock1;
    public Int16 m_UnLock2;
    public Int16 m_UnLock3;
    public Int16 m_Extinction;
    public Int16 m_SkilllevelCondition1;
    public Int16 m_SkilllevelCondition2;
    public Int16 m_SkilllevelCondition3;
    public Int16 m_SkillBonus;
}

public enum SkillType
{
    Common,
    Art,
    Programming,
    GameDesigner
}