using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 학생들의 보너스 스킬을 넣어주는 스크립트
/// 스킬을 획득할 수 있는 조건들의 데이터들이 
/// 변화하는곳에 delegate로 이벤트를 등록해줄 예정
/// 모든 스킬에는 체력과 열정을 함께 판별해주기
/// 
/// 2023.06.04 OceanHouse 마지막 방학날
/// </summary>

// 학생들의 보너스 스킬들을 판별해주는 스크립트.
// 각 보너스 스킬들의 조건에 해당하는 데이터들의 변화가 생길 때 마다 체크를 해준다.
public class StudentSkills : MonoBehaviour
{
    private List<BonusSkillConditions> m_GameJamSkillConditionList = new List<BonusSkillConditions>();
    private List<BonusSkillConditions> m_GameShowSkillConditionList = new List<BonusSkillConditions>();
    private List<BonusSkillConditions> m_ClassSkillConditionList = new List<BonusSkillConditions>();
    private List<BonusSkillConditions> m_IntimacySkillConditionList = new List<BonusSkillConditions>();
    private List<BonusSkillConditions> m_CommonSkillConditionList = new List<BonusSkillConditions>();

    private void OnEnable()
    {
        GameJam.SkillConditionDataChangedEvent += CheckGameJamSkillConditions;
        GameShow.GameShowDataChangeEvent += CheckGameShowSkillConditions;
        InGameTest.StudentDataChangedEvent += CheckClassSkillConditions;
        Interaction.FriendShipChangedEvent += CheckIntimacySkillConditios;
    }

    private void OnDisable()
    {
        GameJam.SkillConditionDataChangedEvent -= CheckGameJamSkillConditions;
        GameShow.GameShowDataChangeEvent -= CheckGameShowSkillConditions;
        InGameTest.StudentDataChangedEvent -= CheckClassSkillConditions;
        Interaction.FriendShipChangedEvent -= CheckIntimacySkillConditios;
    }

    private void Start()
    {
        InitConditionLists();
    }

    private void InitConditionLists()
    {
        foreach (BonusSkillConditions _conditionList in AllOriginalJsonData.Instance.OriginalBonusSkillConditionData)
        {
            if (_conditionList.ClassID != 0)
            {
                m_ClassSkillConditionList.Add(_conditionList);
            }
            else if (_conditionList.GameGenre_ID != "0" || _conditionList.GameJameLevel[0] != "0" || _conditionList.GameConcept_ID != "0" || _conditionList.GamedevelopmentNumber != 0)
            {
                m_GameJamSkillConditionList.Add(_conditionList);
            }
            else if (_conditionList.GameShow_Level[0] != 0)
            {
                m_GameShowSkillConditionList.Add(_conditionList);
            }
            else if (_conditionList.Intimacy != "0")
            {
                m_IntimacySkillConditionList.Add(_conditionList);
            }
            else
            {
                m_CommonSkillConditionList.Add(_conditionList);
            }
        }
    }

    // 스킬의 스크립트 아이디를 넣으면 대사를 반환해주는 함수
    public static string FindSkillScript(int _scriptID)
    {
        List<BonusSkillScript> _skillScript = new List<BonusSkillScript>(AllOriginalJsonData.Instance.OriginalBonusSkillScriptData);

        foreach (BonusSkillScript scriptList in _skillScript)
        {
            if (scriptList.Ability_Script_ID == _scriptID)
            {
                return scriptList.Ability_Script;
            }
        }

        return "";
    }

    // 스킬 아이디로 이름을 찾는 함수
    public static string FindSkillName(int _skillID)
    {
        List<BonusSkillConditions> _skillCondition = new List<BonusSkillConditions>(AllOriginalJsonData.Instance.OriginalBonusSkillConditionData);

        foreach (BonusSkillConditions skillList in _skillCondition)
        {
            if (skillList.Ability_ID == _skillID)
            {
                return skillList.Ability_Name;
            }
        }

        return "";
    }

