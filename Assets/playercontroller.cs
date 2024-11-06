using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpriteGlow;

public class playercontroller : MonoBehaviour
{
    // create variable animator and its collision
    private SpriteRenderer spriteRenderer;
    private SpriteGlowEffect glow;
    private Animator animator;
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider;

    // create movement variables
    private Vector2 movement;
    private float speed;
    private Vector2 minBounds = new Vector2(-8.5f, -4.5f);
    private Vector2 maxBounds = new Vector2(8.5f, 4.5f);

    // create behavior variables
    private string pokemon;
    private typechart types;
    public string getPokemon() { return pokemon; }
    private int health;
    private int maxHealth;
    private healthbar healthBar;
    private int power;
    public int getPower() { return power; }
    private typechart.TYPE attackingType = typechart.TYPE.NA;
    public typechart.TYPE getAttack() { return attackingType; }
    private float attackCooldown;
    public float getCooldown() { return attackCooldown; }
    [SerializeField] private AudioClip thundershock;
    private AudioSource attackSoundEffect;
    public AudioSource getAudio() { return attackSoundEffect; }

    void Awake() {
        // get animator and collision from the inspector
        transform.position = new Vector3(0, 0, 0); // start in center
        spriteRenderer = GetComponent<SpriteRenderer>();
        glow = GetComponent<SpriteGlowEffect>();
        animator = GetComponent<Animator>();
        animator.speed = 1f; // would be 0.66 but then it updates slow
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        healthBar = GetComponentInChildren<healthbar>();
        attackSoundEffect = gameObject.AddComponent<AudioSource>();
        attackEffects(false);
        playerPikachu();
    }

    void Update() {
        DirectionalMovement(pokemon);
    }

    void FixedUpdate() {
        if (rb != null) clamp();
    }

    void clamp() {
        Vector2 newPosition = rb.position + movement * speed * Time.fixedDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        rb.MovePosition(newPosition);
    }

    void DirectionalMovement(string type) {
        // reset parameters to false
        animator.SetBool($"{type}_0", false); // down
        animator.SetBool($"{type}_1", false); // left
        animator.SetBool($"{type}_2", false); // right
        animator.SetBool($"{type}_3", false); // up

        // get input from movement keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // change animation based on movement keys
        if (movement.x > 0) animator.SetBool($"{type}_2", true); // right
        else if (movement.x < 0) animator.SetBool($"{type}_1", true); // left
        if (movement.y > 0) animator.SetBool($"{type}_3", true); // up
        else if (movement.y < 0) animator.SetBool($"{type}_0", true); // down
        UpdatePolygonCollider();
    }

    // update polygon collider based on animation's shape
    void UpdatePolygonCollider() {
        if (polygonCollider != null && spriteRenderer.sprite != null)
        {
            polygonCollider.pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();
            for (int i = 0; i < polygonCollider.pathCount; i++)
            {
                List<Vector2> path = new List<Vector2>();
                spriteRenderer.sprite.GetPhysicsShape(i, path);
                polygonCollider.SetPath(i, path.ToArray());
            }
        }
    }

    public void attackEffects(bool attacking) {
        glow.OutlineWidth =  attacking ? 1 : 0;
    }

    public void TakeDamage(int damage, typechart.TYPE attackedType) {
        health -= types.FindEffectiveness(damage, attackedType, types.getType1(), types.getType2());
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0) playerDeath();
        clamp();
    }

    private void playerDeath() {
        SceneManager.LoadScene("risk");
    }

    public void playerPikachu() {
        pokemon = "025";
        types = new typechart("ELECTRIC");
        animator.SetBool("025_0", true);
        glow.GlowColor = ColorUtility.TryParseHtmlString("#FDF23D", out var color) ? color : glow.GlowColor;
        speed = 8f;
        health = 30;
        maxHealth = 30;
        healthBar.UpdateHealthBar(health, maxHealth);
        attackingType = typechart.TYPE.ELECTRIC;
        power = 14;
        attackCooldown = 0.33f;
        attackSoundEffect.clip = thundershock;
    }
}
