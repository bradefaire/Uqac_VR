using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Revolver : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private TextMeshProUGUI precisionUIText;
    [SerializeField] private TextMeshProUGUI timeUIText;
    [SerializeField] private Timer timer;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject holoSight;
    [SerializeField] private LayerMask mask;
    private CsvSaver csvSaver;
    private LineRenderer line;
    private float targetRadius;
    private bool showTrail = true;
    public AudioClip SoundMiss;
    public AudioClip SoundHit;
    private AudioSource audioSource;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        csvSaver = GetComponent<CsvSaver>();
        targetRadius = target.GetComponent<CircleCollider2D>().radius * target.transform.localScale.x;
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

            StartCoroutine(HideTrail());
        }

        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(muzzle.position, muzzle.forward), 100f, mask);
        if (hit)
        {
            line.startColor = line.endColor = Color.green;

            // Récupération de la précision (distance au centre de la cible) :
            //  • 0 % = bord de la cible
            //  • 100 % = centre de la cible
            Vector3 impactPosition = new Vector3(hit.point.x, hit.point.y,  hit.transform.position.z);
            Vector3 targetPosition = hit.transform.position;
            float distance = Vector3.Distance(impactPosition, targetPosition);
            float normalizedDistance = 1 - distance / targetRadius;
            precisionUIText.text = $"{Mathf.RoundToInt(normalizedDistance * 100f)} %";

            // Récupération du temps de réaction
            float timerEndTime = timer.GetTimerEndTime();
            float reactionTime = -1f;
            if (timerEndTime != 0)
            {
                reactionTime = Time.time - timerEndTime;
                timeUIText.text = $"{System.Math.Round(reactionTime, 2)} s";
            }

            // Enregistrement des données dans le CSV
            csvSaver.SaveTargetShotToCsv(
                targetPosition.z,
                reactionTime,
                normalizedDistance
            );

            audioSource.PlayOneShot(SoundHit);

            Target t = hit.transform.GetComponent<Target>();
            t.SpawnImpact(impactPosition);
        }
        else
        {
            audioSource.PlayOneShot(SoundMiss);
            line.startColor = line.endColor = Color.red;
        }
    }

    IEnumerator HideTrail()
    {
        yield return new WaitForSeconds(1f);
        
        line.startColor = line.endColor = Color.clear;
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
