using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private Revolver revolver;
    [SerializeField] private Timer timer;

    [SerializeField] private Slider distanceSlider;
    
    public void SetTargetDistance(float distance)
    {
        target.SetDistance(distance);
    }
    
    public void ClearImpacts()
    {
        target.ClearImpacts();
    }

    public void ToggleTrails()
    {
        revolver.ToggleTrails();
    }
    
    public void ToggleSight()
    {
        revolver.ToggleSight();
    }

    public void ReStart()
    {
        timer.StartTimer();
    }
}
