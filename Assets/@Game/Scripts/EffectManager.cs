using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions.CustomUtility;

public class EffectManager : MonoBehaviour
{
    //public ParticleSystem ParticlePrefab;
    public GameObject ParticlePrefab;
    public GameObject EffectPanel;

    private float lastTime;

    // Start is called before the first frame update
    void Start()
    {
        //lastTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ////float deltaTime = Time.realtimeSinceStartup - lastTime;
            //ParticleSystem newParticle = Instantiate(ParticlePrefab);
            //newParticle.transform.parent = EffectPanel.transform;
            //Vector3 mousePos = Input.mousePosition;
            ////newParticle.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
            //newParticle.transform.position = mousePos;
            ////newParticle.Simulate(deltaTime, true, false);
            //newParticle.Play();
            //lastTime = Time.realtimeSinceStartup;
            //Destroy(newParticle);

            GameObject newParticle = Instantiate(ParticlePrefab);
            newParticle.transform.parent = EffectPanel.transform;
            Vector3 mousePos = Input.mousePosition;
            newParticle.transform.position = mousePos;
        }
    }
}
