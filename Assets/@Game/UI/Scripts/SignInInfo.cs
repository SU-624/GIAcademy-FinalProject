using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using System.Text;
using PacketBase;

/// <summary>
/// Mang 10. 19
/// 
/// �÷��̾ ���������� ��� �� �� �� ������ �����ϱ� ���� struct
/// </summary>
[System.Serializable]
public class LogData
{
    // �÷��̾� ȸ�� ���� �� �ʿ��� ���� / 'm_IDName' �� ��� ������ �־�� �� ������
    private string m_IDName;
    // �÷��̾� ȸ�� ���� �� �ʿ��� �н�����
    private string m_PWName;

    #region LogData_Property

    // ����Ƽ property
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
/// �÷��̾��� ȸ�����Կ� ���� Ŭ����
/// </summary>
public class SignInInfo : MonoBehaviour
{
    LogData m_NowLogInfo;                                       // ���� �Է��ϴ� �����͸� ��� ������ ����

    public TMP_InputField m_InputID;                            // �÷��̾ �Է��� ID
    public TMP_InputField m_InputPW;                            // �÷��̾ �Է��� PW
    public TMP_InputField m_InputCheckPW;                       // ��й�ȣ Ȯ��

    public TextMeshProUGUI m_PopupNotice;                       // �ȳ����� ����� ����

    // �Լ��� ����Ǿ����� �Ǻ��� bool ����
    // bool m_isSaved = false;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        m_NowLogInfo = new LogData();       // ��� �α��� �����Ͱ� ����� ��ųʸ� �ʱ�ȭ?
    }

    // Update is called once per frame
    void Update()
    {
        if (Network.Instance.ClientNet.networkMessage.Count != 0)
        {
            if (Network.Instance.ClientNet.networkMessage.Peek() == "���̵� ��� ����" ||
                Network.Instance.ClientNet.networkMessage.Peek() == "���̵� �ߺ�")
            {
                IdCheckResult();
            }
            else if (Network.Instance.ClientNet.networkMessage.Peek() == "ȸ�� ���� ����" ||
                     Network.Instance.ClientNet.networkMessage.Peek() == "ȸ�� ���� ����")
            {
                CreateIdResult();
            }
        }
    }

    public void CheckID()
    {
        m_NowLogInfo.MyID = m_InputID.text;

        IdCheckPacket idCheckPacket = new IdCheckPacket();
        idCheckPacket.packetId = (short)PacketId.ReqIdCheck;
        idCheckPacket.packetError = (short)ErrorCode.None;
        idCheckPacket.UserId = m_NowLogInfo.MyID;

        Network.Instance.ClientNet.PacketSend(idCheckPacket);
    }

    private void IdCheckResult()
    {
        if (Network.Instance.ClientNet.networkMessage.Peek() == "���̵� ��� ����")
        {
            m_PopupNotice.text = Network.Instance.ClientNet.networkMessage.Dequeue();

            m_PopupNotice.gameObject.SetActive(true);
            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // �˾�â ���� 
        }
        else if (Network.Instance.ClientNet.networkMessage.Peek() == "���̵� �ߺ�")
        {
            m_PopupNotice.text = Network.Instance.ClientNet.networkMessage.Dequeue();

            m_PopupNotice.gameObject.SetActive(true);
            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // �˾�â ���� 
        }
    }

    public void CheckPW()
    {
        // ��й�ȣ & ��й�ȣ Ȯ�� 
        if (m_InputPW.text != m_InputCheckPW.text)
        {
            m_PopupNotice.text = "��й�ȣ�� ���� �ʽ��ϴ�";

            // '��й�ȣ�� ���� �ʽ��ϴ�' �˾�â ����
            m_PopupNotice.gameObject.SetActive(true);

            Debug.Log("��й�ȣ �ٸ�");

            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // �˾�â ���� 
        }
        else                // ���⼭ ������ �������ֱ�
        {
            m_NowLogInfo.MyID = m_InputID.text;
            m_NowLogInfo.MyPW = m_InputPW.text;

            SHA256Managed sha256Managed = new SHA256Managed();
            byte[] encryptBytes = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(m_NowLogInfo.MyPW));

            string encryptPw = Convert.ToBase64String(encryptBytes);

            SignUpPacket signUpPacket = new SignUpPacket();
            signUpPacket.packetId = (short)PacketId.ReqSignUp;
            signUpPacket.packetError = (short)ErrorCode.None;
            signUpPacket.UserId = m_NowLogInfo.MyID;
            signUpPacket.UserPw = encryptPw;

            Network.Instance.ClientNet.PacketSend(signUpPacket);

            Debug.Log("ID : " + m_NowLogInfo.MyID);
            Debug.Log("PW : " + m_NowLogInfo.MyPW);
        }
    }

    private void CreateIdResult()
    {
        if (Network.Instance.ClientNet.networkMessage.Peek() == "ȸ�� ���� ����")
        {
            m_PopupNotice.text = Network.Instance.ClientNet.networkMessage.Dequeue();

            m_PopupNotice.gameObject.SetActive(true);
            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // �˾�â ���� 
        }
        else if (Network.Instance.ClientNet.networkMessage.Peek() == "ȸ�� ���� ����")
        {
            m_PopupNotice.text = Network.Instance.ClientNet.networkMessage.Dequeue();

            m_PopupNotice.gameObject.SetActive(true);
            m_PopupNotice.gameObject.GetComponent<PopOffUI>().DelayTurnOffUI();    // �˾�â ���� 
        }
    }
}