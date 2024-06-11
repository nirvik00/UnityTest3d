//
//
//


using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerateMesh : MonoBehaviour
{
    GameObject sphere;

    Mesh mesh;

    Vector3[] vertices;
    GameObject[] nodeArr;

    int[] triangles;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {

        nodeArr = new GameObject[4];
        MakeMeshData();
        CreateMesh();
    }

    public void MakeMeshData()
    {
        vertices = new Vector3[]{
          new Vector3(0,0,0),
          new Vector3(0,0,1),
          new Vector3(1,0,0),
          new Vector3(1,0,1),
        };

        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }


    public void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;


        for (int i = 0; i < vertices.Length; i++)
        {
            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // sphere.transform.position = new Vector3(0, 1.5f, 0);
            sphere.transform.position = vertices[i];
            sphere.tag = "Selectable";
        }

    }

    void Update()
    {

    }
}
