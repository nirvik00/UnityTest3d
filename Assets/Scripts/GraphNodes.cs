using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNodes : MonoBehaviour
{
    [System.Serializable]
    public class Space
    {
        public string category;
        public string projectName;
        public string spaceName;
        public string spaceFullName;
        public string level;
        public string area;
        public string uniqueIdentifier;
        public string spaceId;
        public string bounds;
        public string coordinates;
    }

    [System.Serializable]
    public class SpaceList
    {
        public Space[] space;
    }

    [SerializeField]
    public TextAsset SpaceJsonFile;

    [SerializeField]
    public GameObject NodeSphere;

    public SpaceList mySpaceList = new SpaceList();

    void Start()
    {
        // for (var i = 0; i < 10; i++)
        // {
        //     Instantiate(NodeSphere, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        // }
        GetSpacesFromJsonFile();
    }

    void Update()
    {

    }

    public Vector3 GetCentroid(string bounds)
    {
        string[] arr = bounds.Split(',');
        float min_x = float.Parse(arr[0]);
        float min_y = float.Parse(arr[1]);
        float min_z = float.Parse(arr[2]);
        float max_x = float.Parse(arr[3]);
        float max_y = float.Parse(arr[4]);
        float max_z = float.Parse(arr[5]);
        Vector3 p = new Vector3((min_x + max_x) * 0.5f, (min_y + max_y) * 0.5f, (min_z + max_z) * 0.5f);
        return p;
    }

    public Vector3 UpdatePosition(Vector3 min, Vector3 max, Vector3 p)
    {
        float ax = -50f; float bx = 50f;
        float ay = -50f; float by = 50f;
        float az = -1f; float bz = 1f;
        float tx = Math.Abs(p.x) / (Math.Abs(max.x) + Math.Abs(min.x));
        float ty = Math.Abs(p.y) / (Math.Abs(max.y) + Math.Abs(min.y));
        float tz = Math.Abs(p.z) / (Math.Abs(max.z) + Math.Abs(min.z));

        float x = ax * (1 - tx) + bx * tx;
        float y = ay * (1 - ty) + by * ty;
        float z = az * (1 - tz) + bz * tz;
        // flip y and z now
        Vector3 q = new Vector3(x, z, y);
        return q;
    }

    public void GetSpacesFromJsonFile()
    {
        mySpaceList = JsonUtility.FromJson<SpaceList>(SpaceJsonFile.text);
        Vector3 min = new Vector3(0, 0, 0);
        Vector3 max = new Vector3(0, 0, 0);
        int counter = 0;
        foreach (var space in mySpaceList.space)
        {
            Debug.Log(space.spaceName);
            Vector3 p = GetCentroid(space.bounds);
            if (p.x < min.x)
            {
                min.x = p.x;
            }
            if (p.x > max.x)
            {
                max.x = p.x;
            }
            if (p.y < min.y)
            {
                min.y = p.y;
            }
            if (p.y > max.y)
            {
                max.y = p.y;
            }
            if (p.z < min.z)
            {
                min.z = p.z;
            }
            if (p.z > max.z)
            {
                max.z = p.z;
            }

            // move p based on already found min max values
            p = new Vector3(p.x + 150, p.y + 3, p.z + 90);

            //
            // lerp p and flip y and z
            Vector3 q = UpdatePosition(min, max, p);

            GameObject node = Instantiate(NodeSphere, q, Quaternion.identity);
            NodeSphere sph = node.GetComponent(typeof(NodeSphere)) as NodeSphere;
            sph.SetCounter(counter);
            sph.spaceName = space.spaceName;
            sph.level = space.level;
            node.name = space.spaceName;
            counter++;
        }

        Debug.Log("min bounds = " + min);
        // -742, -16, -358

        Debug.Log("max bounds = " + max);
        // 0, 22, 46
    }
}
