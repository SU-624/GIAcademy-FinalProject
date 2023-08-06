using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_glove : MonoBehaviour
{
    public Transform effectPosition;
    public GameObject effectPrefab;

    // Start is called before the first frame update
    public void PlayEffect()
    {
        GameObject effect = Instantiate(effectPrefab, effectPosition.position, transform.rotation);
        effect.transform.SetParent(transform.parent);
        Destroy(effect, 2f);
    }
}