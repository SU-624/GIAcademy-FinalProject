using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class characterInfo : MonoBehaviour
{
    [Header("캐릭터 머리 위 뜰 이름창의 오브젝트들")]
    [SerializeField] private Image CharacterImage;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private Button CharacterInfoButton;

    public Image GetCharacterImage { get { return CharacterImage; } set { CharacterImage = value; } }

    public TextMeshProUGUI CName { get { return CharacterName; } set { CharacterName = value; } }
    public Button InfoButton { get { return CharacterInfoButton; } set { CharacterInfoButton = value; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
