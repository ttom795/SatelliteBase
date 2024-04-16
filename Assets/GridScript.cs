using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://peter-winslow.medium.com/creating-procedural-planets-in-unity-part-1-df83ecb12e91
/// </summary>
public class Polygon
{
    public List<int> m_Vertices;

    public Polygon(int a, int b, int c)
    {
        m_Vertices = new List<int>() { a, b, c };
    }
}

public class GridScript : MonoBehaviour
{
    public List<Vector3> gridPoints;
    public GameObject lr;

    public void Start()
    {
        generate();
    }
    Vector3 coordinate(int x, int y)
    {
        //x = lng, y = lat
        return Quaternion.AngleAxis(x, -Vector3.up) * Quaternion.AngleAxis(y, -Vector3.right) * new Vector3(0, 0, (1274.2f / 2) + 2);
    }

    void generate()
    {
        int max_distance = 360;
        int step = 1;
        for (int y = 0; y < max_distance; y+=step)
        {
            GameObject bandV = Instantiate(lr, transform);
            LineRenderer lrV = bandV.GetComponent<LineRenderer>();
            lrV.positionCount = max_distance/step;

            GameObject bandH = Instantiate(lr, transform);
            LineRenderer lrH = bandH.GetComponent<LineRenderer>();
            lrH.positionCount = max_distance/step;

            for (int x = 0; x < max_distance; x+=step)
            {
                lrV.SetPosition(x, coordinate(x, y));
                lrH.SetPosition(x, coordinate(y, x));
                gridPoints.Add(coordinate(x, y));
            }
        }
    }
}