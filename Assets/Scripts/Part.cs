using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{

    private Assembly assembly;
    public int connectorIndex { get; set; }
    public List<GameObject> connectors { get; set; }
    [field: SerializeField]
    public GameObject connectedTo { get; set; }

    void Awake()
    {
        connectorIndex = 0;
        connectors = GetAllConnectors();
        //connectors = GameObject.FindGameObjectsWithTag("ConnectorIn");
    }

    // Start is called before the first frame update
    void Start()
    {
        assembly = Assembly.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        assembly.part = this.GetComponent<Part>();        

        // Go to edit mode
        assembly.editMode = Assembly.EditModeOption.Orientation;

        Debug.Log("PART SELECTED");

    }

    
    private List<GameObject> GetAllConnectors()
    {
        List<GameObject> connectors = new List<GameObject>();

        foreach (Transform t in this.transform)
        {
            if (t.CompareTag("ConnectorIn"))
            {
                connectors.Add(t.gameObject);
            }
        }
        return connectors;
    }

}
