using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ImportLocations : MonoBehaviour
{
    [MenuItem("World Map/Import Locations")]
    static void ImportFromCSV()
    {
        GameObject mapObj = GameObject.Find("Canvas/World Map");
        WorldMap map = new WorldMap();
        GameObject locationPrefab = Resources.Load<GameObject>("Location");
        GameObject edgePrefab = Resources.Load<GameObject>("Edge");

        //find all existing location objects, store them in dictionary for quick reference later
        Dictionary<string, GameObject> locations = new Dictionary<string, GameObject>(); 
        foreach(Transform child in mapObj.transform.GetChild(1))
        {
            locations.Add(child.name, child.gameObject);
        }

        //do the same for edges
        Dictionary<string, GameObject> edges = new Dictionary<string, GameObject>();
        foreach (Transform child in mapObj.transform.GetChild(0))
        {
            edges.Add(child.name, child.gameObject);
        }

        //split csv into lines
        TextAsset LocationsCSV = Resources.Load<TextAsset>("Locations");
        string locationsText = LocationsCSV.text;
        string[] lines = locationsText.Split('\n');

        ImportLocations IL = new ImportLocations();

        foreach (string line in lines)
        {
            //split line into location, location, travelTime
            string[] parts = line.Split(',');

            //check if location 1 exists
            if (!locations.ContainsKey(parts[0]))
            {
                GameObject loc = IL.CreateObject(parts[0], mapObj.transform.GetChild(1), locationPrefab, locations);
                loc.transform.GetChild(0).GetComponent<Text>().text = parts[0];

            }
            //check if location 2 exists
            if (!locations.ContainsKey(parts[1]))
            {
                GameObject loc = IL.CreateObject(parts[1], mapObj.transform.GetChild(1), locationPrefab, locations);
                loc.transform.GetChild(0).GetComponent<Text>().text = parts[1];
            }

            //check if edge exists
            if(!(edges.ContainsKey(parts[0] + "-" + parts[1]) || edges.ContainsKey(parts[1] + "-" + parts[0])))
            {
                IL.CreateObject(parts[0] + "-" + parts[1], mapObj.transform.GetChild(0), edgePrefab, edges);
            }

        }      
    }

    private GameObject CreateObject(string objName, Transform parentTransform, GameObject prefab, Dictionary<string, GameObject>dict)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(parentTransform, false);
        go.name = objName;

        dict.Add(go.name, go);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

        return go;
    }
}
