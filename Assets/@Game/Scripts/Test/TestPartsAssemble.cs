using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPartsAssemble : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_BaseRenderer;
    [SerializeField] private SkinnedMeshRenderer[] m_PartRenderers;


    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < m_PartRenderers.Length; ++i)
        {
            m_PartRenderers[i].rootBone = m_BaseRenderer.rootBone;
            m_PartRenderers[i].bones = m_BaseRenderer.bones;
        }
    }
}
