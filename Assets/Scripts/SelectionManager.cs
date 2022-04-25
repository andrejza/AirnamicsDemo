using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    private Ray ray;
    private RaycastHit hit;
    private Part part;
    private Assembly assembly;

    void Start()
    {
        assembly = Assembly.Instance;

    }

    void Update()
    {
        
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //Left Click
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider == null)
                {
                    assembly.part = null;
                    assembly.editMode = Assembly.EditModeOption.None;
                    return;
                }

                part = hit.collider.GetComponentInParent<Part>();
                if (part != null)
                {
                    /* Change to red
                    Debug.Log(part.name);
                    hit.collider.GetComponent<Renderer>().material.color = Color.red;*/
                    hit.collider.GetComponent<Renderer>().material.color = Color.red;

                    assembly.part = part;

                    // Go to edit mode
                    assembly.editMode = Assembly.EditModeOption.Orientation;

                    Debug.Log("PART " +  part.gameObject.name + " SELECTED");
                }
                    
                
            }


        }
    }

}
