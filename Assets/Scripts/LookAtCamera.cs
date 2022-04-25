using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateToCamera();
    }

    void OnValidate()
    {
        RotateToCamera();
    }

    void RotateToCamera()
    {       

        this.transform.rotation = Camera.main.transform.rotation;
    }    

}
