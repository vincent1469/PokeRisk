using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class risk : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject background;
    private Camera mainCamera;
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

        // instantiate background
        mainCamera = GetComponent<Camera>();
        AdjustCameraToBackground();
    }

    // simple audio commands 
    public void play() { if (audioSource != null && !audioSource.isPlaying) audioSource.Play(); }
    public void stop() { if (audioSource != null && !audioSource.isPlaying) audioSource.Stop(); }
    public void setVolume(float newVolume) { if (audioSource != null) audioSource.volume = Mathf.Clamp(newVolume, 0f, 1f); }

    // function to adjust camera to the background
    void AdjustCameraToBackground() {
        SpriteRenderer backgroundSprite = background.GetComponent<SpriteRenderer>();

        // use bounds of background
        Bounds backgroundBounds = backgroundSprite.bounds;
        float backgroundWidth = backgroundBounds.size.x;
        float backgroundHeight = backgroundBounds.size.y;

        // calculate aspect ratios
        float aspectScreen = (float)Screen.width / Screen.height;
        float aspectBackground = backgroundWidth / backgroundHeight;

        // fix orthographic size
        if (aspectScreen >= aspectBackground) mainCamera.orthographicSize = backgroundHeight / 2;
        else mainCamera.orthographicSize = (backgroundWidth / aspectScreen) / 2;

        // reposition camera
        mainCamera.transform.position = new Vector3(
            backgroundBounds.center.x,
            backgroundBounds.center.y,
            mainCamera.transform.position.z
        );
    }
}
