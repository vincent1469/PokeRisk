using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class risk : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public GameObject newPlayer;
    [SerializeField] private AudioClip biome_field;
    private AudioSource audioSource;

    void Awake() {
        // create player
        newPlayer = Instantiate<GameObject>(playerPrefab);

        // instantiate background music
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = biome_field;
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        play();
    }

    // simple commands 
    public void play() { if (audioSource != null && !audioSource.isPlaying) audioSource.Play(); }
    public void stop() { if (audioSource != null && !audioSource.isPlaying) audioSource.Stop(); }
    public void setVolume(float newVolume) { if (audioSource != null) audioSource.volume = Mathf.Clamp(newVolume, 0f, 1f); }
}
