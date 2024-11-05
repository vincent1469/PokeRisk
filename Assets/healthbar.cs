using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0, 0.4f, 0);
    
    public void UpdateHealthBar(float currentValue, float maxValue) { slider.value = currentValue / maxValue; }

    void Awake() {
        mainCamera = Camera.main;
    }

    void Update() {
        transform.rotation = mainCamera.transform.rotation;
        transform.position = target.position + offset;  
    }
}
