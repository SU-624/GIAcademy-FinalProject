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
        Debug.Log("Ŭ���̾�Ʈ ��Ʈ��ũ ����");
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

    // ��Ŷ ���°� �ִ��� Ȯ�� �� �սǿ��� üũ
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

            ClientNet.networkMessage.Enqueue("�����κ����� ���� ��Ŷ �м�");
            PacketAnalyze(body);
        }
    }

    private void PacketAnalyze(byte[] data)
    {
        ServerPacket packet = (ServerPacket)ServerPacket.Deserialize(data);

        switch (packet.packetId)
        {
            case (short)PacketId.ConnectResult:
                // ���� �õ� ���
                ClientNet.networkMessage.Enqueue("���� ���� ����");
                break;

            case (short)PacketId.ResLogin:
                // �α��� ���
                if (packet.ServerResult)
                {
                    ClientNet.networkMessage.Enqueue("�α��� ����");
                }
                else
                {
                    ClientNet.networkMessage.Enqueue("�α��� ����");
                }
                break;

            case (short)PacketId.ResIdCheck:
                // ���̵� �ߺ� Ȯ�� ���
                if (packet.ServerResult)
                {
                    ClientNet.networkMessage.Enqueue("���̵� ��� ����");
                }
                else
                {
                    ClientNet.networkMessage.Enqueue("���̵� �ߺ�");
                }
                break;

            case (short)PacketId.ResSignUp:
                // ȸ������ ���
                if (packet.ServerResult)
                {
                    ClientNet.networkMessage.Enqueue("ȸ�� ���� ����");
                }
                else
                {
                    ClientNet.networkMessage.Enqueue("ȸ�� ���� ����");
                }
                break;

            case (short)PacketId.PacketError:
                // ��Ŷ�� ������ ���� ��
                break;
        }
    }
}
