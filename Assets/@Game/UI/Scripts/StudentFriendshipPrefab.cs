using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct FriendshipOption
{
    public string Name;
    public StudentType Department;
    public int FriendshipAmount;
}

public class StudentFriendshipPrefab : MonoBehaviour
{
    [Space(5f)]
    [Header("전체강사목록에 넣을 프리팹을 받을 친구")]

    [SerializeField] private Image DepartmentImg;
    [SerializeField] private Image StudentNameImg;
    [SerializeField] private TextMeshProUGUI StudentName;
    [SerializeField] private Image FriendshipImg;
    [SerializeField] private TextMeshProUGUI FriendshipAmount;
    [SerializeField] private TextMeshProUGUI FriendshipInfoText;

    public Image SetDepartmentImg
    {
        get { return DepartmentImg; }
        set { DepartmentImg = value; }
    }

    public Image SetStudentNameImg
    {
        get { return StudentNameImg; }
        set { StudentNameImg = value; }
    }

    public TextMeshProUGUI SetStudentName
    {
        get { return StudentName; }
        set { StudentName = value; }
    }

    public Image SetFriendshipImg
    {
        get { return FriendshipImg; }
        set { FriendshipImg = value; }
    }

    public TextMeshProUGUI SetFriendshipAmount
    {
        get { return FriendshipAmount; }
        set { FriendshipAmount = value; }
    }

    public TextMeshProUGUI SetFriendshipInfoText
    {
        get { return FriendshipInfoText; }
        set { FriendshipInfoText = value; }
    }
}
