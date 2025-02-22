
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;

[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [SerializeField] Mesh Up;
    [SerializeField] Mesh Down;
    [SerializeField] Mesh Left;
    [SerializeField] Mesh Right;
    [SerializeField] Mesh Forward;
    [SerializeField] Mesh Backward;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh combinedMesh;

    private void OnValidate()
    {
#if UNITY_EDITOR
        EditorApplication.delayCall += CombineMesh;
#endif
    }

    private void CombineMesh()
    {
        if (this == null) return;

        List<Mesh> meshes = new List<Mesh>();

        if (Up != null) meshes.Add(Up);
        if (Down != null) meshes.Add(Down);
        if (Left != null) meshes.Add(Left);
        if (Right != null) meshes.Add(Right);
        if (Forward != null) meshes.Add(Forward);
        if (Backward != null) meshes.Add(Backward);

        if (meshes.Count == 0) return; // ���� ��� �����, �������

        CombineInstance[] combine = new CombineInstance[meshes.Count];

        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
            combine[i].transform = Matrix4x4.identity; // ��������� � ��������� �����������
        }

        if (combinedMesh != null) DestroyImmediate(combinedMesh);

        combinedMesh = new Mesh();
        combinedMesh.name = "Combined Block";
        combinedMesh.CombineMeshes(combine, true, true);

        // ����������, ��� � ������� ���� MeshFilter
        if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
        if (!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = combinedMesh;

        // ���������, ���� �� MeshRenderer
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer) meshRenderer = gameObject.AddComponent<MeshRenderer>();

        if (!GetComponent<MeshCollider>()) this.AddComponent<MeshCollider>();

        // �������������� ����� � Edit Mode
#if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
#endif
    }
}

