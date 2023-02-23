using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using Newtonsoft.Json;

public class TestFirebase : MonoBehaviour
{
    private void Awake()
    {
        var obj = FindObjectsOfType<TestFirebase>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private TMP_InputField IDField;
    [SerializeField]
    private TMP_InputField PasswordField;

    public void IDCreateButton()
    {
        var temptext = IDField.text;
        FirebaseBinder.Instance.CreateEmailID(IDField.text, PasswordField.text);
    }

    public void IDLoginButton()
    {
        FirebaseBinder.Instance.SignInEmail(IDField.text, PasswordField.text);
    }

    public void UploadTest()
    {
        FirebaseBinder.Instance.firestoreSaveTest();
    }

    public void DounloadTest()
    {
        FirebaseBinder.Instance.FirestoreLoadTest();
    }
}
