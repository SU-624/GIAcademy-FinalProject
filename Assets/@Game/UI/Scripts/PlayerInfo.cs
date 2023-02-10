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
    Json jsonTest = new Json();

    public List<StudentSaveData> studentData = new List<StudentSaveData>();

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
    public string m_PlayerID;
    public string m_AcademyName;
    public string m_DirectorName;

    public string m_PlayerServer;

    public int m_MyMoney = 10000;
    // ��¥
    public int m_Year;
    public int m_Month;
    public int m_Week;
    public int m_Day;

    //public int[,] testArr = new int[3, 4];



    // ����Ʈ
    // Ķ����

    // ���� ����
    // ���� �л�
    // ������

    public void SaveNowTime()
    {
        m_Year = GameTime.Instance.FlowTime.NowYear;
        m_Month = GameTime.Instance.FlowTime.NowMonth;
        m_Week = GameTime.Instance.FlowTime.NowWeek;
        m_Day = GameTime.Instance.FlowTime.NowDay;
    }

}
