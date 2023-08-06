using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 설정창에서 랭크를 1위로 설정하여 엔딩씬까지 볼 수있게 하는 버튼
/// 
/// 2023.07.24 Ocean
/// </summary>
public class SettingRank : MonoBehaviour
{
    // 랭크에서 1등을 하기 위한 점수를 셋팅해준다.
    public void SetAcademyScore()
    {
        PlayerInfo.Instance.Famous = 900;
        PlayerInfo.Instance.Management = 900;
        PlayerInfo.Instance.TalentDevelopment = 900;
        PlayerInfo.Instance.Activity = 900;
        PlayerInfo.Instance.Goods = 900;

        PlayerInfo.Instance.CurrentRank = Rank.SSS;
    }

    // 시간을 임의로 랭크를 볼 수 있는 날짜로 변경해준다.
    public void ChangeDay()
    {
        GameTime.Instance.Year = 2;
        GameTime.Instance.Month = 3;
        GameTime.Instance.Week = 1;
        GameTime.Instance.Day = 2;
        PlayerInfo.Instance.IsFirstGameJam = false;
        PlayerInfo.Instance.IsFirstGameShow = false;
        PlayerInfo.Instance.IsFirstClassSetting = false;
    }

    // 랭크에 필요한 나의 아카데미와 다른 AI아카데이의 정보를  QuarterlyReport에 넣어준다. 안넣어주면 비어있어서 문제가 생긴다.
    public void SetQuarterlyReport()
    {
        QuarterlyReport.Instance.TotalAcademyScores.Clear();
        QuarterlyReport.Instance.TotalAcademyScores.Add(new KeyValuePair<string, int>(PlayerInfo.Instance.AcademyName, 4500));
        QuarterlyReport.Instance.TotalAcademyScores.Add(new KeyValuePair<string, int>("숙희아카데미", 4000));
        QuarterlyReport.Instance.TotalAcademyScores.Add(new KeyValuePair<string, int>("나나아카데미", 3800));

        QuarterlyReport.Instance.PrevYearAcademyRank.Clear();
        QuarterlyReport.Instance.PrevYearAcademyRank.Add(new KeyValuePair<string, int>(PlayerInfo.Instance.AcademyName, 1));
        QuarterlyReport.Instance.PrevYearAcademyRank.Add(new KeyValuePair<string, int>("숙희아카데미", 2));
        QuarterlyReport.Instance.PrevYearAcademyRank.Add(new KeyValuePair<string, int>("나나아카데미", 3));

        QuarterlyReport.Instance.AcademyNameToRank.Clear();
        QuarterlyReport.Instance.AcademyNameToRank.Add(new KeyValuePair<string, int>(PlayerInfo.Instance.AcademyName, 1));
        QuarterlyReport.Instance.AcademyNameToRank.Add(new KeyValuePair<string, int>("숙희아카데미", 2));
        QuarterlyReport.Instance.AcademyNameToRank.Add(new KeyValuePair<string, int>("나나아카데미", 3));
    }
}
