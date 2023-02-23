using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGPGS : MonoBehaviour
{
    string log;

    private void Awake()
    {
        var obj = FindObjectsOfType<TestGPGS>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 3);


        if (GUILayout.Button("ClearLog"))
            log = "";

        if (GUILayout.Button("Login 로그인"))
        {
            GPGSBinder.Instance.TryGoogleLogin();
            log = "Try LogIn";
        }

        if (GUILayout.Button("Logout 로그아웃"))
        {
            GPGSBinder.Instance.TryLogout();
            log = "LogOut";
        }
        
        if (GUILayout.Button("SaveCloud 클라우드 저장"))
            GPGSBinder.Instance.SaveCloud("mysave", "Hellow GI", success => log = $"{success}");
        
        if (GUILayout.Button("LoadCloud 클라우드 불러오기"))
            GPGSBinder.Instance.LoadCloud("mysave", (success, data) => log = $"{success}, {data}");
        
        if (GUILayout.Button("DeleteCloud 클라우드 정보 삭제"))
            GPGSBinder.Instance.DeleteCloud("mysave", success => log = $"{success}");
        
        if (GUILayout.Button("ShowAchievementUI 업적 UI 표시"))
            GPGSBinder.Instance.ShowAchievementUI();
        
        if (GUILayout.Button("UnlockAchievement_one 1번째 업적 언락"))
            GPGSBinder.Instance.UnlockAchievement(GPGSIds.achievement_Welcome, success => log = $"{success}");
        
        if (GUILayout.Button("UnlockAchievement_two 두번째 업적 언락"))
            GPGSBinder.Instance.UnlockAchievement(GPGSIds.achievement_two, success => log = $"{success}");
        
        if (GUILayout.Button("IncrementAchievement_three 세번째 업적 10%"))
            GPGSBinder.Instance.IncrementAchievement(GPGSIds.achievement_three, 1, success => log = $"{success}");
        
        if (GUILayout.Button("ShowAllLeaderboardUI 모든 리더보드 UI 표시"))
            GPGSBinder.Instance.ShowAllLeaderboardUI();
        
        if (GUILayout.Button("ShowTargetLeaderboardUI_num 원하는 리더보드 UI 표시"))
            GPGSBinder.Instance.ShowTargetLeaderboardUI(GPGSIds.leaderboard_num);
        
        if (GUILayout.Button("ReportLeaderboard_num 리더보드에 점수 입력 +1000"))
            GPGSBinder.Instance.ReportLeaderboard(GPGSIds.leaderboard_num, 1000, success => log = $"{success}");
        
        if (GUILayout.Button("LoadAllLeaderboardArray_num 모든 리더보드의 점수 가져오기"))
            GPGSBinder.Instance.LoadAllLeaderboardArray(GPGSIds.leaderboard_num, scores =>
            {
                log = "";
                for (int i = 0; i < scores.Length; i++)
                    log += $"{i}, {scores[i].rank}, {scores[i].value}, {scores[i].userID}, {scores[i].date}\n";
            });
        
        if (GUILayout.Button("LoadCustomLeaderboardArray_num 리더보드 조절하여 가져오기"))
            GPGSBinder.Instance.LoadCustomLeaderboardArray(GPGSIds.leaderboard_num, 10,
                GooglePlayGames.BasicApi.LeaderboardStart.PlayerCentered, GooglePlayGames.BasicApi.LeaderboardTimeSpan.Daily, (success, scoreData) =>
                {
                    log = $"{success}\n";
                    var scores = scoreData.Scores;
                    for (int i = 0; i < scores.Length; i++)
                        log += $"{i}, {scores[i].rank}, {scores[i].value}, {scores[i].userID}, {scores[i].date}\n";
                });
        
        if (GUILayout.Button("IncrementEvent_event 이벤트 증가"))
            GPGSBinder.Instance.IncrementEvent(GPGSIds.event_event, 1);
        
        if (GUILayout.Button("LoadEvent_event 이벤트 불러와서 정보 확인"))
            GPGSBinder.Instance.LoadEvent(GPGSIds.event_event, (success, iEvent) =>
            {
                log = $"{success}, {iEvent.Name}, {iEvent.CurrentCount}";
            });
        
        if (GUILayout.Button("LoadAllEvent 이벤트를 모두 받아오기"))
            GPGSBinder.Instance.LoadAllEvent((success, iEvents) =>
            {
                log = $"{success}\n";
                foreach (var iEvent in iEvents)
                    log += $"{iEvent.Name}, {iEvent.CurrentCount}\n";
            });

        if (GUILayout.Button("user.UserId"))
        {
            log =  FirebaseBinder.Instance.UserId;
        }
        
        if (GUILayout.Button("user.DisplayName"))
        {
            log =  FirebaseBinder.Instance.DisplayName;
        }
        
        GUILayout.Label(log);
    }
}