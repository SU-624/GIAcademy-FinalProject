using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect : MonoBehaviour
{
    public GameObject effectPrefab;

    // Start is called before the first frame update
    public void PlayEffect()
    {
        GameObject effect = Instantiate(effectPrefab,transform.position,transform.rotation);
        effect.transform.SetParent(transform.parent);
        //Destroy(effect,2f);
    }

    // Update is called once per frame
}