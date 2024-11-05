using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    // initialize attack field
    private GameObject game;
    private GameObject player;
    [SerializeField] private LayerMask enemyLayer;
    private SpriteRenderer spriteRenderer;
    private playercontroller attacking;
    private AudioSource attackSoundEffect;
    private float attackRadius;
    private float attackCooldown;
    private float lastAttack = 0f;

    void Start() { // start instead of awake to let main camera create player
        // create transparent attack field
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetTransparency(0.5f);
        attackRadius = spriteRenderer.sprite.bounds.extents.x;

        // get player
        game = Camera.main.gameObject;
        risk mc = game.GetComponent<risk>();
        player = mc.newPlayer;
        attacking = player.GetComponent<playercontroller>();
        attackCooldown = attacking.getCooldown();
        attackSoundEffect = attacking.getAudio();
        attackSoundEffect.volume = 0.5f;
    }

    void Update() {
        // follow mouse when not on cooldown
        if (Time.time >= lastAttack + attackCooldown) {
            SetTransparency(0.5f);
            FollowMouse();
            if (Input.GetMouseButtonDown(0)) { // check for left mouse click
                Attack();
                lastAttack = Time.time;
                SetTransparency(0.9f);
            }
        } else { SetTransparency(0.9f); }
    }

    void FollowMouse() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }

    void Attack() {
        // get all colliders within the attack radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        // attack each enemy
        foreach (Collider2D enemy in hitEnemies) {
            oppcontroller attacked = enemy.GetComponent<oppcontroller>();
            if (attacked != null) {
                attacked.TakeDamage(attacking.getPower(), attacking.getAttack());
                attackSoundEffect.Play();
            }
        }
    }

    // set transparency to alpha value
    private void SetTransparency(float alpha) {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
