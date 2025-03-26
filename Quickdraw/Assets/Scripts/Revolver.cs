using UnityEngine;
using UnityEngine.Audio;

public class Revolver : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject holoSight;
    [SerializeField] private LayerMask mask;
    [SerializeField] public bool showTrail = false;
    
    private CsvSaver csvSaver;
    private LineRenderer line;

    public AudioClip SoundMiss;
    public AudioClip SoundHit;

    private AudioSource audioSource;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        csvSaver = GetComponent<CsvSaver>();
        audioSource = GetComponent<AudioSource>();

    }

    public void Select()
    {
        Debug.Log("Select");
    }
    
    public void Deselect()
    {
        Debug.Log("Deselect");
    }

    public void Fire()
    {
        if (showTrail)
        {
            line.SetPosition(0, muzzle.position);
            line.SetPosition(1, muzzle.position + muzzle.forward * 100f);
        }

        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, 100f, mask))
        {
            line.startColor = line.endColor = Color.green;
            Vector3 impactPosition = hit.point;
            Vector3 targetPosition = hit.transform.position;
            float distance = Vector3.Distance(impactPosition, targetPosition);
            audioSource.clip = SoundHit;

            Target t = hit.transform.GetComponent<Target>();
            t.SpawnImpact(impactPosition);
            
            Debug.Log("Hit ! Distance: " + distance);

            csvSaver.SaveTargetShotToCsv(
                distance,
                Vector3.SignedAngle(impactPosition, targetPosition, Vector3.right)
            );
        }
        else
        {
            audioSource.clip = SoundMiss;
            line.startColor = line.endColor = Color.red;
        }
    }
    
    public void ToggleTrails()
    {
        showTrail = !showTrail;
    }
    
    public void ToggleSight()
    {
        holoSight.SetActive(!holoSight.activeSelf);
    }
}