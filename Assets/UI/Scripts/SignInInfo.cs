using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Network;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Mang 10. 19
/// 
/// 플레이어가 계정생성을 계속 할 시 그 정보를 저장하기 위한 struct
/// </summary>
[System.Serializable]
public class LogData
{
    // 플레이어 회원 가입 시 필요한 정보 / 'm_IDName' 은 계속 가지고 있어야 할 데이터
    private string m_IDName;
    // 플레이어 회원 가입 시 필요한 패스워드
    private string m_PWName;

    #region LogData_Property

    // 유니티 property
    public string MyID
    {
        get { return m_IDName; }
        set { m_IDName = value; }
    }

    public string MyPW
    {
        get { return m_PWName; }
        set { m_PWName = value; }
    }
    #endregion
}

/// <summary>
/// Mang 11. 3
/// 
/// 플레이어의 회원가입에 대한 클래스
/// </summary>
public class SignInInfo : MonoBehaviour
{
    LogData m_NowLogInfo;                                       // 현재 입력하는 데이터를 잠시 저장할 변수

    public TMP_InputField m_InputID;                            // 플레이어가 입력한 ID
    public TMP_InputField m_InputPW;                            // 플레이어가 입력한 PW
    public TMP_InputField m_InputCheckPW;                       // 비밀번호 확인

    public TextMeshProUGUI m_PopupNotice;                       // 안내문구 띄워줄 변수

    private Network network;                 // 서버에서 확인한 데이터를 받을 변수

    private string serverStr = "";

    public string ServerStr
    {
        get { return serverStr; }
        set
        {
            serverStr = value;
            if (serverStr == "아이디 중복" || serverStr == "아이디 생성 가능")
            {
                IdCheckResult();
            }
            
        }
    }

    // 함수가 실행되었을지 판별할 bool 변수
    // bool m_isSaved = false;

    private void Awake()
    {
        network = Network.instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_NowLogInfo = new LogData();       // 모든 로그인 데이터가 저장될 딕셔너리 초기화?
        serverStr = " ";
    }

    // Update is called once per frame
    void Update()
    {
        if (network.MysData != " " && (network.MysData == "아이디 중복" || network.MysData == "아이디 생성 가능"))
        {
            ServerStr = network.MysData;
            network.MysData = " ";
        }
    }

    public void CheckID()
    {
        m_NowLogInfo.MyID = m_InputID.text;

        // 데이터 언제 던져주지
        // 여기서 selectMessage 함수 불러서 데이터 넣어줘야 할것같다 여기서 이름을 넣어주도록 하자

        if (network.CheckServerConnect() == true && m_InputID.text != "")        // 서버 연결 되있을때 && 빈칸이 아닐때
        {
            // 여기서 네트워크 - 셀렉트 메세지 함수를 불러야할거같은데
            // network.SelectMessage(m_NowLogInfo, this.gameObject.name);
            ClientPacket cp = new ClientPacket();
            cp.m_packetType = PacketType.IdCheck;
            cp.m_id = m_InputID.text;

            network.PacketSend(cp);

            //if (network.MysData == "아이디 생성 불가")                    // 입력에 특수문자같은 것이 들어있따면
            //{
            //    Debug.Log("빈칸이야~");

            //    m_PopupNotice.text = network.MysData;

            //    // '다른 아이디를 입력해주세요' 팝업창
            //    m_PopupNotice.gameObject.SetActive(true);
            //    m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();
            //}
            
        }
        else          // 서버가 연결이 필요하거나 빈문자열일때
        {
            m_PopupNotice.text = "네트워크 연결 필요";

            // 네트워크 연결 필요 팝업창
            m_PopupNotice.gameObject.SetActive(true);
            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();
        }

    }

    private void IdCheckResult()
    {
        if (serverStr == "아이디 중복")                // 아이디 중복체크
        {
            Debug.Log("아이디 중복이야~");

            m_PopupNotice.text = network.MysData;

            // '아이디 중복' 팝업창
            m_PopupNotice.gameObject.SetActive(true);
            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();
        }
        else if (serverStr == "아이디 생성 가능")      // 아이디 중복아닐경우
        {
            m_PopupNotice.text = network.MysData;

            // '아이디 생성 가능' 팝업창
            m_PopupNotice.gameObject.SetActive(true);
            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();
        }
    }

    public void CheckPW()
    {
        // 비밀번호 & 비밀번호 확인 
        if (m_InputPW.text != m_InputCheckPW.text)
        {
            m_PopupNotice.text = "비밀번호가 같지 않습니다";

            // '비밀번호가 같지 않습니다' 팝업창 띄우기
            m_PopupNotice.gameObject.SetActive(true);

            Debug.Log("비밀번호 다름");

            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // 팝업창 띄우기 
        }
        else                // 여기서 데이터 저장해주기
        {
            m_PopupNotice.text = "아이디 생성 완료";

            // "아이디 생성 완료" 팝업창 띄우기
            m_PopupNotice.gameObject.SetActive(true);

            Debug.Log("아이디 생성 완료");

            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // 팝업창 띄우기 

            m_NowLogInfo.MyID = m_InputID.text;
            m_NowLogInfo.MyPW = m_InputPW.text;

            // Json inst = new Json();

            //network.SelectMessage(m_NowLogInfo, this.gameObject.name);      // 로그인 데이터 서버에 전송

            SHA256Managed sha256Managed = new SHA256Managed();
            byte[] encryptBytes = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(m_NowLogInfo.MyPW));

            string encryptPw = Convert.ToBase64String(encryptBytes);

            ClientPacket cp = new ClientPacket();
            cp.m_packetType = PacketType.SignUp;
            cp.m_id = m_NowLogInfo.MyID;
            cp.m_pw = encryptPw;

            network.PacketSend(cp);

            Debug.Log("ID : " + m_NowLogInfo.MyID);
            Debug.Log("PW : " + m_NowLogInfo.MyPW);

            // PlayerInfo.instance.MyID = m_NowLogInfo.MyID;

            // string str = JsonUtility.ToJson(m_NowLogInfo.MyID);     // 데이터를 제이슨 형식으로 변환

            // Debug.Log("To Json? :" + str);


        }
    }
}