    // 게임잼에 참가 할 때 마다 체크를 해준다.
    private void CheckGameJamSkillConditions()
    {
        foreach (BonusSkillConditions _condition in m_GameJamSkillConditionList)
        {
            int _developConceptCount = 0;
            int _developGenreCount = 0;

            bool _isLevelCondition = true;
            bool _isEntryCountCondition = true;

            bool _isHealthSatisfy = FindHealthOrPassionValue(_condition.Health);
            bool _isPassionSatisfy = FindHealthOrPassionValue(_condition.Passion);
            bool _isGenreIDSatisfy = _condition.GameGenre_ID != "0";
            bool _isConceptIDSatisfy = _condition.GameConcept_ID != "0";
            bool _isGameJamLevelSatisfy = _condition.GameJameLevel[0] != "0";
            bool _isGameJamEntrySatisfy = _condition.GamedevelopmentNumber != 0;

            StudentType studentType = _condition.StudentPart_ID == 0 ? StudentType.None : _condition.StudentPart_ID == 1 ? StudentType.GameDesigner : _condition.StudentPart_ID == 2 ? StudentType.Art : StudentType.Programming;

            foreach (Student _studentList in ObjectManager.Instance.m_StudentList)
            {
                if (studentType == _studentList.m_StudentStat.m_StudentType || studentType == StudentType.None)
                {
                    if (_isGenreIDSatisfy)
                    {
                        _developGenreCount = CompareGameJamDataToCondition(0, _condition.GameGenre_ID, _studentList);
                    }

                    if (_isConceptIDSatisfy)
                    {
                        _developConceptCount = CompareGameJamDataToCondition(1, _condition.GameConcept_ID, _studentList);
                    }

                    if (_isGameJamLevelSatisfy)
                    {
                        _isLevelCondition = CompareGameJamLevelToCondition(_condition.GameJameLevel, _studentList);
                    }

                    if (_isGameJamEntrySatisfy)
                    {
                        _isEntryCountCondition = CompareGameJamEntryCountToCondition(_condition.GamedevelopmentNumber, _studentList);
                    }

                    if (_developConceptCount == _condition.GameConcept_DPNumber && _developGenreCount == _condition.GameGenre_DPNumber && _isLevelCondition && _isEntryCountCondition)
                    {
                        if (!_studentList.m_StudentStat.m_Skills.Contains(_condition.Ability_Name))
                        {
                            _studentList.m_StudentStat.m_Skills.Add(_condition.Ability_Name);
                        }
                    }
                }
            }
        }
    }

    // 게임쇼에 참가 할 때 마다 체크를 해준다.
    private void CheckGameShowSkillConditions()
    {
        foreach (BonusSkillConditions _condition in m_GameShowSkillConditionList)
        {
            foreach (Student _student in ObjectManager.Instance.m_StudentList)
            {
                if (CompareGameShowLevelToCondition(_student, _condition.GameShow_Level))
                {
                    if (!_student.m_StudentStat.m_Skills.Contains(_condition.Ability_Name))
                    {
                        _student.m_StudentStat.m_Skills.Add(_condition.Ability_Name);
                    }
                }
            }
        }
    }

