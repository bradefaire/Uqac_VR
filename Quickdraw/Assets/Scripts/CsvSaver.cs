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
            writer.WriteLine("mean, deviation");
        }
        else
        {
            writer = new StreamWriter(path, append: true);
        }
    }

    public void SaveTargetShotToCsv(float mean, float deviation)
    {
        string formattedMean = mean.ToString(CultureInfo.InvariantCulture);
        string formattedDeviation = deviation.ToString(CultureInfo.InvariantCulture);
        writer.WriteLine(formattedMean + "," + formattedDeviation);
    }

    void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}
