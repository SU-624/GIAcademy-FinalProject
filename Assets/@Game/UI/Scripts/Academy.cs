using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json;

using Conditiondata.Runtime;

/// <summary>
/// 
/// </summary>
public class Academy : MonoBehaviour
{
    public TMP_InputField m_Academy;
    public TMP_InputField m_Director;

    private string m_AcademyName;
    private string m_DirectorName;
    private int m_Money;

    public TextMeshProUGUI m_PopupNotice;                       // 안내문구 띄워줄 변수

    #region AcademyData
    public string MyAcademy
    {
        get { return m_AcademyName; }
        private set { m_AcademyName = value; }
    }
    public string MyDirector
    {
        get { return m_DirectorName; }
        private set { m_DirectorName = value; }
    }
    public int MyMoney
    {
        get { return m_Money; }
        private set { m_Money = value; }
    }
    #endregion

    // 아카데미이름 & 원장 이름 세팅 ( 현재 Title씬 - SetOK 버튼 누르면 실행)
    public void SetAcademyData()
    {
        if (m_Academy.text != "" && m_Director.text != "")
        {
            MyAcademy = m_Academy.text;
            MyDirector = m_Director.text;

            Debug.Log("AcademyName : " + MyAcademy);
            Debug.Log("DirectorName : " + MyDirector);
            PlayerInfo.Instance.m_AcademyName = MyAcademy;
            PlayerInfo.Instance.m_DirectorName = MyDirector;
            ///////////////////////


            StudentSaveData data = new StudentSaveData();
            data.studentStat = new StudentStat();

            data.studentStat.m_StudentName = "김창우";
            data.studentStat.m_StudentType = StudentType.Programming;

            StudentSaveData data1 = new StudentSaveData();
            data1.studentStat = new StudentStat();

            data1.studentStat.m_StudentName = "최재영";
            data1.studentStat.m_StudentType = StudentType.Art;

            StudentSaveData data2 = new StudentSaveData();
            data2.studentStat = new StudentStat();

            data2.studentStat.m_StudentName = "오수연";
            data2.studentStat.m_StudentType = StudentType.GameDesigner;

            // 어떤 로그인인지 체크를 한 다음에 폴더의 이름을 바꿔야 한다

            AllInOneData.Instance.player.m_playerID = PlayerInfo.Instance.m_PlayerID;
            AllInOneData.Instance.player.m_AcademyName = MyAcademy;
            AllInOneData.Instance.player.m_DirectorName = MyDirector;

            AllInOneData.Instance.studentData.Add(data);
            AllInOneData.Instance.studentData.Add(data1);
            AllInOneData.Instance.studentData.Add(data2);
        }
    }
}