    // 수업은 체력, 열정, 장르, 스탯, 수업 아이디를 몇 번 들었는지 판별해줘야한다.
    private void CheckClassSkillConditions()
    {
        int[] m_State = new int[5];
        int[] m_Genre = new int[8];
        bool _isHealth = true;
        bool _isPassion = true;
        bool _isStat = true;
        bool _isGenre = true;
        bool _isClass = true;

        foreach (BonusSkillConditions _classCondition in m_ClassSkillConditionList)
        {
            bool _isStatSatisfy = FindStateOrGenreValue(m_State, _classCondition.State);
            bool _isGenreSatisfy = FindStateOrGenreValue(m_Genre, _classCondition.Genre);
            bool _isHealthSatisfy = FindHealthOrPassionValue(_classCondition.Health);
            bool _isPassionSatisfy = FindHealthOrPassionValue(_classCondition.Passion);
            bool _isClassSatisfy = _classCondition.ClassID != 0;

            foreach (Student _studentList in ObjectManager.Instance.m_StudentList)
            {
                if (_isGenreSatisfy && !_isStatSatisfy)
                {
                    _isGenre = CompareStateValueToCondition(m_Genre, _studentList.m_StudentStat.m_GenreAmountArr);
                }
                else if (!_isGenreSatisfy && _isStatSatisfy)
                {
                    _isStat = CompareStateValueToCondition(m_State, _studentList.m_StudentStat.m_AbilityAmountArr);
                }
                else if (_isGenreSatisfy && _isStatSatisfy)
                {
                    _isGenre = CompareStateValueToCondition(m_Genre, _studentList.m_StudentStat.m_GenreAmountArr);
                    _isStat = CompareStateValueToCondition(m_State, _studentList.m_StudentStat.m_AbilityAmountArr);
                }

                if (_isHealthSatisfy && !_isPassionSatisfy)
                {
                    _isHealth = CompareHealthOrPassionToCondition(_studentList.m_StudentStat.m_Health, _classCondition.Health);
                }
                else if (!_isHealthSatisfy && _isPassionSatisfy)
                {
                    _isPassion = CompareHealthOrPassionToCondition(_studentList.m_StudentStat.m_Passion, _classCondition.Passion);
                }
                else if (_isHealthSatisfy && _isPassionSatisfy)
                {
                    _isHealth = CompareHealthOrPassionToCondition(_studentList.m_StudentStat.m_Health, _classCondition.Health);
                    _isPassion = CompareHealthOrPassionToCondition(_studentList.m_StudentStat.m_Passion, _classCondition.Passion);
                }

                if (_isClassSatisfy)
                {
                    switch (_studentList.m_StudentStat.m_StudentType)
                    {
                        case StudentType.GameDesigner:
                        _isClass = FindLessonClassIDCount(_classCondition.ClassID, _classCondition.Numberoflessons, SelectClass.m_GameDesignerData);
                        break;
                        case StudentType.Art:
                        _isClass = FindLessonClassIDCount(_classCondition.ClassID, _classCondition.Numberoflessons, SelectClass.m_ArtData);
                        break;
                        case StudentType.Programming:
                        _isClass = FindLessonClassIDCount(_classCondition.ClassID, _classCondition.Numberoflessons, SelectClass.m_ProgrammingData);
                        break;
                    }
                }

                if (_isClass && _isGenre && _isStat && _isHealth && _isPassion && !_studentList.m_StudentStat.m_Skills.Contains(_classCondition.Ability_Name))
                {
                    _studentList.m_StudentStat.m_Skills.Add(_classCondition.Ability_Name);
                }
            }
        }
    }

    // 모든 학생들을 돌면서 그 학생의 친밀도를 조건에 맞는지 확인을 해줘야한다.
    private void CheckIntimacySkillConditios()
    {
        foreach (Student _student in ObjectManager.Instance.m_StudentList)
        {
            FindStudentIntimacyToCondition(_student);
        }
    }

