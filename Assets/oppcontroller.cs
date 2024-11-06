using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oppcontroller : MonoBehaviour
{
    // create animator and its collision
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider;

    // create movement variables
    private float speed;
    private enum Direction { X_Pos = 1, X_Neg = 2, Y_Pos = 3, Y_Neg = 4 }
    private Direction currentDirection = Direction.X_Pos;
    private float xBorder = 8.5f;
    private float yBorder = 4.5f;

    // create behavior variables
    private bool justSpawned = false;
    private string pokemon;
    public string getPokemon() { return pokemon; }
    private typechart types;
    private int health;
    private int maxHealth;
    private healthbar healthBar; 
    private enum Behavior {
        Melee_Passive = 1,
        Melee_QuickAttack = 2
    }
    private Behavior previousBehavior;
    private Behavior currentBehavior;

    // create attack variables
    private ScoreCounter scoreCounter;
    private HighScore highScore;
    private GameObject game;
    private GameObject player;
    private int meleePower;
    private float meleeRange;
    private float meleeCooldown;
    private float lastAttack = 0f;
    private bool playerCollision = false;
    [SerializeField] private AudioClip quickattack;
    [SerializeField] private AudioClip spotted;
    private GameObject exclamation;
    private AudioSource attackSoundEffect;

    void Awake() {
        // get animator and collision from the inspector
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.speed = 0.66f; // otherwise he is tweaking
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        polygonCollider = GetComponent<PolygonCollider2D>();
        healthBar = GetComponentInChildren<healthbar>();
        exclamation = transform.Find("exclamation").gameObject;
        exclamation.SetActive(false);

        // get player and score
        game = Camera.main.gameObject;
        risk mc = game.GetComponent<risk>();
        player = mc.newPlayer;
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreCounter = scoreGO.GetComponent<ScoreCounter>();
        GameObject hsGO = GameObject.Find("HighScore");
        highScore = hsGO.GetComponent<HighScore>();
    }

    void Start() {
        UpdatePolygonCollider(); // doesnt work if i call in Awake() for some reason
    }

    void Update() {
        CheckOutOfBounds();
        if (currentBehavior == Behavior.Melee_Passive) Melee_Passive();
        else if (currentBehavior == Behavior.Melee_QuickAttack) Melee_QuickAttack();
    }

    private void CheckOutOfBounds() {
        if (Mathf.Abs(transform.position.x) > xBorder+5) Destroy(this.gameObject);
        else if (Mathf.Abs(transform.position.y) > yBorder+5) Destroy(this.gameObject);
    }

    // update polygon collider based on animation's shape
    private void UpdatePolygonCollider() {
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

    // set animation based on pokemon number
    private void SetAnimator(string type) {
        // set direction based on where it was spawned
        if (justSpawned) {
            if (Mathf.Abs(transform.position.x) == xBorder) {
                if (transform.position.x >= xBorder) currentDirection = Direction.X_Neg;
                else currentDirection = Direction.X_Pos;
            } else {
                if (transform.position.y >= yBorder) currentDirection = Direction.Y_Neg;
                else currentDirection = Direction.Y_Pos;
            }
        }

        // set animation based on direction
        if (currentDirection == Direction.X_Pos) animator.SetBool($"{type}_2", true);
        else if (currentDirection == Direction.X_Neg) animator.SetBool($"{type}_1", true);
        else if (currentDirection == Direction.Y_Pos) animator.SetBool($"{type}_3", true);
        else animator.SetBool($"{type}_0", true);
    }
    private void ClearAnimator(string type) { // clear animation if switching
        animator.SetBool($"{type}_0", false);
        animator.SetBool($"{type}_1", false);
        animator.SetBool($"{type}_2", false);
        animator.SetBool($"{type}_3", false);
    }

    public void TakeDamage(int damage, typechart.TYPE attackingType) {
        health -= types.FindEffectiveness(damage, attackingType, types.getType1(), types.getType2());
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0) enemyDeath();
    }

    private void enemyDeath() {
        Destroy(this.gameObject);
        scoreCounter.addScore(1);
        HighScore.TRY_SET_HIGH_SCORE(scoreCounter.getScore());
    }

    // uses difference between player and enemy to determine direction
    private void SetDirectionTowardsPlayer(Vector2 difference) {
        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y)) currentDirection = difference.x > 0 ? Direction.X_Pos : Direction.X_Neg;
        else currentDirection = difference.y > 0 ? Direction.Y_Pos : Direction.Y_Neg;
    }

    // collision detection with player
    private void OnCollisionEnter2D(Collision2D collision) { if (collision.gameObject == player) playerCollision = true; }
    private void OnCollisionExit2D(Collision2D collision) { if (collision.gameObject == player) playerCollision = false; }

    // template for spawning pokemon
    private void spawn(string pokemonID, string type1, string type2, float moveSpeed, int HP, float meleeR, int meleeP, float meleeC, AudioClip attackNoise, Behavior defaultBehavior) {
        justSpawned = true;
        pokemon = pokemonID;
        SetAnimator(pokemon);
        types = new typechart(type1, type2);
        speed = moveSpeed;
        health = HP;
        maxHealth = HP;
        healthBar.UpdateHealthBar(health, maxHealth);
        meleeRange = meleeR;
        meleePower = meleeP;
        meleeCooldown = meleeC;
        attackSoundEffect = gameObject.AddComponent<AudioSource>();
        attackSoundEffect.clip = attackNoise;
        attackSoundEffect.volume = 0.5f;
        currentBehavior = defaultBehavior;
        previousBehavior = defaultBehavior;
        justSpawned = false;
    }

    // meta information for spawn enemies
    public void spawnPidgey() { spawn("016", "NORMAL", "FLYING", 7f, 30, 2.5f, 5, 1f, quickattack, Behavior.Melee_Passive); }
    public void spawnRattata() { spawn("019", "NORMAL", "NA", 7f, 30, 2f, 5, 1.5f, quickattack, Behavior.Melee_Passive); }

    // behavior where enemies move to other side unless the player intrudes
    private void Melee_Passive() {
        // check out of bounds before anything
        CheckOutOfBounds();

        // move to other side
        switch (currentDirection) {
            case Direction.X_Pos: 
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                break;
            case Direction.X_Neg: 
                transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
                break;
            case Direction.Y_Pos: 
                transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                break;
            case Direction.Y_Neg: 
                transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
                break;
        }
        
        // if you get too close get attacked
        if (Vector2.Distance(transform.position, player.transform.position) <= meleeRange) {
            if (Time.time >= lastAttack + meleeCooldown) {
                previousBehavior = Behavior.Melee_Passive;
                currentBehavior = Behavior.Melee_QuickAttack;
            }
        }
    }

    // behavior where enemies gain a speed boost upon chasing the player
    private void Melee_QuickAttack() {
        // cooldown check
        if (Time.time < lastAttack + meleeCooldown) {
            currentBehavior = previousBehavior;
            return;
        }

        // check out of bounds before anything
        CheckOutOfBounds();

        // play sound of getting spotted and show exclamation
        if (!exclamation.activeSelf) { // was playing sound multiple times without condition
            attackSoundEffect.PlayOneShot(spotted, attackSoundEffect.volume); 
            exclamation.SetActive(true);
        }
        
        // change to direction of player and chase after them
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        SetDirectionTowardsPlayer(directionToPlayer);
        ClearAnimator(pokemon);
        SetAnimator(pokemon);
        animator.speed = 0; // pause animation while chasing
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * 1.25f * Time.deltaTime);

        // attack once collided
        if (playerCollision) {
            playercontroller attacked = player.GetComponent<playercontroller>();
            if (attacked != null) {
                attacked.TakeDamage(meleePower, typechart.TYPE.NORMAL);
                attackSoundEffect.Play();
                lastAttack = Time.time;
            }

            // after attack go back to how things were
            animator.speed = 0.66f;
            ClearAnimator(pokemon);
            SetAnimator(pokemon);
            exclamation.SetActive(false);
            currentBehavior = previousBehavior;
            previousBehavior = Behavior.Melee_QuickAttack;
            playerCollision = false;
        }
    }
}
