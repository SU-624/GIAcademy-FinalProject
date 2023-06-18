using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ºôµå ¼ø¼­ ÁöÅ°ÀÚ 
/// 
/// Title -> InGame -> UI
/// </summary>
public class MergeScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("Á¹ÇÃ_008_test");
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        //SceneManager.LoadScene("InGameScene", LoadSceneMode.Additive);
        //SceneManager.LoadScene("UIScene_OhSoo", LoadSceneMode.Additive);
        // SceneManager.LoadScene("UIScene_MangGGu", LoadSceneMode.Additive);
        //SceneManager.LoadScene("UIScene_ChangWoo", LoadSceneMode.Additive);
    }

}
