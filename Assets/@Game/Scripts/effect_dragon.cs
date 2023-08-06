using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_dragon : MonoBehaviour
{
    public Transform effectPosition;
    public Transform dmgPosition;
    public List<GameObject> effectPrefabs;
    GameObject effectPrefab;

    // Start is called before the first frame update
    public void PlayEffect()
    {
        GameObject fireEffect = effectPrefabs[0];
        GameObject effect = Instantiate(fireEffect, effectPosition.position, transform.rotation);
        effect.transform.SetParent(transform.parent);
        Destroy(effect, 2f);
    }
    public void PlayEffect2()
    {
        GameObject dmgEffect = effectPrefabs[1];
        GameObject effect = Instantiate(dmgEffect, dmgPosition.position, transform.rotation);
        effect.transform.SetParent(transform.parent);
        Destroy(effect, 2f);
    }

}
