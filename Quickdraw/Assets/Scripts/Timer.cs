using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float timerDuration;
    float timerEndTime;
    private AudioSource audioSource;
    
    [SerializeField] private Button startButton;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timerDuration = Random.Range(6, 15);
    }

    private IEnumerator CountDown()
    {
        startButton.interactable = false;
        yield return new WaitForSeconds(timerDuration);
        timerEndTime = Time.time;
        audioSource.Play();
        yield return null;
    }

    public void StartTimer()
    {
        StartCoroutine(CountDown());
    }

    public float GetTimerEndTime()
    {
        startButton.interactable = true;
        float endTime = timerEndTime;
        timerEndTime = 0;
        return endTime;
    }
}
