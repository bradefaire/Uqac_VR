using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timerDuration;
    float timerEndTime;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timerDuration = Random.Range(6, 15);
    }

    private IEnumerator CountDown()
    {
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
        float endTime = timerEndTime;
        timerEndTime = 0;
        return endTime;
    }
}
