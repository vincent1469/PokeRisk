using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exclamation : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0, 0.01f, 0);
    
    void Awake() {
        mainCamera = Camera.main;
    }

    void Update() {
        transform.rotation = mainCamera.transform.rotation;
        transform.position = target.position + offset;  
    }
}
