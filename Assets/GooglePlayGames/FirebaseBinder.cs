using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting; // ToDictionary


public class FirebaseBinder : MonoBehaviour
{
    private static FirebaseBinder instance = null;

    // firebase 계정 관련 정보들
    private FirebaseAuth auth;
    private FirebaseUser user;
    private FirebaseFirestore db;

    // 로그인 여부를 체크하는 델리게이트
    public Action<bool> LoginState;

    //저장 Test용 데이터 읽어오기
    public EventScheduleSystem eventScheduleSystem;
    public MailManagement mailManagement;

    // Unity Singleton
    public static FirebaseBinder Instance
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Firebase UserId를 가져오는 코드
    // 로그인이 되어있지 않을 때는 null을 리턴한다.
    public string UserId
    {
        get
        {
            if (user == null)
            {
                return null;
            }

            return user.UserId;
        }
    }

    // DisplayName을 가져오는 코드
    // 로그인이 되어있지 않을 떄는 null을 리턴한다.
    public string DisplayName
    {
        get
        {
            if (user == null)
            {
                return null;
            }

            return user.DisplayName;
        }
    }

    private void Start()
    {
        // Firebase Unity SDK에는 Google Play  서비스가 필요하고 SDK를 사용하려면 최신버전이여야 함.
        // 구글 플레이 버전이 재대로 되어있는지, 지원하고 있는 버전인지 체크해보자.
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(dependencyTask =>
        {
            // 결과가 나오게 되면 어떻게 처리할 것인지 지정한다. 결과를 task의 Result 안에 있다.
            var dependencyStatus = dependencyTask.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // 사용할 수 있다는 결과가 나오면 그냥 사용하면 된다.
                FirebaseInit();
            }
            else
            {
                // Firebase Unity SDK is not safe to use here.
                UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    private void FirebaseInit()
    {
        Debug.Log("Called Firebase Init");

        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        // 초기화시 자동 로그인 방지
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
        }

        // auth의 상태가 변경될 때마다 특정 이벤트를 하도록 등록 (이벤트 핸들러 등록)         
        auth.StateChanged += AuthStateChanged;
    }

    // 인증 상태가 변경될 떄 작동하는 이벤트
    private void AuthStateChanged(object sender, EventArgs e)
    {
        // 현재 사용자가 변경되지 않은 경우, 아무것도 하지 않음
        if (auth.CurrentUser == user) return;

        // 지금 유저와 이전 유저가 다르다는 것은 게정 상태가 변한것.
        bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
        if (!signed && user != null)
        {
            Debug.Log("파이버베이스 로그아웃");
            LoginState?.Invoke(false);
        }

        user = auth.CurrentUser;
        if (signed)
        {
            Debug.Log("파이어베이스 로그인");
            LoginState?.Invoke(true);
        }
    }

    // 파이어베이스에 구글아이디로 로그인한다.
    public void TryFirebaseGoogleLogin(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }

    // Firebase를 로그아웃 한다.
    public void TryLogout()
    {
        auth.SignOut(); // Firebase 로그아웃
        Debug.Log("파이어베이스 로그아웃");
    }


    // Firebase 계정 정보를 수정한다.
    public void OnChangeDisplayName(string newDisplayName)
    {
        FirebaseUser user = auth.CurrentUser;
        UserProfile profile = new UserProfile();
        profile.DisplayName = newDisplayName;
        user.UpdateUserProfileAsync(profile).ContinueWith(updateTask =>
        {
            if (updateTask.IsCanceled)
            {
                Debug.LogError("UpdateUserProfileAsync was canceled.");
                return;
            }

            if (updateTask.IsFaulted)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + updateTask.Exception);
                return;
            }

            Debug.Log("User profile updated successfully.");
            Debug.Log(profile.DisplayName);
        });
    }

