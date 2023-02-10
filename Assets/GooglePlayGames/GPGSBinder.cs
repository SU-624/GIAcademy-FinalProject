using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi.Events;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class GPGSBinder : MonoBehaviour
{
    private static GPGSBinder instance = null;

    // firebase 계정 관련 정보들
    private FirebaseAuth auth;
    private FirebaseUser user;
    private FirebaseFirestore db;

    // GPGS CloudSave Instance
    public ISavedGameClient SavedGame =>
        PlayGamesPlatform.Instance.SavedGame;
    // GPGS Event Instance
    public IEventsClient Events =>
        PlayGamesPlatform.Instance.Events;

    // 로그인 여부를 체크하는 델리게이트
    public Action<bool> loginState;

    // Unity Singleton
    public static GPGSBinder Instance
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

    // UserId를 가져오는 코드
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
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    void Start()
    {
        Debug.Log("Called GPGSBinder Start");

        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestEmail()
            .Build());

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        // 구글 플레이 게임 활성화

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

    // 유저 정보가 변경될 떄 작동하는 이벤트
    private void AuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser == user) return;

        // 지금 유저와 이전 유저가 다르다는 것은 게정 상태가 변한것.
        bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
        if (!signed && user != null)
        {
            Debug.Log("파이버베이스 로그아웃");
            loginState?.Invoke(false);
        }

        user = auth.CurrentUser;
        if (signed)
        {
            Debug.Log("파이어베이스 로그인");
            loginState?.Invoke(true);
        }
    }

    public void TryGoogleLogin()
    {
        if (!Social.localUser.authenticated) // 로그인 되어 있지 않다면
        {
            Social.localUser.Authenticate(success => // 로그인 시도
            {
                if (success) // 성공하면
                {
                    Debug.Log("Google Login Success");
                    StartCoroutine(TryFirebaseGoogleLogin()); // Firebase Login 시도
                }
                else // 실패하면
                {
                    Debug.Log("Google Login Fail");
                }
            });
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    // 파이어베이스에 구글아이디로 로그인한다.
    IEnumerator TryFirebaseGoogleLogin()
    {
        // 파이어베이스 로그인에 쓸 IDToken을 요청하여 가져온다.
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();


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
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    // GPGS와 Firebase를 로그아웃 한다.
    public void TryLogout()
    {
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃
            auth.SignOut(); // Firebase 로그아웃

            Debug.Log("로그아웃");
        }
        else
        {
            Debug.Log("로그인이 되어있지 않습니다.");
        }
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

    // Firestore 서버에 저장을 한다 (임시코드)
    public void firestoreSaveTest()
    {
        // DocumentReference docRef = db.Collection("Users").Document(UserId);
        // Dictionary<string, object> user = new Dictionary<string, object>
        // {
        //     {
        //         "Timestamp", FieldValue.ServerTimestamp
        //     },
        //     {
        //         "First", "Woody"
        //     },
        //     {
        //         "Last", "Lovelace"
        //     },
        //     {
        //         "Born", 1815
        //     },
        //     {
        //         "work", "Teacher"
        //     },
        // };
        //
        // docRef.SetAsync(user).ContinueWithOnMainThread(task =>
        // {
        //     Debug.Log("Added data to the alovelace document in the users collection.");
        // });
        
        
        
        
        CollectionReference citiesRef = db.Collection("cities");
        citiesRef.Document("SF").SetAsync(new Dictionary<string, object>(){
            { "Name", "San Francisco" },
            { "State", "CA" },
            { "Country", "USA" },
            { "Capital", false },
            { "Population", 860000 }
        }).ContinueWithOnMainThread(task =>
            citiesRef.Document("LA").SetAsync(new Dictionary<string, object>(){
                { "Name", "Los Angeles" },
                { "State", "CA" },
                { "Country", "USA" },
                { "Capital", false },
                { "Population", 3900000 }
            })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("DC").SetAsync(new Dictionary<string, object>(){
                { "Name", "Washington D.C." },
                { "State", null },
                { "Country", "USA" },
                { "Capital", true },
                { "Population", 680000 }
            })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("TOK").SetAsync(new Dictionary<string, object>(){
                { "Name", "Tokyo" },
                { "State", null },
                { "Country", "Japan" },
                { "Capital", true },
                { "Population", 9000000 }
            })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("BJ").SetAsync(new Dictionary<string, object>(){
                { "Name", "Beijing" },
                { "State", null },
                { "Country", "China" },
                { "Capital", true },
                { "Population", 21500000 }
            })
        );
    }


    public void FirestoreLoadTest()
    {
        // 특정 문서를 찾아서...
        DocumentReference docRef = db.Collection("cities").Document("SF");
        
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            
            if (snapshot.Exists) {
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                
                // Dictionary에 저장 완료
                Dictionary<string, object> city = snapshot.ToDictionary();
                
                // Dictionary 내부의 데이터를 읽어보자.
                foreach (KeyValuePair<string, object> pair in city) {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }
                
            } else {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }

    
    
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////

    
    // GPGS 서버에다가 데이터를 저장한다.
    public void SaveCloud(string fileName, string saveData, Action<bool> onCloudSaved = null)
    {
        SavedGame.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood, (status, game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    var update = new SavedGameMetadataUpdate.Builder().Build();
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(saveData);
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    SavedGame.CommitUpdate(game, update, bytes,
                        (status2, game2) => { onCloudSaved?.Invoke(status2 == SavedGameRequestStatus.Success); });
                }
            });
    }

    // GPGS 서버에서 데이터를 가져온다.
    public void LoadCloud(string fileName, Action<bool, string> onCloudLoaded = null)
    {
        SavedGame.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood, (status, game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    SavedGame.ReadBinaryData(game, (status2, loadedData) =>
                    {
                        if (status2 == SavedGameRequestStatus.Success)
                        {
                            var data = System.Text.Encoding.UTF8.GetString(loadedData);
                            onCloudLoaded?.Invoke(true, data);
                        }
                        else
                            onCloudLoaded?.Invoke(false, null);
                    });
                }
            });
    }

    // GPGS 서버에 저장되어있는 데이터를 삭제한다.
    public void DeleteCloud(string fileName, Action<bool> onCloudDeleted = null)
    {
        SavedGame.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, (status, game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    SavedGame.Delete(game);
                    onCloudDeleted?.Invoke(true);
                }
                else
                    onCloudDeleted?.Invoke(false);
            });
    }

    // GPGS 업적 UI를 화면에 띄운다.
    public void ShowAchievementUI() =>
        Social.ShowAchievementsUI();

    // GPGS 업적 하나를 해금한다.
    public void UnlockAchievement(string gpgsId, Action<bool> onUnlocked = null) =>
        Social.ReportProgress(gpgsId, 100, success => onUnlocked?.Invoke(success));

    public void IncrementAchievement(string gpgsId, int steps, Action<bool> onUnlocked = null) =>
        PlayGamesPlatform.Instance.IncrementAchievement(gpgsId, steps, success => onUnlocked?.Invoke(success));


    public void ShowAllLeaderboardUI() =>
        Social.ShowLeaderboardUI();

    public void ShowTargetLeaderboardUI(string gpgsId) =>
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(gpgsId);

    public void ReportLeaderboard(string gpgsId, long score, Action<bool> onReported = null) =>
        Social.ReportScore(score, gpgsId, success => onReported?.Invoke(success));

    public void LoadAllLeaderboardArray(string gpgsId, Action<UnityEngine.SocialPlatforms.IScore[]> onloaded = null) =>
        Social.LoadScores(gpgsId, onloaded);

    public void LoadCustomLeaderboardArray(string gpgsId, int rowCount, LeaderboardStart leaderboardStart,
        LeaderboardTimeSpan leaderboardTimeSpan, Action<bool, LeaderboardScoreData> onloaded = null)
    {
        PlayGamesPlatform.Instance.LoadScores(gpgsId, leaderboardStart, rowCount, LeaderboardCollection.Public,
            leaderboardTimeSpan, data => { onloaded?.Invoke(data.Status == ResponseStatus.Success, data); });
    }

    public void IncrementEvent(string gpgsId, uint steps)
    {
        Events.IncrementEvent(gpgsId, steps);
    }

    public void LoadEvent(string gpgsId, Action<bool, IEvent> onEventLoaded = null)
    {
        Events.FetchEvent(DataSource.ReadCacheOrNetwork, gpgsId,
            (status, iEvent) => { onEventLoaded?.Invoke(status == ResponseStatus.Success, iEvent); });
    }

    public void LoadAllEvent(Action<bool, List<IEvent>> onEventsLoaded = null)
    {
        Events.FetchAllEvents(DataSource.ReadCacheOrNetwork,
            (status, events) => { onEventsLoaded?.Invoke(status == ResponseStatus.Success, events); });
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
}