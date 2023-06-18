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

    public bool IsSavedDataExists = false; // 플레이어가 저장한 데이터 있는지
    public bool IsOriginalDataExists = false; // 초기 원본 데이터

    [SerializeField] private bool UseLoadingData = false;

    public static Json Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("json 인스턴스가 없다");

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
            Debug.Log("인스턴스 생성");
        }
        else
        {
            if (Instance != this)
                Destroy(this.gameObject);
            Debug.Log("인스턴스 이미 있어서 삭제");
        }
    }


    private void Start()
    {
        // 1. 원본데이터는 그대로 로드를 해준다. 언제든 가져다 써야하기 때문에.
        // 첫 로딩에 데이터를 가져온다.
        if (!IsOriginalDataExists)
        {
            LoadAllOriginalData();
            IsOriginalDataExists = true;
        }

        IsSavedDataExists = false; // 확인 전 변수 초기화
        // 저장된 json 파일 존재유무 체크. 로딩 미사용시 빼기.
        if (UseLoadingData)
            IsExistJsonFile();

        // 2. 서버에서 데이터를 불러왔으면 서버 데이터를 사용
        if (AllInOneData.Instance.ServerLoading)
        {
            // 데이터 적절히 넣어주기
            AllInOneData.Instance.DistributeLoadGameData();

            // 저장된 데이터(서버데이터)를 사용한다고 알리고 서버 데이터 사용 정보 정리
            IsSavedDataExists = true;
            AllInOneData.Instance.ServerLoading = false;
        }

        // 3. 로컬 데이터가 있다면 가져온다.
        if (IsSavedDataExists)
        {
            LoadLocalSavedData();

            AllInOneData.Instance.DistributeLoadGameData();
        }
    }

    // 제이슨 파일의 경로가 존재하는지 체크하는 함수 -> 존재하면 true / 없으면 false
    public void CheckFolderExists()
    {
        var folderPath = Path.Combine(Application.persistentDataPath, DataFolderName);
        // var FolderPath = "jar:file://" + Application.dataPath + DataFolderName;

        if (!Directory.Exists(folderPath))
        {
            Debug.Log("false 폴더 경로" + folderPath);
            IsSavedDataExists = false;
        }
        else
        {
            Debug.Log("true 폴더 경로" + folderPath);
            IsSavedDataExists = true;
        }
    }

    // 불러오기 할 데이터가 있는지 확인한다.
    public void CheckFileExists()
    {
        var jsonFilePath = Application.persistentDataPath + "\\" + DataFolderName + "\\LocalSave\\PlayerData.json";

        if (!File.Exists(jsonFilePath))
        {
            Debug.Log("flase 불러올 정보가 없습니다. 파일 경로" + jsonFilePath);
            IsSavedDataExists = false;
        }
        else
        {
            Debug.Log("true 불러올 정보가 있습니다. 파일 경로" + jsonFilePath);
            IsSavedDataExists = true;
        }
    }

    public void ReadyToWhichDataLoad()
    {
    }


    // Json 형식의 파일을 데이터로 변환
    // Json파일 저장을 위한 새로운(기존의) 폴더 생성 (오픈)
    public string CreateJsonFolder(string folderPath, string folderName)
    {
        // 혹시 미래에 저장 갯수 제한 생기면 여기서 처리

        string directoryPath = Path.Combine(folderPath, folderName); // 어플리케이션이 저장 될 저장소 위치 + 저장소 안 폴더이름

        // if (!Directory.Exists(directoryPath))
        if (IsSavedDataExists == false)
        {
            Directory.CreateDirectory(directoryPath); // 저장 경로에 폴더가 없으면 폴더를 생성    -> 확인 완료
        }

        return directoryPath;
    }

    // 원하는 폴더 안에 새로운(기존의) 파일을 생성 (오픈) -> 현재 사용 X
    public FileStream CreateFile(string directoryPath, string fileName)
    {
        string filePath = Path.Combine(directoryPath, fileName); // 저장소 경로 + 파일이름

        FileStream testFile = new FileStream(filePath, FileMode.Create); // 저장 경로의 폴더 안에 파일이 없으면 파일을 생성     -> 확인 완료 

        return testFile;
    }


    // ===== ===== 저장하기 ===== =====
    // 플레이어의 아이디, 아카데미, 원장 이름
    public void SaveToJsonAllInOneData(string path)
    {
        // .json 파일 까지의 모든 경로
        FileInfo fileInfo = new FileInfo(path + "\\PlayerData.json");

        // 파일이 없다면 생성
        if (!fileInfo.Exists)
        {
            using (FileStream fs = fileInfo.Create()) // using 공부하자 뭐냐 using 문이라는데 
            {
                var test = JsonConvert.SerializeObject(AllInOneData.Instance); // 클래스 -> string 변환

                byte[] data = Encoding.UTF8.GetBytes(test); //   string -> byte로 변환
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
        else
        {
            using (FileStream fs = fileInfo.Open(System.IO.FileMode.Create)) // 해당파일 열기 -> 덮어쓰기로
            {
                // 저장 할 데이터 class -> string 변환
                var test = JsonConvert.SerializeObject(AllInOneData.Instance); // 클래스 -> string 변환

                byte[] data = Encoding.UTF8.GetBytes(test); //   string -> byte로 변환
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
    }

    // 인게임 씬에서 저장하기 버튼 눌렀을 때 => UIManager에서 버튼과 연결
    public void SaveDataInLocal()
    {
        string folderPath = CreateJsonFolder(Application.persistentDataPath, DataFolderName); // GiSaveData Folder

        // 5.15 woodpie9 PlayerID 정보 삭제, 폴더 명을 LocalSave 로 사용한다.
        string filePath = CreateJsonFolder(folderPath, "LocalSave"); // UID Floder

        SaveToJsonAllInOneData(filePath); // 플레이어데이터 저장
    }

    // ===== ===== 불러오기 ===== =====
    // 맨 처음에 Json 파일 존재 유무 체크 -> screenTouchButton 연결
    public void IsExistJsonFile()
    {
        CheckFolderExists();
        CheckFileExists();
    }

    // 저장된 데이터들을(json 원본들) 로드 할 함수
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

            // 데이터를 AllinOneData 클리스화 시키는 곳.
            // 데이터 넣는 곳에서 문제 생김
            // 인게임씬에서 이걸 해보자 씬이 달라져서 문제가 생기는건지 뭔지
            AllInOneData.Instance = JsonConvert.DeserializeObject<AllInOneData>(str);
        }
    }

    public void LoadAllOriginalData()
    {
        UnityEngine.Debug.Log("에디터 시작");
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
        // UnityEngine.Debug.Log("경로만 읽은거? 텍스트 에셋으로 저장?" + EventJsonCollectionTextFile);
        // 인재추천 데이터 추가
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
        // 처음 문자열은 마지막거 삭제
        StringCutting(0, ref gameJamContent);
        // 중간 문자열은 앞뒤거 다 삭제
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
        // 마지막 문자열은 앞거 삭제
        StringCutting(2, ref MissionDataList);

        // -----------------------------------------------------------------------------
        // 각각의 제이슨 파일을 읽고 하나로 뭉쳐야 하는데 뭉쳐주질 못해서 일어나는 문제였나??????
        string allJsonOriginal = gameJamContent + "," + SuddenEventList + "," + injaeData + ","
                                 + ChoiceEventScriptRewardList + "," + SimpleExecutionEventRewardList + ","
                                 + EventScriptList + "," + RankScriptList + "," + EmailScriptList + ","
                                 + RewardEmailScriptList + "," + BonusSkillConditionList + "," + BonusSkillScriptList
                                 + "," + ClassAlramScriptList + "," + GameShowDataList + "," + MissionDataList;

        // 데이터를 읽은 다음에 저장을 해야하는데 스트링이 너무 길어서 다 읽지를 못하는 문제가 생긴다. 그래서 각 데이터 별로 클래스를 나눠서 넣어줘야 겠다.
        // 아니다. 각각의 제이슨파일을 읽은 친구들을 
        AllOriginalJsonData.Instance = JsonConvert.DeserializeObject<AllOriginalJsonData>(allJsonOriginal);
    }

    // 제이슨 파일들을 읽고 컨버팅 해주기 전 하나의 스트링으로 만들어서 제이슨파일을 합쳐주고, 하나로 만들어진 스트링을 읽는다
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
        File.SetAttributes(folderPath, FileAttributes.Normal); //폴더 읽기 전용 해제

        foreach (string _folder in Directory.GetDirectories(folderPath)) //폴더 탐색
        {
            DeleteDirectory(_folder); //재귀 호출
        }

        foreach (string _file in Directory.GetFiles(folderPath)) //파일 탐색
        {
            File.SetAttributes(_file, FileAttributes.Normal); //파일 읽기 전용 해제
            File.Delete(_file); //파일 삭제
        }

        Directory.Delete(folderPath); //폴더 삭제
    }

    // 1차 배포를 위한 저장 부분 지우고 커밋
    // private void OnApplicationPause(bool bPaused)
    // {
    //     Debug.Log("OnApplicationPause 실행됨");
    // 
    //     if(bPaused)       // 어플이 일시정지가 될 시 bool 값 true 됨. 
    //     {
    //         SaveAllDataInGameScene();
    //     }
    //     else                // 인간이 어플로 복귀 시 들어올 함수
    //     {
    //         LoadLocalSavedData();
    //     }
    // }
    // 
    // private void OnApplicationQuit()
    // {
    //     Debug.Log("OnApplicationQuit 실행됨");
    // 
    //     SaveAllDataInGameScene();
    // }
}