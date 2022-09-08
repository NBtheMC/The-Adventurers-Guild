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
        GameObject mapObj = GameObject.Find("Main UI/World Map");
        GameObject locationPrefab = Resources.Load<GameObject>("Location");
        GameObject edgePrefab = Resources.Load<GameObject>("Edge");

        //find all existing location objects, store them in dictionary for quick reference later
        Dictionary<string, GameObject> locations = new Dictionary<string, GameObject>(); 
        foreach(Transform child in mapObj.transform.GetChild(2))
        {
            locations.Add(child.name, child.gameObject);
        }

        //do the same for edges
        Dictionary<string, GameObject> edges = new Dictionary<string, GameObject>();
        foreach (Transform child in mapObj.transform.GetChild(1))
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
                GameObject loc = IL.CreateObject(parts[0], mapObj.transform.GetChild(2), locationPrefab, locations);
                var sprite = Resources.Load<Sprite>("MapAssets/" + parts[0]);

                if (sprite != null)
                {
                    loc.GetComponent<Image>().sprite = sprite;
                    loc.GetComponent<Image>().preserveAspect = true;
                    loc.transform.GetChild(0).gameObject.SetActive(false);
                }
                    
            }
            //check if location 2 exists
            if (!locations.ContainsKey(parts[1]))
            {
                GameObject loc = IL.CreateObject(parts[1], mapObj.transform.GetChild(2), locationPrefab, locations);
                var sprite = Resources.Load<Sprite>("MapAssets/" + parts[1]);

                if (sprite != null)
                {
                    loc.GetComponent<Image>().sprite = sprite;
                    loc.GetComponent<Image>().preserveAspect = true;
                    loc.transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            //check if edge exists
            if(!(edges.ContainsKey($"{parts[0]}-{parts[1]}") || edges.ContainsKey($"{parts[1]}-{parts[0]}")))
            {
                GameObject edge = IL.CreateObject($"{parts[0]}-{parts[1]}", mapObj.transform.GetChild(1), edgePrefab, edges);
                edge.GetComponent<EdgeObject>().Node1 = locations[parts[0]].GetComponent<LocationObject>();
                edge.GetComponent<EdgeObject>().Node2 = locations[parts[1]].GetComponent<LocationObject>();

                var sprite = Resources.Load<Sprite>($"MapAssets/{parts[0]}-{parts[1]}");
                if(sprite == null)
                    sprite = Resources.Load<Sprite>($"MapAssets/{parts[1]}-{parts[0]}");

                if (sprite != null)
                {
                    edge.GetComponent<Image>().sprite = sprite;
                    edge.GetComponent<Image>().preserveAspect = true;
                }
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
