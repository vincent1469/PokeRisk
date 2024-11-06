using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class child : MonoBehaviour
{
    // determine sprite placement
    [SerializeField] private Transform target;
    private Camera mainCamera;
    private Vector3 offset = new Vector3(0, 0, 0);
    public void setOffset(Vector3 newOffset) { offset = newOffset; }

    // collision variables on certain attacks
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider;
    private GameObject game;
    private GameObject player;
    private oppcontroller attacker;
    public void setAttacker(oppcontroller opp) { attacker = opp; }
    private string attack;
    public void setAttack(string move) { attack = move; }
    
    void Awake() {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        if (rb) rb.isKinematic = true;
        polygonCollider = GetComponent<PolygonCollider2D>();
        game = Camera.main.gameObject;
        risk mc = game.GetComponent<risk>();
        player = mc.newPlayer;
    }

    void Update() {
        transform.rotation = mainCamera.transform.rotation;
        transform.position = target.position + offset;  
    }

    private void OnCollisionEnter2D(Collision2D collision) { 
        if (collision.gameObject == player) {
            if (attack == "magnitude") attacker.MagnitudeHit();
        } 
    }
}
