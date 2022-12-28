using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Security.Cryptography;
using Assets.Network;

public class Network : MonoBehaviour
{
    public static Network instance = null;       // Manager 변수는 싱글톤으로 사용
    static int count = 0;

    // 싱글톤으로 만든 변수를 안전하게 생성하고 사용하기 위한 초기화(?) 방법
    private void Awake()
    {
        Debug.Log("Awake 첫호출?");

        // Instance 가 존재한다면 gameObject 제거
        if (instance != null)
        {
            Debug.Log("Awake 재호출 리턴");
            count++;
            Debug.Log(count);
            return;
        }

        // 시작될 때 인스턴스 초기화, 씬을 넘어갈때도 유지되기 위한 처리
        instance = this;
        Debug.Log("Awake 새로운 아이");
        count++;
        Debug.Log(count);

    }

    private Socket m_ClientSock = null;      // 네트워크 연결이 됬는지 안됬는지
    private Socket m_CbSock;
    private byte[] m_RecvBuffer;
    private const int MAXSIZE = 4096;
    //private string HOST = "192.168.0.33";
    private string HOST = "221.163.91.111";
    private int PORT = 3000;

    private string sData;        // 서버에서 돌아오는 정보?

    #region sData_Property
    public string MysData
    {
        set { sData = value; }
        get { return sData; }
    }
    #endregion

    // delegate void ctrl_Invoke(TextBox ctrl, string message, string Netmessage);

    // Start is called before the first frame update
    void Start()
    {
        sData = " ";
    }

    public void CreateSocket()
    {
        Debug.Log("Socket 생성 호출?");
        if (m_ClientSock == null)
        {

            Debug.Log("소켓 생성");
            m_RecvBuffer = new byte[MAXSIZE];
            m_ClientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            TryConnect();
        }
    }

    // 로그인 버튼 누르면 서버와 연결시도
    public void TryConnect()
    {
        try
        {
            Debug.Log("서버 연결시도?");
            m_ClientSock.BeginConnect(HOST, PORT, new AsyncCallback(ConnectCallBack), m_ClientSock);
        }
        catch (SocketException e)
        {
            Debug.Log("서버 연결 안됨");
            Debug.Log(e.Message);
            // DialogResult dr = MessageBox.Show("접속에 실패하였습니다. 다시 시도하시겠습니까?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            // if (dr == DialogResult.Yes)
            // {
            //     TryConnect();
            // }
            // else
            // {
            //     this.Close();
            // }
        }
    }

