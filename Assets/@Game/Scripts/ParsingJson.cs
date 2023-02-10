using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsingJson : MonoBehaviour
{
    public List<string> m_StudnetNameList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        TextAsset m_File = Resources.Load<TextAsset>("Json/Name");

        string Name = JsonUtility.FromJson<string>(m_File.ToString());

        if (Name != null)
        {
            Debug.Log(Name);
            Debug.Log("있음");
        }
        else
        {
            Debug.Log("비어있음");

        }
    }


}