    // Firebase 익명로그인을 한다.
    public void SignInAnonymous()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(signInTask =>
        {
            if (signInTask.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }

            if (signInTask.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + signInTask.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = signInTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }


    // Firebase이메일 기반 로그인 / 회원가입
    public void CreateEmailID(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }

            // 회원가입 실패 이유 => 이메일이 비정상, 비밀번호가 너무 간단, 이미 가입된 이메일 등등
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    // Firebase 이메일로 로그인한다.
    public void SignInEmail(string email, string password)
    {
        Firebase.Auth.Credential credential =
            Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void DeleteDataInFirestore()
    {
        CollectionReference userCollection = db.Collection("Users").Document(UserId).Collection("SaveData");
        DocumentReference docRef;

        // Delete Document
        docRef = userCollection.Document("PlayerData");
        docRef.DeleteAsync();

        docRef = userCollection.Document("StudentData");
        docRef.DeleteAsync();

        docRef = userCollection.Document("ProfessorData");
        docRef.DeleteAsync();

        docRef = userCollection.Document("SuddenEventData");
        docRef.DeleteAsync();

        docRef = userCollection.Document("TodaySuddenEventData");
        docRef.DeleteAsync();

        docRef = userCollection.Document("SendMailData");
        docRef.DeleteAsync();

        docRef = userCollection.Document("GameJamToBeRunning");
        docRef.DeleteAsync();

        docRef = userCollection.Document("GameShowToBeRunning");
        docRef.DeleteAsync();

        docRef = userCollection.Document("GameJamSaveData_values");
        docRef.DeleteAsync();

        docRef = userCollection.Document("GameShowSaveData_values");
        docRef.DeleteAsync();

        docRef = db.Collection("Users").Document(UserId);
        docRef.DeleteAsync();
    }

    public void SaveDataInFirestore()
    {
        int count = 0;
        /////////////////////////////// 6월 12일 멘토링 리스트를 직접 넣어도 된다....!!
        PlayerSaveData playerSaveData = new PlayerSaveData();
        Dictionary<string, StudentSaveData> studentData = new Dictionary<string, StudentSaveData>();
        Dictionary<string, ProfessorSaveData> professorData = new Dictionary<string, ProfessorSaveData>();
        Dictionary<string, UsedEventSaveData> usedEventData = new Dictionary<string, UsedEventSaveData>();
        Dictionary<string, TodayEventSaveData> todayEventData =
            new Dictionary<string, TodayEventSaveData>();

        Dictionary<string, SendMailSaveData> sendMailSaveData = new Dictionary<string, SendMailSaveData>();
        Dictionary<string, GameJamSaveData> gameJamToBeRunning = new Dictionary<string, GameJamSaveData>();
        Dictionary<string, GameShowSaveData> gameShowToBeRunning = new Dictionary<string, GameShowSaveData>();

        Dictionary<string, GameJamSaveData> gameJamSaveData_values = new Dictionary<string, GameJamSaveData>();
        Dictionary<string, GameShowSaveData> gameShowSaveData_values = new Dictionary<string, GameShowSaveData>();


        // 플레이어 정보는 리스트가 아니기 떄문에 바로 파이어스토어에 저장
        playerSaveData = AllInOneData.Instance.PlayerData;

        // 리스트 딕셔너리화
        count = 0;
        foreach (StudentSaveData data in AllInOneData.Instance.StudentData)
            studentData.Add($"Student_{count++}", data);

        count = 0;
        foreach (ProfessorSaveData data in AllInOneData.Instance.ProfessorData)
            professorData.Add($"Professor_{count++}", data);

        count = 0;
        foreach (UsedEventSaveData data in AllInOneData.Instance.SuddenEventData)
            usedEventData.Add($"SuddenEvent_{count++}", data);

        count = 0;
        foreach (TodayEventSaveData data in AllInOneData.Instance.TodaySuddenEventData)
            todayEventData.Add($"TodayEvent_{count++}", data);

        count = 0;
        foreach (SendMailSaveData data in AllInOneData.Instance.SendMailData)
            sendMailSaveData.Add($"SendMail_{count++}", data);

        count = 0;
        foreach (GameJamSaveData data in AllInOneData.Instance.GameJamToBeRunning)
            gameJamToBeRunning.Add($"GameJamToBeRunning_{count++}", data);

        count = 0;
        foreach (GameShowSaveData data in AllInOneData.Instance.GameShowToBeRunning)
            gameShowToBeRunning.Add($"GameShowToBeRunning_{count++}", data);


        // 딕셔너리 정보를 키와 값으로 분리하여 저장하자.
        List<int> intKey = new List<int>();
        List<int> intValue = new List<int>();
        List<string> strKey = new List<string>();
        List<string> strValue = new List<string>();

        intKey = playerSaveData.GameDesignerClassDic.Keys.ToList();
        intValue = playerSaveData.GameDesignerClassDic.Values.ToList();
        playerSaveData.GameDesignerClassDic_keys = intKey;
        playerSaveData.GameDesignerClassDic_values = intValue;

        intKey = playerSaveData.ArtClassDic.Keys.ToList();
        intValue = playerSaveData.ArtClassDic.Values.ToList();
        playerSaveData.ArtClassDic_keys = intKey;
        playerSaveData.ArtClassDic_values = intValue;

        intKey = playerSaveData.ProgrammingClassDic.Keys.ToList();
        intValue = playerSaveData.ProgrammingClassDic.Values.ToList();
        playerSaveData.ProgrammingClassDic_keys = intKey;
        playerSaveData.ProgrammingClassDic_values = intValue;


        strKey = playerSaveData.GameJamEntryCount.Keys.ToList();
        intValue = playerSaveData.GameJamEntryCount.Values.ToList();
        playerSaveData.GameJamEntryCount_keys = strKey;
        playerSaveData.GameJamEntryCount_values = intValue;

        ///
        strKey = playerSaveData.NeedGameDesignerStat.Keys.ToList();
        intValue = playerSaveData.NeedGameDesignerStat.Values.ToList();
        playerSaveData.NeedGameDesignerStat_keys = strKey;
        playerSaveData.NeedGameDesignerStat_values = intValue;

        strKey = playerSaveData.NeedArtStat.Keys.ToList();
        intValue = playerSaveData.NeedArtStat.Values.ToList();
        playerSaveData.NeedArtStat_keys = strKey;
        playerSaveData.NeedArtStat_values = intValue;

        strKey = playerSaveData.NeedProgrammingStat.Keys.ToList();
        intValue = playerSaveData.NeedProgrammingStat.Values.ToList();
        playerSaveData.NeedProgrammingStat_keys = strKey;
        playerSaveData.NeedProgrammingStat_values = intValue;


        // 복잡한 클래스를 담은 딕셔너리 인 게임잼이랑 게임쇼 데이터도 분리하자....!!!!!
        List<int> gameJamKey = new List<int>();
        List<List<GameJamSaveData>> gameJamValue = new List<List<GameJamSaveData>>();

        gameJamKey = AllInOneData.Instance.GameJamHistory.Keys.ToList();
        gameJamValue = AllInOneData.Instance.GameJamHistory.Values.ToList(); // 리스트가 여러개

        //Key 값에도 의미가 있네.... 같은 이름의 게임잼이나 게임쇼가 들어가면 필요하다!
        List<int> gameJamDeepKey = new List<int>();
        List<GameJamSaveData> GameJamDeepValue = new List<GameJamSaveData>();


        // 분해하는 큰 리스트를 순회
        for (var outSideIndex = 0; outSideIndex < gameJamValue.Count; outSideIndex++)
        {
            for (var inSideIndex = 0; inSideIndex < gameJamValue[outSideIndex].Count; inSideIndex++)
            {
                gameJamDeepKey.Add(inSideIndex);
                GameJamDeepValue.Add(gameJamValue[outSideIndex][inSideIndex]);
            }
        }

        playerSaveData.GameJamSaveDic_keys = gameJamKey;
        playerSaveData.GameJamSaveDic_Deepkeys = gameJamDeepKey;

        count = 0;
        foreach (GameJamSaveData data in GameJamDeepValue)
            gameJamSaveData_values.Add($"GameJamDeepValue_{count++}", data);


        // 게임 쇼 정보 나누기
        List<int> gameShowKey = new List<int>();
        List<List<GameShowSaveData>> gameShowvalue = new List<List<GameShowSaveData>>();

        gameShowKey = AllInOneData.Instance.GameShowHistory.Keys.ToList();
        gameShowvalue = AllInOneData.Instance.GameShowHistory.Values.ToList(); // 리스트가 여러개

        List<int> gameShowDeepKey = new List<int>();
        List<GameShowSaveData> gameShowDeepValue = new List<GameShowSaveData>();

        for (var outSideIndex = 0; outSideIndex < gameShowvalue.Count; outSideIndex++)
        {
            for (var inSideIndex = 0; inSideIndex < gameShowvalue[outSideIndex].Count; inSideIndex++)
            {
                gameShowDeepKey.Add(inSideIndex);
                gameShowDeepValue.Add(gameShowvalue[outSideIndex][inSideIndex]);
            }
        }

        playerSaveData.GameShowSaveDic_keys = gameShowKey;
        playerSaveData.GameShowSaveDic_Deepkeys = gameShowDeepKey;

        count = 0;
        foreach (GameShowSaveData data in gameShowDeepValue)
            gameShowSaveData_values.Add($"GameShowDeepValue_{count++}", data);


        // 구글 서버에 실제로 저장되는 부분
        CollectionReference userCollection = db.Collection("Users").Document(UserId).Collection("SaveData");
        DocumentReference docRef;

        // Create a new document in the collection
        // 이름 지정 안해주면 UUID 텍스트가 이름이 됨
        docRef = userCollection.Document("PlayerData");
        docRef.SetAsync(playerSaveData);

        docRef = userCollection.Document("StudentData");
        docRef.SetAsync(studentData);

        docRef = userCollection.Document("ProfessorData");
        docRef.SetAsync(professorData);

        docRef = userCollection.Document("SuddenEventData");
        docRef.SetAsync(usedEventData);

        docRef = userCollection.Document("TodaySuddenEventData");
        docRef.SetAsync(todayEventData);

        docRef = userCollection.Document("SendMailData");
        docRef.SetAsync(sendMailSaveData);

        docRef = userCollection.Document("GameJamToBeRunning");
        docRef.SetAsync(gameJamToBeRunning);

        docRef = userCollection.Document("GameShowToBeRunning");
        docRef.SetAsync(gameShowToBeRunning);

        docRef = userCollection.Document("GameJamSaveData_values");
        docRef.SetAsync(gameJamSaveData_values);

        docRef = userCollection.Document("GameShowSaveData_values");
        docRef.SetAsync(gameShowSaveData_values);


        // docRef = userCollection.Document("key");
        // docRef.SetAsync(key);
        //
        // docRef = userCollection.Document("deepKey");
        // docRef.SetAsync(deepKey);
        //
        // docRef = userCollection.Document("deepValue");
        // docRef.SetAsync(deepValue);

        // 이거도 안된다... 그냥 쪼개야 하는건가
        // DicSaveTest test = new DicSaveTest();
        // test = AllInOneData.Instance.TestData;
        // docRef = userCollection.Document("Test");
        // docRef.SetAsync(test);

        // 안된다 ㅠㅠ
        //docRef = userCollection.Document("test");
        //docRef.SetAsync(AllInOneData.Instance.GameJamHistory);
        // 이건 딕셔너리인데 왜 안될까나...
        // docRef = userCollection.Document("GameDesignerClassDic");
        // docRef.SetAsync(playerSaveData.GameDesignerClassDic);


        // 구 저장 데이터 (6월 12일 멘토링에서 전면 수정)
        // 학생 데이터를 딕셔너리로 만들어서 저장하기
        // Dictionary<string, object> studentData = new Dictionary<string, object>();
        //
        // foreach (StudentSaveData saveData in StudentData) // for Test
        //     // foreach (StudentSaveData saveData in AllInOneData.Instance.StudentData)
        // {
        //     studentData.Add("Student_" + index, new Dictionary<string, object>
        //     {
        //         { "StudentID", saveData.StudentID },
        //         { "StudentName", saveData.StudentName },
        //         { "Health", saveData.Health },
        //         { "Passion", saveData.Passion },
        //         { "StudentType", saveData.StudentType },
        //
        //         { "AbilityAmountArr", saveData.AbilityAmountArr },
        //         { "Skills", saveData.Skills },
        //         { "GenreAmountArr", saveData.GenreAmountArr },
        //         { "Personality", saveData.Personality },
        //         { "NumberOfEntries", saveData.NumberOfEntries },
        //
        //         { "Friendship", saveData.Friendship },
        //     });
        //     index++;
        // }
        // index = 0;
        // // 교수 데이터를 딕셔너리로 만들어서 저장하기
        // Dictionary<string, object> professorData = new Dictionary<string, object>();
        //
        // foreach (var saveData in professorDatas) // for Test
        //     // foreach (StudentSaveData saveData in AllInOneData.Instance.ProfessorData)
        // {
        //     professorData.Add($"Professor_{index}", new Dictionary<string, object>
        //     {
        //         { "ProfessorID", saveData.ProfessorID },
        //         { "ProfessorName", saveData.ProfessorName },
        //         { "ProfessorType", saveData.ProfessorType },
        //         { "ProfessorSet", saveData.ProfessorSet },
        //         { "ProfessorPower", saveData.ProfessorPower },
        //
        //         { "ProfessorExperience", saveData.ProfessorExperience },
        //         { "ProfessorSkills", saveData.ProfessorSkills },
        //         { "ProfessorPay", saveData.ProfessorPay },
        //         { "ProfessorHealth", saveData.ProfessorHealth },
        //         { "ProfessorPassion", saveData.ProfessorPassion },
        //     });
        //     index++;
        // }
        //
        // index = 0;
        // // 돌발 이벤트 정보
        // Dictionary<string, object> suddenEventData = new Dictionary<string, object>();
        //nhk j
        // foreach (var saveData in AllInOneData.Instance.SuddenEventData)
        // {
        //     suddenEventData.Add($"SuddenEvent_{index}", new Dictionary<string, object>
        //     {
        //         { "SuddenEventID", saveData.SuddenEventID },
        //         { "YearData", saveData.YearData },
        //         { "MonthData", saveData.MonthData },
        //         { "WeekData", saveData.WeekData },
        //         { "DayData", saveData.DayData },
        //     });
        //     index++;
        // }
        //
        // index = 0;
        // // 오늘 이벤트 목록
        // Dictionary<string, object> todaySuddenEventData = new Dictionary<string, object>();
        //
        // foreach (var saveData in AllInOneData.Instance.TodaySuddenEventData)
        // {
        //     todaySuddenEventData.Add($"TodayEvent_{index}", new Dictionary<string, object>
        //     {
        //         { "SuddenEventID", saveData.SuddenEventID },
        //     });
        //     index++;
        // }
        //
        // index = 0;
        // // 일반 매일 정보
        // //Dictionary<string, object> sendMailSaveData = new Dictionary<string, object>();
        // Dictionary<string, PlayerSaveData> sendMailSaveData = new Dictionary<string, PlayerSaveData>();
        //
        // foreach (var saveData in AllInOneData.Instance.SendMailData)
        // {
        //     //sendMailSaveData.Add($"SendMail_{index}", saveData);
        //
        //
        //
        //     // var dic = new Dictionary<string, object>();
        //     // for (리플렉션으로 프라퍼티 이름을 순회하면서)
        //     // {
        //     //     dic.Add("프라퍼티이름", 해당프라퍼티의값);
        //     // }
        //
        //     // sendMailSaveData.Add($"SendMail_{index}", new Dictionary<string, object>
        //     // {
        //     //     { "MailTitle", saveData.MailTitle },
        //     //     { "SendMailDate", saveData.SendMailDate },
        //     //     { "FromMail", saveData.FromMail },
        //     //     
        //     //     { "MailContent", saveData.MailContent },
        //     //
        //     //     { "Reward1", saveData.Reward1 },
        //     //     { "Reward2", saveData.Reward2 },
        //     //     { "Month", saveData.Month },
        //     //     { "Type", saveData.Type },
        //     //     { "IsNewMail", saveData.IsNewMail },
        //     //
        //     //     // Specification
        //     //     { "IncomeEventResult", saveData.IncomeEventResult },
        //     //     { "IncomeSell", saveData.IncomeSell },
        //     //     { "IncomeActivity", saveData.IncomeActivity },
        //     //     { "IncomeAcademyFee", saveData.IncomeAcademyFee },
        //     //     { "ExpensesEventResult", saveData.ExpensesEventResult },
        //     //
        //     //     { "ExpensesEventCost", saveData.ExpensesEventCost },
        //     //     { "ExpensesActivity", saveData.ExpensesActivity },
        //     //     { "ExpensesSalary", saveData.ExpensesSalary },
        //     //     { "ExpensesFacility", saveData.ExpensesFacility },
        //     //     { "ExpensesTuitionFee", saveData.ExpensesTuitionFee },
        //     //
        //     //     { "TotalIncome", saveData.TotalIncome },
        //     //     { "TotalExpenses", saveData.TotalExpenses },
        //     //     { "NetProfit", saveData.NetProfit },
        //     //     { "GoodsScore", saveData.GoodsScore },
        //     //     { "FamousScore", saveData.FamousScore },
        //     //
        //     //     { "ActivityScore", saveData.ActivityScore },
        //     //     { "ManagementScore", saveData.ManagementScore },
        //     //     { "TalentDevelopmentScore", saveData.TalentDevelopmentScore },
        //     // });
        //     // index++;
        // }
    }

    public void LoadDataForFirebase()
    {
        Debug.Log("Loading Start");

        AllInOneData.Instance.CleanAllGameData();

        // 컬렉션의 모든 문서 가져오기
        // 실제 구현
        Query allCitiesQuery = db.Collection("Users").Document(UserId).Collection("SaveData");
        allCitiesQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            // 게임잼 게임쇼 결합 저장을 위한 임시 리스트
            List<GameJamSaveData> gameJamDeepValues = new(); // 게임잼 데이타
            List<GameShowSaveData> gameShowDeepValues = new(); // 게임잼 데이타

            QuerySnapshot allSaveDataQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in allSaveDataQuerySnapshot.Documents)
            {
                Debug.Log(String.Format
                    ("컬렉션의 모든 문서 가져오기 Document data for ______    {0}    ______ document:", documentSnapshot.Id));


                // 불러온 플레이어 정보 저장
                if (documentSnapshot.Id == "PlayerData")
                {
                    // AllInOneData의 변수에다가 넣자.
                    AllInOneData.Instance.PlayerData = documentSnapshot.ConvertTo<PlayerSaveData>();

                    // 딕셔너리 쪼개둔 것 합치기
                    Dictionary<int, int> toDictionary = new Dictionary<int, int>();
                    toDictionary = AllInOneData.Instance.PlayerData.GameDesignerClassDic_keys
                        .Zip(AllInOneData.Instance.PlayerData.GameDesignerClassDic_values, (k, v)
                            => new { k, v }).ToDictionary(a => a.k, a => a.v);
                    AllInOneData.Instance.PlayerData.GameDesignerClassDic = toDictionary;

                    toDictionary = AllInOneData.Instance.PlayerData.ArtClassDic_keys
                        .Zip(AllInOneData.Instance.PlayerData.ArtClassDic_values, (k, v)
                            => new { k, v }).ToDictionary(a => a.k, a => a.v);
                    AllInOneData.Instance.PlayerData.ArtClassDic = toDictionary;

                    toDictionary = AllInOneData.Instance.PlayerData.ProgrammingClassDic_keys
                        .Zip(AllInOneData.Instance.PlayerData.ProgrammingClassDic_values, (k, v)
                            => new { k, v }).ToDictionary(a => a.k, a => a.v);
                    AllInOneData.Instance.PlayerData.ProgrammingClassDic = toDictionary;

                    Dictionary<string, int> toStrDictionary = new Dictionary<string, int>();
                    toStrDictionary = AllInOneData.Instance.PlayerData.GameJamEntryCount_keys
                        .Zip(AllInOneData.Instance.PlayerData.GameJamEntryCount_values, (k, v)
                            => new { k, v }).ToDictionary(a => a.k, a => a.v);
                    AllInOneData.Instance.PlayerData.GameJamEntryCount = toStrDictionary;


                    toStrDictionary = AllInOneData.Instance.PlayerData.NeedGameDesignerStat_keys
                        .Zip(AllInOneData.Instance.PlayerData.NeedGameDesignerStat_values, (k, v)
                            => new { k, v }).ToDictionary(a => a.k, a => a.v);
                    AllInOneData.Instance.PlayerData.NeedGameDesignerStat = toStrDictionary;

                    toStrDictionary = AllInOneData.Instance.PlayerData.NeedArtStat_keys
                        .Zip(AllInOneData.Instance.PlayerData.NeedArtStat_values, (k, v)
                            => new { k, v }).ToDictionary(a => a.k, a => a.v);
                    AllInOneData.Instance.PlayerData.NeedArtStat = toStrDictionary;

                    toStrDictionary = AllInOneData.Instance.PlayerData.NeedProgrammingStat_keys
                        .Zip(AllInOneData.Instance.PlayerData.NeedProgrammingStat_values, (k, v)
                            => new { k, v }).ToDictionary(a => a.k, a => a.v);
                    AllInOneData.Instance.PlayerData.NeedProgrammingStat = toStrDictionary;
                }

                // 불러온 학생 정보 저장
                else if (documentSnapshot.Id == "StudentData")
                {
                    // TODO :: 저장 데이터를 넣어둘 리스트. 나중에 allInOneData 의 변수로 바꾸자.
                    //List<StudentSaveData> studentSaveDataList = new List<StudentSaveData>();

                    // 읽어온 데이터를 가공가능하도록 딕셔너리화 한다.
                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    // 문서 전체를 돌면서 내용을 가져온다.
                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        var studentSaveDataDic = (Dictionary<string, object>)docSnapshot["Student_" + i];

                        // int 배열은 convert가 안된다...
                        //StudentSaveData studentData = new StudentSaveData();
                        //studentData = studentSaveDataDic.ConvertTo<StudentSaveData>();
                        //StudentSaveData data = studentSaveDataDic.ConvertTo<StudentSaveData>();
                        //AllInOneData.Instance.StudentData.Add(data);

                        StudentSaveData studentData = new StudentSaveData
                        {
                            StudentID = (string)studentSaveDataDic["StudentID"],
                            StudentName = (string)studentSaveDataDic["StudentName"],
                            UserSettingName = (string)studentSaveDataDic["UserSettingName"],
                            Health = (int)(long)studentSaveDataDic["Health"],
                            Passion = (int)(long)studentSaveDataDic["Passion"],
                            Gender = (int)(long)studentSaveDataDic["Gender"],

                            StudentType = (StudentType)(long)studentSaveDataDic["StudentType"],

                            Personality = (string)studentSaveDataDic["Personality"],
                            NumberOfEntries = (int)(long)studentSaveDataDic["NumberOfEntries"],
                            IsActiving = (bool)studentSaveDataDic["IsActiving"],
                            IsRecommend = (bool)studentSaveDataDic["IsRecommend"],

                            FriendshipIndex = (int)(long)studentSaveDataDic["FriendshipIndex"],
                        };

                        // int arr 받아오기
                        List<object> abilityAmountList = (List<object>)studentSaveDataDic["AbilityAmountArr"];
                        int[] abilityAmountArr = new int[abilityAmountList.Count];
                        for (int j = 0; j < abilityAmountList.Count; j++)
                        {
                            abilityAmountArr[j] = Convert.ToInt32(abilityAmountList[j]);
                        }

                        studentData.AbilityAmountArr = abilityAmountArr;

                        List<object> abilitySkillsList = (List<object>)studentSaveDataDic["AbilitySkills"];
                        int[] abilitySkillsArr = new int[abilitySkillsList.Count];
                        for (int j = 0; j < abilitySkillsList.Count; j++)
                        {
                            abilitySkillsArr[j] = Convert.ToInt32(abilitySkillsList[j]);
                        }

                        studentData.AbilitySkills = abilitySkillsArr;

                        List<object> genreAmountList = (List<object>)studentSaveDataDic["GenreAmountArr"];
                        int[] genreAmountArr = new int[genreAmountList.Count];
                        for (int j = 0; j < genreAmountList.Count; j++)
                        {
                            genreAmountArr[j] = Convert.ToInt32(genreAmountList[j]);
                        }

                        studentData.GenreAmountArr = genreAmountArr;

                        // string list 받아오기
                        List<object> skillsObjList = (List<object>)studentSaveDataDic["Skills"];
                        List<string> skillsStrList = skillsObjList.ConvertAll(ObjectToString);
                        studentData.Skills = skillsStrList;

                        List<object> FriendshipObjList = (List<object>)studentSaveDataDic["Friendship"];
                        List<int> FriendshipStrList = FriendshipObjList.ConvertAll(ObjectToint);
                        studentData.Friendship = FriendshipStrList;

                        AllInOneData.Instance.StudentData.Add(studentData);
                    }
                }

                // 불러온 강사 정보 저장
                else if (documentSnapshot.Id == "ProfessorData")
                {
                    // TODO :: 저장 데이터를 넣어둘 리스트. 나중에 allInOneData 의 변수로 바꾸자.
                    //List<ProfessorSaveData> professorSaveDataList = new List<ProfessorSaveData>();

                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        var professorSaveDataDic = (Dictionary<string, object>)docSnapshot["Professor_" + i];

                        ProfessorSaveData professorInfo = new ProfessorSaveData()
                        {
                            ProfessorID = (int)(long)professorSaveDataDic["ProfessorID"],
                            ProfessorName = (string)professorSaveDataDic["ProfessorName"],
                            ProfessorType = (StudentType)(long)professorSaveDataDic["ProfessorType"],
                            ProfessorSet = (string)professorSaveDataDic["ProfessorSet"],
                            ProfessorPower = (int)(long)professorSaveDataDic["ProfessorPower"],

                            ProfessorExperience = (int)(long)professorSaveDataDic["ProfessorExperience"],

                            ProfessorPay = (int)(long)professorSaveDataDic["ProfessorPay"],
                            ProfessorHealth = (int)(long)professorSaveDataDic["ProfessorHealth"],
                            ProfessorPassion = (int)(long)professorSaveDataDic["ProfessorPassion"],

                            IsUnlockProfessor = (bool)professorSaveDataDic["IsUnlockProfessor"],
                        };

                        List<object> professorSkillsObjList = (List<object>)professorSaveDataDic["ProfessorSkills"];
                        List<string> professorSkillsstrList = professorSkillsObjList.ConvertAll(ObjectToString);
                        professorInfo.ProfessorSkills = professorSkillsstrList;

                        //professorSaveDataList.Add(professorInfo); // Test
                        AllInOneData.Instance.ProfessorData.Add(professorInfo);
                    }
                }

                // 불러온 돌발 이벤트 정보 저장
                else if (documentSnapshot.Id == "SuddenEventData")
                {
                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        var suddenEventSaveDataDic = (Dictionary<string, object>)docSnapshot["SuddenEvent_" + i];

                        UsedEventSaveData eventInfo = new UsedEventSaveData()
                        {
                            SuddenEventID = (int)(long)suddenEventSaveDataDic["SuddenEventID"],
                            YearData = (int)(long)suddenEventSaveDataDic["YearData"],
                            MonthData = (int)(long)suddenEventSaveDataDic["MonthData"],
                            WeekData = (int)(long)suddenEventSaveDataDic["WeekData"],
                            DayData = (int)(long)suddenEventSaveDataDic["DayData"],
                        };

                        AllInOneData.Instance.SuddenEventData.Add(eventInfo);
                    }
                }

                else if (documentSnapshot.Id == "TodaySuddenEventData")
                {
                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        var todayEventSaveDataDic = (Dictionary<string, object>)docSnapshot["TodayEvent_" + i];

                        TodayEventSaveData eventSaveInfo = new TodayEventSaveData()
                        {
                            SuddenEventID = (int)todayEventSaveDataDic["SuddenEventID"],
                        };

                        AllInOneData.Instance.TodaySuddenEventData.Add(eventSaveInfo);
                    }
                }

