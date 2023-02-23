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
/// 저장 -> 1개만 , 
/// 자동저장 and 저장버튼 클릭
/// 
/// 다른 데이터 값들을 여기서 모두 모아서 저장, 
/// 어느 로그인데이터에 Json 저장할지는 다른 script에서
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

    // 제이슨 파일의 경로가 존재하는지 체크하는 함수 -> 존재하면 true / 없으면 false
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

    // Json 형식의 파일을 데이터로 변환
    // Json파일 저장을 위한 새로운(기존의) 폴더 생성 (오픈)
    public string CreateJsonFolder(string folderPath, string folderName)
    {
        // 혹시 미래에 저장 갯수 제한 생기면 여기서 처리

        string directoryPath = Path.Combine(folderPath, folderName);      // 어플리케이션이 저장 될 저장소 위치 + 저장소 안 폴더이름

        // if (!Directory.Exists(directoryPath))
        if (IsDataExists == false)
        {
            Directory.CreateDirectory(directoryPath);       // 저장 경로에 폴더가 없으면 폴더를 생성    -> 확인 완료
        }

        return directoryPath;
    }

    // 원하는 폴더 안에 새로운(기존의) 파일을 생성 (오픈) -> 현재 사용 X
    public FileStream CreateFile(string directoryPath, string fileName)
    {
        string filePath = Path.Combine(directoryPath, fileName);                     // 저장소 경로 + 파일이름

        FileStream testFile = new FileStream(filePath, FileMode.Create);       // 저장 경로의 폴더 안에 파일이 없으면 파일을 생성     -> 확인 완료 

        return testFile;
    }

    // 플레이어의 아이디, 아카데미, 원장 이름
    public void SaveToJsonPlayerData(string path)
    {
        // .json 파일 까지의 모든 경로
        FileInfo fileInfo = new FileInfo(path + "\\PlayerData.json");

        // 파일이 없다면 생성
        if (!fileInfo.Exists)
        {
            using (FileStream fs = fileInfo.Create())       // using 공부하자 뭐냐 using 문이라는데 
            {
                var test = JsonConvert.SerializeObject(AllInOneData.Instance);        // 클래스 -> string 변환

                byte[] data = Encoding.UTF8.GetBytes(test);     //   string -> byte로 변환
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
        else
        {
            using (FileStream fs = fileInfo.Open(System.IO.FileMode.Create))        // 해당파일 열기 -> 덮어쓰기로
            {
                // 저장 할 데이터 class -> string 변환
                var test = JsonConvert.SerializeObject(AllInOneData.Instance);        // 클래스 -> string 변환

                byte[] data = Encoding.UTF8.GetBytes(test);     //   string -> byte로 변환
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
    }

    // 여러 데이터 저장해서 테스트 해 볼 함수
    public void SaveDataInTitleScene()
    {
        string FolderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder

        string FilePath = CreateJsonFolder(FolderPath, PlayerInfo.Instance.m_PlayerID);         // UID Floder

        SaveToJsonPlayerData(FilePath);       // 플레이어데이터 저장
    }

    // 인게임 씬에서 저장하기 버튼 눌렀을 때
    public void SaveAllDataInGameScene()
    {
        var filePath = Application.persistentDataPath + "\\" + DataFolderName + "\\" + AllInOneData.Instance.player.m_playerID;
        // var filePath1 = Path.Combine(filePath, DataFolderName);
        // var filePath2 = Path.Combine(filePath1, AllInOneData.Instance.player.m_playerID);        // 경로를 때려박았는데 값이 안들어가서 null 이 뜬다 왜그런진 모르지만 combine()을 써서 경로를 하나로 만들어줬다

        AllInOneData.Instance.SaveAllJsonData();

        SaveToJsonPlayerData(filePath);       // 플레이어데이터 저장
    }

    // Json 을 인게임 데이터로 넣어줄 테스트 함수
    public void TestLoadData(string Folderpath)
    {
        string FolderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder
                                                                                              // string FilePath = 
        // JsonConvert.DeserializeObject();
    }

    // 맨 처음에 Json 파일 존재 유무 체크 -> 
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
            // 모든 데이터 로드 후 InGameScene으로 이동

            pop.PopUpUIOnTitleScene(Panel);
            LoadAllJsonData();
            MoveSceneManager.m_Instance.MoveToInGameScene();
        }
    }

    // 맨 처음 로딩 시 모든 필요한 데이터들을(json 원본들) 로드 할 함수
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

            // 데이터 넣는 곳에서 문제 생김
            // 인게임씬에서 이걸 해보자 씬이 달라져서 문제가 생기는건지 뭔지
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
