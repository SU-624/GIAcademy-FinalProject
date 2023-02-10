using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Security.Cryptography;
using NetworkModule;
using PacketBase;

public class Network : MonoBehaviour
{
    private Network() { }

    private static Network instance;

    public static Network Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Network>();
                if (instance == null)
                {
                    GameObject NetworkObj = new GameObject();
                    NetworkObj.name = "Network";
                    instance = NetworkObj.AddComponent<Network>();
                }
            }
            return instance;
        }
    }

    //private string HOST = "192.168.0.33";
    private string HOST = "221.163.91.111";
    private int PORT = 3030;

    public ClientNetwork ClientNet
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (ClientNet == null)
        {
            Init(HOST, PORT);
        }
    }

    public void Init(string host, int port)
    {
        Debug.Log("클라이언트 네트워크 생성");
        ClientNet = new ClientNetwork();
        ClientNet.Init(host, port);
    }

    private void Update()
    {
        ReceivePacket();
    }

    private void LateUpdate()
    {
        NetworkMessageLog();
    }

    private void NetworkMessageLog()
    {
        if (ClientNet.networkMessage.Count != 0)
        {
            var msg = ClientNet.networkMessage.Dequeue();
            Debug.Log(msg);
        }
    }

    // 패킷 들어온게 있는지 확인 후 손실여부 체크
    private void ReceivePacket()
    {
        if (ClientNet.dataQueue.Count != 0)
        {
            byte[] data = ClientNet.dataQueue.Dequeue();

            byte[] header = new byte[PacketDefine.HEADERSIZE];
            Buffer.BlockCopy(data, 0, header, 0, PacketDefine.HEADERSIZE);

            int bodySize = BitConverter.ToInt32(header, 0);
            byte[] body = new byte[bodySize];

            Buffer.BlockCopy(data, PacketDefine.HEADERSIZE, body, 0, bodySize);

            ClientNet.networkMessage.Enqueue("서버로부터의 수신 패킷 분석");
            PacketAnalyze(body);
        }
    }

    private void PacketAnalyze(byte[] data)
    {
        ServerPacket packet = (ServerPacket)ServerPacket.Deserialize(data);

        switch (packet.packetId)
        {
            case (short)PacketId.ConnectResult:
                // 연결 시도 결과
                ClientNet.networkMessage.Enqueue("서버 접속 성공");
                break;

            case (short)PacketId.ResLogin:
                // 로그인 결과
                if (packet.ServerResult)
                {
                    ClientNet.networkMessage.Enqueue("로그인 성공");
                }
                else
                {
                    ClientNet.networkMessage.Enqueue("로그인 실패");
                }
                break;

            case (short)PacketId.ResIdCheck:
                // 아이디 중복 확인 결과
                if (packet.ServerResult)
                {
                    ClientNet.networkMessage.Enqueue("아이디 사용 가능");
                }
                else
                {
                    ClientNet.networkMessage.Enqueue("아이디 중복");
                }
                break;

            case (short)PacketId.ResSignUp:
                // 회원가입 결과
                if (packet.ServerResult)
                {
                    ClientNet.networkMessage.Enqueue("회원 가입 성공");
                }
                else
                {
                    ClientNet.networkMessage.Enqueue("회원 가입 실패");
                }
                break;

            case (short)PacketId.PacketError:
                // 패킷에 문제가 있을 시
                break;
        }
    }
}
