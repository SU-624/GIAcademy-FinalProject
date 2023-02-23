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

    // GPGS CloudSave Instance
    public ISavedGameClient SavedGame =>
        PlayGamesPlatform.Instance.SavedGame;

    // GPGS Event Instance
    public IEventsClient Events =>
        PlayGamesPlatform.Instance.Events;

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

    private void Start()
    {
        Debug.Log("Called GPGSBinder Start");

        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestEmail()
            .Build());

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
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

                    // 파이어베이스 로그인에 쓸 IDToken을 요청하여 가져온다.
                    string idToken = null;
                    StartCoroutine(TryFirebaseLogin());
                }
                else // 실패하면
                {
                    Debug.Log("Google Login Fail");
                }
            });
        }
    }

    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        
        // Firebase Login 시도
        FirebaseBinder.Instance.TryFirebaseGoogleLogin(idToken);
    }


    // GPGS에서 로그아웃 한다.
    public void TryLogout()
    {
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃

            Debug.Log("로그아웃");
        }
        else
        {
            Debug.Log("로그인이 되어있지 않습니다.");
        }
    }

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
}