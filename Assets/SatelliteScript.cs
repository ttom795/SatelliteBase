using UnityEngine;
using SGP4Methods;
using System;
using System.Collections.Generic;

public class SatelliteScript : MonoBehaviour
{
    private string satelliteName;
    private string line1;
    private string line2;

    SGP4Lib calc = new SGP4Lib();
    public SGP4Lib.elsetrec satrec = new SGP4Lib.elsetrec();
    public double tsince;

    double startmfe, stopmfe, deltamin;
    double[] position = new double[3];
    double[] velocity = new double[3];

    private LineRenderer lr;
    private bool renderOverride = false;

    void calculatePoints()
    {
        // this took WAY too long to get working (╯°□°）╯︵ ┻━┻
        double rawOrbitalPeriodMinutes = (2.0 * Math.PI) / satrec.nm;
        int orbitalPeriodMinutes = (int)Math.Floor(rawOrbitalPeriodMinutes);

        // plan to use this for fractional line rendering, WIP
        double remainderOrbitalPeriodMinutes = rawOrbitalPeriodMinutes - orbitalPeriodMinutes;

        List<Vector3> linePoints = new List<Vector3>();

        double[] tempPosition = new double[3];
        double[] tempVelocity = new double[3];
        for (int i = 0; i < orbitalPeriodMinutes; i++)
        {
            calc.sgp4(ref satrec, tsince+i, tempPosition, tempVelocity);
            Vector3 futurePosition = new Vector3((float)tempPosition[0], (float)tempPosition[2], (float)tempPosition[1]) * 0.01f;
            linePoints.Add(futurePosition);
        }
  

        lr.positionCount = linePoints.Count;
        lr.SetPositions(linePoints.ToArray());
    }

    public void renderOrbit(bool toggle)
    {
        if (renderOverride == false)
        {
            lr.enabled = toggle;
        }
    }

    public void permaRender()
    {
        if (renderOverride == false)
        {
            renderOverride = true;
            lr.enabled = true;
        }
        else {
            renderOverride = false;
        }
    }

    public void Initialize(string name, string line1, string line2)
    {
        satelliteName = name;
        this.line1 = line1;
        this.line2 = line2;
    }

    public void calculateAndUpdatePosition()
    {
        calc.sgp4(ref satrec, tsince, position, velocity);
        transform.position = new Vector3((float)position[0], (float)position[2], (float)position[1]) * 0.01f;
    }

    private void Start()
    {
        calc.twoline2rv(line1, line2, 'c', 'm', 'a', SGP4Lib.gravconsttype.wgs72, out startmfe, out stopmfe, out deltamin, out satrec);
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (lr.enabled)
        {
            calculatePoints();
        }
    }
}
