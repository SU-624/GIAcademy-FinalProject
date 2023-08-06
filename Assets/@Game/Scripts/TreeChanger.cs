using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;

public class TreeChanger : MonoBehaviour
{
    [SerializeField] private List<Material> SeasonTreeMaterial = new List<Material>();
    [SerializeField] private List<Material> SeasonVineMaterial = new List<Material>();
    [SerializeField] private List<Material> SeasonGrassMaterial = new List<Material>();


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            ChangeMaterial(0);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            ChangeMaterial(1);
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            ChangeMaterial(2);
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            ChangeMaterial(3);
        }
    }

    public void ChangeMaterial(int season)
    {
        GameObject[] Trees = GameObject.FindGameObjectsWithTag("Tree");

        foreach (var tr in Trees)
        {
            tr.transform.GetChild(0).GetComponent<MeshRenderer>().material = SeasonTreeMaterial[season];
        }

        GameObject[] Vines = GameObject.FindGameObjectsWithTag("Vine");

        foreach (var vi in Vines)
        {
            vi.GetComponent<MeshRenderer>().material = SeasonVineMaterial[season];
        }

        GameObject[] Grasses = GameObject.FindGameObjectsWithTag("Grass");

        foreach (var gr in Grasses)
        {
            gr.GetComponent<MeshRenderer>().material = SeasonGrassMaterial[season];
        }

        GameObject[] GrassDoors = GameObject.FindGameObjectsWithTag("GrassDoor");

        foreach (var gr in GrassDoors)
        {
            gr.GetComponent<MeshRenderer>().materials = new Material[2] { SeasonGrassMaterial[season], SeasonGrassMaterial[season] };
        }
    }
}
