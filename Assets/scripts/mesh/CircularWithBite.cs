
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircularWithBite : MonoBehaviour
{
    public float radius = 1.0f;
    public float radiusInner = 0.5f;
    public float height = 0.4f;
    public int details = 20;
    // if last piece and end piece have a gap samaller than threshold ,add another piece
    private float threshold = 0.01f;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        generatePieByRadian();
    }
    [ContextMenu("GeneratePie")]
    public void generatePieByRadian()
    {
        var arcs = new List<float>();
        float r = Random.Range(0.1f, 0.9f);
        r  *= 2 * Mathf.PI;
        arcs.Add(0);
        arcs.Add(r);
        generatePie(arcs);
    }
    public void generatePie(List<float> arcs)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();
        verts.Add(new Vector3(0, height/2.0f, 0));
        verts.Add(new Vector3(0, -height / 2.0f, 0));
        float start = arcs[0], end = arcs[1];
        float step = (end - start) / details;
        float arc;
        // initialize verts
        for(arc=start;arc<=end;arc+=step)
        {
            Vector3 upper = new Vector3(radius * Mathf.Cos(arc), height/2.0f,radius * Mathf.Sin(arc));
            Vector3 lower = new Vector3(radius * Mathf.Cos(arc), -height / 2.0f, radius * Mathf.Sin(arc));
            verts.Add(upper);
            verts.Add(lower);
        }
        if(end-arc>threshold)
        {
            Vector3 upper = new Vector3(radius * Mathf.Cos(end), height / 2.0f, radius * Mathf.Sin(end));
            Vector3 lower = new Vector3(radius * Mathf.Cos(end), -height / 2.0f, radius * Mathf.Sin(end));
            verts.Add(upper);
            verts.Add(lower);
        }
        // initialize trians;
        int vertCount = verts.Count;
        for(int i=2;i<vertCount-3;i+=2)
        {
            // upper
            tris.Add(i);tris.Add(0);tris.Add(i+2);
            // lower
            tris.Add(i + 1);tris.Add(i+3);tris.Add(1);
            // 侧曲面
            tris.Add(i);tris.Add(i + 2);tris.Add(i + 1);
            tris.Add(i + 1);tris.Add(i + 2);tris.Add(i + 3);
        }
        // 侧平面
        tris.Add(0); tris.Add(2); tris.Add(3);
        tris.Add(0); tris.Add(3); tris.Add(1);
        tris.Add(0); tris.Add(1); tris.Add(vertCount-1);
        tris.Add(0); tris.Add(vertCount-1); tris.Add(vertCount - 2);
        // set
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        meshFilter.mesh = mesh;
    }
   

    void Start()
    {

    }
    //private void OnDrawGizmos()
    //{
    //    if (this.vertices == null)
    //        return;
    //    Gizmos.color = Color.black;
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {

    }
}

