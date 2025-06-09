using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Astra : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField] float distance = 5f;
    [SerializeField] private GameObject particles;
    [SerializeField] private List<GameObject> destroyables = new List<GameObject>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip horn;
    [SerializeField] private AudioClip crash;
    [SerializeField] private GameObject lights;

    private void Start()
    {
        startPos = transform.position;
        endPos = transform.position + transform.forward * distance;
        lights.SetActive(false);
        /*TurnOnLights();
        Honk();
        Invoke("Go", 6f);*/
    }

    public void TurnOnLights()
    {
        lights.SetActive(true);
    }
    public void TurnOffLights()
    {
        lights.SetActive(false);
    }

    public void Honk()
    {
        audioSource.clip = horn;
        audioSource.Play();
    }

    public void Go()
    {
        audioSource.clip = crash;
        audioSource.Play();
        Invoke("StartDriving", 0.5f);
    }

    void StartDriving()
    {
        StartCoroutine(Drive());
        Invoke("Boom", 0.5f);
    }

    void Boom()
    {
        Instantiate(particles, transform.position + transform.forward * 2, Quaternion.identity);
        foreach (GameObject g in destroyables)
        {
            Destroy(g);
        }
        GameObject.Find("GameOver").GetComponent<GameOver>().Over();
    }

    IEnumerator Drive()
    {
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = elapsed / duration;

            transform.position = Vector3.Lerp(startPos, endPos, x);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }
}
