using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameShowAnimationPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_AnimationObj;
    [SerializeField] private GameObject m_GoodAnimationEffectObj;
    [SerializeField] private GameObject m_SatisfactionStatusObj;
    [SerializeField] private GameObject m_JudgementAniObj;
    [SerializeField] private GameObject m_StudentFaceObj;
    [SerializeField] private GameObject m_StudentGoodFace1;
    [SerializeField] private GameObject m_StudentGoodFace2;
    [SerializeField] private GameObject m_StudentGoodFace3;
    [SerializeField] private GameObject m_StudentFace1;
    [SerializeField] private GameObject m_StudentFace2;
    [SerializeField] private GameObject m_StudentFace3;
    [SerializeField] private Image m_Animation;
    [SerializeField] private Image m_Emoji;
    [SerializeField] private Image m_StudentFace1Image;
    [SerializeField] private Image m_StudentFace2Image;
    [SerializeField] private Image m_StudentFace3Image;
    [SerializeField] private TextMeshProUGUI m_MyAcademyName;
    [SerializeField] private TextMeshProUGUI m_EvaluationResponseScore;
    [SerializeField] private TextMeshProUGUI m_JudgesScript;
    [SerializeField] private Animator m_EmojiAnimPlay;
    [SerializeField] private Animator m_JudgesScriptAnimPlay;
    [SerializeField] private Animator[] m_JudgesAnimPlay;

    //private string m_EmojiAnimationTrigger = "RePlay";
    //private string m_JudgesScriptAnimationTrigger = "RePlay";

    public void SetAcademyName(string _academyName)
    {
        m_MyAcademyName.text = _academyName;
    }

    public void ChangeSpriteAnimation(Sprite _animation,bool _flag = false)
    {
        m_Animation.sprite = _animation;
        m_GoodAnimationEffectObj.SetActive(_flag);
    }

    public void ChangeEvalutionResponseScore(string _score)
    {
        m_EvaluationResponseScore.text = _score;
    }

    public void ChangeSpriteEmoji(Sprite _emoji)
    {
        m_Emoji.sprite = _emoji;

        m_EmojiAnimPlay.Play("EmojiAnim", 0, 0f);
    }

    public void ChangeJudgesScript(string _script)
    {
        m_JudgesScript.text = _script;

        m_JudgesScriptAnimPlay.Play("JudgesScriptAnim", 0, 0f);
    }

    public void StopAnimation()
    {
        m_JudgesScriptAnimPlay.StopPlayback();
        m_EmojiAnimPlay.StopPlayback();

        for (int i = 0; i < m_JudgesAnimPlay.Length; i++)
        {
            m_JudgesAnimPlay[i].StopPlayback();
        }
    }

    public void StartJudesAnim()
    {
        for (int i = 0; i < m_JudgesAnimPlay.Length; i++)
        {
            m_JudgesAnimPlay[i].Play(m_JudgesAnimPlay[i].name, 0, 0f);
        }
    }

    public void ChangeStudentFaceImage(Sprite _student1, Sprite _student2, Sprite _student3)
    {
        if (m_StudentGoodFace1.activeSelf)
        {
            m_StudentFace1.SetActive(true);
            m_StudentFace2.SetActive(true);
            m_StudentFace3.SetActive(true);

            m_StudentGoodFace1.SetActive(false);
            m_StudentGoodFace2.SetActive(false);
            m_StudentGoodFace3.SetActive(false);
        }

        m_StudentFace1Image.sprite = _student1;
        m_StudentFace2Image.sprite = _student2;
        m_StudentFace3Image.sprite = _student3;
    }

    public void ChangeStudnetFaceImageGood()
    {
        m_StudentFace1.SetActive(false);
        m_StudentFace2.SetActive(false);
        m_StudentFace3.SetActive(false);

        m_StudentGoodFace1.SetActive(true);
        m_StudentGoodFace2.SetActive(true);
        m_StudentGoodFace3.SetActive(true);
    }

    public void SetActiveAnimationPanel()
    {
        m_AnimationObj.SetActive(false);
        m_SatisfactionStatusObj.SetActive(true);
        m_JudgementAniObj.SetActive(true);
        m_StudentFaceObj.SetActive(true);
    }

    public void InitPanel()
    {
        m_AnimationObj.SetActive(true);
        m_GoodAnimationEffectObj.SetActive(false);
        m_SatisfactionStatusObj.SetActive(false);
        m_JudgementAniObj.SetActive(false);

        m_StudentFaceObj.SetActive(false);
    }
}
