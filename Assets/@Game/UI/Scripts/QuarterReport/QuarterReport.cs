using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �б⺰�� �п��� ������ �����ִ� ��ũ��Ʈ
/// 
/// 2023.04.17 Ocean
/// </summary>
public class QuarterReport : MonoBehaviour
{
    [SerializeField] private QuarterReportPanel m_QuarterReportPanel;
    [SerializeField] private QuarterSpecificationPanel m_QuarterSpecificationPanel;

    private List<KeyValuePair<string, int>> m_AcademyList = new List<KeyValuePair<string, int>>();               // AI�п���� �� �п��� �������� �ְ� �������ַ��� ���� ��ųʸ�
    private List<KeyValuePair<string, int>> m_prevAcademyRank = new List<KeyValuePair<string, int>>();           // ������ ���
    private List<KeyValuePair<string, int>> m_prevYearAcademyRank = new List<KeyValuePair<string, int>>();       // ������ ���
    private List<KeyValuePair<string, int>> m_AcademyNameToRank = new List<KeyValuePair<string, int>>();         // �������� ������ ����ߴ��� �϶��ߴ��� �ƴϸ� �����ߴ��� �����ϱ� ���� ��ųʸ�

    private bool m_IsQuaterMonth;
    private bool m_Flag;                // ���� �г��� �ѹ��� ����ֱ� ���� �Ұ�
    private int m_NowWeek;               // �б⺸�� �г��� �ѹ��� ����ֱ� ���� �Ұ�
    private string m_Quarter;
    public static int m_MyAcademyQuarterRank = 0;                                                                   // �̺�Ʈ�� ���� �б⿡ �޼��� �� ��ī���� ��ũ

