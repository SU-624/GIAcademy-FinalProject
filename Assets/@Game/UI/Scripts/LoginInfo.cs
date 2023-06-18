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
/// �÷��̾��� �α��ο� ���� Ŭ����
/// </summary>
public class LoginInfo : MonoBehaviour
{
    LogData m_NowLogInfo;                                       // ���� �Է��ϴ� �����͸� ��� ������ ����

    public TMP_InputField m_InputID;                           // �÷��̾ �Է��� ID
    public TMP_InputField m_InputPW;                           // �÷��̾ �Է��� PW

    public TextMeshProUGUI m_PopupNotice;                       // �ȳ����� ����� ����

    public GameObject m_AcademySetting;

    private bool isLoginSuccess = false;

    private PopUpUI PopUpSetAcademyNamePanel;          // ���� �α��� �� ����� �˾� ������

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        m_NowLogInfo = new LogData();       // ��� �α��� �����Ͱ� ����� ��ųʸ� �ʱ�ȭ?

        // ���̾�̽� �α��� ���θ� üũ�ϴ� ��������Ʈ
        //FirebaseBinder.Instance.LoginState += GoogleLogin;
    }

    // Update is called once per frame
    void Update()
    {
        //m_net.ReceivePacket();

        //if (Network.Instance.ClientNet.networkMessage.Count != 0)
        //{
        //    if (Network.Instance.ClientNet.networkMessage.Peek() == "�α��� ����" ||
        //        Network.Instance.ClientNet.networkMessage.Peek() == "�α��� ����")
        //    {
        //        LoginTest();
        //    }
        //}


        // if (network.MysData != " " && (network.MysData == "�α��� ����" || network.MysData == "�α��� ����"))
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

    // ���̵� ã��
    public void FindIDs()
    {

    }

    // �Խ�Ʈ�α����Լ�
    public void GuestLoginInfoSave()
    {
        // �ӽ����� �ο�
        m_NowLogInfo.MyID = "@GuestLogin";
        Debug.Log("Guest : " + m_NowLogInfo.MyID);

        // 5.15 woodpie9 PlayerID ���� ����
        //PlayerInfo.Instance.m_PlayerID = m_NowLogInfo.MyID;

#if UNITY_EDITOR
        // var json = GameObject.Find("Json");             // ���� �ٸ��� ������ Json �� ������ �̷��� ����� �Ѵ� 
        // json.GetComponent<Json>().SaveDataInSetAcademyPanel();
        //Json.Instance.SaveDataInSetAcademyPanel();
#elif UNITY_ANDROID
        //var json = GameObject.Find("Json");             // ���� �ٸ��� ������ Json �� ������ �̷��� ����� �Ѵ� 
        //json.GetComponent<Json>().SaveDataInSetAcademyPanel();

        // string str = JsonUtility.ToJson(m_NowLogInfo.MyID);     // �����͸� ���̽� �������� ��ȯ

        // Debug.Log("To Json? :" + str);
#endif
    }

    public void LoginTest()
    {
        //if (Network.Instance.ClientNet.networkMessage.Peek() == "�α��� ����")
        //{
        //    // 5.15 woodpie9 PlayerID ���� ����
        //    //PlayerInfo.Instance.m_PlayerID = m_InputID.text;
        //    isLoginSuccess = true;

        //    // ��ī���� & ���� �̸� ���ϱ� 
        //    m_AcademySetting.SetActive(true);
        //}
        //else if (Network.Instance.ClientNet.networkMessage.Peek() == "�α��� ����")
        //{
        //    m_PopupNotice.text = "�α��� ����";

        //    m_PopupNotice.gameObject.SetActive(true);
        //    m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // �˾�â ���� 
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
