using UnityEngine;
using System.IO;
using System.Globalization;

public class CsvSaver : MonoBehaviour
{

    private StreamWriter writer;
    private string path;

    void Start()
    {
        path = Application.dataPath + "/data.csv";
        if (!File.Exists(path))
        {
            writer = new StreamWriter(path);
            writer.WriteLine("mean reaction time, reaction time deviation, mean accuracy, accuracy deviation");
        }
        else
        {
            writer = new StreamWriter(path, append: true);
        }
    }

    public void SaveTargetShotToCsv(float meanRT, float RTdeviation, float meanAccuracy, float accuracyDeviation)
    {
        string formattedMeanRT = meanRT.ToString(CultureInfo.InvariantCulture);
        string formattedRTdeviation = RTdeviation.ToString(CultureInfo.InvariantCulture);
        string formattedMeanAccuracy = meanAccuracy.ToString(CultureInfo.InvariantCulture);
        string formattedAccuracyDeviation = accuracyDeviation.ToString(CultureInfo.InvariantCulture);
        writer.WriteLine(formattedMeanRT + "," + formattedRTdeviation + "," + formattedMeanAccuracy + "," + formattedAccuracyDeviation);
    }

    void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}
