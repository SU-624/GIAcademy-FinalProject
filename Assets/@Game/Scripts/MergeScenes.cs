using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MergeScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("มนวร_008_test");
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }

}
