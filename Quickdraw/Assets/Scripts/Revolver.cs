using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private LayerMask mask;
    private CsvSaver csvSaver;

    private LineRenderer line;
    
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        csvSaver = GetComponent<CsvSaver>();
    }

    public void Select()
    {
        Debug.Log("Select");
        // Disable controller visual and lock grip
    }
    
    public void Deselect()
    {
        Debug.Log("Deselect");
    }

    public void Fire()
    {
        Debug.Log("Bang");
        
        line.SetPosition(0, muzzle.position);
        line.SetPosition(1, muzzle.position + muzzle.forward * 100f);
        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, 100f, mask))
        {
            line.startColor = line.endColor = Color.green;
            Vector3 impactPosition = hit.point;
            Vector3 targetPosition = hit.transform.position;
            float distance = Vector3.Distance(impactPosition, targetPosition);
            Debug.Log("Hit ! Distance: " + distance);
            csvSaver.SaveTargetShotToCsv(
                distance,
                Vector3.SignedAngle(impactPosition, targetPosition, Vector3.right)
            );
        }
        else
        {
            line.startColor = line.endColor = Color.red;
            Debug.Log("Miss !");
        }
    }
}
