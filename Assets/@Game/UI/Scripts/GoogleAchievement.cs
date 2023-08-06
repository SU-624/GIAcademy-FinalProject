using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoogleAchievement : MonoBehaviour
{
    [SerializeField] private GPGSBinder GpgsBinder;

    private bool[] _achievementCheck;
    float _timeSpan; //경과 시간을 갖는 변수
    float _checkTime; // 특정 시간을 갖는 변수

    // Start is called before the first frame update
    void Start()
    {
        _achievementCheck = new bool[19];
        _achievementCheck = Enumerable.Repeat(false, 19).ToArray(); // 배열을 false로 19칸 초기화
        _timeSpan = 0.0f;
        _checkTime = 5.0f; // 특정시간을 5초로 지정
    }

    // Update is called once per frame
    void Update()
    {
        _timeSpan += Time.deltaTime; // 경과 시간을 계속 등록
        if (_timeSpan > _checkTime) // 경과 시간이 특정 시간이 보다 커졋을 경우
        {
            if (Social.localUser.authenticated)
            {
                if (_achievementCheck[1] == false && PlayerInfo.Instance.IsFirstAcademySetting == true)
                {
                    UnlockAchievement(1);
                    _achievementCheck[1] = true;
                }
                else if (_achievementCheck[2] == false && PlayerInfo.Instance.LuckyBox != 0)
                {
                    UnlockAchievement(2);
                    _achievementCheck[2] = true;
                }
                else if (_achievementCheck[3] == false && PlayerInfo.Instance.IsFirstAcademySetting)
                {
                    UnlockAchievement(3);
                    _achievementCheck[3] = true;
                }
                else if (_achievementCheck[4] == false && PlayerInfo.Instance.ProfessorUpgrade != 0)
                {
                    UnlockAchievement(4);
                    _achievementCheck[4] = true;
                }
                else if (_achievementCheck[5] == false && PlayerInfo.Instance.IsFirstGameJam == false)
                {
                    UnlockAchievement(5);
                    _achievementCheck[5] = true;
                }
                else if (_achievementCheck[6] == false && PlayerInfo.Instance.GameJamRankAUP != 0) // A 랭크 게임 제작 정보 찾는거 알아보기
                {
                    UnlockAchievement(6);
                    _achievementCheck[6] = true;
                }
                else if (_achievementCheck[7] == false && PlayerInfo.Instance.IsFirstGameShow == false)
                {
                    UnlockAchievement(7);
                    _achievementCheck[7] = true;
                }
                else if (_achievementCheck[8] == false && PlayerInfo.Instance.IsGameJamMiniGameFirst == false)
                {
                    UnlockAchievement(8);
                    _achievementCheck[8] = true;
                }
                else if (_achievementCheck[9] == false && PlayerInfo.Instance.IsFirstInJaeRecommend == false)
                {
                    UnlockAchievement(9);
                    _achievementCheck[9] = true;
                }
                else if (_achievementCheck[10] == false && GameTime.Instance.FlowTime.NowYear >= 2
                                                        && GameTime.Instance.FlowTime.NowMonth >= 3)
                {
                    UnlockAchievement(10);
                    _achievementCheck[10] = true;
                }
                else if (_achievementCheck[11] == false && PlayerInfo.Instance.ActionCenterCount >= 12)
                {
                    UnlockAchievement(11);
                    _achievementCheck[11] = true;
                }
                else if (_achievementCheck[12] == false && PlayerInfo.Instance.SimulationCenterCount >= 12)
                {
                    UnlockAchievement(12);
                    _achievementCheck[12] = true;
                }
                else if (_achievementCheck[13] == false && PlayerInfo.Instance.ShootingCenterCount >= 12)
                {
                    UnlockAchievement(13);
                    _achievementCheck[13] = true;
                }
                else if (_achievementCheck[14] == false && PlayerInfo.Instance.RhythmCenterCount >= 12)
                {
                    UnlockAchievement(14);
                    _achievementCheck[14] = true;
                }
                else if (_achievementCheck[15] == false && PlayerInfo.Instance.RPGCenterCount >= 12)
                {
                    UnlockAchievement(15);
                    _achievementCheck[15] = true;
                }
                else if (_achievementCheck[16] == false && PlayerInfo.Instance.SportsCenterCount >= 12)
                {
                    UnlockAchievement(16);
                    _achievementCheck[16] = true;
                }
                else if (_achievementCheck[17] == false && PlayerInfo.Instance.AdventureCenterCount >= 12)
                {
                    UnlockAchievement(17);
                    _achievementCheck[17] = true;
                }
                else if (_achievementCheck[18] == false && PlayerInfo.Instance.PuzzleCenterCount >= 12)
                {
                    UnlockAchievement(18);
                    _achievementCheck[18] = true;
                }
            }

            _timeSpan = 0;
        }
    }

    public void GoogleAchevment()
    {
        Debug.Log("구글 업적 창 띄우기");

        // 인증되어있는지 확인
        if (Social.localUser.authenticated)
        {
            GPGSBinder.Instance.ShowAchievementUI();
        }
        else
        {
            GpgsBinder.TryGoogleLogin();
        }
    }

    public void UnlockAchievement(int index)
    {
        switch (index)
        {
            case 1: // welcome
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_01);
                break;
            case 2: // lotto
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_02);
                break;
            case 3: // _mail
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_03);
                break;
            case 4: // _profLevelUP
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_04);
                break;
            case 5: // _gameJam
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_05);
                break;
            case 6: //_AGame
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_06);
                break;
            case 7: //_gameShow
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_07);
                break;
            case 8: //_minigame
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_08);
                break;
            case 9: //_Inje
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_09);
                break;
            case 10: //_newStudent
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_10);
                break;
            case 11: //_Action
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_11);
                break;
            case 12: //_Simul
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_12);
                break;
            case 13: //_Shooting
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_13);
                break;
            case 14: //_Rhythm
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_14);
                break;
            case 15: //_RPG
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_15);
                break;
            case 16: //_Sports
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_16);
                break;
            case 17: //Adventure
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_17);
                break;
            case 18: // _Puzzle
                GpgsBinder.UnlockAchievement(GPGSIds.achievement_18);
                break;
        }
    }


    // 안쓰는 함수
    public void IsSetFirstSchedule()
    {
        if (Social.localUser.authenticated)
        {
            //GPGSBinder.Instance.UnlockAchievement(GPGSIds.achievement_SetClass);     //구글 업적 달성
        }
        else
        {
        }
    }
}