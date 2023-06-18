using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptsManager : MonoBehaviour
{
    private static ScriptsManager instance = null;

    public static ScriptsManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // �л�
    public List<List<string>> GenreRoomScripts = new List<List<string>>();      // �帣�� 8����
    public List<string> RepairScripts = new List<string>();                     // �帣�� ���� ��ũ��Ʈ
    public List<List<string>> FacilityScripts = new List<List<string>>();       // �ü� 4����
    public List<string> StudyRoom2M = new List<string>();                       // �ü� 4���� �� �ڽ��Ǹ� 2���� �޶���
    public List<List<string>> ObjectScripts = new List<List<string>>();         // ������Ʈ 5����
    public List<List<string>> ClassScripts = new List<List<string>>();          // �а����� 3����, 2�� 3����
    public List<List<string>> InteractionScripts = new List<List<string>>();    // �л��� ģ�е��� 3����
    public List<string> FreeWalkScripts = new List<string>();                   // �����̵���
    public List<string> FreeWalkScripts2 = new List<string>();                  // 2�� �����̵���
    public List<List<string>> VacationScripts = new List<List<string>>();       // ���� �λ�, ���� �̵�

    // ����
    public List<List<string>> ProfScripts = new List<List<string>>();           // �а����� 3����, 2�� 3����
    public List<List<string>> StuProScripts = new List<List<string>>();         // �л�-���� ģ�е��� 3����
    public List<List<string>> ProStuScripts = new List<List<string>>();         // ����-�л� ģ�е��� 3����
    public List<List<string>> ProProScripts = new List<List<string>>();         // ����-���� �⺻, 2��

    // ������õ �Ⱓ ��ȭ ��ũ��Ʈ��
    public List<List<string>> CStuCStuScripts = new List<List<string>>();       // ������õ �� �л� - ������õ �� �л� ģ�е��� 3����
    public List<List<string>> CStuIStuScripts = new List<List<string>>();       // ������õ �� �л� - ������õ �̿� �л� ģ�е��� 3����
    public List<List<string>> IStuCStuScripts = new List<List<string>>();       // ������õ �̿� �л� - ������õ �� �л� ģ�е��� 3����
    public List<List<string>> IStuIStuScripts = new List<List<string>>();       // ������õ �̿� �л� - ������õ �̿� �л� ģ�е��� 3����

    public List<List<string>> CStuProScripts = new List<List<string>>();        // ������õ �� �л� - ���� ģ�е��� 3����
    public List<List<string>> IStuProScripts = new List<List<string>>();        // ������õ �̿� �л� - ���� ģ�е��� 3����
    public List<List<string>> ProCStuScripts = new List<List<string>>();        // ���� - ������õ �� �л� ģ�е��� 3����
    public List<List<string>> ProIStuScripts = new List<List<string>>();        // ���� - ������õ �̿� �л� ģ�е��� 3����

    //public List<List<string>> ProfFacilityScripts = new List<List<string>>();

    public Sprite[] Emoticons;
    public Sprite[] GenreSprites;
    public Sprite[] StatSprites;
    public Sprite[] OtherSprites; // ģ�е�, ü��, ����

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // �帣��
        for (int i = 0; i < 8; i++)
        {
            // ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("Ǯ����!");
                newScripts.Add("����~");
                newScripts.Add("�ȶ�������!");
                newScripts.Add("������,����");
                newScripts.Add("�Ӹ�����");
                GenreRoomScripts.Add(newScripts);
            }
            // �ùķ��̼�
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��������$");
                newScripts.Add("���б��~");
                newScripts.Add("��ȭ���!");
                newScripts.Add("��ȭ�ӱ���");
                newScripts.Add("��ճ�");
                newScripts.Add("�󸶿�����?");
                newScripts.Add("����");
                GenreRoomScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��ĩ��ĩ��");
                newScripts.Add("�̸�����..");
                newScripts.Add("����̴�����!");
                newScripts.Add("���ܴ�~");
                newScripts.Add("������");
                newScripts.Add("�������غ���~");
                newScripts.Add("�뷡������..");
                GenreRoomScripts.Add(newScripts);
            }
            // ��庥��
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������ ���?");
                newScripts.Add("������ ������~");
                newScripts.Add("�Ϳ���!");
                newScripts.Add("�ȳ�!������~");
                newScripts.Add("�ų��¸���!");
                newScripts.Add("��̾���...");
                GenreRoomScripts.Add(newScripts);
            }
            // RPG
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�;ơ�");
                newScripts.Add("��վ�!");
                newScripts.Add("�����");
                newScripts.Add("RPG¯!");
                newScripts.Add("�Ͼ�..");
                newScripts.Add("���� �밡��!");
                newScripts.Add("���ȭ����!");
                newScripts.Add("��̾���...");
                GenreRoomScripts.Add(newScripts);
            }
            // ������
            else if (i == 5)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�߰ſ���Ʈ��~");
                newScripts.Add("����������!");
                newScripts.Add("����!");
                newScripts.Add("�������");
                newScripts.Add("�����̶�������");
                newScripts.Add("�տ�������");
                newScripts.Add("�����ȸ¾�");
                GenreRoomScripts.Add(newScripts);
            }
            // �׼�
            else if (i == 6)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���");
                newScripts.Add("���Ѵ�");
                newScripts.Add("����!");
                newScripts.Add("����~");
                newScripts.Add("�׼��谨!");
                newScripts.Add("�������̾�");
                GenreRoomScripts.Add(newScripts);
            }
            // ����
            else if (i == 7)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���ӽ�ȭ?");
                newScripts.Add("���ƶ�����!");
                newScripts.Add("��������~");
                newScripts.Add("����!");
                newScripts.Add("����!");
                newScripts.Add("���ξ�");
                GenreRoomScripts.Add(newScripts);
            }
        }

        // �帣�� ����
        RepairScripts.Add("��������!!!");
        RepairScripts.Add("���峵��");
        RepairScripts.Add("���ε� ���ϰ��ϳ�");
        RepairScripts.Add("�������ּ���");
        RepairScripts.Add("����;�");
        RepairScripts.Add("����ü ���°��!!!");
        RepairScripts.Add("�ٸ��� ������");
        RepairScripts.Add("���� ����?");
        RepairScripts.Add("������");

        // �ü�
        for (int i = 0; i < 5; i++)
        {
            // ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("����� ��!");
                newScripts.Add("�̰Ż��?");
                newScripts.Add("��");
                newScripts.Add("�������");
                newScripts.Add("�̰��ּ���");
                newScripts.Add("�ʹ���ο�");
                FacilityScripts.Add(newScripts);
            }
            // ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���� �����̾�");
                newScripts.Add("�ɿ��ϳ�");
                newScripts.Add("�����̴�!");
                newScripts.Add("������ ���");
                newScripts.Add("��û�β���");
                newScripts.Add("�̰��ּ���");
                newScripts.Add("�̰ɷ��ҰԿ�");
                FacilityScripts.Add(newScripts);
            }
            // �ڽ���
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("zZz");
                newScripts.Add("��Ƣ���ƴ�...");
                newScripts.Add("�ϱ�Ⱦ�");
                newScripts.Add("���޾Ҵ�!");
                newScripts.Add("���ٽ���");
                FacilityScripts.Add(newScripts);
                
                StudyRoom2M.Add("zZz");
                StudyRoom2M.Add("�� �����");
                StudyRoom2M.Add("�� ��..����!");
                StudyRoom2M.Add("�� �̰ſ���");
                StudyRoom2M.Add("������!");
            }
            // ����� 1
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��������~");
                newScripts.Add("����!");
                newScripts.Add("�޽Ľð���");
                newScripts.Add("�ų���!");
                newScripts.Add("��~");
                FacilityScripts.Add(newScripts);
            }
            // ����� 2
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��������~");
                newScripts.Add("����!");
                newScripts.Add("�޽Ľð���");
                newScripts.Add("�ų���!");
                newScripts.Add("��~");
                FacilityScripts.Add(newScripts);
            }
        }

        // ������Ʈ
        for (int i = 0; i < 5; i++)
        {
            // ���Ǳ�
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ÿ���");
                newScripts.Add("��������");
                ObjectScripts.Add(newScripts);
            }
            // ȭ��
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�������");
                newScripts.Add("����Ŀ��~");
                newScripts.Add("������!");
                newScripts.Add("���̴�!");
                ObjectScripts.Add(newScripts);
            }
            // ���ӱ�
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��մ�!");
                newScripts.Add("��ſ�");
                newScripts.Add("�� �׾���..");
                ObjectScripts.Add(newScripts);
            }
            // ������
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��Ͱ���");
                newScripts.Add("��~");
                newScripts.Add("�ÿ���");
                ObjectScripts.Add(newScripts);
            }
            // �Խ���
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�̰��� �Խ����̿�");
                newScripts.Add("�Խ��ǿ� �ƹ��͵� ����");
                ObjectScripts.Add(newScripts);
            }
        }

        // ����
        for (int i = 0; i < 6; i++)
        {
            // ��ȹ
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���� ���̵� ���ö���!");
                newScripts.Add("���� �ƹ� ������ ����...");
                newScripts.Add("������ �����ؿ�");
                ClassScripts.Add(newScripts);
            }
            // ��Ʈ
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��! ���� ���� ���߱׸�");
                newScripts.Add("�ո��� ����...");
                newScripts.Add("�𵨸� ����~");
                ClassScripts.Add(newScripts);
            }
            // ���α׷���
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("01101");                 
                newScripts.Add("0111");         
                newScripts.Add("01111100"); 
                ClassScripts.Add(newScripts);
            }
            // 2��
            // ��ȹ
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��..����");
                newScripts.Add("��ƴ�");
                newScripts.Add("��ȹ�ǵ���?");
                newScripts.Add("������ �߿���");
                newScripts.Add("�ո��� ����");
                ClassScripts.Add(newScripts);
            }
            // ��Ʈ
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��..Ʈ");
                newScripts.Add("�����..");
                newScripts.Add("AI�����");
                newScripts.Add("������ �츮��");
                newScripts.Add("�㸮 ������");
                ClassScripts.Add(newScripts);
            }
            // ���α׷���
            else if (i == 5)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���α׷���..");
                newScripts.Add("�� �� �ֳ�?");
                newScripts.Add("���뽺����");
                newScripts.Add("1010101");
                newScripts.Add("���� ������");
                ClassScripts.Add(newScripts);
            }
        }

        // �л�����
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ȳ�?/�ȳ社");
                newScripts.Add("������~/�ʵ�~");
                newScripts.Add("�츮ģ������/����");
                newScripts.Add("ó������/��������?");
                newScripts.Add("����/����");
                newScripts.Add("�̸���?/�����氡��");
                newScripts.Add("�ȶ�/�̻��Ѿֳ�");
                newScripts.Add("�� ������/����");
                newScripts.Add("����ģ���ҷ�?/��������");
                newScripts.Add("���������/����");
                newScripts.Add("�������/����");
                newScripts.Add("16/11");
                newScripts.Add("�ȳ�?/16");
                InteractionScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("����úθ�/�ʴ���");
                newScripts.Add("�ȳ�~/�ȳ�!");
                newScripts.Add("����?/�����Ф�");
                newScripts.Add("������?/����!");
                newScripts.Add("���ƺ��̳�/��ģ�����");
                newScripts.Add("�������־�?/��ģ�����");
                newScripts.Add("��������/�ۿ�������?");
                newScripts.Add("�ʹ������/����������!");
                newScripts.Add("���������/�������غ���");
                newScripts.Add("�����԰�ٳ�?/�� �踦��");
                newScripts.Add("�������̾�!/16");
                newScripts.Add("16/16");
                newScripts.Add("�ݰ���!/�ݰ���!");
                InteractionScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("����úθ�!/��~");
                newScripts.Add("�ȳ��ϻ��!/�޾�ġ��!");
                newScripts.Add("�ȳ�!/�ȳ�!");
                newScripts.Add("��������?/�׷�!");
                newScripts.Add("���������/�����Ʊ���");
                newScripts.Add("�����/���Ѹ�����?");
                newScripts.Add("�ָ�������?/���ۤ�");
                newScripts.Add("�ָ����ٺ�?/��������!");
                newScripts.Add("�ָ�������!/����ų�!");
                newScripts.Add("14/14");
                newScripts.Add("13/14");
                newScripts.Add("�Ӹ��ٲ��?/14");
                InteractionScripts.Add(newScripts);
            }
        }

        // ����
        FreeWalkScripts.Add("�ɽ���..");
        FreeWalkScripts.Add("������?");
        FreeWalkScripts.Add("��");
        FreeWalkScripts.Add("�Ͼ�");
        FreeWalkScripts.Add("�������?");

        FreeWalkScripts2.Add("���Ӹ����ʹ�");
        FreeWalkScripts2.Add("�Ͼ�..");
        FreeWalkScripts2.Add("����ϰ�ʹ�");
        FreeWalkScripts2.Add("���;�");
        FreeWalkScripts2.Add("Ż��ī���̿���");
        FreeWalkScripts2.Add("���������ؾ���");

        // ���� ���� ��ũ��Ʈ
        for (int i = 0; i < 3; i++)
        {
            // ��ȹ
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��������!");
                newScripts.Add("�����ϼ���");
                newScripts.Add("�ý�����..");
                newScripts.Add("��ȹ�̶�..");
                newScripts.Add("�������ٰ�");
                newScripts.Add("����");
                newScripts.Add("�Ͼ�..");
                newScripts.Add("�����ʱ�.");
                ProfScripts.Add(newScripts);
            }
            // ��Ʈ
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�� ����?");
                newScripts.Add("�̰� �� ����");
                newScripts.Add("������ �߿���");
                newScripts.Add("�����!");
                newScripts.Add("��������!");
                newScripts.Add("����");
                newScripts.Add("�Ͼ�..");
                newScripts.Add("�����ʱ�.");
                ProfScripts.Add(newScripts);
            }
            // ���α׷���
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�����ߺ���");
                newScripts.Add("�� �ȵ���?");
                newScripts.Add("��������!");
                newScripts.Add("PBR�̶�..");
                newScripts.Add("JAVA��..");
                newScripts.Add("����");
                newScripts.Add("�Ͼ�..");
                newScripts.Add("�����ʱ�.");
                ProfScripts.Add(newScripts);
            }
            // 2��
            // ��ȹ
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���� Ʈ�����..");
                newScripts.Add("���� ������..");
                newScripts.Add("��õ�� ����..");
                newScripts.Add("�� ���̱�");
                newScripts.Add("����");
                newScripts.Add("���� �ʾ�.");
                newScripts.Add("�հݷ���..");
                newScripts.Add("������ ������");
                ProfScripts.Add(newScripts);
            }
            // ��Ʈ
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������");
                newScripts.Add("�� ������?");
                newScripts.Add("���� �ʱ�.");
                newScripts.Add("���߳�");
                newScripts.Add("��������..");
                newScripts.Add("������");
                newScripts.Add("��õ�� ����..");
                newScripts.Add("ȯ�����̾�");
                ProfScripts.Add(newScripts);
            }
            // ���α׷���
            else if (i == 5)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������ ���ƾ�");
                newScripts.Add("���!");
                newScripts.Add("������..");
                newScripts.Add("��ü����?");
                newScripts.Add("�հݷ���..");
                newScripts.Add("���� ��õ����");
                newScripts.Add("�ڵ�������..");
                newScripts.Add("���� �ڵ��!");
                ProfScripts.Add(newScripts);
            }
        }

        // �л�-���� ��ȭ
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ȳ��ϼ���/�ȳ�~");
                newScripts.Add("�ܸ�����/����");
                newScripts.Add("���ݾƿ�../����..");
                newScripts.Add("16/11");
                newScripts.Add("�̺�����/��������");
                StuProScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ȳ��ϼ���/�ȳ�!");
                newScripts.Add("��!/16");
                StuProScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�����!/�׷���?!");
                newScripts.Add("�� ���Ŀ�/����!!");
                newScripts.Add("��վ��!/14");
                StuProScripts.Add(newScripts);
            }
        }

        // ����-�л� ��ȭ
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�Ӹ��̻ڳ�./�����մϴ�.");
                newScripts.Add("���߸԰�ٳ�/11");
                newScripts.Add("������ʴ�?/�Ҹ��ؿ�.");
                newScripts.Add("�ȳ�/�ȳ��ϼ���");
                newScripts.Add("������?/�����ƿ�");
                newScripts.Add("�������ߴ�?/10");
                ProStuScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������ħ!/�ܵο�!");
                newScripts.Add("�ȳ�?/�ȳ��ϼ���!");
                newScripts.Add("������Ͱ���/��¥��?");
                newScripts.Add("��������/��!");
                newScripts.Add("�����Ѵ�/�����մϴ�!");
                ProStuScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ȳ�!/�ȳ��ϼ���!");
                newScripts.Add("�����?/��!");
                newScripts.Add("������?/�ƴ�!");
                newScripts.Add("�׻� ������/������");
                newScripts.Add("�����ߴ�?/18");
                ProStuScripts.Add(newScripts);
            }
        }

        // ����-���� ��ȭ
        for (int i = 0; i < 2; i++)
        {
            // 3�� ~ 1��
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�������Դϴ�./�׷��׿�.");
                newScripts.Add("�л��� ���?/�ߵ���Ϳ�.");
                newScripts.Add("ȭ����!/20");
                newScripts.Add("����׿�./ȭ����");
                newScripts.Add("�ȳ��ϼ���/�ȳ��ϼ���");
                ProProScripts.Add(newScripts);
            }
            // 2��
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������ ������/�׷��Ը��Դϴ�");
                newScripts.Add("�̺��� ���׿�/�����׿�");
                newScripts.Add("�� ��������?/�� ��������");
                newScripts.Add("�̹� �ص�/����ϼ̽��ϴ�");
                ProProScripts.Add(newScripts);
            }
        }

        // ������õ �Ⱓ
        // ���� �λ�, �̵�
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��ſ���");
                newScripts.Add("������ ��!");
                newScripts.Add("���� ��� �ֱ�");
                newScripts.Add("�ƽ���");
                newScripts.Add("ȸ�翡�� ����!");
                newScripts.Add("�������");
                newScripts.Add("��������ž�");
                newScripts.Add("�̺��̳�");
                newScripts.Add("�� ����");
                newScripts.Add("���п� ����");
                VacationScripts.Add(newScripts);
            }
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("����ٽð�����");
                newScripts.Add("�ƽΰ����ؾ���");
                newScripts.Add("������");
                newScripts.Add("���� ������");
                newScripts.Add("���ϰ� ����");
                newScripts.Add("�帣�氡����");
                newScripts.Add("�ʹ� ���Ҿ�");
                newScripts.Add("�αٵα�");
                newScripts.Add("�ƽ���...");
                newScripts.Add("�ʹ� ����");
                VacationScripts.Add(newScripts);
            }
        }

        // �� �л� - �� �л�
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���߳�/����");
                newScripts.Add("�հ��ϰ�;�/����");
                newScripts.Add("�����鹹�Ұž�/��ž�");
                newScripts.Add("�츮�ٽø���../...?");
                newScripts.Add("ȸ����¥������/����¥?���");
                CStuCStuScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�� �� ����/�ȵɵ�");
                newScripts.Add("�� �� ����?/�翬�Ѱžƴ�?");
                newScripts.Add("�� �հ��ε�/�ٷ� �Ұ��");
                newScripts.Add("�������⹹��/���� ���߾�");
                newScripts.Add("����߾�/����");
                newScripts.Add("���� ���/�Ϻ�����");
                CStuCStuScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���ϰ� �Ծ�?/�Ѿ�");
                newScripts.Add("���ϰ� �Ծ�?/�� �� ���ѵ�");
                newScripts.Add("�� �հ��ε�/�ƴ� �ٷ�Ż��");
                newScripts.Add("�� �հ��ε�/�� Ż���ϵ�");
                newScripts.Add("������Ҿ�/������Ҵ�");
                newScripts.Add("����������;�/����������");
                newScripts.Add("������Ӱ�?/��~!!!");
                CStuCStuScripts.Add(newScripts);
            }
        }

        // �� �л� - �̿� �л�
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�� �������¾�/�����");
                newScripts.Add("������ ����?/�߰ž�");
                newScripts.Add("�� �ٳ����/����");
                newScripts.Add("������ ������/�η��פ�");
                CStuIStuScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�غ� ����/�Фа���");
                newScripts.Add("��������Ծ�/Ȧ���f");
                newScripts.Add("�� �ٳ�Ծ�/����ߴ�");
                newScripts.Add("���� �/�����...");
                newScripts.Add("�� �Ǿ��/�׷� ���̳�");
                newScripts.Add("�� ��/�η���");
                CStuIStuScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�� �������¾�/�������");
                newScripts.Add("�غ��ϰ��־�?/������ ������");
                newScripts.Add("������ ����/�ʹ�����!");
                newScripts.Add("��������Ծ�/���?!");
                newScripts.Add("�����ȴ�/�ɰž�");
                CStuIStuScripts.Add(newScripts);
            }
        }

        // �̿� �л� - �� �л�
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���� �˷���/���� ��?");
                newScripts.Add("���� �ߺ��¹�/�� ���� ����");
                newScripts.Add("�ʹ� �����/ȭ���� ��");
                newScripts.Add("����/����");
                IStuCStuScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������ ���/�� ��");
                newScripts.Add("���� ���� ��/�ּ��� ��^.^");
                newScripts.Add("��¥ ��ƴ�/���� �����?");
                newScripts.Add("������/�˷��ٰ�!");
                newScripts.Add("����!!!/�αٵα�!");
                IStuCStuScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�����!�����/�� ���");
                newScripts.Add("���� �����/�˷��ٰ�!");
                newScripts.Add("�ʹ� �����../�����ٰ�!");
                newScripts.Add("��� �߾�?/�̰� �̷���..");
                newScripts.Add("�����/����� ���ٰ�");
                newScripts.Add("��.../��...");
                IStuCStuScripts.Add(newScripts);
            }
        }

        // �̿� �л� - �̿� �л�
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���� ��������/�� ȥ�ڰ�����");
                newScripts.Add("���� �ҷ�?/�붯ť");
                newScripts.Add("�� �Ű�?/�׷�����");
                newScripts.Add("���� ����/���� ������");
                newScripts.Add("�� �հ��� ��/����");
                IStuIStuScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���� ����/��");
                newScripts.Add("�����غ��ҷ�?/���� ��������");
                newScripts.Add("�� �ϰ��־�?/11");
                newScripts.Add("�α� �α�/����Ҹ� ���");
                newScripts.Add("�ɱ�?/�����ȴ�");
                newScripts.Add("�����ϰ�;�/���ж� ����");
                IStuIStuScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��� ��������/���� �����̾�");
                newScripts.Add("������ ����/���� ����");
                newScripts.Add("������ ������/���� �̰��̾�");
                newScripts.Add("���� �غ�����/���� �ҷ�");
                newScripts.Add("12/19");
                newScripts.Add("����/��...");
                newScripts.Add("�αٵα�/��ٵ��");
                IStuIStuScripts.Add(newScripts);
            }
        }

        // �� �л� - ����
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��!/20");
                newScripts.Add("�� �ߵɱ��?/�� �ɰž�");
                newScripts.Add("�����ſ�/������");
                newScripts.Add("�ٳ�Ծ��/�׷�");
                newScripts.Add("�ȳ��ϼ���/�ȳ�");
                CStuProScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�����ؿ� ��/������Ҵ�");
                newScripts.Add("�ȳ��ϼ���/�� �ٳ�Ծ�?");
                newScripts.Add("���� �����/�հ��Ѱžƴϴ�");
                newScripts.Add("���� ���׿�/�׷���");
                CStuProScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�� �����̿���/����");
                newScripts.Add("�ķ��ؿ�/���ϴ�");
                newScripts.Add("����/16");
                newScripts.Add("���Ѱ� ���ƿ�/���� ���̾�");
                newScripts.Add("���� ���ƿ�/�ʹ� ����");
                newScripts.Add("�ų���!/���� �ų���");
                CStuProScripts.Add(newScripts);
            }
        }

        // �̿� �л� - ����
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("��!/20");
                newScripts.Add("�������/�˷��ٰ�");
                newScripts.Add("������/������!");
                IStuProScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ȳ��ϼ���/�ȳ�~");
                newScripts.Add("�̷��� ����../���� ���ָ�");
                newScripts.Add("�����/������ ����!");
                newScripts.Add("��� �ؿ�?/�����ٰ�");
                IStuProScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������/���� ���̾�");
                newScripts.Add("���� �ּ���/�� ȸ���..");
                newScripts.Add("��õ���ּ���/��");
                newScripts.Add("�ȳ��ϼ���/����� ����?");
                newScripts.Add("�� ���ּ���/��������?");
                IStuProScripts.Add(newScripts);
            }
        }

        // ���� - �� �л�
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ȳ�/�ȳ��ϼ���!");
                newScripts.Add("���� ��ħ/13");
                newScripts.Add("����� �?/�����ƿ�");
                newScripts.Add("����ߴ�/�����ؿ�");
                ProCStuScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ȳ�!/�ȳ��ϼ���!");
                newScripts.Add("���� ��ħ�̾�/���� �׷��Կ�");
                newScripts.Add("�� ����Դ�?/�ּ��� ���߾��");
                newScripts.Add("��� ���Ҿ�/�����մϴ�");
                newScripts.Add("���� ��ڳ�!/�ʹ� �ູ�ؿ�");
                newScripts.Add("����� �?/�����̿���");
                ProCStuScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�ȳ�~/�ȳ��ϼ���!");
                newScripts.Add("���� �Ծ���?/��!!!");
                newScripts.Add("���� �Ծ���?/�� ���ּ���");
                newScripts.Add("�� �ٳ�Դ�?/�������Ͱ��ƿ�");
                newScripts.Add("��� �߾�/�����ؿ� ��");
                newScripts.Add("���� ��ڳ�!/���� ��������!");
                ProCStuScripts.Add(newScripts);
            }
        }

        // ���� - �̿� �л�
        for (int i = 0; i < 3; i++)
        {
            // �ƴ� ����
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������?/����ּ���");
                newScripts.Add("�غ��ؾ���/�˰ڽ��ϴ�");
                ProIStuScripts.Add(newScripts);
            }
            // ģ�� ����
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������?/10");
                newScripts.Add("���� �԰�ٴϴ�/�� �Ƹ���..");
                ProIStuScripts.Add(newScripts);
            }
            // ����
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�� ���?/�������");
                newScripts.Add("�غ�� ���ߴ�?/��! ���߾��");
                newScripts.Add("�����غ���/�����غ��Կ�");
                ProIStuScripts.Add(newScripts);
            }
        }
    }
}
