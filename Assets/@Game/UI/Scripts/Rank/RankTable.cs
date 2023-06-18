using System;
using System.Collections.Generic;

public class RankTable
{
    public Rank Grade;
    public int MinValue;
    public int MaxValue;

    public RankTable(Rank grade, int minValue, int maxValue)
    {
        Grade = grade;
        MinValue = minValue;
        MaxValue = maxValue;
    }
}

public class ScriptRankCondition
{
    public Rank MyRank;
    public int Condition;
    public bool RepeatRank;

    public ScriptRankCondition(Rank myRank, int condition, bool repeatRank)
    {
        MyRank = myRank;
        Condition = condition;
        RepeatRank = repeatRank;
    }
}

public class ScriptGradeCondition
{
    public int MyGrade;
    public int Condition;
    public bool RepeatGrade;

    public ScriptGradeCondition(int myGrade, int condition, bool repeatRank)
    {
        MyGrade = myGrade;
        Condition = condition;
        RepeatGrade = repeatRank;
    }
}

// ������� ���Ӽ� ���̵��� ���� Ŭ����
public class GameDifficulty
{
    public Rank MyRank;
    public int Difficulty;

    public GameDifficulty(Rank myRank, int difficulty)
    {
        MyRank = myRank;
        Difficulty = difficulty;
    }
}

// ���Ӽ� ���̵��� ���� ��Ʈ ������
public class DifficultyPreset
{
    public int PresetID;
    public int Difficulty;
    public int FunnyHeart;
    public int GraphicHeart;
    public int PerfectionHeart;
    public int GenreHeart;

    public DifficultyPreset(int presetID, int difficulty, int funnyHeart, int graphicHeart, int perfectionHeart, int genreHeart)
    {
        PresetID = presetID;
        Difficulty = difficulty;
        FunnyHeart = funnyHeart;
        GraphicHeart = graphicHeart;
        PerfectionHeart = perfectionHeart;
        GenreHeart = genreHeart;
    }
}

public class SkillSpriteByStatAmount
{
    public int Amount;
    public string SpriteName;

    public SkillSpriteByStatAmount(int amount, string spriteName)
    {
        Amount = amount;
        SpriteName = spriteName;
    }
}

// ��ũ���� �������� ������ �� �� ����� Ŭ����
public class MailSender
{
    public int SenderID;
    public string SenderName;

    public MailSender(int senderID, string senderName)
    {
        SenderID = senderID;
        SenderName = senderName;
    }
}

// ���Ӽ�� ���̵��� �������� ���� �ݾ��� ���� �� ����� Ŭ����
public class GameShowRewardByDifficulty
{
    public Assessment GameShowAssessment;
    public int Level;
    public int RewardByAssessment;
    public int Famous;
    public int StudentPassion;
    public int StudentHealth;

    public GameShowRewardByDifficulty(Assessment _assessment, int _level, int _reward, int _famous, int _passion, int _health)
    {
        GameShowAssessment = _assessment;
        Level = _level;
        RewardByAssessment = _reward;
        Famous = _famous;
        StudentPassion = _passion;
        StudentHealth = _health;
    }
}

// ������ ��Ʈ�� ���� ���� ����
public class GameShowScoreByHeart
{
    public int HeartCount;
    public int Score;
    public int Level;

    public GameShowScoreByHeart(int _heartCount, int _score, int _level)
    {
        HeartCount = _heartCount;
        Score = _score;
        Level = _level;
    }
}

// �� ���� ���� Ŭ����, �� �� ���� ������ ���� ������ �ο����ش�.
public class EvaluationScoreRange
{
    public Assessment GameShowAssessment;
    public int MinValue;
    public int MaxValue;

    public EvaluationScoreRange(Assessment _gameShowAssessment, int _minValue, int _maxValue)
    {
        GameShowAssessment = _gameShowAssessment;
        MinValue = _minValue;
        MaxValue = _maxValue;
    }
}

// ���������� ���� �������� �� ���� ���� ����� ��Ÿ���� Ŭ����
public class FinalScoreRange
{
    public Assessment FinalAssessment;
    public int MinValue;
    public int MaxValue;

    public FinalScoreRange(Assessment _finalAssessment, int _minValue, int _maxValue)
    {
        FinalAssessment = _finalAssessment;
        MinValue = _minValue;
        MaxValue = _maxValue;
    }
}

// ���� ���� �� ������ ���� ����ȯ�� Ŭ����
public class ScoreConversion
{
    public Assessment ScoreAssessment;
    public int Score;

    public ScoreConversion(Assessment scoreAssessment, int score)
    {
        ScoreAssessment = scoreAssessment;
        Score = score;
    }
}

// �� ������ ���� �̹����� ��ũ��Ʈ ����� ���� Ŭ����
public class AssessmentResponse
{
    public Assessment PartAssessment;
    public string Part;
    public string Script;

    public AssessmentResponse(Assessment partAssessment, string part, string script)
    {
        PartAssessment = partAssessment;
        Part = part;
        Script = script;
    }
}

// ������ ����� ���� �����ũ��Ʈ ����ֱ� ���� Ŭ����
// ScriptOrdr : 1 => intro, 2 => main(�⺻ ����), 3 => main(���� ����, ���), 4 => main(���� ����, �׷���), 5 => main(���� ����, �ϼ���), 6 => end
public class ResultScript
{
    public Assessment ResultAssessment;
    public int ScriptOrder;
    public string Script;

    public ResultScript(Assessment resultAssessment, int scriptOrder, string script)
    {
        ResultAssessment = resultAssessment;
        ScriptOrder = scriptOrder;
        Script = script;
    }
}

// �л� ������ ������ ������ �̱����� Ŭ����
public class StudentRandomStatRange
{
    public Rank AcademyRank;
    public int StatMinValue;
    public int StatMaxValue;
    public int GenreMinValue;
    public int GenreMaxValue;

    public StudentRandomStatRange(Rank academyRank, int statMinValue, int statMaxValue, int genreMinValue, int genreMaxValue)
    {
        AcademyRank = academyRank;
        StatMinValue = statMinValue;
        StatMaxValue = statMaxValue;
        GenreMinValue = genreMinValue;
        GenreMaxValue = genreMaxValue;
    }
}

// �л� ������ ������ ������ �ֱ����� Ŭ����
public class StudentPersonality
{
    public int ID;
    public string Name;
    public string Script;

    public StudentPersonality(int id, string name, string script)
    {
        ID = id;
        Name = name;
        Script = script;
    }
}

public class StudentBasicSkills
{
    public int MinStat;
    public int MaxStat;
    public int Level;

    public StudentBasicSkills(int level, int minStat, int maxStat)
    {
        Level = level;
        MinStat = minStat;
        MaxStat = maxStat;
    }
}

// ������ ����� ���� Ŭ����
public class GameJamReward
{
    public int Difficulty;
    public string GameJamRank;
    public int Reward;

    public GameJamReward(int difficulty, string gameJamRank, int reward)
    {
        Difficulty = difficulty;
        GameJamRank = gameJamRank;
        Reward = reward;
    }
}
