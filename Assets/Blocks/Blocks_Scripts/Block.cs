
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;

[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [SerializeField] private Mesh up;
    [SerializeField] private Mesh down;
    [SerializeField] private Mesh left;
    [SerializeField] private Mesh right;
    [SerializeField] private Mesh forward;
    [SerializeField] private Mesh backward;
    [SerializeField] private Material material;

    public Mesh Up => up;
    public Mesh Down => down;
    public Mesh Left => left;
    public Mesh Right => right;
    public Mesh Forward => forward;
    public Mesh Backward => backward;

    public Material Material => material;

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

        if (meshes.Count == 0) return; // Если нет мешей, выходим

        CombineInstance[] combine = new CombineInstance[meshes.Count];

        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
            combine[i].transform = Matrix4x4.identity; // Оставляем в локальных координатах
        }

        if (combinedMesh != null) DestroyImmediate(combinedMesh);

        combinedMesh = new Mesh();
        combinedMesh.name = "Combined Block";
        combinedMesh.CombineMeshes(combine, true, true);

        // Убеждаемся, что у объекта есть MeshFilter
        if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
        if (!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = combinedMesh;

        // Проверяем, есть ли MeshRenderer
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer) meshRenderer = gameObject.AddComponent<MeshRenderer>();

        if (material != null) GetComponent<MeshRenderer>().material = material;
       
        if (!GetComponent<MeshCollider>()) this.AddComponent<MeshCollider>();

        // Перерисовываем сцену в Edit Mode
#if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
#endif
    }
}

