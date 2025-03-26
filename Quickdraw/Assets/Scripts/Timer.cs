using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float startTimer;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startTimer = Random.Range(6, 15);
    }

    private IEnumerator CountDown()
    {

        yield return new WaitForSeconds(startTimer);
        audioSource.Play();
        yield return null;
    }

    public void StartTimer()
    {
        StartCoroutine(CountDown());
    }
}
