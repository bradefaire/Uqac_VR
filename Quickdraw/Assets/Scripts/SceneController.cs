using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private Revolver revolver;
    [SerializeField] private Timer timer;

    [SerializeField] private Slider distanceSlider;
    [SerializeField] private TMP_Text distanceText;
    
    public void SetTargetDistance(float distance)
    {
        target.SetDistance(distance);
        distanceText.text = distance.ToString();
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
