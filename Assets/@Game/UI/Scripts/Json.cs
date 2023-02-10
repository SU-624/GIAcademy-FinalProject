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


    // Json 형식으로 데이터 변환
    public void TranslateToJson()
    {
        string str = JsonUtility.ToJson(PlayerInfo.Instance);

        Debug.Log("To Json : " + str);
    }

    // Json 형식의 파일을 데이터로 변환

    // Json파일 저장을 위한 새로운(기존의) 폴더 생성 (오픈)
    public string CreateJsonFolder(string folderPath, string folderName)
    {
        // 혹시 미래에 저장 갯수 제한 생기면 여기서 처리

        string directoryPath = Path.Combine(folderPath, folderName);      // 어플리케이션이 저장 될 저장소 위치 + 저장소 안 폴더이름

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);       // 저장 경로에 폴더가 없으면 폴더를 생성    -> 확인 완료
        }

        return directoryPath;
    }

    // 로그인 성공 시 딱 한번 만들어지는 한 유저의 폴더
    // playerInfo -> 아카데미 명 값이 null이 아닐때 만들어지기
    public void CreateUserIDFolder(string path)
    {
        CreateJsonFolder(path, "PlayerData");
    }

    // 원하는 폴더 안에 새로운(기존의) 파일을 생성 (오픈)
    public FileStream CreateFile(string directoryPath, string fileName)
    {
        string filePath = Path.Combine(directoryPath, fileName);                     // 저장소 경로 + 파일이름

        FileStream testFile = new FileStream(filePath, FileMode.Create);       // 저장 경로의 폴더 안에 파일이 없으면 파일을 생성     -> 확인 완료 

        return testFile;
    }

    // 플레이어의 아이디, 아카데미, 원장 이름
    public void SavePlayerData(string path)
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
                // fs.Seek(0, System.IO.SeekOrigin.Begin);
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
                // fs.Seek(0, System.IO.SeekOrigin.Begin);
            }
        }
    }

    // 현재 시간, 활성화된 스케줄
    public void SaveInGameData(string path)
    {
        CreateFile(path, "InGameData.json");
    }

    // 활성화된 메일
    public void SaveMailData(string path)
    {
        CreateFile(path, "MailData.json");
    }

    // 현재 활성화된 학생 - 스탯, , 졸업여부..
    public void SaveStudantData(string path)
    {
        CreateFile(path, "StudentData.json");
    }

    // 현재 활성화된 교수 - 스탯..
    public void SaveProffesor(string path)
    {
        CreateFile(path, "ProffesorData.json");
    }

    public void LoadPlayerData(string path)
    {


        // if(!)
    }

    // 여러 데이터 저장해서 테스트 해 볼 함수
    public void TestSaveJson()
    {
        string FolderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder

        string FilePath = CreateJsonFolder(FolderPath, PlayerInfo.Instance.m_PlayerID);         // UID Floder

        SavePlayerData(FilePath);       // 플레이어데이터 저장
        // SaveMailData(FilePath);
        // SaveStudantData(FilePath);
        // SaveProffesor(FilePath);
    }

    // Json 을 인게임 데이터로 넣어줄 테스트 함수
    public void TestLoadData(string Folderpath)
    {
        string FolderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder
                                                                                              // string FilePath = 

        // JsonConvert.DeserializeObject();

    }

    // 맨 처음에 Json 파일 존재 유무 체크
    public void IsExistJsonFile()
    {
        DirectoryInfo FolderInfo = new DirectoryInfo(Application.persistentDataPath + "\\" + DataFolderName);

        if (!FolderInfo.Exists)
        {
            // 로그인 정보가 없으므로 타이틀 씬 이동 -> 로그인 해야함
            MoveSceneManager.m_Instance.MoveToTitleScene();
        }
        else
        {
            // 모든 데이터 로드 후 InGameScene으로 이동
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
            player = JsonConvert.DeserializeObject<AllInOneData>(str);

            Debug.Log(player);
        }
    }

    //제이슨 읽는거
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
