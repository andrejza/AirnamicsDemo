using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEditor;


// Singleton implementation of the Assembly
public class Assembly : MonoBehaviour
{
    public static Assembly Instance { get; private set; }

    public enum EditModeOption { None, Dropdown, Position, Orientation };

    public Canvas canvas;
    public GameObject dropdownPrefab;
    public EditModeOption editMode { get; set; }
    public Part part { get; set; }


    private TextMeshProUGUI labelInfo, labelWarning, labelError;
    private GameObject dropdownPrefabInstance;
    private string[] partPrefabsGuids;
    private List<string> partList;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;

        }

    }


    // Start is called before the first frame update
    void Start()
    {
        editMode = EditModeOption.None;;
        labelInfo = canvas.transform.Find("LabelInfo").GetComponent<TextMeshProUGUI>();
        labelWarning = canvas.transform.Find("LabelWarning").GetComponent<TextMeshProUGUI>();
        labelError = canvas.transform.Find("LabelError").GetComponent<TextMeshProUGUI>();
        Debug.Log(labelInfo.name);

        // Gather part prefabs
        GatherPartPrefabs();

    }

   

    private void TweetToLabel( TextMeshProUGUI label, string text )
    {

        label.CrossFadeAlpha(1.0f, 0.05f, false);
        label.text = text;
        StartCoroutine(FadeLabel(label, text.Length * 0.05f));

    }

    private IEnumerator FadeLabel( TextMeshProUGUI label, float delay)
    {        
        yield return new WaitForSeconds(delay);
        label.CrossFadeAlpha(0.0f, 0.5f, false);
    }
   

    private void GatherPartPrefabs()
    {
        
        partPrefabsGuids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/Prefabs/Parts" });
        partList = new List<string>();
        foreach (string partGuid in partPrefabsGuids)
        {
            string partPath = AssetDatabase.GUIDToAssetPath(partGuid);
            string partName = Path.GetFileNameWithoutExtension(partPath);

            partList.Add(partName);                
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (editMode == EditModeOption.Dropdown)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                editMode = EditModeOption.None;
                Destroy(dropdownPrefabInstance);
            }
            
        }


        if (editMode == EditModeOption.Position)
        {
            TweetToLabel(labelInfo, "You are in Edit Position mode. Please press Esc to exit or Enter to confirm.");
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                // Exit Edit mode
                editMode = EditModeOption.None;
                TweetToLabel(labelInfo, "");
            }

            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                editMode = EditModeOption.Orientation;                

                // Hide out connector
                part.connectedTo.SetActive(false);
            }
            else
            {

                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    if (part != null)
                    {
                        part.connectorIndex = (part.connectorIndex + 1) % part.connectors.Count;
                        //Debug.Log("left " + part.connectorIndex + " "+ part.connectors.Count);
                        PositionPart();
                    }

                }
                else if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if (part != null)
                    {
                        part.connectorIndex = Mathf.Abs((part.connectorIndex - 1) % part.connectors.Count);
                        //Debug.Log("right " + part.connectorIndex + " " + part.connectors.Count);
                        PositionPart();
                    }

                }
            }

        }
        else if (editMode == EditModeOption.Orientation)
        {
            TweetToLabel(labelInfo, "You are in Edit Orientation mode. Please press Esc to exit or Enter to confirm.");
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                // Exit Edit mode
                editMode = EditModeOption.None;
                TweetToLabel(labelInfo, "");
            }

            if (Input.GetKeyUp(KeyCode.KeypadEnter)) // End of edit
            {
                editMode = EditModeOption.None;
                TweetToLabel(labelInfo, "");
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    OrientPart(part.connectedTo.GetComponent<ConnectorOut>().rotationStep);

                }
                else if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    OrientPart(-part.connectedTo.GetComponent<ConnectorOut>().rotationStep);

                }
            }
            

        }
            

    }

    private void PositionPart()
    {
        part.transform.position = part.connectedTo.transform.parent.position;
        part.transform.position = part.connectedTo.transform.parent.transform.position + part.connectedTo.transform.position - part.connectors[part.connectorIndex].transform.position;
    }

    private void OrientPart(float degrees)
    {
        part.transform.RotateAround(part.connectedTo.transform.position, part.connectors[part.connectorIndex].transform.right, degrees);
    }

    public void ChoosePart(GameObject connectorOut)    
    {
        if (editMode != EditModeOption.None)
        {
            TweetToLabel(labelInfo, "You are in Edit mode. Please press Esc to exit.");
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(connectorOut.transform.position);

        // Show dropdown at screenPos
        if (dropdownPrefab!=null)
        {
            dropdownPrefabInstance = Instantiate(dropdownPrefab);
            dropdownPrefabInstance.transform.SetParent(canvas.transform);

            RectTransform rectTransform = dropdownPrefabInstance.GetComponent<RectTransform>();
            rectTransform.position = screenPos + Vector3.right * rectTransform.rect.width/2.0f;

            TMP_Dropdown dropdown = dropdownPrefabInstance.GetComponent<TMP_Dropdown>();

            // Fill dropdown with part prefabs
            foreach (string partName in partList)
            {
                TMP_Dropdown.OptionData item = new TMP_Dropdown.OptionData();
                item.text = partName;
                dropdown.options.Add(item);
            }

            dropdown.onValueChanged.AddListener(delegate { 
                OnPartChosen(dropdown, connectorOut);             
            });

            editMode = EditModeOption.Dropdown;


        }

    }
    
    private void OnPartChosen(TMP_Dropdown dropdown, GameObject connectorOut)
    {
        //Debug.Log(dropdown.value.ToString());
        
        string path = AssetDatabase.GUIDToAssetPath(partPrefabsGuids[dropdown.value-1]);
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        GameObject partInstance = Instantiate(go, connectorOut.transform.parent);
        part = partInstance.AddComponent<Part>();
        part.connectorIndex = 0;
        part.connectedTo = connectorOut;

        partInstance.transform.position = part.connectedTo.transform.parent.transform.position + part.connectedTo.transform.position - part.connectors[0].transform.position;        
        Vector3 rotation = part.connectedTo.transform.rotation.eulerAngles - part.connectors[part.connectorIndex].transform.rotation.eulerAngles ;
        partInstance.transform.RotateAround(part.connectedTo.transform.position, Vector3.right, rotation.x);
        partInstance.transform.RotateAround(part.connectedTo.transform.position, Vector3.up, rotation.y);        
        partInstance.transform.RotateAround(part.connectedTo.transform.position, Vector3.forward, rotation.z);


        // Enter edit mode
        editMode = EditModeOption.Position;

        //editMode = EditMode.Orientation;

        Destroy(dropdownPrefabInstance);
    }



}


