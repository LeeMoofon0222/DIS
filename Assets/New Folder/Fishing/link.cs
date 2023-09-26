using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class link : MonoBehaviour
{
    public LineRenderer line;
    public Transform startPoint;
    public Transform endPoint;

    void Start()
    {
        line.positionCount = 2;
    }

    void Update()
    {
        line.SetPosition(0, startPoint.localPosition);
        line.SetPosition(1, endPoint.localPosition);
    }
}