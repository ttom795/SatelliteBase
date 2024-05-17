using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CameraControls : MonoBehaviour
{
    public Transform target; // Target point to orbit around
    private float distance = 32.0f; // Distance from the target
    public float scrollSensitivity = 5f; // Mouse sensitivity
    public float panSensitivity = 0.1f; // Mouse sensitivity
    private Vector3 lastMousePosition;
    public TextMeshProUGUI highlightName;

    GameObject hoveredObject, hoveredObjectLastFrame;
    bool mouseClicked = false;

    void scan()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hoveredObject = hit.collider.gameObject;
            hoveredObject.GetComponent<SatelliteScript>().renderOrbit(true);
            string name = hoveredObject.GetComponent<SatelliteScript>().satrec.satnum;
            highlightName.text = "Hovered: Satellite ID" + name;
            if (Input.GetMouseButton(1) && !mouseClicked)
            {
                hoveredObject.GetComponent<SatelliteScript>().permaRender();
                mouseClicked = true;
            }
        }
        else
        {
            hoveredObject = null;
            mouseClicked = false;
        }

        if (hoveredObjectLastFrame != hoveredObject)
        {
            if (hoveredObjectLastFrame != null) hoveredObjectLastFrame.GetComponent<SatelliteScript>().renderOrbit(false);
            hoveredObjectLastFrame = hoveredObject;
        }
    }



    void Update()
    {
        scan();
        // Rotate camera around target based on mouse input
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;
            transform.RotateAround(target.position, Vector3.up, deltaMouse.x * panSensitivity);
            transform.RotateAround(target.position, transform.right, -deltaMouse.y * panSensitivity);
        }

        // Move camera closer or farther from target with mouse scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * scrollSensitivity;
        distance = Mathf.Clamp(distance, 32f, 100f); // Adjust the min and max distance as needed

        // Apply changes to camera position
        Camera.main.orthographicSize = distance;

        lastMousePosition = Input.mousePosition;
    }
}
