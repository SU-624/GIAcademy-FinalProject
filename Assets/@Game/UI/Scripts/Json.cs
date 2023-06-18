using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    public bool IsSavedDataExists = false; // �÷��̾ ������ ������ �ִ���
    public bool IsOriginalDataExists = false; // �ʱ� ���� ������

    [SerializeField] private bool UseLoadingData = false;

    public static Json Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("json �ν��Ͻ��� ����");

                return null;
            }

            return _instance;
        }

        set
        {
            Instance = value;

            Debug.Log("set json");
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("�ν��Ͻ� ����");
        }
        else
        {
            if (Instance != this)
                Destroy(this.gameObject);
            Debug.Log("�ν��Ͻ� �̹� �־ ����");
        }
    }


    private void Start()
    {
        // 1. ���������ʹ� �״�� �ε带 ���ش�. ������ ������ ����ϱ� ������.
        // ù �ε��� �����͸� �����´�.
        if (!IsOriginalDataExists)
        {
            LoadAllOriginalData();
            IsOriginalDataExists = true;
        }

        IsSavedDataExists = false; // Ȯ�� �� ���� �ʱ�ȭ
        // ����� json ���� �������� üũ. �ε� �̻��� ����.
        if (UseLoadingData)
            IsExistJsonFile();

        // 2. �������� �����͸� �ҷ������� ���� �����͸� ���
        if (AllInOneData.Instance.ServerLoading)
        {
            // ������ ������ �־��ֱ�
            AllInOneData.Instance.DistributeLoadGameData();

            // ����� ������(����������)�� ����Ѵٰ� �˸��� ���� ������ ��� ���� ����
            IsSavedDataExists = true;
            AllInOneData.Instance.ServerLoading = false;
        }

        // 3. ���� �����Ͱ� �ִٸ� �����´�.
        if (IsSavedDataExists)
        {
            LoadLocalSavedData();

            AllInOneData.Instance.DistributeLoadGameData();
        }
    }

    // ���̽� ������ ��ΰ� �����ϴ��� üũ�ϴ� �Լ� -> �����ϸ� true / ������ false
    public void CheckFolderExists()
    {
        var folderPath = Path.Combine(Application.persistentDataPath, DataFolderName);
        // var FolderPath = "jar:file://" + Application.dataPath + DataFolderName;

        if (!Directory.Exists(folderPath))
        {
            Debug.Log("false ���� ���" + folderPath);
            IsSavedDataExists = false;
        }
        else
        {
            Debug.Log("true ���� ���" + folderPath);
            IsSavedDataExists = true;
        }
    }

    // �ҷ����� �� �����Ͱ� �ִ��� Ȯ���Ѵ�.
    public void CheckFileExists()
    {
        var jsonFilePath = Application.persistentDataPath + "\\" + DataFolderName + "\\LocalSave\\PlayerData.json";

        if (!File.Exists(jsonFilePath))
        {
            Debug.Log("flase �ҷ��� ������ �����ϴ�. ���� ���" + jsonFilePath);
            IsSavedDataExists = false;
        }
        else
        {
            Debug.Log("true �ҷ��� ������ �ֽ��ϴ�. ���� ���" + jsonFilePath);
            IsSavedDataExists = true;
        }
    }

    public void ReadyToWhichDataLoad()
    {
    }


    // Json ������ ������ �����ͷ� ��ȯ
    // Json���� ������ ���� ���ο�(������) ���� ���� (����)
    public string CreateJsonFolder(string folderPath, string folderName)
    {
        // Ȥ�� �̷��� ���� ���� ���� ����� ���⼭ ó��

        string directoryPath = Path.Combine(folderPath, folderName); // ���ø����̼��� ���� �� ����� ��ġ + ����� �� �����̸�

        // if (!Directory.Exists(directoryPath))
        if (IsSavedDataExists == false)
        {
            Directory.CreateDirectory(directoryPath); // ���� ��ο� ������ ������ ������ ����    -> Ȯ�� �Ϸ�
        }

        return directoryPath;
    }

    // ���ϴ� ���� �ȿ� ���ο�(������) ������ ���� (����) -> ���� ��� X
    public FileStream CreateFile(string directoryPath, string fileName)
    {
        string filePath = Path.Combine(directoryPath, fileName); // ����� ��� + �����̸�

        FileStream testFile = new FileStream(filePath, FileMode.Create); // ���� ����� ���� �ȿ� ������ ������ ������ ����     -> Ȯ�� �Ϸ� 

        return testFile;
    }


    // ===== ===== �����ϱ� ===== =====
    // �÷��̾��� ���̵�, ��ī����, ���� �̸�
    public void SaveToJsonAllInOneData(string path)
    {
        // .json ���� ������ ��� ���
        FileInfo fileInfo = new FileInfo(path + "\\PlayerData.json");

        // ������ ���ٸ� ����
        if (!fileInfo.Exists)
        {
            using (FileStream fs = fileInfo.Create()) // using �������� ���� using ���̶�µ� 
            {
                var test = JsonConvert.SerializeObject(AllInOneData.Instance); // Ŭ���� -> string ��ȯ

                byte[] data = Encoding.UTF8.GetBytes(test); //   string -> byte�� ��ȯ
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
        else
        {
            using (FileStream fs = fileInfo.Open(System.IO.FileMode.Create)) // �ش����� ���� -> ������
            {
                // ���� �� ������ class -> string ��ȯ
                var test = JsonConvert.SerializeObject(AllInOneData.Instance); // Ŭ���� -> string ��ȯ

                byte[] data = Encoding.UTF8.GetBytes(test); //   string -> byte�� ��ȯ
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
    }

    // �ΰ��� ������ �����ϱ� ��ư ������ �� => UIManager���� ��ư�� ����
    public void SaveDataInLocal()
    {
        string folderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder

        // 5.15 woodpie9 PlayerID ���� ����, ���� ���� LocalSave �� ����Ѵ�.
        string filePath = CreateJsonFolder(folderPath, "LocalSave"); // UID Floder

        SaveToJsonAllInOneData(filePath); // �÷��̾���� ����
    }

    // ===== ===== �ҷ����� ===== =====
    // �� ó���� Json ���� ���� ���� üũ -> screenTouchButton ����
    public void IsExistJsonFile()
    {
        CheckFolderExists();
        CheckFileExists();
    }

    // ����� �����͵���(json ������) �ε� �� �Լ�
    // 
    public void LoadLocalSavedData()
    {
        string jsonFilePath = Application.persistentDataPath + "\\" + DataFolderName + "\\LocalSave\\PlayerData.json";

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

            // �����͸� AllinOneData Ŭ����ȭ ��Ű�� ��.
            // ������ �ִ� ������ ���� ����
            // �ΰ��Ӿ����� �̰� �غ��� ���� �޶����� ������ ����°��� ����
            AllInOneData.Instance = JsonConvert.DeserializeObject<AllInOneData>(str);
        }
    }

    public void LoadAllOriginalData()
    {
        UnityEngine.Debug.Log("������ ����");
        TextAsset jsondata = Resources.Load<TextAsset>("Json/GameJamData4");
        TextAsset injaeJsondata = Resources.Load<TextAsset>("Json/InJaeData3");
        // -----------------------------------------------------------------------------
        TextAsset SuddenEventTableListTextFile = Resources.Load<TextAsset>("Json/SuddenEventTableList");
        TextAsset OptionChoiceEventRewardTextFile = Resources.Load<TextAsset>("Json/OptionChoiceEventReward");
        TextAsset SimpleExecutionEventRewardTextFile = Resources.Load<TextAsset>("Json/SimpleExecutionEventReward");
        TextAsset EventScriptTextFile = Resources.Load<TextAsset>("Json/EventScript");
        TextAsset RankScriptFile = Resources.Load<TextAsset>("Json/rankScript");
        TextAsset EmailScriptFile = Resources.Load<TextAsset>("Json/EmailData");
        TextAsset RewardEmailScriptFile = Resources.Load<TextAsset>("Json/RewardEmail");
        TextAsset BonusSkillConditionFile = Resources.Load<TextAsset>("Json/BonusSkillConditionData");
        TextAsset BonusSkillScriptFile = Resources.Load<TextAsset>("Json/BonusSkillScriptData");
        TextAsset ClassAlramScriptfile = Resources.Load<TextAsset>("Json/ClassAlramData");
        TextAsset GameShowDataFile = Resources.Load<TextAsset>("Json/GameShowData");
        TextAsset MissionDataFile = Resources.Load<TextAsset>("Json/OrigianlMissionData");
        // -----------------------------------------------------------------------------
        // UnityEngine.Debug.Log("��θ� ������? �ؽ�Ʈ �������� ����?" + EventJsonCollectionTextFile);
        // ������õ ������ �߰�
        string gameJamContent = jsondata.text;
        string injaeData = injaeJsondata.text;
        // -----------------------------------------------------------------------------
        string SuddenEventList = SuddenEventTableListTextFile.text;
        string ChoiceEventScriptRewardList = OptionChoiceEventRewardTextFile.text;
        string SimpleExecutionEventRewardList = SimpleExecutionEventRewardTextFile.text;
        string EventScriptList = EventScriptTextFile.text;
        string RankScriptList = RankScriptFile.text;
        string EmailScriptList = EmailScriptFile.text;
        string RewardEmailScriptList = RewardEmailScriptFile.text;
        string BonusSkillConditionList = BonusSkillConditionFile.text;
        string BonusSkillScriptList = BonusSkillScriptFile.text;
        string ClassAlramScriptList = ClassAlramScriptfile.text;
        string GameShowDataList = GameShowDataFile.text;
        string MissionDataList = MissionDataFile.text;
        // -----------------------------------------------------------------------------
        // ó�� ���ڿ��� �������� ����
        StringCutting(0, ref gameJamContent);
        // �߰� ���ڿ��� �յڰ� �� ����
        StringCutting(1, ref injaeData);
        StringCutting(1, ref SuddenEventList);
        // // -----------------------------------------------------------------------------
        StringCutting(1, ref ChoiceEventScriptRewardList);
        StringCutting(1, ref SimpleExecutionEventRewardList);
        StringCutting(1, ref EventScriptList);
        StringCutting(1, ref RankScriptList);
        StringCutting(1, ref EmailScriptList);
        StringCutting(1, ref RewardEmailScriptList);
        StringCutting(1, ref BonusSkillConditionList);
        StringCutting(1, ref BonusSkillScriptList);
        StringCutting(1, ref ClassAlramScriptList);
        StringCutting(1, ref GameShowDataList);
        // -----------------------------------------------------------------------------
        // ������ ���ڿ��� �հ� ����
        StringCutting(2, ref MissionDataList);

        // -----------------------------------------------------------------------------
        // ������ ���̽� ������ �а� �ϳ��� ���ľ� �ϴµ� �������� ���ؼ� �Ͼ�� ��������??????
        string allJsonOriginal = gameJamContent + "," + SuddenEventList + "," + injaeData + ","
                                 + ChoiceEventScriptRewardList + "," + SimpleExecutionEventRewardList + ","
                                 + EventScriptList + "," + RankScriptList + "," + EmailScriptList + ","
                                 + RewardEmailScriptList + "," + BonusSkillConditionList + "," + BonusSkillScriptList
                                 + "," + ClassAlramScriptList + "," + GameShowDataList + "," + MissionDataList;

        // �����͸� ���� ������ ������ �ؾ��ϴµ� ��Ʈ���� �ʹ� �� �� ������ ���ϴ� ������ �����. �׷��� �� ������ ���� Ŭ������ ������ �־���� �ڴ�.
        // �ƴϴ�. ������ ���̽������� ���� ģ������ 
        AllOriginalJsonData.Instance = JsonConvert.DeserializeObject<AllOriginalJsonData>(allJsonOriginal);
    }

    // ���̽� ���ϵ��� �а� ������ ���ֱ� �� �ϳ��� ��Ʈ������ ���� ���̽������� �����ְ�, �ϳ��� ������� ��Ʈ���� �д´�
    public void StringCutting(int _i, ref string _string)
    {
        switch (_i)
        {
            case 0:
            {
                if (_string.EndsWith("}"))
                {
                    _string = _string.Substring(0, _string.Length - 1);
                }
            }
                break;

            case 1:
            {
                if (_string.EndsWith("}"))
                {
                    _string = _string.Substring(0, _string.Length - 1);
                }

                if (_string.StartsWith("{"))
                {
                    _string = _string.Substring(1, _string.Length - 1);
                }
            }
                break;

            case 2:
            {
                if (_string.StartsWith("{"))
                {
                    _string = _string.Substring(1, _string.Length - 1);
                }
            }
                break;
        }
    }

    public void DeleteJsonFolder()
    {
        Debug.Log("Delete save file folder");
        DeleteDirectory(Application.persistentDataPath + "\\" + DataFolderName);
        CheckFolderExists();
    }

    private void DeleteDirectory(string folderPath)
    {
        File.SetAttributes(folderPath, FileAttributes.Normal); //���� �б� ���� ����

        foreach (string _folder in Directory.GetDirectories(folderPath)) //���� Ž��
        {
            DeleteDirectory(_folder); //��� ȣ��
        }

        foreach (string _file in Directory.GetFiles(folderPath)) //���� Ž��
        {
            File.SetAttributes(_file, FileAttributes.Normal); //���� �б� ���� ����
            File.Delete(_file); //���� ����
        }

        Directory.Delete(folderPath); //���� ����
    }

    // 1�� ������ ���� ���� �κ� ����� Ŀ��
    // private void OnApplicationPause(bool bPaused)
    // {
    //     Debug.Log("OnApplicationPause �����");
    // 
    //     if(bPaused)       // ������ �Ͻ������� �� �� bool �� true ��. 
    //     {
    //         SaveAllDataInGameScene();
    //     }
    //     else                // �ΰ��� ���÷� ���� �� ���� �Լ�
    //     {
    //         LoadLocalSavedData();
    //     }
    // }
    // 
    // private void OnApplicationQuit()
    // {
    //     Debug.Log("OnApplicationQuit �����");
    // 
    //     SaveAllDataInGameScene();
    // }
}