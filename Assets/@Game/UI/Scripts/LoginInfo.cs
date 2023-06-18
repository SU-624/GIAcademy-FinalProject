using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using System.Text;
//using PacketBase;

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

    public GameObject m_AcademySetting;

    private bool isLoginSuccess = false;

    private PopUpUI PopUpSetAcademyNamePanel;          // 구글 로그인 시 띄워줄 팝업 유아이

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        m_NowLogInfo = new LogData();       // 모든 로그인 데이터가 저장될 딕셔너리 초기화?

        // 파이어베이스 로그인 여부를 체크하는 델리게이트
        //FirebaseBinder.Instance.LoginState += GoogleLogin;
    }

    // Update is called once per frame
    void Update()
    {
        //m_net.ReceivePacket();

        //if (Network.Instance.ClientNet.networkMessage.Count != 0)
        //{
        //    if (Network.Instance.ClientNet.networkMessage.Peek() == "로그인 성공" ||
        //        Network.Instance.ClientNet.networkMessage.Peek() == "로그인 실패")
        //    {
        //        LoginTest();
        //    }
        //}


        // if (network.MysData != " " && (network.MysData == "로그인 성공" || network.MysData == "로그인 실패"))
        // {
        //     ServerStr = network.MysData;
        //     network.MysData = " ";
        // }
    }

    public void CheckLoginData()
    {
        //m_NowLogInfo.MyID = m_InputID.text;
        //m_NowLogInfo.MyPW = m_InputPW.text;

        ////network.SelectMessage(m_NowLogInfo, this.gameObject.name);

        //SHA256Managed sha256Managed = new SHA256Managed();
        //byte[] encryptBytes = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(m_NowLogInfo.MyPW));

        //string encryptPw = Convert.ToBase64String(encryptBytes);

        //LoginPacket loginPacket = new LoginPacket();
        //loginPacket.packetId = (short)PacketId.ReqLogin;
        //loginPacket.packetError = (short)ErrorCode.None;
        //loginPacket.UserId = m_NowLogInfo.MyID;
        //loginPacket.UserPw = encryptPw;

        //Network.Instance.ClientNet.PacketSend(loginPacket);

        //ClientPacket cp = new ClientPacket();
        //cp.m_packetType = PacketType.Login;
        //cp.m_id = m_NowLogInfo.MyID;
        //cp.m_pw = encryptPw;
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

        // 5.15 woodpie9 PlayerID 정보 삭제
        //PlayerInfo.Instance.m_PlayerID = m_NowLogInfo.MyID;

#if UNITY_EDITOR
        // var json = GameObject.Find("Json");             // 씬이 다르기 때문에 Json 을 쓰려면 이렇게 해줘야 한다 
        // json.GetComponent<Json>().SaveDataInSetAcademyPanel();
        //Json.Instance.SaveDataInSetAcademyPanel();
#elif UNITY_ANDROID
        //var json = GameObject.Find("Json");             // 씬이 다르기 때문에 Json 을 쓰려면 이렇게 해줘야 한다 
        //json.GetComponent<Json>().SaveDataInSetAcademyPanel();

        // string str = JsonUtility.ToJson(m_NowLogInfo.MyID);     // 데이터를 제이슨 형식으로 변환

        // Debug.Log("To Json? :" + str);
#endif
    }

    public void LoginTest()
    {
        //if (Network.Instance.ClientNet.networkMessage.Peek() == "로그인 성공")
        //{
        //    // 5.15 woodpie9 PlayerID 정보 삭제
        //    //PlayerInfo.Instance.m_PlayerID = m_InputID.text;
        //    isLoginSuccess = true;

        //    // 아카데미 & 원장 이름 정하기 
        //    m_AcademySetting.SetActive(true);
        //}
        //else if (Network.Instance.ClientNet.networkMessage.Peek() == "로그인 실패")
        //{
        //    m_PopupNotice.text = "로그인 실패";

        //    m_PopupNotice.gameObject.SetActive(true);
        //    m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // 팝업창 띄우기 
        //}

        //Network.Instance.ClientNet.networkMessage.Dequeue();
    }

    public void GoogleLogin(bool sign)
    {
        if (sign == true)
        {
            PopUpSetAcademyNamePanel.PopUpMyUI();
        }
    }
}
