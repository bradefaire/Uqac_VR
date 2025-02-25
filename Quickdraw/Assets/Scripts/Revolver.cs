using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private LayerMask mask;

    private LineRenderer line;
    
    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Fire()
    {
        Debug.Log("Bang");
        
        line.SetPosition(0, muzzle.position);
        line.SetPosition(1, muzzle.position + muzzle.forward * 100f);
        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, 100f, mask))
        {
            line.startColor = line.endColor = Color.green;
            Debug.Log("Hit ! Distance: " + Vector3.Distance(hit.transform.position, hit.point));
        }
        else
        {
            line.startColor = line.endColor = Color.red;
            Debug.Log("Miss !");
        }
    }
}