    // 내 컴퓨터가 서버와 연결됬는지 데이터 -> 서버로부터 반환(연결여부)
    private void ConnectCallBack(IAsyncResult IAR)
    {
        try
        {
            Socket tempSocket = (Socket)IAR.AsyncState;
            IPEndPoint svEP = (IPEndPoint)tempSocket.RemoteEndPoint;

            if (m_ClientSock.Connected == true)
                Debug.Log("서버 접속 성공 ");

            // MessageBox.Show("접속 성공", "Client", MessageBoxButtons.OK, MessageBoxIcon.Information);

            tempSocket.EndConnect(IAR);
            m_CbSock = tempSocket;
            m_CbSock.BeginReceive(m_RecvBuffer, 0, m_RecvBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallBack), m_CbSock);
        }
        catch (SocketException e)
        {
            if (e.SocketErrorCode == SocketError.NotConnected)
            {
                Debug.Log("서버 접속 실패 ");
                // MessageBox.Show("서버 접속 실패", "Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TryConnect();
            }
        }
    }

    public void PacketSend(ClientPacket cp)
    {
        SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
        if (sendEventArgs == null)
        {
            //MessageBox.Show("SocketAsyncEventArgs is null");
            return;
        }

        sendEventArgs.Completed += SendComplected;
        sendEventArgs.UserToken = this;

        byte[] sendData = cp.Serialize();

        sendEventArgs.SetBuffer(sendData, 0, sendData.Length);

        bool pending = m_ClientSock.SendAsync(sendEventArgs);
        if (!pending)
        {
            SendComplected(null, sendEventArgs);
        }
    }

    private void SendComplected(object sender, SocketAsyncEventArgs e)
    {

    }

    // 로그인 버튼 누르면 서버에서 데이터 확인 후 반환하는 데이터
    private void OnReceiveCallBack(IAsyncResult IAR)
    {
        Debug.Log("OnReceiveCallBack 함수 호출");

        try
        {
            Socket tempSock = (Socket)IAR.AsyncState;
            int nReadSize = tempSock.EndReceive(IAR);

            if (nReadSize != 0)
            {
                var sp = ServerPacket.Deserialize(m_RecvBuffer);

                PacketCheck(sp);

                //sData = Encoding.Unicode.GetString(m_RecvBuffer, 0, nReadSize);

                //Debug.Log("서버로부터 로그인 결과 수신");
                //Debug.Log("sdata : " + sData);

                //// MessageBox.Show(sData);
                //if (sData == "로그인 성공")
                //{
                //    Debug.Log("로그인 성공");
                //    // SetInvisible();
                //    // Form3 lobbyForm = new Form3();
                //    // lobbyForm.SetForm(this);
                //    // lobbyForm.UserID = textBox1.Text;
                //    // lobbyForm.ShowDialog();
                //}
            }

            Receive();
        }
        catch (SocketException e)
        {
            if (e.SocketErrorCode == SocketError.ConnectionReset)
            {
                TryConnect();
            }
        }
    }

    // 소켓에 데이터가 들어올때가지 대기?
    public void Receive()
    {
        m_CbSock.BeginReceive(m_RecvBuffer, 0, m_RecvBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallBack), m_CbSock);
    }

    // 로그인 할떄만 가능함
    // 아이디 비밀번호를 서버에다가 전송 시도
    // message 에다가 ID + PW 정보 넣어서 전달해야함
    public void TrySend(string message)
    {
        Debug.Log("들어오니?");

        try
        {
            Debug.Log("try 들어오니?");
            if (m_ClientSock.Connected)
            {
                Debug.Log("서버에 데이터 던지기? 전송시도");
                byte[] buffer = Encoding.Unicode.GetBytes(message);
                m_ClientSock.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), message);
            }
        }
        catch (SocketException e)
        {
            Debug.Log("전송실패?");
            // MessageBox.Show("전송 실패", "Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    // TrySend() 에서 인자 message를 던져 줄 함수?
    private void SendCallBack(IAsyncResult IAR)
    {
        string message = (string)IAR.AsyncState;

        Debug.Log("전송 완료?");
        // MessageBox.Show("전송 완료", "Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // TrySend() 실행 전 분기
    //public void SelectMessage(LogData data, string button)
    //{
    //    string message = "";


    //    if (button == "Login_Button")          // 로그인 버튼 눌렀을때
    //    {
    //        Debug.Log("로그인 버튼 클릭 -> ");

    //        SHA256Managed sha256Managed = new SHA256Managed();
    //        byte[] encrptBytes = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(data.MyPW));

    //        string encryptString = Convert.ToBase64String(encrptBytes);
    //        message = "@login@" + "@id@" + data.MyID + "@pw@" + encryptString;
    //        SendPacket(data.MyID, encryptString);
    //    }
    //    else if (button == "CheckID_Button")     // 아이디 중복 확인
    //    {
    //        Debug.Log("아이디 중복 확인 -> ");

    //        message = "@idcheck@" + data.MyID;
    //    }
    //    else if (button == "Make_Button")     // 회원가입
    //    {
    //        Debug.Log("회원가입 -> ");

    //        SHA256Managed sha256Managed = new SHA256Managed();
    //        byte[] encrptBytes = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(data.MyPW));

    //        string encryptString = Convert.ToBase64String(encrptBytes);
    //        message = "@signup@" + "@id@" + data.MyID + "@pw@" + encryptString;
    //        SendPacket(data.MyID, encryptString);
    //    }

    //    //TrySend(message);
    //}

    private void PacketCheck(ServerPacket sp)
    {
        if (sp.m_packetType == PacketType.Login)
        {
            if (sp.m_isSuccess)
            {
                //m_form1.SetInvisible();
                //Form3 lobbyForm = new Form3();
                //lobbyForm.SetForm(m_form1);
                //lobbyForm.UserID = m_userId;
                //lobbyForm.ShowDialog();

                // 로그인 성공 처리
                Debug.Log("로그인 성공");
                sData = "로그인 성공";
            }
            else
            {
                Debug.Log("로그인 실패");
                sData = "로그인 실패";
                // MessageBox.Show("아이디 또는 비밀번호가 잘못되었습니다.");
            }
        }

        else if (sp.m_packetType == PacketType.IdCheck)
        {
            if (sp.m_isSuccess)
            {
                Debug.Log("아이디 생성 가능");
                sData = "아이디 생성 가능";
                //MessageBox.Show("아이디 생성이 가능합니다.");
            }
            else
            {
                Debug.Log("아이디 중복");
                sData = "아이디 중복";
                //MessageBox.Show("아이디가 중복입니다.");
            }
        }

        else if (sp.m_packetType == PacketType.SignUp)
        {
            if (sp.m_isSuccess)
            {
                //MessageBox.Show("회원가입 성공");
                //m_form2.FormClose();
                Debug.Log("회원가입 성공");
            }
            else
            {
                //MessageBox.Show("아이디 또는 비밀번호를 확인해주세요.");
                Debug.Log("회원가입 실패");
            }
        }

        else if (sp.m_packetType == PacketType.DuplicationCheck)
        {
            if (sp.m_isSuccess)
            {
                // true
            }
            else
            {
                //MessageBox.Show("중복 IP에서 접속되었습니다.");
                //m_form1.FormClose();
                Debug.Log("중복 IP에서 접속되었습니다.");
            }
        }
    }

    // m_ClientSock가 잘 됬는지 체크
    public bool CheckServerConnect()
    {
        if (m_ClientSock == null)           // 서버 연결 X
        {

            return false;
        }
        else                               // 서벼 연결 O
        {
            return true;
        }
    }
}
