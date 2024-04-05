using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Update()
    {
    }

    IEnumerator LoadSatellites()
    {
        string tleData = File.ReadAllText(System.IO.Path.Combine(Application.streamingAssetsPath, "gp.txt"));
        string[] tleSets = tleData.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < tleSets.Length; i+=3)
        {
            string line1 = tleSets[i+1];
            string line2 = tleSets[i+2];

            GameObject newSatellite = Instantiate(satellitePrefab);

            newSatellite.GetComponent<SatelliteScript>().Initialize(name, line1, line2);

            satellitesList.Add(newSatellite);

            if (i % 50 == 0)
            {
                yield return null;
            }
        }
        yield return null;
    }
}
