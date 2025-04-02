using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CustomNearFarInteractor : NearFarInteractor
{
    private bool allowDeselect = true;

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (!allowDeselect)
        {
            Debug.Log("Deselect is blocked.");
            return;
        }

        base.OnSelectExiting(args);
    }

    public void DisableDeselect()
    {
        allowDeselect = false;
    }
}