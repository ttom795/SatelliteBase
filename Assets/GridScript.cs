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
    public GameObject lr;
    private int stepLastFrame;
    private List<GameObject> verticalBands = new List<GameObject>();
    private List<GameObject> horizontalBands = new List<GameObject>();

    public void Start()
    {
        initBands();
        generate(1);
    }

    Vector3 coordinate(int x, int y)
    {
        //x = lng, y = lat
        return Quaternion.AngleAxis(x, -Vector3.up) * Quaternion.AngleAxis(y, -Vector3.right) * new Vector3(0, 0, (6371*0.1f)+3);
    }

    float MapValue(float value, float originalMin, float originalMax, float targetMin, float targetMax)
    {
        return (value - originalMin) * (targetMax - targetMin) / (originalMax - originalMin) + targetMin;
    }

    void Update()
    {
        float distance = MapValue(Camera.main.orthographicSize, 32f, 100f, 1f, 20f);
        int step = (int)Mathf.Round(distance / 5) * 5;
        step = Mathf.Clamp(step, 1, 20);
        if (step != stepLastFrame)
        {
            generate(step);
        }
        stepLastFrame = step;
    }


    void initBands()
    {
        for (int y = 0; y < 360; y += 1)
        {
            GameObject bandV = Instantiate(lr, transform);
            LineRenderer lrv = bandV.GetComponent<LineRenderer>();
            lrv.positionCount = 360;
            verticalBands.Add(bandV);

            GameObject bandH = Instantiate(lr, transform);
            LineRenderer lrh = bandH.GetComponent<LineRenderer>();
            lrh.positionCount = 360;
            horizontalBands.Add(bandH);

            for (int x = 0; x < 360; x += 1)
            {
                lrv.SetPosition(x, coordinate(x, y));
                lrh.SetPosition(x, coordinate(y, x));
            }
        }
    }

    void generate(int step)
    {
        for (int y = 0; y < 360; y += 1)
        {
            LineRenderer lrv = verticalBands[y].GetComponent<LineRenderer>();
            LineRenderer lrh = horizontalBands[y].GetComponent<LineRenderer>();

            if (y % step == 0)
            {
                lrh.enabled = true;
                lrv.enabled = true;
            }
            else
            {
                lrh.enabled = false;
                lrv.enabled = false;
            }
        }
    }
}