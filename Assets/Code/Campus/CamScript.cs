using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {

    public Camera sideCam;
    Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();

    }

    public float deltaScale = .02f;
    public float deltaYScale = 1f;
    public float scroolSpeed = 2f;

    // Update is called once per frame
    Vector2 lastMousePos;
	void Update () {

        if (Input.GetMouseButtonDown(1)) {
            lastMousePos = Input.mousePosition;
        }
        
        if (Input.GetMouseButton(1)) {
            Vector2 delta = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - lastMousePos) ;
            delta *= deltaScale * cam.orthographicSize/7.5f;
            delta.y *= deltaYScale;
            delta = Quaternion.Euler(0, 0, -45) * delta;
            
            transform.position += new Vector3(delta.x,0, delta.y);
        }
        cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel")* scroolSpeed;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, .5f, 30);

        sideCam.orthographicSize = cam.orthographicSize;

        lastMousePos = Input.mousePosition;
    }
}
