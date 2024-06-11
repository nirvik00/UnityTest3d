using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSphere : MonoBehaviour
{

    [SerializeField] public int counter = -1;

    [SerializeField] public string spaceName = "name not set";

    [SerializeField] public string level = "level not set";


    public int GetCounter()
    {
        return counter;
    }

    public void SetCounter(int t)
    {
        counter = t;
    }
}
