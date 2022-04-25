using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNavigation : MonoBehaviour
{

    public Transform target;
    public float distance;

    float panSpeed = 5f;
    float smooth = 5f;
    float xSpeed = 250.0f;
    float ySpeed = 120.0f;

    public Transform aimer;

    int yMinLimit = -20;
    int yMaxLimit = 80;

    private float x = 0.0f;
    private float y = 0.0f;
    private float z = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        {
            //Orbit = ALT + LEFT MOUSE BUTTON
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0) ) {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                var rotation2 = Quaternion.Euler(y, x, z);
                transform.rotation = rotation2;
            }
            if (target)
            {
                var rotation3 = Quaternion.Euler(y, x, z);
                var position3 = rotation3 * new Vector3(0.0f, 0.0f, -distance) + target.position;
                transform.rotation = rotation3;
                transform.position = Vector3.Lerp(transform.position, position3, Time.deltaTime * smooth);
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0) ) {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                var rotation = Quaternion.Euler(y, x, z);
                var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
                transform.rotation = rotation;
                transform.position = position;
            }

            //Pan = ALT + RIGHT MOUSE BUTTON
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(1) ) {
                aimer.position = target.position;
                target = aimer;
                aimer.Translate(transform.right * -Input.GetAxis("Mouse X") * panSpeed, Space.World);
                aimer.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
                var rotation1 = Quaternion.Euler(y, x, z);
                var position1 = rotation1 * new Vector3(0.0f, 0.0f, -distance) + aimer.position;
                transform.rotation = rotation1;
                transform.position = position1;

            }
            //Zoom = ALT + MIDDLE MOUSE BUTTON
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(2) ) {
                aimer.Translate(transform.forward * Input.GetAxis("Mouse X") * panSpeed, Space.World);
                transform.Translate(transform.forward * Input.GetAxis("Mouse X") * panSpeed, Space.World);
            }
        }
    }


    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}




//@script AddComponentMenu("Camera-Control/Mouse Orbit")


