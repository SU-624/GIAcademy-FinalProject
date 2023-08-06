using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// �� �б⸶�� ������ ��ī���̵��� ���� �� ��ŷ �κ�
/// </summary>

public struct AcademyInfoForQaurterlyReport
{
    public string AcademyName;
    public string Money;
    public string Fame;
    public string Activity;
    public string Operate;
    public string TalentDevelopment;
}

public struct AcademyInfoForGameJam
{
    public string AcademyName;
    public string Funny;
    public string Graphic;
    public string Perfection;
    public string GenreBonus;
}

public class QuarterlyReport
{
    private static QuarterlyReport instance = null;

    public static QuarterlyReport Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new QuarterlyReport();
            }
            return instance;
        }
    }

    // �������� ���� �� ��ī���� ����Ʈ
    private List<AcademyInfoForQaurterlyReport> m_academyList = new List<AcademyInfoForQaurterlyReport>();

    // 1~4�б⵿�� ���� ��ī���̵��� ����. 1�� �� �� ��ũ�� �����ٶ� ���, �� ��ī���̴� ����,,
    private List<KeyValuePair<string, int>> m_totalAIAcademyScores = new List<KeyValuePair<string, int>>();
    private List<KeyValuePair<string, int>> m_TotalAcademyScores = new List<KeyValuePair<string, int>>();

    // ����, ���� ��ī���̵��� ����. ������ �󸶳� �ö����� �Ǻ��ϱ� ���� ���
    private List<KeyValuePair<string, int>> m_prevYearAcademyRank = new List<KeyValuePair<string, int>>();
    //private Dictionary<string, int> m_prevYearAcademyRank = new Dictionary<string, int>(); // ������ ���
    private List<KeyValuePair<string, int>> m_nowAcademyScore = new List<KeyValuePair<string, int>>(); // ������ ���
    private List<KeyValuePair<string, int>> m_nowGameJamAcademyScore = new List<KeyValuePair<string, int>>();
    private List<KeyValuePair<string, int>> m_AcademyNameToRank = new List<KeyValuePair<string, int>>();    // �������� ������ ����ߴ��� �϶��ߴ��� �ƴϸ� �����ߴ��� �����ϱ� ���� ��ųʸ�

    public List<AcademyInfoForQaurterlyReport> AcademyList => m_academyList;

    public List<KeyValuePair<string, int>> TotalAIAcademyScores => m_totalAIAcademyScores;
    public List<KeyValuePair<string, int>> TotalAcademyScores { get { return m_TotalAcademyScores; } set { m_TotalAcademyScores = value; } }
    public List<KeyValuePair<string, int>> PrevYearAcademyRank { get { return m_prevYearAcademyRank; } set { m_prevYearAcademyRank = value; } }
    public List<KeyValuePair<string, int>> AcademyNameToRank { get { return m_AcademyNameToRank; } set { m_AcademyNameToRank = value; } }
    public List<KeyValuePair<string, int>> NowAcademyScore => m_nowAcademyScore;
    public List<KeyValuePair<string, int>> NowGameJamAcademyScore => m_nowGameJamAcademyScore;

    public void Init()
    {
        m_academyList.Clear();
        m_totalAIAcademyScores.Clear();
        m_nowAcademyScore.Clear();
    }

    public void AddNewAcademy(string name, string money, string fame, string activity, string operate, string talentDevelopment)
    {
        AcademyInfoForQaurterlyReport newAcademy = new AcademyInfoForQaurterlyReport();

        newAcademy.AcademyName = name;
        newAcademy.Money = money;
        newAcademy.Fame = fame;
        newAcademy.Activity = activity;
        newAcademy.Operate = operate;
        newAcademy.TalentDevelopment = talentDevelopment;
        m_academyList.Add(newAcademy);
        m_totalAIAcademyScores.Add(new KeyValuePair<string, int>(name, 0));
    }

    public void AddNewGameJamAcademy(string _name, string _funny, string _graphic, string _perfection, string _genreBouns)
    {
        AcademyInfoForGameJam newGameJamAcademy = new AcademyInfoForGameJam();

        newGameJamAcademy.AcademyName = _name;
        newGameJamAcademy.Funny = _funny;
        newGameJamAcademy.Graphic = _graphic;
        newGameJamAcademy.Perfection = _perfection;
        newGameJamAcademy.GenreBonus = _genreBouns;
    }

    // ��� ��ī������ ������ �ű�� �κ�
    public void AcademyScoreCal()
    {
        m_nowAcademyScore.Clear();

        for (int i = 0; i < m_academyList.Count; i++)
        {
            int score = 0;
            score += CalculateRank(m_academyList[i].Money);
            score += CalculateRank(m_academyList[i].Fame);
            score += CalculateRank(m_academyList[i].Activity);
            score += CalculateRank(m_academyList[i].Operate);
            score += CalculateRank(m_academyList[i].TalentDevelopment);
            int randB = Random.Range(95, 106);
            score = (int)(score * (randB * 0.01));

            KeyValuePair<string, int> _modifiy = m_totalAIAcademyScores.Find(x => x.Key == m_academyList[i].AcademyName);
            int _modifiyScore = _modifiy.Value + score;
            KeyValuePair<string, int> _new = new KeyValuePair<string, int>(_modifiy.Key, _modifiyScore);

            m_totalAIAcademyScores[m_totalAIAcademyScores.IndexOf(_modifiy)] = _new;

            m_nowAcademyScore.Add(new KeyValuePair<string, int>(m_academyList[i].AcademyName, score));
        }
    }

    private int CalculateRank(string rank)
    {
        int rand = 0;

        switch (rank)
        {
            case "SSS":
            rand = Random.Range(320, 401);
            break;

            case "SS":
            rand = Random.Range(280, 361);
            break;

            case "S":
            rand = Random.Range(240, 321);
            break;

            case "A":
            rand = Random.Range(200, 281);
            break;

            case "B":
            rand = Random.Range(160, 241);
            break;

            case "C":
            rand = Random.Range(120, 201);
            break;

            case "D":
            rand = Random.Range(80, 161);
            break;

            case "E":
            rand = Random.Range(40, 121);
            break;

            case "F":
            rand = Random.Range(0, 81);
            break;
        }
        return rand;
    }
}
