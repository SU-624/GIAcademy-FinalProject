using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionPrefab : MonoBehaviour
{
    [SerializeField] private Image NowMissionPrefab;                // 현재 미션 프리팹
    [SerializeField] private Image CheckBoxCheckedImg;                // 미션 클리어 유무 체크 이미지
    [Header("미션 이름")]
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI QuestName;             // 미션 이름

    [SerializeField] private Slider QuestSlideBar;                  // 미션 진행도 슬라이드바
    [SerializeField] private TextMeshProUGUI MissionAmountText;     // 미션 조건 퍼센테이지 적어줄 텍스트

    [SerializeField] private Button ClearQuestButton;              // 미션 클리어 버튼
    [SerializeField] private Image QuestClearImg;                  // 미션 클리어 이미지


    // 미션의 리워드 텍스트 보여줄 것
    [Space(10f)]
    [Header("미션 보상 부분 그리기")]
    [SerializeField] private RectTransform MissionRewardText;                  // 미션 리워드 텍스트 전체

    [SerializeField] private Image MissionReward1;                             // 미션 리워드 1 
    [SerializeField] private Image MissionReward2;                             // 미션 리워드 2

    [Space(5f)]
    [SerializeField] private Image MissionReward1Img;                          // 미션 리워드 1 이미지
    [SerializeField] private TextMeshProUGUI MissionReward1AmountText;         // 미션 리워드 1 텍스트

    [SerializeField] private Image MissionReward2Img;                          // 미션 리워드 2 이미지
    [SerializeField] private TextMeshProUGUI MissionReward2AmountText;         // 미션 리워드 2 텍스트

    #region 프로퍼티들
    public Image GetNowMissionPrefab
    {
        get { return NowMissionPrefab; }
        set { NowMissionPrefab = value; }
    }

    public Image GetCheckBoxCheckedImg
    {
        get { return CheckBoxCheckedImg; }
        set { CheckBoxCheckedImg = value; }
    }
    // -------------------------
    public TextMeshProUGUI GetQuestName
    {
        get { return QuestName; }
        set { QuestName = value; }
    }
    // -------------------------
    public Slider GetQuestSlideBar
    {
        get { return QuestSlideBar; }
        set { QuestSlideBar = value; }
    }

    public TextMeshProUGUI GetMissionAmountText
    {
        get { return MissionAmountText; }
        set { MissionAmountText = value; }
    }
    // -------------------------
    public Button GetClearQuestButton
    {
        get { return ClearQuestButton; }
        set { ClearQuestButton = value; }
    }
    public Image GetQuestClearImg
    {
        get { return QuestClearImg; }
        set { QuestClearImg = value; }
    }

    // -------------------------
    public RectTransform GetMissionRewardText
    {
        get { return MissionRewardText; }
        set { MissionRewardText = value; }
    }
    // -------------------------
    public Image GetMissionReward1
    {
        get { return MissionReward1; }
        set { MissionReward1 = value; }
    }
    public Image GetMissionReward2
    {
        get { return MissionReward2; }
        set { MissionReward2 = value; }
    }
    // -------------------------
    public Image GetMissionReward1Img
    {
        get { return MissionReward1Img; }
        set { MissionReward1Img = value; }
    }
    public TextMeshProUGUI GetMissionReward1AmountText
    {
        get { return MissionReward1AmountText; }
        set { MissionReward1AmountText = value; }
    }
    // -------------------------
    public Image GetMissionReward2Img
    {
        get { return MissionReward2Img; }
        set { MissionReward2Img = value; }
    }
    public TextMeshProUGUI GetMissionReward2AmountText
    {
        get { return MissionReward2AmountText; }
        set { MissionReward2AmountText = value; }
    }

    #endregion
}
