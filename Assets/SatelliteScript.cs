using UnityEngine;
using SGP4Methods;

public class SatelliteScript : MonoBehaviour
{
    private string satelliteName;
    private string line1;
    private string line2;

    SGP4Lib calc = new SGP4Lib();
    SGP4Lib.elsetrec satrec = new SGP4Lib.elsetrec();

    double startmfe, stopmfe, deltamin;
    public double tsince = 0;
    double[] position = new double[3];
    double[] velocity = new double[3];

    public void Initialize(string name, string line1, string line2)
    {
        satelliteName = name;
        this.line1 = line1;
        this.line2 = line2;
    }

    private void calculateAndUpdatePosition()
    {
        tsince += 1.0 * Time.deltaTime;

        calc.sgp4(ref satrec, tsince, position, velocity);
        transform.position = new Vector3((float)position[0], (float)position[2], (float)position[1]) * 0.01f;
    }

    private void Start()
    {
        calc.twoline2rv(line1, line2, 'c', 'm', 'a', SGP4Lib.gravconsttype.wgs72, out startmfe, out stopmfe, out deltamin, out satrec);
        calculateAndUpdatePosition();
    }

    private void Update()
    {
        //tsince += 0.01;
        calculateAndUpdatePosition();
    }
}
