using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NewGameJam : MonoBehaviour
{
    private List<DifficultyPreset> m_HeartPresetList = new List<DifficultyPreset>();
    private List<GameDifficulty> m_Difficulty = new List<GameDifficulty>();                                                 // 내 아카데미 등급에 따른 난이도를 설정하는 리스트

    private List<GameShowData> m_TotalGameShowData = new List<GameShowData>();                                              // 이번달 공고에 띄워줄 게임쇼들을 모두 모아놓을 리스트
    private List<GameShowData> m_FixedGameShowData = new List<GameShowData>();
    private List<GameShowData> m_RandomGameShowData = new List<GameShowData>();
    private int m_NowMonth;

    // 내 아카데미 점수에 따른 게임쇼 난이도 뽑아주는 함수
    private int GetRankByMyAcademyRank(Rank myRank)
    {
        foreach (GameDifficulty table in m_Difficulty)
        {
            if (table.MyRank == myRank)
            {
                return table.Difficulty;
            }
        }

        return 0;
    }

    private void InitHeartPresetList()
    {
        // 프리셋 1
        m_HeartPresetList.Add(new DifficultyPreset(1, 1, 1, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(1, 2, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(1, 3, 3, 1, 1, 2));
        m_HeartPresetList.Add(new DifficultyPreset(1, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(1, 5, 2, 2, 3, 3));

        // 프리셋 2
        m_HeartPresetList.Add(new DifficultyPreset(2, 1, 2, 1, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 2, 2, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 3, 3, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(2, 5, 3, 3, 2, 2));

        // 프리셋 3
        m_HeartPresetList.Add(new DifficultyPreset(3, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(3, 2, 1, 2, 1, 2));
        m_HeartPresetList.Add(new DifficultyPreset(3, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(3, 4, 1, 3, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(3, 5, 2, 3, 3, 2));

        // 프리셋 4
        m_HeartPresetList.Add(new DifficultyPreset(4, 1, 1, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 2, 2, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 3, 1, 2, 3, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 4, 2, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(4, 5, 3, 2, 3, 2));

        // 프리셋 5
        m_HeartPresetList.Add(new DifficultyPreset(5, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 2, 2, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 3, 2, 3, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(5, 5, 3, 3, 2, 2));

        // 프리셋 6
        m_HeartPresetList.Add(new DifficultyPreset(6, 1, 2, 1, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 2, 2, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 4, 2, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(6, 5, 3, 2, 3, 2));

        // 프리셋 7
        m_HeartPresetList.Add(new DifficultyPreset(7, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 2, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 4, 2, 3, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(7, 5, 2, 3, 3, 2));

        // 프리셋 8
        m_HeartPresetList.Add(new DifficultyPreset(8, 1, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 2, 2, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 3, 2, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 4, 3, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(8, 5, 3, 3, 3, 2));
    }

    private void ClassifyGameShowData()
    {
        for (int i = 0; i < AllOriginalJsonData.Instance.OriginalRandomGameShowData.Count; i++)
        {
            if (AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Fix)
            {
                m_FixedGameShowData.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
            }
            else
            {
                m_RandomGameShowData.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
            }
        }
    }

    private void BringFixedGameShowData()
    {
        List<GameShowData> m_MonthGameShowData = new List<GameShowData>();

        int difficulty = GetRankByMyAcademyRank(PlayerInfo.Instance.m_CurrentRank);
        int _fun = 0;
        int _graphic = 0;
        int _perfection = 0;
        int _genre = 0;

        for (int i = 0; i < m_FixedGameShowData.Count; i++)
        {
            // 매년 발생하는 공고
            if (m_FixedGameShowData[i].GameShow_Year == 0 &&
                m_FixedGameShowData[i].GameShow_Month == m_NowMonth + 1)
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);
            }
            // 홀수년도 발생
            else if (m_FixedGameShowData[i].GameShow_Year == -1 &&
                 (10 % GameTime.Instance.FlowTime.NowYear) != 0 &&
                 m_FixedGameShowData[i].GameShow_Month == m_NowMonth + 1)
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);
            }
            // 짝수년도 발생
            else if (m_FixedGameShowData[i].GameShow_Year == -2 &&
                 (10 % GameTime.Instance.FlowTime.NowYear) == 0 &&
                 m_FixedGameShowData[i].GameShow_Month == m_NowMonth + 1)
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);
            }
        }

        switch (difficulty)
        {
            // 난이도1 2개, 난이도 2,3,4,5 중 1개
            case 1:
            {
                int _randomDifficulty = UnityEngine.Random.Range(2, 6);
                int _randomIndex = UnityEngine.Random.Range(0, m_MonthGameShowData.Count);

                for (int i = 0; i < m_MonthGameShowData.Count; i++)
                {
                    if (i != _randomIndex)
                    {
                        SetGameShowData(m_MonthGameShowData, i, 1);
                    }
                    else
                    {
                        SetGameShowData(m_MonthGameShowData, _randomIndex, _randomDifficulty);
                    }
                }
            }
            break;

            case 2:
            {
                int _randomDifficulty = UnityEngine.Random.Range(2, 6);
                int _randomIndex = UnityEngine.Random.Range(0, m_MonthGameShowData.Count);

                for (int i = 0; i < m_MonthGameShowData.Count; i++)
                {


                }
            }
            break;

            case 3:
            {

            }
            break;

            case 4:
            {

            }
            break;

            case 5:
            {

            }
            break;
        }
    }

    private (int fun, int graphic, int perfection, int genre) GetPresetForDifficulty(int presetID, int difficulty)
    {
        foreach (DifficultyPreset data in m_HeartPresetList)
        {
            if (presetID == data.PresetID && difficulty == data.Difficulty)
            {
                return (data.FunnyHeart, data.GraphicHeart, data.PerfectionHeart, data.GenreHeart);
            }
        }

        return (0, 0, 0, 0);
    }

    private void SetGameShowData(List<GameShowData> monthList, int listIndex, int difficulty)
    {
        int _fun = 0;
        int _graphic = 0;
        int _perfection = 0;
        int _genre = 0;

        var (randomFun, randomGraphic, randomPerfection, randomGenre) = GetPresetForDifficulty(monthList[listIndex].Preset_ID, difficulty);

        _fun = randomFun;
        _graphic = randomGraphic;
        _perfection = randomPerfection;
        _genre = randomGenre;

        monthList[listIndex].GameShow_State["Fun"] = _fun;
        monthList[listIndex].GameShow_State["Perfection"] = _perfection;
        monthList[listIndex].GameShow_State["Graphic"] = _graphic;
        monthList[listIndex].GameShow_State["Genre"] = _genre;
    }
}