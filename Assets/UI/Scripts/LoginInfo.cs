using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Network;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Mang 11. 3
/// 
/// 플레이어의 로그인에 대한 클래스
/// </summary>
public class LoginInfo : MonoBehaviour
{
    LogData m_NowLogInfo;                                       // 현재 입력하는 데이터를 잠시 저장할 변수

    public TMP_InputField m_InputID;                           // 플레이어가 입력한 ID
    public TMP_InputField m_InputPW;                           // 플레이어가 입력한 PW

    public TextMeshProUGUI m_PopupNotice;                       // 안내문구 띄워줄 변수

    private Network network;                // 서버에서 확인한 데이터를 받을 변수

    public GameObject m_AcademySetting;

    private string serverStr = "";

    private bool isLoginSuccess = false;

    public string ServerStr
    {
        get { return serverStr; }
        set
        {
            serverStr = value;
            if (!isLoginSuccess)
            {
                LoginTest();
            }
        }
    }

    private void Awake()
    {
        network = Network.instance;
        serverStr = " ";
    }

    // Start is called before the first frame update
    void Start()
    {
        m_NowLogInfo = new LogData();       // 모든 로그인 데이터가 저장될 딕셔너리 초기화?
    }

    // Update is called once per frame
    void Update()
    {
        if (network.MysData != " " && (network.MysData == "로그인 성공" || network.MysData == "로그인 실패"))
        {
            ServerStr = network.MysData;
            network.MysData = " ";
        }
    }

    public void CheckLoginData()
    {
        m_NowLogInfo.MyID = m_InputID.text;
        m_NowLogInfo.MyPW = m_InputPW.text;

        //network.SelectMessage(m_NowLogInfo, this.gameObject.name);

        SHA256Managed sha256Managed = new SHA256Managed();
        byte[] encryptBytes = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(m_NowLogInfo.MyPW));

        string encryptPw = Convert.ToBase64String(encryptBytes);

        ClientPacket cp = new ClientPacket();
        cp.m_packetType = PacketType.Login;
        cp.m_id = m_NowLogInfo.MyID;
        cp.m_pw = encryptPw;

        network.PacketSend(cp);

        // 로그인
        //if (m_NowLogInfo.MyID != "" && m_NowLogInfo.MyPW != "" && network.MysData == "로그인 성공")   // 아이디만 입력했다면
        //{
        //    PlayerInfo.Instance.m_PlayerID = m_NowLogInfo.MyID;

        //    // 아카데미 & 원장 이름 정하기 
        //    m_AcademySetting.SetActive(true);

        //}
        //else if (m_NowLogInfo.MyID != "" && m_NowLogInfo.MyPW != "" && network.MysData == "로그인 실패")   //  아무것도 입력 안했다면
        //{
        //    m_PopupNotice.text = network.MysData;

        //    // '비밀번호가 같지 않습니다' 팝업창 띄우기
        //    m_PopupNotice.gameObject.SetActive(true);
        //}

    }

    // 아이디 찾기
    public void FindIDs()
    {

    }

    // 게스트로그인함수
    public void GuestLoginInfoSave()
    {
        // 임시정보 부여
        m_NowLogInfo.MyID = "@GuestLogin";
        Debug.Log("Guest : " + m_NowLogInfo.MyID);

        PlayerInfo.Instance.m_PlayerID = m_NowLogInfo.MyID;

        // string str = JsonUtility.ToJson(m_NowLogInfo.MyID);     // 데이터를 제이슨 형식으로 변환

        // Debug.Log("To Json? :" + str);
    }

    public void LoginTest()
    {
        if (m_NowLogInfo.MyID != "" && m_NowLogInfo.MyPW != "" && serverStr == "로그인 성공")   // 아이디만 입력했다면
        {
            PlayerInfo.Instance.m_PlayerID = m_NowLogInfo.MyID;
            isLoginSuccess = true;

            // 아카데미 & 원장 이름 정하기 
            m_AcademySetting.SetActive(true);
        }
        else if (m_NowLogInfo.MyID != "" && m_NowLogInfo.MyPW != "" && serverStr == "로그인 실패")   //  아무것도 입력 안했다면
        {
            m_PopupNotice.text = serverStr;

            m_PopupNotice.gameObject.SetActive(true);
            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // 팝업창 띄우기 
        }
    }
}