    // 체력과 열정, 기본스탯과 장르들이 변할 때 마다 체크해준다.
    private void CheckCommonSkillConditios()
    {
        int[] m_State = new int[5];
        int[] m_Genre = new int[8];

        bool _isStatSatisfy = false;
        bool _isGenreSatisfy = false;
        bool _isHealthSatisfy = false;
        bool _isPassionSatisfy = false;

        bool _isSatisfy = false;

        foreach (BonusSkillConditions _classCondition in m_ClassSkillConditionList)
        {
            _isStatSatisfy = FindStateOrGenreValue(m_State, _classCondition.State);
            _isGenreSatisfy = FindStateOrGenreValue(m_Genre, _classCondition.Genre);
            _isHealthSatisfy = FindHealthOrPassionValue(_classCondition.Health);
            _isPassionSatisfy = FindHealthOrPassionValue(_classCondition.Passion);

            foreach (Student _studentList in ObjectManager.Instance.m_StudentList)
            {
                // 장르만 조건이 있을 경우
                if (_isGenreSatisfy && !_isStatSatisfy)
                {
                    _isSatisfy = CompareStateValueToCondition(m_Genre, _studentList.m_StudentStat.m_GenreAmountArr);
                }
                // 스탯만 조건이 있을 경우
                else if (!_isGenreSatisfy && _isStatSatisfy)
                {
                    _isSatisfy = CompareStateValueToCondition(m_State, _studentList.m_StudentStat.m_AbilityAmountArr);
                }
                // 둘 다 있을 경우
                else if (_isGenreSatisfy && _isStatSatisfy)
                {
                    _isSatisfy = CompareStateValueToCondition(m_Genre, _studentList.m_StudentStat.m_GenreAmountArr);
                    _isSatisfy = CompareStateValueToCondition(m_State, _studentList.m_StudentStat.m_AbilityAmountArr);
                }

                // 체력만 있는 경우
                if (_isHealthSatisfy && !_isPassionSatisfy)
                {
                    _isSatisfy = CompareHealthOrPassionToCondition(_studentList.m_StudentStat.m_Health, _classCondition.Health);
                }
                // 열정만 있는 경우
                else if (!_isHealthSatisfy && _isPassionSatisfy)
                {
                    _isSatisfy = CompareHealthOrPassionToCondition(_studentList.m_StudentStat.m_Passion, _classCondition.Passion);
                }
                else if (_isHealthSatisfy && _isPassionSatisfy)
                {
                    _isSatisfy = CompareHealthOrPassionToCondition(_studentList.m_StudentStat.m_Health, _classCondition.Health);
                    _isSatisfy = CompareHealthOrPassionToCondition(_studentList.m_StudentStat.m_Passion, _classCondition.Passion);
                }

                // 조건을 다 거쳤는데도 m_IsSatisfy가 true이면 스킬 획득
                if (_isSatisfy)
                {
                    if (!_studentList.m_StudentStat.m_Skills.Contains(_classCondition.Ability_Name))
                    {
                        _studentList.m_StudentStat.m_Skills.Add(_classCondition.Ability_Name);
                    }
                }
            }
        }
    }

    // 값이 하나라도 있으면 조건이 있는거니 판별을 위해 비교를 해줘야한다.
    private bool FindStateOrGenreValue(int[] arry, int[] conditionState)
    {
        for (int i = 0; i < arry.Length; i++)
        {
            if (conditionState[i] != 0)
            {
                arry[i] = conditionState[i];
                return true;
            }
        }

        return false;
    }

    // 값이 하나라도 있으면 조건이 있는거니 판별을 위해 비교를 해줘야한다.
    private bool FindHealthOrPassionValue(int _healthOrPassion)
    {
        if (_healthOrPassion != 0)
        {
            return true;
        }

        return false;
    }

    // 학생들의 친밀도를 다 돌면서 해당 조건에 맞는 친밀도를 갖고있는 대상이 얼마나 있는지 확인한다.
    private void FindStudentIntimacyToCondition(Student _student)
    {
        int _studentIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(_student));
        List<int> _studentFrendShipList = new List<int>(ObjectManager.Instance.m_Friendship[_studentIndex]);

        int _friendCount = 0;

