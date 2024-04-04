using UnityEngine;

public class SatelliteScript : MonoBehaviour
{
    private string satelliteName;
    private double[] position;
    private double[] velocity;

    public void Initialize(string name, double[] pos, double[] vel)
    {
        satelliteName = name;
        position = pos;
        velocity = vel;
        transform.position = new Vector3((float)pos[0], (float)pos[1], (float)pos[2])*0.01f;
    }

    // Example method to display satellite information
    public void DisplaySatelliteInfo()
    {
        Debug.Log("Satellite Name: " + satelliteName);
        Debug.Log("Position: (" + position[0] + ", " + position[1] + ", " + position[2] + ")");
        Debug.Log("Velocity: (" + velocity[0] + ", " + velocity[1] + ", " + velocity[2] + ")");
    }

    private void Update()
    {
        Vector3 rotationPoint = Vector3.zero;
        Vector3 direction = new Vector3((float)velocity[0], (float)velocity[1], (float)velocity[2]);
        float angle = 0.1f;

        Vector3 rotationAxis = Vector3.Cross(direction, transform.position - rotationPoint).normalized;

        // Create a rotation quaternion
        Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);

        // Rotate the object around the rotation point
        transform.position = rotation * (transform.position - rotationPoint) + rotationPoint;

    }
}
