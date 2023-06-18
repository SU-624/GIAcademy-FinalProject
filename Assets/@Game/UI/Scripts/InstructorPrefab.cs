using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InstructorPrefab : MonoBehaviour
{
    [Header("전체강사목록에 넣을 프리팹을 받을 친구")]
    [SerializeField] private Image TeacherProfileImage;
    [SerializeField] private Image TeacherTypeImage;
    [SerializeField] private Image TeacherNameImage;
    [SerializeField] private TextMeshProUGUI TeacherNameText;
    [SerializeField] private Button TeacherImageButton;
    [Space(5f)]
    [SerializeField] private Sprite FullTimeTeacherImage;
    [SerializeField] private Sprite outsideTeacherImage;
    // [Space(5f)]
    // [SerializeField] private Sprite outsideTeacherImage;

    List<Sprite> DepartmentIndexImgList = new List<Sprite>();

    public Image GetTeacherProfileImage
    {
        get { return TeacherProfileImage; }
        set { TeacherProfileImage = value; }
    }

    public Image GetInstructorTypeImage
    {
        get { return TeacherTypeImage; }
        set { TeacherTypeImage = value; }
    }

    public Image GetTeacherNameImage
    {
        get { return TeacherNameImage; }
        set { TeacherNameImage = value; }
    }

    public TextMeshProUGUI GetInstructorName
    {
        get { return TeacherNameText; }
        set { TeacherNameText = value; }
    }

    public Button GetInstructorImageButton
    {
        get { return TeacherImageButton; }
        set { TeacherImageButton = value; }
    }

    public Sprite GetFullTimeTeacherImage
    {
        get { return FullTimeTeacherImage; }
    }

    public Sprite GetoutsideTeacherImage
    {
        get { return outsideTeacherImage; }
    }

    public List<Sprite> GetDepartmentIndexImgList
    {
        get { return DepartmentIndexImgList; }
    }
}
