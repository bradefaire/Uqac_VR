using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject impact;

    private List<GameObject> impacts = new List<GameObject>();
    
    public void SetDistance(float distance)
    {
        transform.position = new Vector3(0, 1, distance);
    }

    public void SpawnImpact(Vector3 position)
    {
        impacts.Add(Instantiate(impact, position + new Vector3(0, 0, -0.02f), Quaternion.identity, transform));
    }
    
    public void ClearImpacts()
    {
        foreach (var i in impacts)
        {
            Destroy(i);
        }
        
        impacts.Clear();
    }
}