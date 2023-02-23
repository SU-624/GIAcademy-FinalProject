using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json;

using Conditiondata.Runtime;
using UnityEngine.UI;

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
    public GameObject m_SekOKButton;
    public GameObject m_Loading_Panel;

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

    // 버튼에 아카데미명, 원장명 체크 후 씬변경 하는 함수 달아두기
    private void Start()
    {
        m_SekOKButton.GetComponent<Button>().onClick.AddListener(SetAcademyData);
    }

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
            PlayerInfo.Instance.m_PrincipalName = MyDirector;
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
            AllInOneData.Instance.player.m_PrincipalName = MyDirector;

            AllInOneData.Instance.studentData.Add(data);
            AllInOneData.Instance.studentData.Add(data1);
            AllInOneData.Instance.studentData.Add(data2);

            SceneChangeAfterCheckData();
        }
        else
        {
            m_PopupNotice.text = "둘 다 입력해주세요";

            // '비밀번호가 같지 않습니다' 팝업창 띄우기
            m_PopupNotice.gameObject.SetActive(true);

            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // 팝업창 띄우기 
        }
    }

    public void SceneChangeAfterCheckData()
    {
        // 데이터 들어갔는지 체크 후
        if(AllInOneData.Instance.player.m_AcademyName != "" && AllInOneData.Instance.player.m_PrincipalName != "")
        {
            // 로딩씬 띄우고
            m_Loading_Panel.SetActive(true);
            // 씬변경
            MoveSceneManager.m_Instance.MoveToInGameScene();
        }   
    }
}
