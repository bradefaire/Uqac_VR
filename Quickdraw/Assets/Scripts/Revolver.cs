using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject holoSight;
    [SerializeField] private LayerMask mask;
    private CsvSaver csvSaver;
    private LineRenderer line;
    private int shots;
    private List<float> impactDistances;
    private float targetRadius;
    
    private bool showTrail = false;
    private CsvSaver csvSaver;
    private LineRenderer line;
    private int shots;
    private List<float> impactDistances;
    private float targetRadius;

    public AudioClip SoundMiss;
    public AudioClip SoundHit;

    private AudioSource audioSource;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        csvSaver = GetComponent<CsvSaver>();
        impactDistances = new List<float>();
        targetRadius = target.GetComponent<Collider>().bounds.extents.magnitude;
        audioSource = GetComponent<AudioSource>();
    }

    public void Select()
    {
        shots = 0;
        Debug.Log("Select");
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
        if (showTrail)
        {
            line.SetPosition(0, muzzle.position);
            line.SetPosition(1, muzzle.position + muzzle.forward * 100f);
        }

        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(muzzle.position, muzzle.forward), 100f, mask);
        // if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, 100f, mask))
        if (hit)
        {
            line.startColor = line.endColor = Color.green;
            Vector3 impactPosition = new Vector3(hit.point.x, hit.point.y,  hit.transform.position.z);
            Vector3 targetPosition = hit.transform.position;
            float distance = Vector3.Distance(impactPosition, targetPosition);
            float normalizedDistance = distance / targetRadius;
            impactDistances.Add(normalizedDistance);
            shots++; 
            Debug.Log("Hit ! Distance: " + distance);
            csvSaver.SaveTargetShotToCsv(
                distance,
                Vector3.SignedAngle(impactPosition, targetPosition, Vector3.right)
            );

            audioSource.clip = SoundHit;

            Target t = hit.transform.GetComponent<Target>();
            t.SpawnImpact(impactPosition);
        }
        else
        {
            audioSource.clip = SoundMiss;
            line.startColor = line.endColor = Color.red;
        }
        
        audioSource.Play();
    }

    private float ComputeStandardDeviation(List<float> values)
        {
            if (values.Count == 0) return 0;
    
            float average = values.Average();
            float squareDifferencesSum = values.Select(val => (val - average) * (val - average)).Sum();
            return Mathf.Sqrt(squareDifferencesSum / values.Count);
        }
}
