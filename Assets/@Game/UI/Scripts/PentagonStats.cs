using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonStats
{
    public static int STAT_MIN = 1;
    public static int STAT_MAX = 100;

    private SingleStat m_Sense;
    private SingleStat m_Concentration;
    private SingleStat m_Wit;
    private SingleStat m_Techinque;
    private SingleStat m_Insight;

    public PentagonStats(int _sense, int _concentration, int _wit, int _techinque, int _insight)
    {
        m_Sense = new SingleStat(_sense);
        m_Concentration = new SingleStat(_concentration);
        m_Wit = new SingleStat(_wit);
        m_Techinque = new SingleStat(_techinque);
        m_Insight = new SingleStat(_insight);
    }

    private SingleStat GetSingleStat(AbilityType abilityType)
    {
        switch (abilityType)
        {
            default:
            case AbilityType.Sense: return m_Sense;
            case AbilityType.Concentration: return m_Concentration;
            case AbilityType.Wit: return m_Wit;
            case AbilityType.Technique: return m_Techinque;
            case AbilityType.Insight: return m_Insight;
        }
    }

    public void SetStatAmount(AbilityType abilityType, int _statAmount)
    {
        GetSingleStat(abilityType).SetStatAmount(_statAmount);
    }

    public float GetStatAmount(AbilityType abilityType)
    {
        return GetSingleStat(abilityType).GetStatAmount();
    }

    public float GetStatAmountNormalized(AbilityType abilityType)
    {
        return GetSingleStat(abilityType).GetStatAmountNormalized();
    }

    private class SingleStat
    {
        private int m_Stat;

        public SingleStat(int _statAmount)
        {
            SetStatAmount(_statAmount);
        }

        public void SetStatAmount(int _Stat)
        {
            m_Stat = Mathf.Clamp(_Stat, STAT_MIN, STAT_MAX);
        }

        public float GetStatAmount()
        {
            return m_Stat;
        }

        public float GetStatAmountNormalized()
        {

            Debug.Log("Stat : " + m_Stat);
            Debug.Log("STAT_MAX : " + STAT_MAX);

            return (float)m_Stat / (float)STAT_MAX;
        }

    }
}
