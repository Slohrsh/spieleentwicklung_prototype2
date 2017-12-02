using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour {

    public int LevelArea = 100;

    public int RotationSpeed = 20;

    public int ZoomSpeed = 25;
    public int ZoomMin = 20;
    public int ZoomMax = 100;

    public int ScrollArea = 25;
    public int ScrollSpeed = 25;
    public int DragSpeed = 100;

    private Camera camera;

    // Use this for initialization
    void Start () {
        camera = GetComponentInChildren<Camera>();
    }
	
	// Update is called once per frame
	void Update () {

        var translation = Vector3.zero;
        var rotation = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A pressed");
            rotation = new Vector3(0, -1 * RotationSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotation = new Vector3(0, RotationSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            translation += transform.forward * ScrollSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            translation += transform.forward * -ScrollSpeed * Time.deltaTime;
        }

        var zoomDelta = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;
        if (zoomDelta != 0)
        {
            translation -= Vector3.up * ZoomSpeed * zoomDelta;
        }

        if (Input.GetMouseButton(2)) // MMB
        {
            // Hold button and drag camera around
            translation -= new Vector3(Input.GetAxis("Mouse X") * DragSpeed * Time.deltaTime, 0,
                               Input.GetAxis("Mouse Y") * DragSpeed * Time.deltaTime);
        }
        else
        {
            // Move camera if mouse pointer reaches screen borders
            if (Input.mousePosition.x < ScrollArea)
            {
                translation += transform.right * -ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.x >= Screen.width - ScrollArea)
            {
                translation += transform.right * ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y < ScrollArea)
            {
                translation += transform.forward * -ScrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y > Screen.height - ScrollArea)
            {
                translation += transform.forward * ScrollSpeed * Time.deltaTime;
            }
        }

        var desiredPosition = camera.transform.position + translation;
        if (desiredPosition.x < -LevelArea || LevelArea < desiredPosition.x)
        {
            translation.x = 0;
        }
        if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)
        {
            translation.y = 0;
        }
        if (desiredPosition.z < -LevelArea || LevelArea < desiredPosition.z)
        {
            translation.z = 0;
        }

        transform.position += translation;
        transform.Rotate(rotation);
    }
}
