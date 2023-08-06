using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����â���� ��ũ�� 1���� �����Ͽ� ���������� �� ���ְ� �ϴ� ��ư
/// 
/// 2023.07.24 Ocean
/// </summary>
public class SettingRank : MonoBehaviour
{
    // ��ũ���� 1���� �ϱ� ���� ������ �������ش�.
    public void SetAcademyScore()
    {
        PlayerInfo.Instance.Famous = 900;
        PlayerInfo.Instance.Management = 900;
        PlayerInfo.Instance.TalentDevelopment = 900;
        PlayerInfo.Instance.Activity = 900;
        PlayerInfo.Instance.Goods = 900;

        PlayerInfo.Instance.CurrentRank = Rank.SSS;
    }

    // �ð��� ���Ƿ� ��ũ�� �� �� �ִ� ��¥�� �������ش�.
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

    // ��ũ�� �ʿ��� ���� ��ī���̿� �ٸ� AI��ī������ ������  QuarterlyReport�� �־��ش�. �ȳ־��ָ� ����־ ������ �����.
    public void SetQuarterlyReport()
    {
        QuarterlyReport.Instance.TotalAcademyScores.Clear();
        QuarterlyReport.Instance.TotalAcademyScores.Add(new KeyValuePair<string, int>(PlayerInfo.Instance.AcademyName, 4500));
        QuarterlyReport.Instance.TotalAcademyScores.Add(new KeyValuePair<string, int>("�����ī����", 4000));
        QuarterlyReport.Instance.TotalAcademyScores.Add(new KeyValuePair<string, int>("������ī����", 3800));

        QuarterlyReport.Instance.PrevYearAcademyRank.Clear();
        QuarterlyReport.Instance.PrevYearAcademyRank.Add(new KeyValuePair<string, int>(PlayerInfo.Instance.AcademyName, 1));
        QuarterlyReport.Instance.PrevYearAcademyRank.Add(new KeyValuePair<string, int>("�����ī����", 2));
        QuarterlyReport.Instance.PrevYearAcademyRank.Add(new KeyValuePair<string, int>("������ī����", 3));

        QuarterlyReport.Instance.AcademyNameToRank.Clear();
        QuarterlyReport.Instance.AcademyNameToRank.Add(new KeyValuePair<string, int>(PlayerInfo.Instance.AcademyName, 1));
        QuarterlyReport.Instance.AcademyNameToRank.Add(new KeyValuePair<string, int>("�����ī����", 2));
        QuarterlyReport.Instance.AcademyNameToRank.Add(new KeyValuePair<string, int>("������ī����", 3));
    }
}
