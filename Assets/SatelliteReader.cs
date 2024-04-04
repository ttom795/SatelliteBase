using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGP4Methods;
using System;
using System.IO;

public class SatelliteReader : MonoBehaviour
{
    public GameObject satellitePrefab;
    private List<GameObject> satellitesList = new List<GameObject>();

    void Start()
    {
        StartCoroutine(LoadSatellites());
    }

    IEnumerator LoadSatellites()
    {
        string tleData = File.ReadAllText(System.IO.Path.Combine(Application.streamingAssetsPath, "gp.txt"));

        // Split the data by new lines to get individual sets
        string[] tleSets = tleData.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        Debug.Log(tleSets.Length);
        // Process each TLE set
        for (int i = 0; i < tleSets.Length; i+=3)
        {
            SGP4Lib calc = new SGP4Lib();

            // Now, tleLines array contains two lines of TLE data for each set
            // You can pass these two lines to the twoline2rv function
            string line1 = tleSets[i+1];
            string line2 = tleSets[i+2];

            Debug.Log("Line 1:" + line1);
            Debug.Log("Line 2:" + line2);

            double startmfe, stopmfe, deltamin;
            SGP4Lib.elsetrec satrec = new SGP4Lib.elsetrec();

            // Call twoline2rv method to convert TLE data to internal representation
            calc.twoline2rv(line1, line2, 'c', 'e', 'a', SGP4Lib.gravconsttype.wgs72, out startmfe, out stopmfe, out deltamin, out satrec);
            
            double[] position = new double[3];
            double[] velocity = new double[3];
            double tsince = 129400;

            calc.sgp4(ref satrec, tsince, position, velocity);

            string joinedString = string.Join(", ", position);
            Debug.Log(joinedString);
            Debug.Log("");

            // Create a new Satellite object and add it to the list
            GameObject newSatellite = Instantiate(satellitePrefab);

            newSatellite.GetComponent<SatelliteScript>().Initialize(name, position, velocity);

            satellitesList.Add(newSatellite);

            if (i % 50 == 0)
            {
                yield return null;
            }
        }
        yield return null; // Yield to update after every satellite is instantiated
    }
}
