using UnityEngine;

public class StatuSelection : MonoBehaviour
{
    public GameObject statuUi;
    public AudioSource statuAudioSource;
    void Start()
    {
        statuUi.SetActive(false);
    }

    void Update()
    {
        if(!statuAudioSource.isPlaying)
        {
            statuUi.SetActive(false);
        }
    } 

    public void SelectStatu()
    {
        statuUi.SetActive(true);
        statuAudioSource.Play();
    }
}
