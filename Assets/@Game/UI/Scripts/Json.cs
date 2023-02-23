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
    //public AllInOneData player = new AllInOneData();

    public bool IsDataExists;

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

        set
        {
            Instance = value;
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
            if (Instance != this)
                Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // AllInOneData.Instance.LoadAllJsonData();
        LoadAllOriginalData();
    }

    private void Update()
    {
        // AllInOneData.Instance.SaveAllJsonData();
    }

    // ���̽� ������ ��ΰ� �����ϴ��� üũ�ϴ� �Լ� -> �����ϸ� true / ������ false
    public void CheckFolderExists()
    {
        var FolderPath = Path.Combine(Application.persistentDataPath, DataFolderName);

        if (!Directory.Exists(FolderPath))
        {
            IsDataExists = false;
        }
        else
        {
            IsDataExists = true;
        }
    }

    public void CheckFileExists()
    {
        var jsonFilePath = Application.persistentDataPath + "\\" + DataFolderName + "\\@GuestLogin\\PlayerData.json";
        
        if (!File.Exists(jsonFilePath))
        {
            IsDataExists = false;
        }
        else
        {
            IsDataExists = true;
        }
    }

    // Json ������ ������ �����ͷ� ��ȯ
    // Json���� ������ ���� ���ο�(������) ���� ���� (����)
    public string CreateJsonFolder(string folderPath, string folderName)
    {
        // Ȥ�� �̷��� ���� ���� ���� ����� ���⼭ ó��

        string directoryPath = Path.Combine(folderPath, folderName);      // ���ø����̼��� ���� �� ����� ��ġ + ����� �� �����̸�

        // if (!Directory.Exists(directoryPath))
        if (IsDataExists == false)
        {
            Directory.CreateDirectory(directoryPath);       // ���� ��ο� ������ ������ ������ ����    -> Ȯ�� �Ϸ�
        }

        return directoryPath;
    }

    // ���ϴ� ���� �ȿ� ���ο�(������) ������ ���� (����) -> ���� ��� X
    public FileStream CreateFile(string directoryPath, string fileName)
    {
        string filePath = Path.Combine(directoryPath, fileName);                     // ����� ��� + �����̸�

        FileStream testFile = new FileStream(filePath, FileMode.Create);       // ���� ����� ���� �ȿ� ������ ������ ������ ����     -> Ȯ�� �Ϸ� 

        return testFile;
    }

    // �÷��̾��� ���̵�, ��ī����, ���� �̸�
    public void SaveToJsonPlayerData(string path)
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
            }
        }
    }

    // ���� ������ �����ؼ� �׽�Ʈ �� �� �Լ�
    public void SaveDataInTitleScene()
    {
        string FolderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder

        string FilePath = CreateJsonFolder(FolderPath, PlayerInfo.Instance.m_PlayerID);         // UID Floder

        SaveToJsonPlayerData(FilePath);       // �÷��̾���� ����
    }

    // �ΰ��� ������ �����ϱ� ��ư ������ ��
    public void SaveAllDataInGameScene()
    {
        var filePath = Application.persistentDataPath + "\\" + DataFolderName + "\\" + AllInOneData.Instance.player.m_playerID;
        // var filePath1 = Path.Combine(filePath, DataFolderName);
        // var filePath2 = Path.Combine(filePath1, AllInOneData.Instance.player.m_playerID);        // ��θ� �����ھҴµ� ���� �ȵ��� null �� ��� �ֱ׷��� ������ combine()�� �Ἥ ��θ� �ϳ��� ��������

        AllInOneData.Instance.SaveAllJsonData();

        SaveToJsonPlayerData(filePath);       // �÷��̾���� ����
    }

    // Json �� �ΰ��� �����ͷ� �־��� �׽�Ʈ �Լ�
    public void TestLoadData(string Folderpath)
    {
        string FolderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder
                                                                                              // string FilePath = 
        // JsonConvert.DeserializeObject();
    }

    // �� ó���� Json ���� ���� ���� üũ -> 
    public void IsExistJsonFile(GameObject Panel)
    {
        PopUpUI pop = new PopUpUI();
        //DirectoryInfo FolderInfo = new DirectoryInfo(Application.persistentDataPath + "\\" + DataFolderName);

        CheckFolderExists();
        CheckFileExists();

        // if (!FolderInfo.Exists)
        if (IsDataExists == false)
        {
        }
        else
        {
            // ��� ������ �ε� �� InGameScene���� �̵�

            pop.PopUpUIOnTitleScene(Panel);
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
            AllInOneData.Instance = JsonConvert.DeserializeObject<AllInOneData>(str);

            Debug.Log(AllInOneData.Instance);
        }
    }

    // 
    public void LoadAllOriginalData()
    {
        string jsonFilePath = Application.dataPath + "\\Json\\TestOriginalEventData.json";
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

            AllOriginalJsonData.Instance = JsonConvert.DeserializeObject<AllOriginalJsonData>(str);

            Debug.Log(AllOriginalJsonData.Instance);
        }
    }
}
