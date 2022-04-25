using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorOut : MonoBehaviour
{    
    
    private Assembly assembly;
    public int rotationStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        assembly = Assembly.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Triggered when you click on the sphere
    private void OnMouseDown()
    {

        assembly.ChoosePart(this.gameObject);
        
    }
}
