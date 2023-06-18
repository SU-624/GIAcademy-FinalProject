using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsRadar : MonoBehaviour
{
    [SerializeField] private CanvasRenderer radarMeshCanvasRenderer;
    [SerializeField] private Material m_RadarMaterial;
    [SerializeField] private Texture2D m_RadarTexture;

    private PentagonStats m_StudentStats;

    public void SetStats(PentagonStats stats)
    {
        this.m_StudentStats = stats;

        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e)
    {
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual()
    {
        Mesh mesh = new Mesh();

        Vector3[] _vertices = new Vector3[6];
        Vector2[] _uv = new Vector2[6];
        int[] _triangles = new int[3 * 5];

        float angleIncrement = 360f / 5;
        float radarChartSize = 172f;


        var test = m_StudentStats.GetStatAmountNormalized(AbilityType.Sense);

        Debug.Log("TEST : " + test);
        Debug.Log("radarChartSaize : " + radarChartSize);

        Vector3 SenseVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * m_StudentStats.GetStatAmountNormalized(AbilityType.Sense);
        int SenseVertexIndex = 1;

        Vector3 ConcentrationVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * m_StudentStats.GetStatAmountNormalized(AbilityType.Concentration);
        int ConcentrationVertexIndex = 2;

        Vector3 WitVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * m_StudentStats.GetStatAmountNormalized(AbilityType.Wit);
        int WitVertexIndex = 3;

        Vector3 TechniqueVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * m_StudentStats.GetStatAmountNormalized(AbilityType.Technique);
        int TechniqueVertexIndex = 4;

        Vector3 InsightVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * m_StudentStats.GetStatAmountNormalized(AbilityType.Insight);
        int InsightVertexIndex = 5;

        _vertices[0] = Vector3.zero;
        _vertices[SenseVertexIndex] = SenseVertex;
        _vertices[ConcentrationVertexIndex] = ConcentrationVertex;
        _vertices[WitVertexIndex] = WitVertex;
        _vertices[TechniqueVertexIndex] = TechniqueVertex;
        _vertices[InsightVertexIndex] = InsightVertex;

        _uv[0] = Vector2.zero;
        _uv[SenseVertexIndex] = Vector2.one;
        _uv[ConcentrationVertexIndex] = Vector2.one;
        _uv[WitVertexIndex] = Vector2.one;
        _uv[TechniqueVertexIndex] = Vector2.one;
        _uv[InsightVertexIndex] = Vector2.one;

        _triangles[0] = 0;
        _triangles[1] = SenseVertexIndex;
        _triangles[2] = ConcentrationVertexIndex;

        _triangles[3] = 0;
        _triangles[4] = ConcentrationVertexIndex;
        _triangles[5] = WitVertexIndex;

        _triangles[6] = 0;
        _triangles[7] = WitVertexIndex;
        _triangles[8] = TechniqueVertexIndex;

        _triangles[9] = 0;
        _triangles[10] = TechniqueVertexIndex;
        _triangles[11] = InsightVertexIndex;

        _triangles[12] = 0;
        _triangles[13] = InsightVertexIndex;
        _triangles[14] = SenseVertexIndex;

        mesh.vertices = _vertices;
        mesh.uv = _uv;
        mesh.triangles = _triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(m_RadarMaterial, m_RadarTexture);
    }
}
