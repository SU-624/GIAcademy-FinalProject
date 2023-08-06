using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirebaseUI : MonoBehaviour
{
    // 싱글톤 파이어베이스와 연결하는 Manager
    public FirebaseBinder firebaseBinder;
    
    // 정보를 표시하기 위한 TMP
    public TextMeshProUGUI singInfoLabel;
    public TextMeshProUGUI userIDLabel;
    
    // 이메일 로그인을 위한 정보가 담긴 필드
    public TMP_InputField email;
    public TMP_InputField password;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Set onChangedState");
        firebaseBinder.LoginState += OnChangedState;

        singInfoLabel.text = "Online_Login";
        userIDLabel.text = "UID :";
    }

    private void OnChangedState(bool sign)
    {
        Debug.Log("Called onChangedState");
        singInfoLabel.text = sign ? "로그인" : "로그아웃";
        userIDLabel.text = sign ? "UID : " + firebaseBinder.UserId : "SignOut...";
       
        // if (sign)
        // {
        //     if (FirebaseBinder.DisplayName != null)
        //         usernameInput.text = FirebaseBinder.Instance.DisplayName;
        //     else
        //     {
        //         usernameInput.text = "Test_Guest";
        //     }
        // }
    }
    
    public void CreateEmailIDBtn()
    {
        firebaseBinder.CreateEmailID(email.text, password.text);
    }

    public void SignInEmailBtn()
    {
        firebaseBinder.SignInEmail(email.text, password.text);
    }
    
    public void SignOutBtn()
    {
        singInfoLabel.text = "Firebase Chat";
        userIDLabel.text = "SignOut...";
       
        firebaseBinder.TryLogout();
    }

    public void SaveTestBut()
    {
        //AllInOneData.Instance.FirestoreTestData();
        firebaseBinder.SaveDataInFirestore();
    }
    
    public async void LoadTestBut()
    {
        firebaseBinder.LoadDataForFirebase();
        
        //List<GameShowData> dataString = await firebaseBinder.LoadGameShowData();

        //Debug.Log(dataString[0].GameShow_Health);
        
    }

    public void ManyDataLoadTest()
    {
       // firebaseBinder.ManyDataSaveLoadTest();
    }
    
    
}
