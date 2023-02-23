using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseBinder : MonoBehaviour
{
    private static FirebaseBinder instance = null;

    // firebase 계정 관련 정보들
    private FirebaseAuth auth;
    private FirebaseUser user;
    private FirebaseFirestore db;

    // 로그인 여부를 체크하는 델리게이트
    public Action<bool> loginState;

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
            DontDestroyOnLoad(gameObject);
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
            loginState?.Invoke(false);
        }

        user = auth.CurrentUser;
        if (signed)
        {
            Debug.Log("파이어베이스 로그인");
            loginState?.Invoke(true);
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
        citiesRef.Document("SF").SetAsync(new Dictionary<string, object>()
        {
            { "Name", "San Francisco" },
            { "State", "CA" },
            { "Country", "USA" },
            { "Capital", false },
            { "Population", 860000 }
        }).ContinueWithOnMainThread(task =>
            citiesRef.Document("LA").SetAsync(new Dictionary<string, object>()
            {
                { "Name", "Los Angeles" },
                { "State", "CA" },
                { "Country", "USA" },
                { "Capital", false },
                { "Population", 3900000 }
            })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("DC").SetAsync(new Dictionary<string, object>()
            {
                { "Name", "Washington D.C." },
                { "State", null },
                { "Country", "USA" },
                { "Capital", true },
                { "Population", 680000 }
            })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("TOK").SetAsync(new Dictionary<string, object>()
            {
                { "Name", "Tokyo" },
                { "State", null },
                { "Country", "Japan" },
                { "Capital", true },
                { "Population", 9000000 }
            })
        ).ContinueWithOnMainThread(task =>
            citiesRef.Document("BJ").SetAsync(new Dictionary<string, object>()
            {
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

            if (snapshot.Exists)
            {
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));

                // Dictionary에 저장 완료
                Dictionary<string, object> city = snapshot.ToDictionary();

                Dictionary<string, object> save = new Dictionary<string, object>();

                // Dictionary 내부의 데이터를 읽어보자.
                foreach (KeyValuePair<string, object> pair in city)
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));

                    save.Add(pair.Key, pair.Value);
                }
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });

        // var arr = Json.Instance.player.arr;
    }
}