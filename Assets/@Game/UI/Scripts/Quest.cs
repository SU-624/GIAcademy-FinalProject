using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 2023. 06. 12 Mang
/// 
/// 기획적으로 게임의 방향성이 부족하다 는 생각에 퀘스트가 생김.
/// 
/// </summary>
public class Quest : MonoBehaviour
{
    [Header("팝업 퀘스트 패널")]
    // [SerializeField] private PopUpUI PopUpQuestPanel;
    // [SerializeField] private PopOffUI PopOffQuestPanel;

    [SerializeField] private Image QuestPanel;         // 퀘스트 패널

    [Space(5f)]
    [SerializeField] private Image QuestStepImg1;           // 퀘스트 패널 1
    [SerializeField] private Image QuestStepImg2;           // 퀘스트 패널 2
    [SerializeField] private Image QuestStepImg3;           // 퀘스트 패널 3
    [SerializeField] private Image QuestStepImg4;           // 퀘스트 패널 4
    [SerializeField] private Image QuestStepImg5;           // 퀘스트 패널 5
    [Space(5f)]
    [SerializeField] private Image Quest1;                  // 퀘스트 1
    [SerializeField] private Image Quest2;                  // 퀘스트 2
    [SerializeField] private Image Quest3;                  // 퀘스트 3
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI QuestName1;    // 퀘스트 1 이름
    [SerializeField] private TextMeshProUGUI QuestName2;    // 퀘스트 2 이름
    [SerializeField] private TextMeshProUGUI QuestName3;    // 퀘스트 3 이름
    [Space(5f)]
    [SerializeField] private Image CheckedImgQuest1;        // 퀘스트 1 체크된 이미지
    [SerializeField] private Image CheckedImgQuest2;        // 퀘스트 2 체크된 이미지
    [SerializeField] private Image CheckedImgQuest3;        // 퀘스트 3 체크된 이미지
    [Space(5f)]
    [SerializeField] private Slider QuestSlideBar1;         // 퀘스트 1 진행도 슬라이드바
    [SerializeField] private Slider QuestSlideBar2;         // 퀘스트 2 진행도 슬라이드바
    [SerializeField] private Slider QuestSlideBar3;         // 퀘스트 3 진행도 슬라이드바

    // 퀘스트의 진행도를 체크하고, 값을 변경 해 줄 변수들
    private int QuestStepNum = 1;           //  1~ 5단계 까지의 퀘스트 스텝
    private int NowStepQuestClear = 0;      // 현재 단계의 퀘스트를 몇 개 깼는지 체크
    private int TargetValueQuestNnm = 0;    // 현재 단계에서 깨야 할 퀘스트의 갯수

    // 퀘스트 오픈 버튼은 InGameUI 것을 불러와서 쓰자

    private bool isQuestPanelOpen = false;

    public void Start()
    {
        InGameUI.Instance.GetQuestOpenButton.onClick.AddListener(IfClickQuestPanelButton);
    }

    // 인게임패널의 퀘스트 버튼을 클릭 시 현재 퀘스트 목록의 리스트를 보여 줄 함수
    public void IfClickQuestPanelButton()
    {
        if (QuestPanel.gameObject.activeSelf)
        {
            QuestPanel.gameObject.SetActive(false);
        }
        else
        {
            QuestPanel.gameObject.SetActive(true);
        }
    }

    public void SetOriginalQuestData()
    {
    }
}
