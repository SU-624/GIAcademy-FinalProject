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

// 게임잼과 게임쇼 난이도에 쓰일 클래스
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

// 게임쇼 난이도에 따른 하트 프리셋
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

// 랭크에서 마지막에 보상을 줄 때 사용할 클래스
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

// 게임쇼에서 난이도와 평가점수에 따른 금액을 정할 때 사용할 클래스
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

// 레벨과 하트의 수에 따른 점수
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

// 평가 점수 범위 클래스, 이 평가 점수 범위를 토대로 반응을 부여해준다.
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

// 최종적으로 받은 점수들의 총 합을 통해 결과를 나타내줄 클래스
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

// 내가 받은 평가 반응에 대한 점수환산 클래스
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

// 평가 반응에 따른 이미지와 스크립트 출력을 위한 클래스
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

// 마지막 결과에 따른 결과스크립트 띄워주기 위한 클래스
// ScriptOrdr : 1 => intro, 2 => main(기본 반응), 3 => main(스탯 반응, 재미), 4 => main(스탯 반응, 그래픽), 5 => main(스탯 반응, 완성도), 6 => end
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

// 학생 생성시 랜덤한 스탯을 뽑기위한 클래스
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

// 학생 생성시 랜덤한 성격을 주기위한 클래스
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

// 게임잼 상금을 위한 클래스
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
