using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{

    public Transform assembly;
    public float speed = 100;
    private float distance;
    private float angleX;
    private float angleY;


    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(transform.position, assembly.position);
        angleY = transform.rotation.eulerAngles.x;
        angleX = transform.rotation.eulerAngles.y;
    }
    /*
    // Update is called once per frame
    void Update()
    {
        // Rotate around assembly on input
        transform.RotateAround(assembly.position, Vector3.up, -Input.GetAxis("Horizontal") * speed * Time.deltaTime);

       
    }*/

    void LateUpdate()
    {
        float angleDelta = speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            angleX += angleDelta;
        }
        if (Input.GetKey(KeyCode.D))
        {
            angleX -= angleDelta;
        }
        if (Input.GetKey(KeyCode.W))
        {
            angleY -= angleDelta;
        }
        if (Input.GetKey(KeyCode.S))
        {
            angleY += angleDelta;
        }

        if ( Input.GetKey(KeyCode.UpArrow) )
        {
            distance += 0.001f * speed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            distance -= 0.001f * speed;
        }

        //Limits
        angleX = Mathf.Repeat(angleX, 360f);
        angleY = Mathf.Repeat(angleY, 360f);


        Quaternion cameraRotation =
            Quaternion.AngleAxis(angleX, Vector3.up)
            * Quaternion.AngleAxis(angleY, Vector3.right);

        Vector3 cameraPosition =
            assembly.position
            + cameraRotation * Vector3.back * distance;

        transform.position = cameraPosition;
        transform.transform.rotation = cameraRotation;
    }
}
