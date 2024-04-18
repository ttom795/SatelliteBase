using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;
using SGP4Methods;

public class SatelliteReader : MonoBehaviour
{
    public GameObject satellitePrefab;
    private List<GameObject> satellitesList = new List<GameObject>();
    public TextMeshProUGUI dataDisplay;
    private double timeMultiplier = 1;
    DateTime reftime;
    public ParticleSystem visualSatellites;


    void Start()
    {
        StartCoroutine(LoadSatellites());
        reftime = DateTime.UtcNow;
    }

    public void multiplierUpdate(float val)
    {
        timeMultiplier = val;
    }

    public void offsetTwelve(bool add)
    {
        if (add)
        {
            reftime = reftime.AddHours(12);
        }
        else
        {
            reftime = reftime.AddHours(-12);
        }
    }

    private void Update()
    {

        // Time management
        TimeSpan accumulatedTimeSpan = TimeSpan.FromSeconds(Time.deltaTime * timeMultiplier);
        reftime = reftime.Add(accumulatedTimeSpan);

        // Satellite visualisation
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[satellitesList.Count];
        visualSatellites.GetParticles(particles);

        // Satellite updating
        for (int i = 0; i < satellitesList.Count; i++)
        {
            SatelliteScript satScriptReference = satellitesList[i].GetComponent<SatelliteScript>();
            DateTime data = JulianToDateTime(satScriptReference.satrec.epochyr + 2000, satScriptReference.satrec.epochdays);
            satScriptReference.tsince = (reftime - data).TotalMinutes;
            satScriptReference.calculateAndUpdatePosition();

            particles[i].position = satScriptReference.transform.position;
            particles[i].startSize = 10f;

        }
        visualSatellites.SetParticles(particles, satellitesList.Count);
        dataDisplay.text = reftime.ToString("UTC yyyy-MM-dd HH:mm:ss") +
            "\nRunning at " + timeMultiplier + "x speed";

        // Earth rotation
        double utcDecimalHours = reftime.TimeOfDay.TotalHours;
        double utcFractionOfDay = utcDecimalHours / 24.0;
        float rotationAngle = (float)(utcFractionOfDay * 360.0) * -1f;
        transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }

    public DateTime JulianToDateTime(int year, double julianDate)
    {
        DateTime epoch = new DateTime(year, 1, 1);
        TimeSpan span = TimeSpan.FromDays(julianDate);
        return epoch + span;
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
