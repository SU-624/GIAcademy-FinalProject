using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoMinigameSceneBtn()
    {
        SceneManager.LoadScene("MiniGameScene");
    }
    
    public void GoTitleSceneBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
