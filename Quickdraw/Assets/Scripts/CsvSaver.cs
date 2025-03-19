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
            writer.WriteLine("radius, angle");
        }
        else
        {
            writer = new StreamWriter(path, append: true);
        }
    }

    public void SaveTargetShotToCsv(float radius, float angle)
    {
        string formattedRadius = radius.ToString(CultureInfo.InvariantCulture);
        string formattedAngle = angle.ToString(CultureInfo.InvariantCulture);
        writer.WriteLine(formattedRadius + "," + formattedAngle);
    }

    void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}
