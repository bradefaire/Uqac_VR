using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private LayerMask mask;
    private CsvSaver csvSaver;
    private LineRenderer line;
    private int shots;
    private List<float> impactDistances;
    
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        csvSaver = GetComponent<CsvSaver>();
        impactDistances = new List<float>();
    }

    public void Select()
    {
        shots = 0;
        Debug.Log("Select");
        // Disable controller visual and lock grip
    }
    
    public void Deselect()
    {
        Debug.Log("Deselect");      
        csvSaver.SaveTargetShotToCsv(
            impactDistances.Count > 0 ? impactDistances.Average() : 0,
            impactDistances.Count > 0 ? ComputeStandardDeviation(impactDistances) : 0
        );
    }

    public void Fire()
    {
        line.SetPosition(0, muzzle.position);
        line.SetPosition(1, muzzle.position + muzzle.forward * 100f);
        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, 100f, mask))
        {
            line.startColor = line.endColor = Color.green;
            Vector3 impactPosition = hit.point;
            Vector3 targetPosition = hit.transform.position;
            float targetRadius = hit.collider.bounds.extents.magnitude;
            float distance = Vector3.Distance(impactPosition, targetPosition);
            float normalizedDistance = distance / targetRadius;
            Debug.Log("Target radius : " + targetRadius);
            impactDistances.Add(normalizedDistance);
            //Debug.Log("Hit ! Distance : " + normalizedDistance);
            Debug.Log("Raw distance : " + distance);
            shots++; 
        }
        else
        {
            line.startColor = line.endColor = Color.red;
        }
    }

    private float ComputeStandardDeviation(List<float> values)
        {
            if (values.Count == 0) return 0;
    
            float average = values.Average();
            float sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            return Mathf.Sqrt(sumOfSquaresOfDifferences / values.Count);
        }
}
