using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float moveSpeed = 10.0f;
    public float turnSpeed = 6.0f;      // Speed of camera turning when mouse moves in along an axis
    public float zoomSpeed = 2.0f;      // Speed of the camera going back and forth
    private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
    private bool isRotating;    // Is the camera being rotated?
    private bool isZooming;     // Is the camera zooming?
    [SerializeField]
    private float zoomPos;

    private float border;

    void Start()
    {
        zoomPos = 0.0f;
    }
    //
    // UPDATE
    //

    void Update()
    {
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed, Space.Self);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed, Space.Self);
        }
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed, Space.Self);
        }
        if (Input.GetKey("y"))
        {
            if (transform.position.y > 3.0f)
                transform.Translate(Vector3.down * Time.deltaTime * moveSpeed, Space.Self);
        }
        if (Input.GetKey("x"))
        {
            if (transform.position.y < 13.0f)
                transform.Translate(Vector3.up * Time.deltaTime * moveSpeed, Space.Self);
        }

        // Get the left mouse button
        if (Input.GetMouseButtonDown(1))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isRotating = true;
        }

        // Get the middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isZooming = true;
        }

        // Disable movements on button release
        if (!Input.GetMouseButton(1)) isRotating = false;
        if (!Input.GetMouseButton(2)) isZooming = false;

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            //transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }

        // Move the camera linearly along Z axis
        if (isZooming)
        {
            if (!(zoomPos > -5.0f && zoomPos < 5.0f))
            {
                if (zoomPos <= -5.0f)
                    zoomPos = -4.99f;
                if (zoomPos >= 5.0f)
                    zoomPos = 4.99f;
                isZooming = false;
            }
            else
            {

                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

                zoomPos += pos.y * zoomSpeed;


                Vector3 move = pos.y * zoomSpeed * transform.forward;
                transform.Translate(move, Space.World);
            }
        }
    }
}