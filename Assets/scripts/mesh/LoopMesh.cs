
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider))]
public class LoopMesh : MonoBehaviour
{
    public float borderGap = 0.1f;
    public float radius = 5f;
    public float radiusInner = 3f;
    public float height = 1f;
    public int details = 20;
    public float minDifScale = 0.8f;
    public float maxDifScale = 0.55f;
    public float minDeathDifScale = 0.1f;
    public float maxDeathDifScale = 0.5f;
    // if last piece and end piece have a gap samaller than threshold ,add another piece
    [HideInInspector]
    public int loopDifficulty;
    private float threshold = 0.001f;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    // Start is called before the first frame update
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        GeneratePieByDifficulty();
    }
    [ContextMenu("GeneratePie")]
    public void GeneratePieByRadian()
    {
        var arcs = new List<float>();
        float r = Random.Range(0.7f, 0.9f);
        r *= 2 * Mathf.PI;
        arcs.Add(0);
        arcs.Add(r);
        GeneratePie(arcs);
    }
    public void GeneratePieByDifficulty()
    {

        loopDifficulty = GameObject.Find("GameController").GetComponent<GameController>().difficulty;
        loopDifficulty = Mathf.Clamp(loopDifficulty, 0, 10);
        if(CompareTag("Loop"))
        {
            var arcs = new List<float>();
            float r = minDifScale + (maxDifScale - minDifScale) / 10 * loopDifficulty;
            r *= 2 * Mathf.PI;
            arcs.Add(0);
            arcs.Add(r);
            GeneratePie(arcs);
        }
        else if(CompareTag("DeathLoop"))
        {
            var arcs = new List<float>();
            float r = minDeathDifScale + (maxDeathDifScale - minDeathDifScale) / 10 * loopDifficulty;
            r *= 2 * Mathf.PI;
            arcs.Add(-0.03f);
            arcs.Add(r+0.03f);
            GeneratePie(arcs);
        }
    }
    public void randomRotate()
    {
        if (CompareTag("DeathLoop"))
        {
            float r = minDeathDifScale + (maxDeathDifScale - minDeathDifScale) / 10 * loopDifficulty;
            float rl = minDifScale + (maxDifScale - minDifScale) / 10 * loopDifficulty;
            float s = Random.Range(0.0f, rl - r);
            //// 防止接缝很诡异
            if (rl - r - s < borderGap)
            {
                s = rl - r+0.02f;
            }
            else if (s < borderGap)
            {
                s = -0.02f;
            }
            s*= 360.0f;
            float parentY = this.transform.parent.rotation.eulerAngles.y;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, parentY-s, 0));   
        }
    }
    public void GeneratePie(List<float> arcs)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> norms = new List<Vector3>();
        List<int> tris = new List<int>();
        float start = arcs[0], end = arcs[1];
        float step = (end - start) / details;
        float arc;
        // initialize verts
        for (arc = start; arc <= end; arc += step)
        {
            Vector3 upperInner = new Vector3(radiusInner * Mathf.Cos(arc), height / 2.0f, radiusInner * Mathf.Sin(arc));
            Vector3 lowerInner = new Vector3(radiusInner * Mathf.Cos(arc), -height / 2.0f, radiusInner * Mathf.Sin(arc));
            Vector3 upper = new Vector3(radius * Mathf.Cos(arc), height / 2.0f, radius * Mathf.Sin(arc));
            Vector3 lower = new Vector3(radius * Mathf.Cos(arc), -height / 2.0f, radius * Mathf.Sin(arc));
            for(int i=0;i<2;i++)
            {
                verts.Add(upperInner);
                verts.Add(lowerInner);
                verts.Add(upper);
                verts.Add(lower);
            }
            norms.Add(new Vector3(0, 1, 0));
            norms.Add(new Vector3(0, -1, 0));
            norms.Add(new Vector3(0, 1, 0));
            norms.Add(new Vector3(0, -1, 0));
            Vector3 normalized = Vector3.Normalize(new Vector3(radius * Mathf.Cos(arc), 0, radius * Mathf.Sin(arc)));
            for(int i=0;i<4;i++)
            {
                norms.Add(normalized);
            }
        }
        if (end - arc > threshold)
        {
            Vector3 upperInner = new Vector3(radiusInner * Mathf.Cos(end), height / 2.0f, radiusInner * Mathf.Sin(end));
            Vector3 lowerInner = new Vector3(radiusInner * Mathf.Cos(end), -height / 2.0f, radiusInner * Mathf.Sin(end));
            Vector3 upper = new Vector3(radius * Mathf.Cos(end), height / 2.0f, radius * Mathf.Sin(end));
            Vector3 lower = new Vector3(radius * Mathf.Cos(end), -height / 2.0f, radius * Mathf.Sin(end));
            for(int i=0;i<2;i++)
            {
                verts.Add(upperInner);
                verts.Add(lowerInner);
                verts.Add(upper);
                verts.Add(lower);
            }
            norms.Add(new Vector3(0, 1, 0));
            norms.Add(new Vector3(0, -1, 0));
            norms.Add(new Vector3(0, 1, 0));
            norms.Add(new Vector3(0, -1, 0));
            Vector3 normalized = Vector3.Normalize(new Vector3(radius * Mathf.Cos(end), 0, radius * Mathf.Sin(end)));
            for (int i = 0; i < 4; i++)
            {
                norms.Add(normalized);
            }
        }
        // add another copy ，三角点拷贝三次
        int vertCount = verts.Count;
        for (int i = 0; i < 4; i++)
        {
            verts.Add(verts[i]);
            Vector3 tmpNorm = new Vector3(verts[i].x, 0, verts[i].z);
            tmpNorm = Vector3.Normalize(Quaternion.Euler(0, -90, 0) * tmpNorm);
            norms.Add(tmpNorm);
        }
        for (int i = 0; i < 4; i++)
        {
            verts.Add(verts[vertCount - 4 + i]);
            Vector3 tmpNorm = new Vector3(verts[i].x, 0, verts[i].z);
            tmpNorm = Vector3.Normalize(Quaternion.Euler(0, 90, 0) * tmpNorm);
            norms.Add(tmpNorm);
        }
        // initialize trians;
        for (int i = 0; i < vertCount - 15; i += 8)
        {
            //  上下表面
            // upper1
            tris.Add(i); tris.Add(i + 8); tris.Add(i + 10);
            tris.Add(i); tris.Add(i + 10); tris.Add(i + 2);
            // lower1
            tris.Add(i + 1); tris.Add(i + 3); tris.Add(i + 11);
            tris.Add(i + 1); tris.Add(i + 11); tris.Add(i + 9);
            // 内侧曲面2
            tris.Add(i+4); tris.Add(i + 5); tris.Add(i + 13);
            tris.Add(i+4); tris.Add(i + 13); tris.Add(i + 12);
            // 外侧曲面2
            tris.Add(i + 6); tris.Add(i + 14); tris.Add(i + 15);
            tris.Add(i + 6); tris.Add(i + 15); tris.Add(i + 7);
        }
        // 侧平面(n0)
        vertCount = verts.Count;
        tris.Add(vertCount-8); tris.Add(vertCount-6); tris.Add(vertCount-5);
        tris.Add(vertCount - 8); tris.Add(vertCount - 5); tris.Add(vertCount - 7);
        tris.Add(vertCount - 3); tris.Add(vertCount - 2); tris.Add(vertCount - 4);
        tris.Add(vertCount - 3); tris.Add(vertCount - 1); tris.Add(vertCount - 2);
        // set
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.normals = norms.ToArray();
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = meshFilter.mesh;
        //StartCoroutine(SequenceTest());
    }
    IEnumerator SequenceTest()
    {
        for (int i = 0; i < meshFilter.mesh.triangles.Length; i += 24)
        {
            Debug.DrawLine(transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i]]),
                transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 1]]),
                Color.blue,
                100f);

            yield return new WaitForSeconds(2.0f);
            Debug.DrawLine(transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 1]]),
                transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 2]]),
                Color.blue,
                100f);

            yield return new WaitForSeconds(2.0f);
            Debug.DrawLine(transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 2]]),
                transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i]]),
                Color.blue,
                100f);

            yield return new WaitForSeconds(2.0f);
            Debug.DrawLine(transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 3]]),
                transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 4]]),
                Color.yellow,
                100f);

            yield return new WaitForSeconds(2.0f);
            Debug.DrawLine(transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 4]]),
                transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 5]]),
                Color.yellow,
                100f);

            yield return new WaitForSeconds(2.0f);
            Debug.DrawLine(transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[i + 5]]),
                transform.TransformPoint(meshFilter.mesh.vertices[meshFilter.mesh.triangles[3]]),
                Color.yellow,
                100f);

            yield return new WaitForSeconds(2.0f);

        }
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

