using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGame : MonoBehaviour
{
    private int m_PressedCommandButtonLastFrame;
    private bool m_IsPressedCommandButtonLastFrame;

    public bool forceStopPhase3 = false;

    public event System.Action<int> onPreStartPhase; // param: <phase_number>
    public event System.Action<int, int> onFinishPhase; // param: <phase_number, correct_count>
    public event System.Action<List<int>, List<bool>> onCommandListRefreshed; // param: <command_list, is_special_command>
    public event System.Action<int> onCorrectCommand; // param: <command_index>
    public event System.Action<int> onWrongCommand; // param: <command_index>
    public event System.Action<int> onSpecialCommandPressed; // param: <command_index>
    public event System.Action<float, float> onRemainingTimeChanged; // param: <remaining_time_percentage, remaining_time_sec>
    public event System.Action<int> onScoreChanged; // param: <new_score>

    public void PressCommandButton(int _command)
    {
        m_IsPressedCommandButtonLastFrame = true;
        m_PressedCommandButtonLastFrame = _command;
    }

    private bool TryReceiveCommandButtonPressed(out int _value)
    {
        bool _pressed = m_IsPressedCommandButtonLastFrame;
        if (_pressed)
        {
            _value = m_PressedCommandButtonLastFrame;
            m_IsPressedCommandButtonLastFrame = false;
            m_PressedCommandButtonLastFrame = -1;
            return true;
        }

        _value = -1;
        return false;
    }

    public IEnumerator Phase12Coroutine(
        int _phaseIndex, float _phaseDuration,
        int _commandCount, int _specialCommandCount)
    {
        {
            // 선입력된 입력 정보를 지웁니다.
            TryReceiveCommandButtonPressed(out int _tmp);
        }

        onPreStartPhase?.Invoke(_phaseIndex);

        int[] _commandArray = new int[_commandCount];
        bool[] _isSpecialCommandArray = new bool[_commandCount];

        for (int i = 0; i < _specialCommandCount; ++i)
        {
            // 특수 커맨드의 위치를 임의로 고릅니다.
            int _index = Random.Range(0, _commandCount);
            if (_isSpecialCommandArray[_index] == true)
                continue;
            _isSpecialCommandArray[_index] = true;
        }

        for (int i = 0; i < _commandCount; ++i)
        {
            // 임의의 커맨드 목록을 만듭니다.
            int _command = Random.Range(0, 3);
            _commandArray[i] = _command;
        }

        onCommandListRefreshed?.Invoke(_commandArray.ToList(), _isSpecialCommandArray.ToList());

        int _nextCommandIndex = 0;
        int _correctCount = 0;
        float _phaseStartTime = Time.unscaledTime;
        int _specialCommandPressCount = 0;

        while (true)
        {
            float _elapsedTime = Time.unscaledTime - _phaseStartTime;
            float _remainingTimePercentage = 1.0f - (_elapsedTime / _phaseDuration);
            onRemainingTimeChanged?.Invoke(_remainingTimePercentage, _phaseDuration - _elapsedTime);

            if (TryReceiveCommandButtonPressed(out int _clickedCommand))
            {
                // 버튼을 누른 경우, 올바르게 눌렀는지 여부를 확인합니다.

                int _targetCommand = _commandArray[_nextCommandIndex];
                bool _isSpecialCommand = _isSpecialCommandArray[_nextCommandIndex];

                if (_isSpecialCommand && _clickedCommand == _targetCommand)
                {
                    // (스페셜 커맨드) 버튼을 올바르게 눌렀습니다.
                    ++_specialCommandPressCount;

                    onSpecialCommandPressed?.Invoke(_nextCommandIndex);

                    if (_specialCommandPressCount == 3)
                    {
                        // 스페셜 커맨드를 3번 올바르게 눌렀다면, 다음으로 넘어갑니다.
                        _specialCommandPressCount = 0;

                        onCorrectCommand?.Invoke(_nextCommandIndex);
                        ++_correctCount;
                        ++_nextCommandIndex;
                    }
                }
                else if (_clickedCommand == _targetCommand)
                {
                    // 버튼을 올바르게 눌렀습니다. 잘 했습니다.
                    onCorrectCommand?.Invoke(_nextCommandIndex);
                    ++_correctCount;
                    ++_nextCommandIndex;
                }
                else
                {
                    // 버튼을 올바르지 않게 눌렀습니다.
                    onWrongCommand?.Invoke(_nextCommandIndex);
                    ++_nextCommandIndex;
                }

                if (_nextCommandIndex == _commandArray.Length)
                {
                    // 모든 버튼을 눌렀을 때 phase를 끝냅니다.
                    break;
                }
            }

            if (_elapsedTime > _phaseDuration)
            {
                // 제한 시간을 넘긴 경우, phase가 강제로 종료됩니다.
                break;
            }

            yield return null;
        }

        onFinishPhase?.Invoke(_phaseIndex, _correctCount);
    }

    public IEnumerator Phase3Coroutine(
        int _phaseIndex, float _phaseDuration)
    {
        {
            // 선입력된 입력 정보를 지웁니다.
            TryReceiveCommandButtonPressed(out int _tmp);
        }

        onPreStartPhase?.Invoke(_phaseIndex);

        int _pressCount = 0;
        float _phaseStartTime = Time.unscaledTime;

        while (true)
        {
            float _elapsedTime = Time.unscaledTime - _phaseStartTime;
            float _remainingTimePercentage = 1.0f - (_elapsedTime / _phaseDuration);
            onRemainingTimeChanged?.Invoke(_remainingTimePercentage, _phaseDuration - _elapsedTime);

            if(forceStopPhase3)
            {
                break;
            }

            if (_elapsedTime > _phaseDuration)
            {
                // 제한 시간을 넘긴 경우, phase가 강제로 종료됩니다.
                break;
            }

            yield return null;
        }

        onFinishPhase?.Invoke(_phaseIndex, _pressCount);

        forceStopPhase3 = false;
    }
}