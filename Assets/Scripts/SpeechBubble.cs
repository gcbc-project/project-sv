using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Runtime.CompilerServices;
public class SpeechBubble : MonoBehaviour
{
    // Dictionary to store Name-Dialog pairs (with dialog as a List)
    static Dictionary<string, List<string>> dialogDictionary = new Dictionary<string, List<string>>();
    public GameObject tmpSpeechBubblePrefab;
    static GameObject speechBubblePrefab;
    // Start is called before the first frame update
    void Start()
    {
        // Path to the CSV file (place it in Assets/Resources folder for easy access)
        string filePath = Path.Combine(Application.dataPath, "Resources/csv/", "61315 게임잼 스프레드시트 - 상호작용대사.csv");
        speechBubblePrefab = tmpSpeechBubblePrefab;
        // Read the CSV file and populate the dictionary
        ReadCSV(filePath);
    }

    // Method to read CSV and populate the dictionary
    void ReadCSV(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            // Start from line 2 assuming the first line contains column headers
            for (int i = 2; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split(',');

                if (columns.Length >= 2)
                {
                    string name = columns[0];   // The "Name" column
                    string dialog = columns[1]; // The "Dialog" column

                    // If the name already exists, add the dialog to the list
                    if (dialogDictionary.ContainsKey(name))
                    {
                        dialogDictionary[name].Add(dialog);
                    }
                    else
                    {
                        // If the name doesn't exist, create a new list and add the dialog
                        dialogDictionary[name] = new List<string> { dialog };
                    }
                }
            }
        }
        else
        {
            Debug.LogError("CSV file not found at path: " + filePath);
        }
    }

    // Method to get a random dialog for a specific name
    public static IEnumerable GetRandomDialog(string buildingName,Transform CharacteTransform)
    {
        var tmp=Instantiate(speechBubblePrefab,CharacteTransform);
        tmp.transform.position = new Vector3(tmp.transform.position.x, tmp.transform.position.y+5, tmp.transform.position.z);


        if (dialogDictionary.ContainsKey(buildingName))
        {
            List<string> dialogs = dialogDictionary[buildingName];
            int randomIndex = Random.Range(0, dialogs.Count); // Random index within the list
            tmp.GetComponentInChildren<TextMeshProUGUI>().text = dialogs[randomIndex];
           
        }
        else
        {
            Debug.LogError("Name not found in the dictionary: " + buildingName);
            
        }
        yield return new WaitForSeconds(3f);
        Destroy(speechBubblePrefab);

    }
}
