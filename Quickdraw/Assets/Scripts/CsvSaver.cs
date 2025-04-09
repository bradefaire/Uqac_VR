using UnityEngine;
using System.IO;
using System.Globalization;

public class CsvSaver : MonoBehaviour
{

    private StreamWriter writer;
    private string path;

    void Start()
    {
        path = Application.dataPath + "/" + System.DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".csv";
        if (!File.Exists(path))
        {
            writer = new StreamWriter(path);
            writer.WriteLine("target distance, reaction time, accuracy");
        }
        else
        {
            writer = new StreamWriter(path, append: true);
        }
    }

    public void SaveTargetShotToCsv(float targetDistance, float reactionTime, float accuracy)
    {
        string formattedTargetDistance = targetDistance.ToString(CultureInfo.InvariantCulture);
        string formattedReactionTime = reactionTime.ToString(CultureInfo.InvariantCulture);
        string formattedAccuracy = accuracy.ToString(CultureInfo.InvariantCulture);
        writer.WriteLine(formattedTargetDistance + "," + formattedReactionTime + "," + formattedAccuracy);
    }

    void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}
