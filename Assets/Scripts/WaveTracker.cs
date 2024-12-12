using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveTracker : MonoBehaviour
{
    [Header("Dynamic")]
    private int wave = 1;
    public int getWave() { return wave; }
    private float waveInterval = 15f;
    private float startTime;
    private float timePassed = 0f;
    [SerializeField] private Slider slider;
    TextMeshProUGUI gt;

    void Start() {
        gt = GetComponent<TextMeshProUGUI>();
        startTime = Time.time;
        StartCoroutine(WaveCooldown());
    }

    void Update() {
        gt.SetText("WAVE " + wave.ToString("#,0"));
        UpdateSlider(Time.time - startTime - timePassed, waveInterval);
    }

    private IEnumerator WaveCooldown() {
        while (true) {
            yield return new WaitForSeconds(waveInterval);
            timePassed += waveInterval;
            wave++;
        }
    }

    private void UpdateSlider(float currentValue, float maxValue) { slider.value = currentValue / maxValue; }
}