        foreach (BonusSkillConditions _condition in m_IntimacySkillConditionList)
        {
            foreach (int _friendship in _studentFrendShipList)
            {
                int _friendshipValue = _friendship;
                string _studentFriendshipLevel = GetFriendshipLevel(_friendshipValue);

                if (CompareIntimacyToCondition(_studentFriendshipLevel, _condition.Intimacy))
                {
                    _friendCount++;
                }
            }

            bool _isSatisfy = _friendCount == _condition.Intimacy_Number;

            if (_isSatisfy && !_student.m_StudentStat.m_Skills.Contains(_condition.Ability_Name))
            {
                _student.m_StudentStat.m_Skills.Add(_condition.Ability_Name);
            }
        }
    }

    // 수업 아이디로 학생들이 그 수업을 몇 번 들었는지 확인해주는 함수.
    private bool FindLessonClassIDCount(int classID, int classNumberOfLesson, List<SaveSelectClassInfoData> classData)
    {
        int classNumberoflessonsCount = 0;

        switch (classData[0].m_SelectProfessorDataSave.m_ProfessorType)
        {
            case StudentType.GameDesigner:
            {
                if (PlayerInfo.Instance.GameDesignerClassDic.ContainsKey(classID))
                {
                    classNumberoflessonsCount = PlayerInfo.Instance.GameDesignerClassDic[classID];
                }
                else
                {
                    classNumberoflessonsCount = -1;
                }
            }
            break;

            case StudentType.Art:
            {
                if (PlayerInfo.Instance.ArtClassDic.ContainsKey(classID))
                {
                    classNumberoflessonsCount = PlayerInfo.Instance.ArtClassDic[classID];
                }
                else
                {
                    classNumberoflessonsCount = -1;
                }
            }
            break;

            case StudentType.Programming:
            {
                if (PlayerInfo.Instance.ProgrammingClassDic.ContainsKey(classID))
                {
                    classNumberoflessonsCount = PlayerInfo.Instance.ProgrammingClassDic[classID];
                }
                else
                {
                    classNumberoflessonsCount = -1;
                }
            }
            break;
        }

        return classNumberOfLesson == classNumberoflessonsCount;
    }

    // 학생 장르, 스탯 값과 스킬 조건의 장르, 스탯 값이 같은지 비교해주는 함수
    private bool CompareStateValueToCondition(int[] array, int[] studentState)
    {
        bool _isTrue = false;
        for (int i = 0; i < 5; i++)
        {
            if (array[i] >= studentState[i])
            {
                _isTrue = true;
            }
            else
            {
                _isTrue = false;
                break;
            }
        }
        return _isTrue;
    }

    // 학생 체력, 열정 값과 스킬 조건의 체력, 열정 값이 같은지 비교해주는 함수
    private bool CompareHealthOrPassionToCondition(int studentValue, int conditionValue)
    {
        return studentValue >= conditionValue;
    }

    // 친밀도가 조건과 맞으면 값을 올려준다.
    private bool CompareIntimacyToCondition(string _studentFriendshipLevel, string _conditionFriendshipLevel)
    {
        return _studentFriendshipLevel == _conditionFriendshipLevel;
    }

    // 장르와 컨셉에 맞는 게임을 조건에 맞게 만들었는지 확인해주는 함수. (0 => 장르, 1 => 컨셉)
    private int CompareGameJamDataToCondition(int _genreOrConcept, string _gameJamSatisfy, Student _entryStudent)
    {
        int _count = 0;

        foreach (var keyValue in GameJam.m_GameJamHistory)
        {
            foreach (var gameJamData in keyValue.Value)
            {
                GameJamInfo _nowData = GameJam.SearchAllGameJamInfo(keyValue.Key);

                Student _gameDesigner = ObjectManager.Instance.SearchStudentInfo(gameJamData.m_GameDesignerStudentName);
                Student _art= ObjectManager.Instance.SearchStudentInfo(gameJamData.m_ArtStudentName);
                Student _programming= ObjectManager.Instance.SearchStudentInfo(gameJamData.m_ProgrammingStudentName);

                string _condtionValue = _genreOrConcept == 0 ? gameJamData.m_Genre : _nowData.m_GjamConcept;

                if (_condtionValue == _gameJamSatisfy)
                {
                    switch (_entryStudent.m_StudentStat.m_StudentType)
                    {
                        case StudentType.GameDesigner:
                        if (_gameDesigner == _entryStudent)
                        {
                            _count++;
                        }
                        break;

                        case StudentType.Art:
                        if (_art == _entryStudent)
                        {
                            _count++;
                        }
                        break;

                        case StudentType.Programming:
                        if (_programming == _entryStudent)
                        {
                            _count++;
                        }
                        break;
                    }
                }
            }
        }

        return _count;
    }

    // 조건과 같은 등급의 게임을 만든적이 있나 확인해주는 함수.
    private bool CompareGameJamLevelToCondition(string[] _level, Student _entryStudent)
    {
        for (int i = 0; i < GameJam.m_GameJamHistory.Count; i++)
        {
            for (int j = 0; j < GameJam.m_GameJamHistory.ElementAt(i).Value.Count; j++)
            {
                Student _gameDesigner = ObjectManager.Instance.SearchStudentInfo(GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameDesignerStudentName);
                Student _art = ObjectManager.Instance.SearchStudentInfo(GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_ArtStudentName);
                Student _programming = ObjectManager.Instance.SearchStudentInfo(GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_ProgrammingStudentName);

                switch (_entryStudent.m_StudentStat.m_StudentType)
                {
                    case StudentType.GameDesigner:
                    {
                        foreach (string _levelName in _level)
                        {
                            if (GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_Rank == _levelName && _gameDesigner == _entryStudent)
                            {
                                return true;
                            }
                        }
                    }
                    break;

                    case StudentType.Art:
                    {
                        foreach (string _levelName in _level)
                        {
                            if (GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_Rank == _levelName && _art == _entryStudent)
                            {
                                return true;
                            }
                        }
                    }
                    break;

                    case StudentType.Programming:
                    {
                        foreach (string _levelName in _level)
                        {
                            if (GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_Rank == _levelName && _programming == _entryStudent)
                            {
                                return true;
                            }
                        }
                    }
                    break;
                }
            }
        }

        return false;
    }

    // 
    private bool CompareGameJamEntryCountToCondition(int _entryCount, Student _entryStudent)
    {
        int _count = 0;

        for (int i = 0; i < GameJam.m_GameJamHistory.Count; i++)
        {
            for (int j = 0; j < GameJam.m_GameJamHistory.ElementAt(i).Value.Count; j++)
            {
                Student _gameDesigner = ObjectManager.Instance.SearchStudentInfo(GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameDesignerStudentName);
                Student _art = ObjectManager.Instance.SearchStudentInfo(GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_ArtStudentName);
                Student _programming = ObjectManager.Instance.SearchStudentInfo(GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_ProgrammingStudentName);

                switch (_entryStudent.m_StudentStat.m_StudentType)
                {
                    case StudentType.GameDesigner:
                    {
                        if (_gameDesigner == _entryStudent)
                        {
                            _count++;
                        }
                    }
                    break;

                    case StudentType.Art:
                    {
                        if (_art == _entryStudent)
                        {
                            _count++;
                        }
                    }
                    break;

                    case StudentType.Programming:
                    {
                        if (_programming == _entryStudent)
                        {
                            _count++;
                        }
                    }
                    break;
                }
            }
        }

        return _entryCount == _count ? true : false;
    }

    // 조건과 같은 레벨의 게임쇼에서 좋음 이상을 받았는지 확인해주는 함수
    private bool CompareGameShowLevelToCondition(Student _entryStudent, int[] _level)
    {
        foreach (var gameShow in GameShow.m_GameShowHistory)
        {
            foreach (var showData in gameShow.Value)
            {
                int participatingIndex = (int)_entryStudent.m_StudentStat.m_StudentType;

                switch (_entryStudent.m_StudentStat.m_StudentType)
                {
                    case StudentType.GameDesigner:
                    Student _gamedesigner = ObjectManager.Instance.SearchStudentInfo(showData.m_GameDesignerStudentName);
                    if (_gamedesigner == _entryStudent)
                        participatingIndex = 0;
                    break;

                    case StudentType.Art:
                    Student _art = ObjectManager.Instance.SearchStudentInfo(showData.m_GameDesignerStudentName);
                    if (_art == _entryStudent)
                        participatingIndex = 1;
                    break;

                    case StudentType.Programming:
                    Student _programming = ObjectManager.Instance.SearchStudentInfo(showData.m_GameDesignerStudentName);
                    if (_programming == _entryStudent)
                        participatingIndex = 2;
                    break;
                }

                int resultAssessment = (int)showData.m_GameShowResultAssessment;

                foreach (int levelName in _level)
                {
                    if (resultAssessment >= levelName)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private string GetFriendshipLevel(int _friendship)
    {
        if (_friendship < 150)
        {
            return "아는사이";
        }
        else if (_friendship < 300)
        {
            return "친한사이";
        }
        else
        {
            return "베스트프렌드";
        }
    }
}

