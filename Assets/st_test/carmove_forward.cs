using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carmove_forward : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("destroy"))
        {
           this.gameObject.SetActive(false);
        }
    }

}