    void Update()
    {
        if ((GameTime.Instance.FlowTime.NowMonth == 6 || GameTime.Instance.FlowTime.NowMonth == 9 ||
            GameTime.Instance.FlowTime.NowMonth == 12 || GameTime.Instance.FlowTime.NowMonth == 3) && GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 5 && !m_IsQuaterMonth)
        {
            if (GameTime.Instance.FlowTime.NowYear == 1 && GameTime.Instance.FlowTime.NowMonth == 3)
            {
                return;
            }
            else if (GameTime.Instance.FlowTime.NowMonth == 6 && MonthlyReporter.Instance.FourthQuarterScore != 0)
            {
                // �б⺰ ������ �ִ� �����̵�� �ʱ�ȭ���ֱ�
                InitQuarterSpecificationPanel();
            }

            if (GameTime.Instance.FlowTime.NowMonth == 6)
            {
                m_Quarter = "1�б�";
            }
            else if (GameTime.Instance.FlowTime.NowMonth == 9)
            {
                m_Quarter = "2�б�";

            }
            else if (GameTime.Instance.FlowTime.NowMonth == 12)
            {
                m_Quarter = "3�б�";

            }
            else if (GameTime.Instance.FlowTime.NowMonth == 3)
            {
                m_Quarter = "4�б�";
            }

            m_QuarterReportPanel.PopUpReportPanel();

            m_QuarterReportPanel.ChangeQuarterName(m_Quarter + " ������ ���Խ��ϴ�.\n���� �ٷ� Ȯ���Ͻ���!");

            m_IsQuaterMonth = true;
        }

        if (m_IsQuaterMonth && !m_Flag)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                m_QuarterReportPanel.ClickPanel();
                m_Flag = true;
                SetQuarterSepcificationPanel();
            }
        }

        if (m_NowWeek != GameTime.Instance.FlowTime.NowWeek)
        {
            m_IsQuaterMonth = false;
            m_Flag = false;
            m_NowWeek = GameTime.Instance.FlowTime.NowWeek;
        }
    }

    // �б⺸�� �гο��� �б� ����, �б⺰ ����, �б���� ���� �������ִ� �Լ�
    private void SetQuarterSepcificationPanel()
    {
        if (GameTime.Instance.FlowTime.NowMonth == 6)
        {
            // 1�б�
            int _caculatePercent = (MonthlyReporter.Instance.FirstQuarterScore / 4995) * 100;

            m_QuarterSpecificationPanel.ChangeQuarterTitle("1");
            m_QuarterSpecificationPanel.Set1stQuarterScore(MonthlyReporter.Instance.FirstQuarterScore.ToString(), _caculatePercent);
            m_QuarterSpecificationPanel.SetQuarterRankTitle("1");
        }
        else if (GameTime.Instance.FlowTime.NowMonth == 9)
        {
            // 2�б�
            int _caculatePercent = (MonthlyReporter.Instance.SecondQuarterScore / 4995) * 100;

            m_QuarterSpecificationPanel.ChangeQuarterTitle("2");
            m_QuarterSpecificationPanel.Set2ndQuarterScore(MonthlyReporter.Instance.SecondQuarterScore.ToString(), _caculatePercent);
            m_QuarterSpecificationPanel.SetQuarterRankTitle("2");
        }
        else if (GameTime.Instance.FlowTime.NowMonth == 12)
        {
            // 3�б�
            int _caculatePercent = (MonthlyReporter.Instance.ThirdQuarterScore / 4995) * 100;

            m_QuarterSpecificationPanel.ChangeQuarterTitle("3");
            m_QuarterSpecificationPanel.Set3rdQuarterScore(MonthlyReporter.Instance.ThirdQuarterScore.ToString(), _caculatePercent);
            m_QuarterSpecificationPanel.SetQuarterRankTitle("3");
        }
        else if (GameTime.Instance.FlowTime.NowMonth == 3)
        {
            // 4�б�
            int _caculatePercent = (MonthlyReporter.Instance.FourthQuarterScore / 4995) * 100;
            m_QuarterSpecificationPanel.ChangeQuarterTitle("4");
            m_QuarterSpecificationPanel.Set4thQuarterScore(MonthlyReporter.Instance.FourthQuarterScore.ToString(), _caculatePercent);
            m_QuarterSpecificationPanel.SetQuarterRankTitle("4");
        }

        string _totalIncome = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.TotalIncome) + "\\";
        string _totalIncomeEventResult = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.IncomeEventResult) + "\\";
        string _totalIncomeSell = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.IncomeSell) + "\\";
        string _totalIncomeActivity = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.IncomeActivity) + "\\";
        string _totalIncomeAcademyFee = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.IncomeAcademyFee) + "\\";

        string _totalExpenses = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.TotalExpenses) + "\\";
        string _totalExpensesEventResult = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.ExpensesEventResult) + "\\";
        string _totalExpensesEventCost = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.ExpensesEventCost) + "\\";
        string _totalExpensesActivity = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.ExpensesActivity) + "\\";
        string _totalExpensesSalary = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.ExpensesSalary) + "\\";
        string _totalExpensesFacility = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.ExpensesFacility) + "\\";
        string _totalExpensesTuitionFee = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.ExpensesTuitionFee) + "\\";
        string _totalNetPtofit = string.Format("{0:#,0}", MonthlyReporter.Instance.m_PrevQuarter.NetProfit) + "\\";

        string _totalGoodsScore = MonthlyReporter.Instance.m_PrevQuarter.GoodsScore.ToString();
        string _totalManagementScore = MonthlyReporter.Instance.m_PrevQuarter.ManagementScore.ToString();
        string _totalFamousScore = MonthlyReporter.Instance.m_PrevQuarter.FamousScore.ToString();
        string _totalActivityScore = MonthlyReporter.Instance.m_PrevQuarter.ActivityScore.ToString();
        string _totalTalentDevelopmentScore = MonthlyReporter.Instance.m_PrevQuarter.TalentDevelopmentScore.ToString();

        CalculateAcademyScore();

        m_QuarterSpecificationPanel.SetTotalIncomeText(_totalIncome, _totalIncomeEventResult, _totalIncomeSell, _totalIncomeActivity, _totalIncomeAcademyFee);
        m_QuarterSpecificationPanel.SetTotalExpensesText(_totalExpenses, _totalExpensesEventResult, _totalExpensesEventCost, _totalExpensesActivity, _totalExpensesSalary, _totalExpensesFacility, _totalExpensesTuitionFee);

        m_QuarterSpecificationPanel.SetNetProfitText(_totalNetPtofit);

        m_QuarterSpecificationPanel.SetPlayScore(_totalGoodsScore, _totalManagementScore, _totalFamousScore, _totalActivityScore, _totalTalentDevelopmentScore);

        QuarterlyReport.Instance.AcademyScoreCal();

        SetQuarterRank();
        QuarterlyReport.Instance.TotalAcademyScores = m_AcademyList.ToList();

        CompareRank(m_prevAcademyRank);
        QuarterlyReport.Instance.AcademyNameToRank = m_AcademyNameToRank.ToList();

        if (m_AcademyList.Count != 0)
        {
            CopyToPrevRank(m_prevAcademyRank);
        }

        if (m_AcademyList.Count != 0 && GameTime.Instance.FlowTime.NowMonth == 3)
        {
            CompareRank(m_prevYearAcademyRank);
            QuarterlyReport.Instance.AcademyNameToRank = m_AcademyNameToRank.ToList();

            CopyToPrevRank(m_prevYearAcademyRank);
            QuarterlyReport.Instance.PrevYearAcademyRank.Clear();
            QuarterlyReport.Instance.PrevYearAcademyRank = m_prevYearAcademyRank.ToList();
        }

        SetQuarterPanel();

        m_AcademyList.Clear();
    }

    // UI�� �����͵��� �־��ش�.
    private void SetQuarterPanel()
    {
        string _1stAcademyName = m_AcademyList[0].Key;
        string _2ndAcademyName = m_AcademyList[1].Key;
        string _3rdAcademyName = m_AcademyList[2].Key;

        string _1stAcademyScore = m_AcademyList[0].Value.ToString();
        string _2ndAcademyScore = m_AcademyList[1].Value.ToString();
        string _3rdAcademyScore = m_AcademyList[2].Value.ToString();

        string _1stAcademyChangeScore = m_AcademyNameToRank[0].Value.ToString();
        string _2ndAcademyChangeScore = m_AcademyNameToRank[1].Value.ToString();
        string _3rdAcademyChangeScore = m_AcademyNameToRank[2].Value.ToString();

        string _myAcademyName = PlayerInfo.Instance.AcademyName;
        int _myScoreIndex = m_AcademyList.FindIndex(x => x.Key == PlayerInfo.Instance.AcademyName);
        string _myAcademyScore = m_AcademyList[_myScoreIndex].Value.ToString();

        int _myChangeScoreIndex = m_AcademyNameToRank.FindIndex(x => x.Key == PlayerInfo.Instance.AcademyName);
        string _myAcademyChangeScore = m_AcademyNameToRank[_myChangeScoreIndex].Value.ToString();

        int _myAcademyIndex = FindIndexToList(m_AcademyList, _myAcademyName);

        string _myAcademyRank;
        string _academyName1;

        string _academyRank1;
        string _academyScore1;
        string _academyChangeScore1;

        string _academyRank2;
        string _academyName2;
        string _academyScore2;
        string _academyChangeScore2;

        if (_myAcademyIndex == 0)
        {
            _myAcademyRank = "1";

            _academyRank1 = "2";
            _academyName1 = m_AcademyList[_myAcademyIndex + 1].Key;
            _academyScore1 = m_AcademyList[_myAcademyIndex + 1].Value.ToString();
            _academyChangeScore1 = m_AcademyNameToRank[_myAcademyIndex + 1].Value.ToString();

            _academyRank2 = "3";
            _academyName2 = m_AcademyList[_myAcademyIndex + 2].Key;
            _academyScore2 = m_AcademyList[_myAcademyIndex + 2].Value.ToString();
            _academyChangeScore2 = m_AcademyNameToRank[_myAcademyIndex + 2].Value.ToString();
        }
        else
        {
            _myAcademyRank = _myAcademyIndex.ToString();

            _academyName1 = m_AcademyList[_myAcademyIndex - 1].Key;
            _academyRank1 = FindIndexToList(m_AcademyList, _academyName1).ToString();
            _academyScore1 = m_AcademyList[_myAcademyIndex - 1].Value.ToString();
            _academyChangeScore1 = m_AcademyNameToRank[_myAcademyIndex - 1].Value.ToString();

            _academyName2 = m_AcademyList[_myAcademyIndex + 1].Key;
            _academyRank2 = FindIndexToList(m_AcademyList, _academyName2).ToString();
            _academyScore2 = m_AcademyList[_myAcademyIndex + 1].Value.ToString();
            _academyChangeScore2 = m_AcademyNameToRank[_myAcademyIndex + 1].Value.ToString();
        }


        m_MyAcademyQuarterRank = _myAcademyIndex;

        // 1���� �п��� ������ �־��ش�.
        m_QuarterSpecificationPanel.SetQuarter1stAcademyInfo(_1stAcademyName, _1stAcademyScore);
        //  1~3���� �̸��� ����, ��ŷ��ȭ�� �־��ش�.
        m_QuarterSpecificationPanel.SetAcademysPanel(_1stAcademyName, _1stAcademyScore, _1stAcademyChangeScore, _2ndAcademyName, _2ndAcademyScore, _2ndAcademyChangeScore
            , _3rdAcademyName, _3rdAcademyScore, _3rdAcademyChangeScore);
        // �� ��ī���̰� 1���̸� ������ ���� ��ī���� 2���� ������, �ƴ϶�� ������ ���� ������ ���� ��ī���̸� �Ѱ��� �����ش�.
        m_QuarterSpecificationPanel.SetMyAcademyRankPalce(_academyName1, _academyScore1, _academyRank1, _academyChangeScore1, _myAcademyName, _myAcademyScore, _myAcademyRank, _myAcademyChangeScore,
            _academyName2, _academyScore2, _academyRank2, _academyChangeScore2);

        if (_1stAcademyName == _myAcademyName)
        {
            m_QuarterSpecificationPanel.MyAcademyRankPlace.transform.SetSiblingIndex(0);
            // ��� ��ī������ ������ ȭ��ǥ ǥ��
            m_QuarterSpecificationPanel.SetIncreaseImage(m_AcademyNameToRank[0].Value, m_AcademyNameToRank[1].Value, m_AcademyNameToRank[2].Value, m_AcademyNameToRank[_myChangeScoreIndex].Value,
                m_AcademyNameToRank[_myAcademyIndex + 1].Value, m_AcademyNameToRank[_myAcademyIndex + 2].Value);
        }
        else
        {
            m_QuarterSpecificationPanel.MyAcademyRankPlace.transform.SetSiblingIndex(1);
            m_QuarterSpecificationPanel.SetIncreaseImage(m_AcademyNameToRank[0].Value, m_AcademyNameToRank[1].Value, m_AcademyNameToRank[2].Value, m_AcademyNameToRank[_myChangeScoreIndex].Value,
                m_AcademyNameToRank[_myAcademyIndex - 1].Value, m_AcademyNameToRank[_myAcademyIndex + 1].Value);
        }
    }

    private int FindIndexToList(List<KeyValuePair<string, int>> _academyList, string _name)
    {
        int _index = 0;

        for (int i = 0; i < _academyList.Count; i++)
        {
            if (m_AcademyList[i].Key == _name)
            {
                _index = i;
                break;
            }
        }

        return _index;
    }

    // 1�⾿ ���� �� ���� �б� ������ �ʱ�ȭ ��������Ѵ�.
    private void InitQuarterSpecificationPanel()
    {
        m_QuarterSpecificationPanel.Set2ndQuarterScore("0", 0);
        m_QuarterSpecificationPanel.Set3rdQuarterScore("0", 0);
        m_QuarterSpecificationPanel.Set4thQuarterScore("0", 0);
    }

    // AI�� �� ��ī������ ������ ������ ���ĸ� ���� �� �б������ ������ �־��ش�.
    private void SetQuarterRank()
    {
        int _score = MonthlyReporter.Instance.m_PrevQuarter.GoodsScore + MonthlyReporter.Instance.m_PrevQuarter.ManagementScore + MonthlyReporter.Instance.m_PrevQuarter.FamousScore +
        MonthlyReporter.Instance.m_PrevQuarter.ActivityScore + MonthlyReporter.Instance.m_PrevQuarter.TalentDevelopmentScore;
        m_AcademyList = QuarterlyReport.Instance.NowAcademyScore;

        // �̹� �б⿡ �´� ���� �־��ֱ�
        m_AcademyList.Add(new KeyValuePair<string, int>(PlayerInfo.Instance.AcademyName, _score));

        m_AcademyList.Sort((x, y) =>
        {
            if (x.Value > y.Value) return -1;
            if (x.Value < y.Value) return 1;
            else return 0;
        });
        //m_AcademyList.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        int _count = m_AcademyList.Count;

        // �� ���� �п��� �� �п��̶�� �� �������� ���� �� ���� �п��� ���� �־��ش�.
        if (m_AcademyList[_count - 1].Key == PlayerInfo.Instance.AcademyName)
        {
            m_AcademyList.Add(new KeyValuePair<string, int>("�����ī����", (int)(_score * 0.7)));
        }
    }

    // �� ��ī������ ������ �������ش�.
    private void CalculateAcademyScore()
    {
        int _score = MonthlyReporter.Instance.m_PrevQuarter.GoodsScore + MonthlyReporter.Instance.m_PrevQuarter.ManagementScore + MonthlyReporter.Instance.m_PrevQuarter.FamousScore +
            MonthlyReporter.Instance.m_PrevQuarter.ActivityScore + MonthlyReporter.Instance.m_PrevQuarter.TalentDevelopmentScore;

        PlayerInfo.Instance.AcademyScore += _score;
    }

    // ���� ��ŷ�� �����ص� �Լ� m_prevAcademyRank ���⿡ 1����� �־��ش�.
    private void CopyToPrevRank(List<KeyValuePair<string, int>> _list)
    {
        _list.Clear();

        int rank = 1;

        foreach (var academy in m_AcademyList)
        {
            _list.Add(new KeyValuePair<string, int>(academy.Key, rank));
            rank++;
        }
    }

    // ������ ���� ���ʴ�� ������ ���ش�.
    private void CompareRank(List<KeyValuePair<string, int>> _list)
    {
        if (_list.Count == 0)
        {
            int rank = 1;

            foreach (var academy in m_AcademyList)
            {
                m_AcademyNameToRank.Add(new KeyValuePair<string, int>(academy.Key, 0));
                rank++;
            }
        }
        else
        {
            int rank = 1;

            m_AcademyNameToRank.Clear();
            foreach (var academy in m_AcademyList)
            {
                int _index = _list.FindIndex(x => x.Key == academy.Key);
                int prevRank = _list[_index].Value;

                int ChangeRank = 0;

                ChangeRank = prevRank - rank;
                m_AcademyNameToRank.Add(new KeyValuePair<string, int>(academy.Key, ChangeRank));

                rank++;
            }
        }
    }

    // UI�� ����� ���Ŀ� �ѹ� ����ش�.
    private void InitList()
    {
        m_AcademyList.Clear();
        m_AcademyNameToRank.Clear();
    }
}
