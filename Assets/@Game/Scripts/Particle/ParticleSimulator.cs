using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSimulator : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    private float lastTime;
    private float deleteTimer;

    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.realtimeSinceStartup;
        //particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - lastTime;

        particle.Simulate(deltaTime, true, false);

        lastTime = Time.realtimeSinceStartup;
        deleteTimer += deltaTime;

        if (deleteTimer >= 3)
        {
            Destroy(this.gameObject);
        }
    }
}
