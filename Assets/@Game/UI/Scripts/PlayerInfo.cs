using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Mang 11. 08
/// 
/// �÷��̾� ��ü�� ����
/// ���⿡ �÷��̾ ������� �����͵��� �� �־�θ� �������� ����������Ұ� ��
/// </summary>
public class PlayerInfo
{
    // 5/15 woodpie9 
    // public List<StudentSaveData> studentData = new List<StudentSaveData>();

    private static PlayerInfo instance = null;       // Manager ������ �̱������� ���

    public static PlayerInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerInfo();
            }
            return instance;
        }
    }

    // �α��� ������
    // public string m_PlayerID;  // �̻��...
    public string m_AcademyName;
    public string m_TeacherName;

    public int m_MyMoney;
    public int m_SpecialPoint;              // 2�� ��ȭ�Դϴ�
    public int m_AcademyScore;
    public string m_PlayerServer;           // ���� ��𿡴� �����ߴ���?

    public int m_Awareness;                 // ������ - ����
    public int m_TalentDevelopment;         // ���� �缺
    public int m_Management;                // �
    public int m_Activity;                  // Ȱ������
    public int m_Goods;                     // ��ȭ����
    public Rank m_CurrentRank;

    // Ʃ�丮�� ����
    public bool IsFirstAcademySetting;
    public bool IsFirstClassSetting;
    public bool IsFirstClassSettingPDEnd;
    public bool IsFirstGameJam;
    public bool IsFirstClassEnd;

    // ����Ʈ
    public bool[] isMissionClear;               // ����Ʈ Ŭ���� ������ ����

    // Ķ����
    public int StudentProfileClickCount;        // �л� �� ������ Ȯ�� ī��Ʈ
    public int TeacherProfileClickCount;        // ���� �� ������ Ȯ�� ī��Ʈ
    public int GenreRoomClickCount;             // ��� �帣�� ��ġ ī��Ʈ

    public int UnLockTeacherCount;



    // ���� ����
    // ���� �л�
    // ������
}
