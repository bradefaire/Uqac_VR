using UnityEngine;

public class HideController : MonoBehaviour
{
    [SerializeField] private GameObject controller;
    
    public void Show()
    {
        controller.SetActive(true);
    }
    
    public void Hide()
    {
        controller.SetActive(false);
    }
}
