using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

/// <summary>
/// 2023 . 02. 02 Mang
/// 
/// ���� -> 1���� , 
/// �ڵ����� and �����ư Ŭ��
/// 
/// �ٸ� ������ ������ ���⼭ ��� ��Ƽ� ����, 
/// ��� �α��ε����Ϳ� Json ���������� �ٸ� script����
/// </summary>
public class Json : MonoBehaviour
{
    private static Json _instance = null;

    const string DataFolderName = "GISaveData";
    public AllInOneData player = new AllInOneData();

    public static Json Instance
    {
        get
        {
            if (Instance == null)
            {
                return null;
            }
                return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "LoadingScene")
        {
            IsExistJsonFile();
        }
        //LoadPlayerData();
    }


    // Json �������� ������ ��ȯ
    public void TranslateToJson()
    {
        string str = JsonUtility.ToJson(PlayerInfo.Instance);

        Debug.Log("To Json : " + str);
    }

    // Json ������ ������ �����ͷ� ��ȯ

    // Json���� ������ ���� ���ο�(������) ���� ���� (����)
    public string CreateJsonFolder(string folderPath, string folderName)
    {
        // Ȥ�� �̷��� ���� ���� ���� ����� ���⼭ ó��

        string directoryPath = Path.Combine(folderPath, folderName);      // ���ø����̼��� ���� �� ����� ��ġ + ����� �� �����̸�

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);       // ���� ��ο� ������ ������ ������ ����    -> Ȯ�� �Ϸ�
        }

        return directoryPath;
    }

    // �α��� ���� �� �� �ѹ� ��������� �� ������ ����
    // playerInfo -> ��ī���� �� ���� null�� �ƴҶ� ���������
    public void CreateUserIDFolder(string path)
    {
        CreateJsonFolder(path, "PlayerData");
    }

    // ���ϴ� ���� �ȿ� ���ο�(������) ������ ���� (����)
    public FileStream CreateFile(string directoryPath, string fileName)
    {
        string filePath = Path.Combine(directoryPath, fileName);                     // ����� ��� + �����̸�

        FileStream testFile = new FileStream(filePath, FileMode.Create);       // ���� ����� ���� �ȿ� ������ ������ ������ ����     -> Ȯ�� �Ϸ� 

        return testFile;
    }

    // �÷��̾��� ���̵�, ��ī����, ���� �̸�
    public void SavePlayerData(string path)
    {
        // .json ���� ������ ��� ���
        FileInfo fileInfo = new FileInfo(path + "\\PlayerData.json");

        // ������ ���ٸ� ����
        if (!fileInfo.Exists)
        {
            using (FileStream fs = fileInfo.Create())       // using �������� ���� using ���̶�µ� 
            {
                var test = JsonConvert.SerializeObject(AllInOneData.Instance);        // Ŭ���� -> string ��ȯ

                byte[] data = Encoding.UTF8.GetBytes(test);     //   string -> byte�� ��ȯ
                fs.Write(data, 0, data.Length);
                fs.Close();
                // fs.Seek(0, System.IO.SeekOrigin.Begin);
            }
        }
        else
        {
            using (FileStream fs = fileInfo.Open(System.IO.FileMode.Create))        // �ش����� ���� -> ������
            {
                // ���� �� ������ class -> string ��ȯ
                var test = JsonConvert.SerializeObject(AllInOneData.Instance);        // Ŭ���� -> string ��ȯ


                byte[] data = Encoding.UTF8.GetBytes(test);     //   string -> byte�� ��ȯ

                fs.Write(data, 0, data.Length);
                fs.Close();
                // fs.Seek(0, System.IO.SeekOrigin.Begin);
            }
        }
    }

    // ���� �ð�, Ȱ��ȭ�� ������
    public void SaveInGameData(string path)
    {
        CreateFile(path, "InGameData.json");
    }

    // Ȱ��ȭ�� ����
    public void SaveMailData(string path)
    {
        CreateFile(path, "MailData.json");
    }

    // ���� Ȱ��ȭ�� �л� - ����, , ��������..
    public void SaveStudantData(string path)
    {
        CreateFile(path, "StudentData.json");
    }

    // ���� Ȱ��ȭ�� ���� - ����..
    public void SaveProffesor(string path)
    {
        CreateFile(path, "ProffesorData.json");
    }

    public void LoadPlayerData(string path)
    {


        // if(!)
    }

    // ���� ������ �����ؼ� �׽�Ʈ �� �� �Լ�
    public void TestSaveJson()
    {
        string FolderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder

        string FilePath = CreateJsonFolder(FolderPath, PlayerInfo.Instance.m_PlayerID);         // UID Floder

        SavePlayerData(FilePath);       // �÷��̾���� ����
        // SaveMailData(FilePath);
        // SaveStudantData(FilePath);
        // SaveProffesor(FilePath);
    }

    // Json �� �ΰ��� �����ͷ� �־��� �׽�Ʈ �Լ�
    public void TestLoadData(string Folderpath)
    {
        string FolderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder
                                                                                              // string FilePath = 

        // JsonConvert.DeserializeObject();

    }

    // �� ó���� Json ���� ���� ���� üũ
    public void IsExistJsonFile()
    {
        DirectoryInfo FolderInfo = new DirectoryInfo(Application.persistentDataPath + "\\" + DataFolderName);

        if (!FolderInfo.Exists)
        {
            // �α��� ������ �����Ƿ� Ÿ��Ʋ �� �̵� -> �α��� �ؾ���
            MoveSceneManager.m_Instance.MoveToTitleScene();
        }
        else
        {
            // ��� ������ �ε� �� InGameScene���� �̵�
            LoadAllJsonData();
            MoveSceneManager.m_Instance.MoveToInGameScene();
        }
    }

    // �� ó�� �ε� �� ��� �ʿ��� �����͵���(json ������) �ε� �� �Լ�
    public void LoadAllJsonData()
    {
        string jsonFilePath = Application.persistentDataPath + "\\" + DataFolderName + "\\@GuestLogin\\PlayerData.json";
        FileInfo playerFile = new FileInfo(jsonFilePath);

        using (FileStream fs = playerFile.OpenRead())
        {
            byte[] arr = new byte[fs.Length + 10];

            UTF8Encoding temp = new UTF8Encoding(true);
            string str = "";

            while (fs.Read(arr, 0, arr.Length) > 0)
            {
                str += temp.GetString(arr);
            }

            // ������ �ִ� ������ ���� ����
            // �ΰ��Ӿ����� �̰� �غ��� ���� �޶����� ������ ����°��� ����
            player = JsonConvert.DeserializeObject<AllInOneData>(str);

            Debug.Log(player);
        }
    }

    //���̽� �д°�
    public void LoadPlayerData()
    {
        string jsonFilePath = Application.persistentDataPath + "\\" + DataFolderName + "\\@GuestLogin\\PlayerData.json";
        FileInfo playerFile = new FileInfo(jsonFilePath);

        using (FileStream fs = playerFile.OpenRead())
        {
            byte[] arr = new byte[1024];

            UTF8Encoding temp = new UTF8Encoding(true);
            string str = "";

            while (fs.Read(arr, 0, arr.Length) > 0)
            {
                str += temp.GetString(arr);
            }

            AllInOneData player = JsonConvert.DeserializeObject<AllInOneData>(str);

            // string tempData =
            //PlayerInfo playerData = (PlayerInfo)JsonToken.read

            Debug.Log("");
        }
    }
}
