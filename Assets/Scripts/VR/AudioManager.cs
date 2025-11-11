using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Central manager for playing unique audio clips by their name ID.
/// Requires an AudioSource component on the same GameObject.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Tooltip("Drag all AudioClips you want to use here.")]
    public AudioClip[] audioClips;
    public GameObject[] statuUis;

    // The component responsible for playing the sound
    private AudioSource audioSource;
    public static AudioManager instance;
    
    // Dictionary to map clip names (IDs) to the actual AudioClip objects
    private Dictionary<string, AudioClip> audioMap = new Dictionary<string, AudioClip>();
    private Dictionary<string, GameObject> statuUiMap = new Dictionary<string, GameObject>();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PopulateAudioMap();
        PopulateUIMap();
        StartCoroutine(UiUnActive());
    }
    
    /// <summary>
    /// Fills the audioMap dictionary from the public array.
    /// </summary>
    private void PopulateAudioMap()
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip != null)
            {
                // Use the clip's file name as the unique ID
                string clipID = clip.name;

                if (!audioMap.ContainsKey(clipID))
                {
                    audioMap.Add(clipID, clip);
                }
                else
                {
                    Debug.LogWarning($"[AudioManager] Duplicate audio clip name found: {clipID}. Only the first one will be used.");
                }
            }
        }
        Debug.Log($"[AudioManager] Loaded {audioMap.Count} unique audio clips.");
    }
    private void PopulateUIMap()
    {
        foreach(GameObject ui in statuUis)
        {
            if(ui != null)
            {
                string uiID = ui.name;
                if(!statuUiMap.ContainsKey(uiID))
                {
                    statuUiMap.Add(uiID, ui);
                }
                else
                {
                    Debug.LogWarning($"[AudioManager] Duplicate UI name found: {uiID}. Only the first one will be used.");
                }
            }
        }
    }

    /// <summary>
    /// Stops the current audio and plays the new clip corresponding to the given ID.
    /// </summary>
    /// <param name="audioID">The name/ID of the audio clip to play.</param>
    public void PlayAudioByID(string audioID)
    {
        if (audioMap.TryGetValue(audioID, out AudioClip clipToPlay))
        {
            // CRITICAL STEP: Stop any currently playing audio
            
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Assign the new clip and play it
            audioSource.clip = clipToPlay;
            audioSource.Play();
            Debug.Log($"Playing audio clip: {audioID}");
        }
        else
        {
            Debug.LogError($"[AudioManager] Audio ID '{audioID}' not found in the library.");
        }
    }
    public void PlayUIbyID(string UIid)
    {
        if (statuUiMap.TryGetValue(UIid, out GameObject UiTOPlay))

        { UiTOPlay.SetActive(true);
    
            Debug.Log($"UI: {UiTOPlay.name} is Activated");
        }
        else
        {
            Debug.LogError($"[AudioManager] UI ID '{UiTOPlay}' not found in the library.");
        }
    }


    IEnumerator UiUnActive()
    {
        while (true)
        {
            foreach (KeyValuePair<string, GameObject> entry in statuUiMap)
            {
                GameObject uiElement = entry.Value;
                if (uiElement.activeSelf)
                {
                    // Assuming the audioID matches the UI name
                    string audioID = entry.Key;
                    if (!audioSource.isPlaying)
                    {
                        uiElement.SetActive(false);
                        Debug.Log($"UI: {uiElement.name} is Deactivated");
                    }
                }
            }
             yield return new WaitForSeconds(0.5f); 
        }
        
    }
    
}