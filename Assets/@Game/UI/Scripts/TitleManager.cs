using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Button TouchScreenButton;
    public SoundManager Sound;

    [Header("게임 리셋 버튼 관련")]
    [Space(5)]
    [SerializeField] private Button GameResetButton;

    [SerializeField] private Image ResetGamePanel;
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;


    // w 2340 / h 1080 / 0.6배
    private int width = 1404;
    private int height = 648;

    private void Start()
    {
        GameResetButton.onClick.AddListener(IfClickResetGameButton);
        YesButton.onClick.AddListener(IfClickGameResetYesNoButton);
        NoButton.onClick.AddListener(IfClickGameResetYesNoButton);
    }

    private void Awake()
    {
        // *0.6
        Screen.SetResolution(width, height, false);
        ResetGamePanel.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        // EventScriptManager의 버튼에 함수 달기 되려나
        Sound.PlayTitleBgm();
        
        Json.Instance.DataSetting();
    }

    // Update is called once per frame
    void Update()
    {
        // 변경되었다면 바꾼다.
        //if (width != Screen.width)
        {
            Screen.SetResolution(Screen.width, Screen.width * 1080 / 2340, false);
        }
    }

    public void DeleteSaveDataBtn()
    {
        Json.Instance.DeleteJsonFolder();
        Json.Instance.UseLoadingData = false;
    }

    public void IfClickResetGameButton()
    {
        ResetGamePanel.gameObject.SetActive(true);
    }

    public void IfClickGameResetYesNoButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;

        if(nowClick.name == "YesButton")
        {
            ResetGamePanel.gameObject.SetActive(false);
            DeleteSaveDataBtn();
        }
        else        // NoButton 클릭
        {
            ResetGamePanel.gameObject.SetActive(false);
        }
    }
}