                else if (documentSnapshot.Id == "SendMailData")
                {
                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        var sendMailSaveDataDic = (Dictionary<string, object>)docSnapshot["SendMail_" + i];

                        SendMailSaveData sendMailSaveInfo = new SendMailSaveData()
                        {
                            MailTitle = (string)sendMailSaveDataDic["MailTitle"],
                            /// SendMailDate = (string)sendMailSaveDataDic["SendMailDate"], // 바뀐 메일 데이터 형식. string에서 int[]로 변경됐다.
                            FromMail = (string)sendMailSaveDataDic["FromMail"],

                            MailContent = (string)sendMailSaveDataDic["MailContent"],

                            Reward1 = (string)sendMailSaveDataDic["Reward1"],
                            Reward2 = (string)sendMailSaveDataDic["Reward2"],
                            Month = (string)sendMailSaveDataDic["Month"],
                            Type = (MailType)(long)sendMailSaveDataDic["Type"],
                            IsNewMail = (bool)sendMailSaveDataDic["IsNewMail"],

                            //MonthReportMailContent = (Specification)sendMailSaveDataDic["Specification"],

                            // Specification
                            IncomeEventResult = (int)(long)sendMailSaveDataDic["IncomeEventResult"],
                            IncomeSell = (int)(long)sendMailSaveDataDic["IncomeSell"],
                            IncomeActivity = (int)(long)sendMailSaveDataDic["IncomeActivity"],
                            IncomeAcademyFee = (int)(long)sendMailSaveDataDic["IncomeAcademyFee"],
                            ExpensesEventResult = (int)(long)sendMailSaveDataDic["ExpensesEventResult"],

                            ExpensesEventCost = (int)(long)sendMailSaveDataDic["ExpensesEventCost"],
                            ExpensesActivity = (int)(long)sendMailSaveDataDic["ExpensesActivity"],
                            ExpensesSalary = (int)(long)sendMailSaveDataDic["ExpensesSalary"],
                            ExpensesFacility = (int)(long)sendMailSaveDataDic["ExpensesFacility"],
                            ExpensesTuitionFee = (int)(long)sendMailSaveDataDic["ExpensesTuitionFee"],

                            TotalIncome = (int)(long)sendMailSaveDataDic["TotalIncome"],
                            TotalExpenses = (int)(long)sendMailSaveDataDic["TotalExpenses"],
                            NetProfit = (int)(long)sendMailSaveDataDic["NetProfit"],
                            GoodsScore = (int)(long)sendMailSaveDataDic["GoodsScore"],
                            FamousScore = (int)(long)sendMailSaveDataDic["FamousScore"],

                            ActivityScore = (int)(long)sendMailSaveDataDic["ActivityScore"],
                            ManagementScore = (int)(long)sendMailSaveDataDic["ManagementScore"],
                            TalentDevelopmentScore = (int)(long)sendMailSaveDataDic["TalentDevelopmentScore"],
                        };

                        // int arr 받아오기
                        List<object> mailDateList = (List<object>)sendMailSaveDataDic["SendMailDate"];
                        int[] mailDateArr = new int[mailDateList.Count];
                        for (int j = 0; j < mailDateList.Count; j++)
                        {
                            mailDateArr[j] = Convert.ToInt32(mailDateList[j]);
                        }

                        sendMailSaveInfo.SendMailDate = mailDateArr;


                        //professorSaveDataList.Add(professorInfo); // Test
                        AllInOneData.Instance.SendMailData.Add(sendMailSaveInfo);
                    }
                }

                else if (documentSnapshot.Id == "GameJamToBeRunning")
                {
                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        // 이번에 저장할 데이터 dic
                        var GameJamSaveDataDic = (Dictionary<string, object>)docSnapshot["GameJamToBeRunning_" + i];

                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                        GameJamSaveData gameJamSaveData = new GameJamSaveData();

                        // 함수의 프라퍼티 정보를 담는다.
                        var setData = typeof(GameJamSaveData).GetProperties(flags);
                        // 프라퍼티 전체를 순회
                        foreach (var set in setData)
                        {
                            foreach (var dic in GameJamSaveDataDic)
                            {
                                if (set.Name == dic.Key)
                                {
                                    set.SetValue(gameJamSaveData, dic.Value);
                                    break;
                                }
                            }
                        }

                        AllInOneData.Instance.GameJamToBeRunning.Add(gameJamSaveData);
                    }
                }

                else if (documentSnapshot.Id == "GameJamSaveData_values")
                {
                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        // 이번에 저장할 데이터 dic
                        var GameJamSaveDataDic = (Dictionary<string, object>)docSnapshot["GameJamDeepValue_" + i];

                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                        GameJamSaveData gameJamSaveData = new GameJamSaveData();

                        // 함수의 프라퍼티 정보를 담는다.
                        var setData = typeof(GameJamSaveData).GetProperties(flags);
                        // 프라퍼티 전체를 순회
                        foreach (var set in setData)
                        {
                            foreach (var dic in GameJamSaveDataDic)
                            {
                                if (set.Name == dic.Key)
                                {
                                    set.SetValue(gameJamSaveData, dic.Value);
                                    break;
                                }
                            }
                        }

                        gameJamDeepValues.Add(gameJamSaveData);
                    }
                }


                else if (documentSnapshot.Id == "GameShowToBeRunning")
                {
                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        var GameJamSaveDataDic = (Dictionary<string, object>)docSnapshot["GameShowToBeRunning_" + i];

                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                        GameShowSaveData gameShowSaveData = new GameShowSaveData();

                        var setData = typeof(GameShowSaveData).GetProperties(flags);
                        foreach (var set in setData)
                        {
                            // foreach (var dic in GameJamSaveDataDic)
                            // {
                            //     if (string.Equals(set.Name, dic.Key, StringComparison.OrdinalIgnoreCase))
                            //     //if (set.Name == dic.Key)
                            //     {
                            //         set.SetValue(gameShowSaveData, dic.Value);
                            //         break;
                            //     }
                            // }

                            if (GameJamSaveDataDic.ContainsKey(set.Name))
                            {
                                set.SetValue(gameShowSaveData, GameJamSaveDataDic[set.Name]);
                            }
                        }

                        AllInOneData.Instance.GameShowToBeRunning.Add(gameShowSaveData);
                    }
                }

                else if (documentSnapshot.Id == "GameShowSaveData_values")
                {
                    Dictionary<string, object> docSnapshot = documentSnapshot.ToDictionary();

                    for (int i = 0; i < docSnapshot.Count; i++)
                    {
                        // 이번에 저장할 데이터 dic
                        var GameShowSaveDataDic = (Dictionary<string, object>)docSnapshot["GameShowDeepValue_" + i];

                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                        GameShowSaveData gameShowSaveData = new GameShowSaveData();

                        // 함수의 프라퍼티 정보를 담는다.
                        var setData = typeof(GameShowSaveData).GetProperties(flags);
                        // 프라퍼티 전체를 순회
                        foreach (var set in setData)
                        {
                            foreach (var dic in GameShowSaveDataDic)
                            {
                                if (GameShowSaveDataDic.ContainsKey(set.Name))
                                {
                                    set.SetValue(gameShowSaveData, GameShowSaveDataDic[set.Name]);
                                }
                            }
                        }

                        gameShowDeepValues.Add(gameShowSaveData);
                    }
                }
            }

            // TODO GameJamHistory 와 GameShowHistory 쪼개논거 합쳐서 저장해야 함.


            // GameJam 정보 합치기
            //AllInOneData.Instance.PlayerData.GameJamSaveDic_Deepkeys +  gameJamDeepValues
            //같은 이름 게임잼 하나로 묶기 위해 필요
            //AllInOneData.Instance.PlayerData.GameJamSaveDic_keys + 위의 결과

            // 이걸 어떻게 만들까  앞 리스트의 숫자는 DeepKey 가 맞는듯.
            List<List<GameJamSaveData>> jamList = new List<List<GameJamSaveData>>();

            int valueIndex = 0;
            int jamCount = 0; // 몇번째 게임잼 정보인지
            foreach (var listCount in AllInOneData.Instance.PlayerData.GameJamSaveDic_Deepkeys)
            {
                if (listCount == 0)
                {
                    // 새로운 게임잼 이면 새로 할당하여 정보 넣기
                    var newJamList = new List<GameJamSaveData>();
                    newJamList.Add(gameJamDeepValues[valueIndex++]);
                    jamList.Add(newJamList);
                    jamCount++;
                }
                else
                {
                    // 이전과 같은 게임잼 이면 같은 자리에다가 정보 넣기
                    jamList[jamCount - 1].Add(gameJamDeepValues[valueIndex++]);
                }
            }

            Dictionary<int, List<GameJamSaveData>> jamDictionary = new Dictionary<int, List<GameJamSaveData>>();
            jamDictionary = AllInOneData.Instance.PlayerData.GameJamSaveDic_keys
                .Zip(jamList, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);

            AllInOneData.Instance.GameJamHistory = jamDictionary;


            List<List<GameShowSaveData>> showList = new List<List<GameShowSaveData>>();

            valueIndex = 0;
            int showCount = 0; // 몇번째 게임잼 정보인지
            foreach (var listCount in AllInOneData.Instance.PlayerData.GameShowSaveDic_Deepkeys)
            {
                if (listCount == 0)
                {
                    // 새로운 게임잼 이면 새로 할당하여 정보 넣기
                    var newShowList = new List<GameShowSaveData>();
                    newShowList.Add(gameShowDeepValues[valueIndex++]);
                    showList.Add(newShowList);
                    showCount++;
                }
                else
                {
                    // 이전과 같은 게임잼 이면 같은 자리에다가 정보 넣기
                    showList[showCount - 1].Add(gameShowDeepValues[valueIndex++]);
                }
            }

            Dictionary<int, List<GameShowSaveData>> showDictionary = new Dictionary<int, List<GameShowSaveData>>();
            showDictionary = AllInOneData.Instance.PlayerData.GameShowSaveDic_keys
                .Zip(showList, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);

            AllInOneData.Instance.GameShowHistory = showDictionary;


            Debug.Log("Loading end");

            // 구글 서버에서 로딩했음을 알린다.
            AllInOneData.Instance.ServerLoading = true;
        });
    }


    private static string ObjectToString(object obj)
    {
        return obj.ToString();
    }

    private static int ObjectToint(object obj)
    {
        return Convert.ToInt32(obj);
    }


    // 연습, 테스트 코드 모음


    // 리스트를 딕셔너리로 만들어서 저장하는 예시코드
    public void TestSavePlayerDataToFirestore(List<GameShowData> gameShowDatas)
    {
        // Convert the list to a dictionary
        Dictionary<string, object> data = new Dictionary<string, object>();
        int index = 0;

        // 딕셔너리를 만들어서 저장하기
        foreach (GameShowData gameShow in gameShowDatas)
        {
            data.Add("GameShow_" + index.ToString(), new Dictionary<string, object>
            {
                { "GameShow_ID", gameShow.GameShow_ID },
                { "GameShow_Name", gameShow.GameShow_Name },
                { "GameShow_Level", gameShow.GameShow_Level },
                { "GameShow_Health", gameShow.GameShow_Health },
                { "GameShow_Pasion", gameShow.GameShow_Pasion },

                { "GameShow_State", gameShow.GameShow_State },
                { "GameShow_Host_ID", gameShow.GameShow_Host_ID },
                { "GameShow_Year", gameShow.GameShow_Year },
                { "GameShow_Month", gameShow.GameShow_Month },
                { "GameShow_Week", gameShow.GameShow_Week },

                { "GameShow_Day", gameShow.GameShow_Day },
                //{ "GameShow_Special", gameShow.GameShow_Special },
                //{ "Testarr", gameShow.Testarr }
            });
            index++;
        }


        // Save data to Firestore
        // Create a collection with today's date under the user's document
        CollectionReference userCollection = db.Collection("Users").Document(UserId).Collection("SaveData");

        // Create a new document in the collection
        // 이름 지정 안해주면 랜덤 텍스트가 이름이 됨
        DocumentReference docRef = userCollection.Document("GameShow");

        docRef.SetAsync(data);


        Debug.Log("Data saved to Firestore");
    }

    // 하나의 문서를 불러오는 예시 코드
    public async Task<List<GameShowData>> LoadGameShowData()
    {
        // Create a query to retrieve the document containing the saved game show data
        DocumentReference docRef = db.Collection("Users").Document(UserId).Collection("SaveData").Document("GameShow");

        // Retrieve the document snapshot
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        // Check if the document exists
        if (snapshot.Exists)
        {
            // Get the data dictionary from the snapshot
            Dictionary<string, object> data = snapshot.ToDictionary();

            // Convert the data dictionary back into a list of GameShowData objects
            List<GameShowData> gameShowDataList = new List<GameShowData>();

            // 문서 내의 딕셔너리를 가져오는 코드.
            for (var i = 0; i < data.Count; i++)
            {
                var gameShowDataDict = (Dictionary<string, object>)data["GameShow_" + i.ToString()];

                GameShowData gameShowData = new GameShowData();
                gameShowData.GameShow_ID = (int)(long)gameShowDataDict["GameShow_ID"];
                gameShowData.GameShow_Name = (string)gameShowDataDict["GameShow_Name"];
                gameShowData.GameShow_Level = (int)(long)gameShowDataDict["GameShow_Level"];
                gameShowData.GameShow_Health = (int)(long)gameShowDataDict["GameShow_Health"];
                gameShowData.GameShow_Pasion = (int)(long)gameShowDataDict["GameShow_Pasion"];
                gameShowData.GameShow_State =
                    ((Dictionary<string, object>)gameShowDataDict["GameShow_State"])
                    .ToDictionary(x => (string)x.Key, x => (int)(long)x.Value);
                gameShowData.GameShow_Host_ID = (int)(long)gameShowDataDict["GameShow_Host_ID"];
                gameShowData.GameShow_Year = (int)(long)gameShowDataDict["GameShow_Year"];
                gameShowData.GameShow_Month = (int)(long)gameShowDataDict["GameShow_Month"];
                gameShowData.GameShow_Week = (int)(long)gameShowDataDict["GameShow_Week"];
                gameShowData.GameShow_Day = (int)(long)gameShowDataDict["GameShow_Day"];
                //gameShowData.GameShow_Special = (bool)gameShowDataDict["GameShow_Special"];


                // // int 배열 가져오기
                // List<object> array = (List<object>)gameShowDataDict["Testarr"];
                // int[] intArray = new int[array.Count];
                // for (int j = 0; j < array.Count; j++)
                // {
                //     intArray[j] = Convert.ToInt32(array[j]);
                // }

                //gameShowData.Testarr = intArray;

                gameShowDataList.Add(gameShowData);
            }

            return gameShowDataList;
        }
        else
        {
            Debug.LogError("Failed to load game show data: Document does not exist");
            return null;
        }
    }

    // public void FirestoreTestData()
    // {
    // // 여러 자료형 지정
    // int[] arr1 = { 1, 2, 3, 4, 5 };
    // int[] arr2 = { 6, 7, 8 };
    // Dictionary<string, int> dictionary1 = new Dictionary<string, int>
    // {
    //     { "wow", 1 },
    //     { "wow3", 3 },
    //     { "wow2", 2 }
    // };
    // Dictionary<string, int> dictionary2 = new Dictionary<string, int>
    // {
    //     { "wow", 9 },
    //     { "wow3", 8 },
    //     { "wow2", 7 }
    // };
    //
    // // 저장 위치 설정
    // CollectionReference citiesRef = db.Collection("Users").Document(UserId).Collection("SaveData");
    //
    // // Dictionart 만들기.
    // citiesRef.Document("SF").SetAsync(new Dictionary<string, object>()
    // {
    //     { "Name", "San Francisco" },
    //     { "State", "CA" },
    //     { "Country", "USA" },
    //     { "Capital", false },
    //     { "Population", 860000 },
    //     { "TestArr", arr1 }, // 1차원 배열
    //     { "TestDic", dictionary2 } // 딕셔너리
    // }).ContinueWithOnMainThread(task =>
    //     citiesRef.Document("LA").SetAsync(new Dictionary<string, object>()
    //     {
    //         { "Name", "Los Angeles" },
    //         { "State", "CA" },
    //         { "Country", "USA" },
    //         { "Capital", false },
    //         { "Population", 3900000 },
    //         { "TestArr", arr1 },
    //         { "TestDic", dictionary1 }
    //     })
    // ).ContinueWithOnMainThread(task =>
    //     citiesRef.Document("DC").SetAsync(new Dictionary<string, object>()
    //     {
    //         { "Name", "Washington D.C." },
    //         { "State", null },
    //         { "Country", "USA" },
    //         { "Capital", true },
    //         { "Population", 680000 },
    //         { "TestArr", arr2 },
    //     })
    // ).ContinueWithOnMainThread(task =>
    //     citiesRef.Document("TOK").SetAsync(new Dictionary<string, object>()
    //     {
    //         { "Name", "Tokyo" },
    //         { "State", null },
    //         { "Country", "Japan" },
    //         { "Capital", true },
    //         { "Population", 9000000 },
    //         { "TestArr", arr2 },
    //         { "TestDic", dictionary2 }
    //     })
    // ).ContinueWithOnMainThread(task =>
    //     citiesRef.Document("BJ").SetAsync(new Dictionary<string, object>()
    //     {
    //         { "Name", "Beijing" },
    //         { "State", null },
    //         { "Country", "China" },
    //         { "Capital", true },
    //         { "Population", 21500000 }
    //     })
    // );
    // Debug.Log("저장 완료");
    //
    // // 단일 문서 접근
    // // docRef가 참조하는 위치에 문서가 없으면 결과 document는 비어 있게 되고 여기서 exists를 호출하면 false가 반환됩니다.
    // DocumentReference docRef = db.Collection("Users").Document(UserId).Collection("SaveData").Document("SF");
    // docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    // {
    //     DocumentSnapshot snapshot = task.Result;
    //     if (snapshot.Exists)
    //     {
    //         Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
    //         Dictionary<string, object> city = snapshot.ToDictionary();
    //         foreach (KeyValuePair<string, object> pair in city)
    //         {
    //             Debug.Log(String.Format("단일문서 {0}: {1}", pair.Key, pair.Value));
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log(String.Format("Document {0} does not exist! 단일문서", snapshot.Id));
    //     }
    // });
    //
    //
    // 커스텀 객체
    // DocumentReference docRef2 = db.Collection("Users").Document(UserId).Collection("SaveData").Document("TOK");
    //
    // docRef2.GetSnapshotAsync().ContinueWith((task) =>
    // {
    //     var snapshot = task.Result;
    //     if (snapshot.Exists)
    //     {
    //         Debug.Log(String.Format("커스텀 객체 Document data for {0} document:", snapshot.Id));
    //
    //         City city = snapshot.ConvertTo<City>();
    //         Debug.Log(String.Format("Name: {0}", city.Name));
    //         Debug.Log(String.Format("State: {0}", city.State));
    //         Debug.Log(String.Format("Country: {0}", city.Country));
    //         Debug.Log(String.Format("Capital: {0}", city.Capital));
    //         Debug.Log(String.Format("Population: {0}", city.Population));
    //         Debug.Log(String.Format("TestArr: {0}", city.TestArr[0]));
    //
    //         // 딕셔너리 불러오기
    //         int temp;
    //         city.TestDic.TryGetValue("woo", out temp);
    //
    //         // 불러오기 가능한거 확인.
    //         for (int i = 0; i < city.TestDic.Count; i++)
    //         {
    //             KeyValuePair<string, int> entry = city.TestDic.ElementAt(i);
    //             Debug.Log("Key: " + entry.Key + ", Value: " + entry.Value);
    //         }
    //
    //         Debug.Log(String.Format("TestDic: {0}", temp));
    //     }
    //     else
    //     {
    //         Debug.Log(String.Format("커스텀 객체 Document {0} does not exist!", snapshot.Id));
    //     }
    // });
    //
    //
    // // 여러 문서 가져오기
    // Query capitalQuery = db.Collection("Users").Document(UserId).Collection("SaveData")
    //     .WhereEqualTo("Capital", true);
    // capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    // {
    //     QuerySnapshot capitalQuerySnapshot = task.Result;
    //     foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
    //     {
    //         Debug.Log(String.Format("여러 문서 가져오기 Document data for {0} document:", documentSnapshot.Id));
    //         Dictionary<string, object> city = documentSnapshot.ToDictionary();
    //         foreach (KeyValuePair<string, object> pair in city)
    //         {
    //             Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
    //         }
    //
    //         // Newline to separate entries
    //         Debug.Log("");
    //     }
    // });


    //     GameShowData temp = new GameShowData
    //     {
    //         GameShow_ID = 1,
    //         GameShow_Name = "ho",
    //         GameShow_Level = 2,
    //         GameShow_Health = 3,
    //         GameShow_Pasion = 4,
    //         GameShow_State = new Dictionary<string, int>
    //         {
    //             { "wow", 1 },
    //             { "wow3", 3 },
    //             { "wow2", 2 }
    //         },
    //         GameShow_Host_ID = 5,
    //         GameShow_Year = 6,
    //         GameShow_Month = 7,
    //         GameShow_Week = 8,
    //         GameShow_Day = 9,
    //         GameShow_Special = true,
    //         //GameShow_ResultCheck = 11
    //     };
    //     _GameShowData.Add(temp);
    //
    //
    //     GameShowData temp2 = new GameShowData
    //     {
    //         GameShow_ID = 11,
    //         GameShow_Name = "hho",
    //         GameShow_Level = 22,
    //         GameShow_Health = 33,
    //         GameShow_Pasion = 44,
    //         GameShow_State = new Dictionary<string, int>
    //         {
    //             { "wwow", 14 },
    //             { "wwow3", 33 },
    //             { "woww2", 22 }
    //         },
    //         GameShow_Host_ID = 56,
    //         GameShow_Year = 66,
    //         GameShow_Month = 76,
    //         GameShow_Week = 86,
    //         GameShow_Day = 96,
    //         GameShow_Special = false,
    //     };
    //     _GameShowData.Add(temp2);
    //
    //     GameShowData temp3 = new GameShowData
    //     {
    //         GameShow_ID = 11,
    //         GameShow_Name = "hho",
    //         GameShow_Level = 22,
    //         GameShow_Health = 33,
    //         GameShow_Pasion = 44,
    //         GameShow_State = new Dictionary<string, int>
    //         {
    //             { "wwow", 14 },
    //             { "wwow3", 33 },
    //             { "woww2", 22 }
    //         },
    //         GameShow_Host_ID = 56,
    //         GameShow_Year = 66,
    //         GameShow_Month = 76,
    //         GameShow_Week = 86,
    //         GameShow_Day = 96,
    //         GameShow_Special = false,
    //     };
    //     _GameShowData.Add(temp3);
    //
    //     GameShowData temp4 = new GameShowData
    //     {
    //         GameShow_ID = 11,
    //         GameShow_Name = "hho",
    //         GameShow_Level = 22,
    //         GameShow_Health = 33,
    //         GameShow_Pasion = 44,
    //         GameShow_State = new Dictionary<string, int>
    //         {
    //             { "wwow", 14 },
    //             { "wwow3", 33 },
    //             { "woww2", 22 }
    //         },
    //         GameShow_Host_ID = 56,
    //         GameShow_Year = 66,
    //         GameShow_Month = 76,
    //         GameShow_Week = 86,
    //         GameShow_Day = 96,
    //         GameShow_Special = false,
    //     };
    //     _GameShowData.Add(temp4);
    //
    //     FirebaseBinder.Instance.TestSavePlayerDataToFirestore(_GameShowData);
    // }

// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Firebase.Extensions;
// using Firebase.Firestore;
// using UnityEngine;
    //
    // public class FirestoreManager : MonoBehaviour
    // {
    //     private FirebaseFirestore db;
    //
    //     void Start()
    //     {
    //         db = FirebaseFirestore.DefaultInstance;
    //     }
    //
    //     // Save a list of strings to Firestore
    //     public async Task SaveList(List<string> listToSave, string collectionName, string documentName,
    //         string fieldName)
    //     {
    //         DocumentReference docRef = db.Collection(collectionName).Document(documentName);
    //         Dictionary<string, object> data = new Dictionary<string, object>
    //         {
    //             { fieldName, listToSave.ToArray() }
    //         };
    //
    //         await docRef.SetAsync(data, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
    //         {
    //             if (task.IsFaulted)
    //             {
    //                 Debug.LogError("Failed to save list: " + task.Exception);
    //             }
    //             else if (task.IsCompleted)
    //             {
    //                 Debug.Log("List saved successfully!");
    //             }
    //         });
    //     }
    //
    //     // Load a list of strings from Firestore
    //     public async Task<List<string>> LoadList(string collectionName, string documentName, string fieldName)
    //     {
    //         List<string> loadedList = new List<string>();
    //         DocumentReference docRef = db.Collection(collectionName).Document(documentName);
    //
    //         DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
    //         if (snapshot.Exists)
    //         {
    //             Dictionary<string, object> data = snapshot.ToDictionary();
    //             if (data.ContainsKey(fieldName))
    //             {
    //                 object[] objArray = (object[])data[fieldName];
    //                 foreach (object obj in objArray)
    //                 {
    //                     string str = obj.ToString();
    //                     loadedList.Add(str);
    //                 }
    //             }
    //         }
    //
    //         return loadedList;
    //     }
    //
    //     // Save an int array to Firestore
    //     public async Task SaveIntArray(int[] intArrayToSave, string collectionName, string documentName,
    //         string fieldName)
    //     {
    //         DocumentReference docRef = db.Collection(collectionName).Document(documentName);
    //         Dictionary<string, object> data = new Dictionary<string, object>
    //         {
    //             { fieldName, intArrayToSave }
    //         };
    //
    //         await docRef.SetAsync(data, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
    //         {
    //             if (task.IsFaulted)
    //             {
    //                 Debug.LogError("Failed to save int array: " + task.Exception);
    //             }
    //             else if (task.IsCompleted)
    //             {
    //                 Debug.Log("Int array saved successfully!");
    //             }
    //         });
    //     }
    //
    //     // Load an int array from Firestore
    //     public async Task<int[]> LoadIntArray(string collectionName, string documentName, string fieldName)
    //     {
    //         int[] loadedIntArray = null;
    //         DocumentReference docRef = db.Collection(collectionName).Document(documentName);
    //
    //         DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
    //         if (snapshot.Exists)
    //         {
    //             Dictionary<string, object> data = snapshot.ToDictionary();
    //             if (data.ContainsKey(fieldName))
    //             {
    //                 loadedIntArray = ((List<object>)data[fieldName]).ConvertAll(x => (int)(long)x).ToArray();
    //             }
    //         }
    //
    //         return loadedIntArray;
    //     }
    // }
}