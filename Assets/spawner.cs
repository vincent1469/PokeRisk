using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // pokemon variables
    [SerializeField] private GameObject oppPrefab;
    private string oppSelect;
    private float defaultSpawnRate = 3f;
    private float spawnrate;
    private WaveTracker waveTracker;

    // spawner movement variables
    private enum Direction { X = 1, Y = 2 }
    private Direction currentDirection = Direction.X;
    private int xDirection = 1;
    private int yDirection = 1;
    private float changeDirectionChance = 0.1f;
    private float xBorder = 8.5f;
    private float yBorder = 4.5f;
    private float speed = 20;

    void Awake() {
        GameObject waveGO = GameObject.Find("WaveTracker");
        waveTracker = waveGO.GetComponent<WaveTracker>();
        transform.position = new Vector3(-xBorder, yBorder, 0);
        spawnrate = defaultSpawnRate;
        Invoke("spawn", 1f); // start spawning after 1 second
    }

    // function that spawns opps
    void spawn() {
        if (Random.value < 0.5f) oppSelect = "016"; // pidgey
        else oppSelect = "019"; // rattata

        // instantiate prefab
        GameObject spawning = Instantiate(oppPrefab);
        spawning.transform.position = transform.position;

        // get behavior based on oppSelect
        oppcontroller oppScript = spawning.GetComponent<oppcontroller>();
        if (oppScript) {
            switch (oppSelect) {
                case "016":
                    oppScript.spawnPidgey();
                    break;
                case "019":
                    oppScript.spawnRattata();
                    break;
            }
        }

        Invoke("spawn", spawnrate);
    }

    void Update() {
        // adjust spawnrate based on wave
        if (spawnrate > 1) spawnrate = defaultSpawnRate - ((waveTracker.getWave() - 1) * 0.2f);
        else if (spawnrate > 0.1) spawnrate = Mathf.Max(0.1f, 1 - 0.1f * (waveTracker.getWave() + 1 - ((defaultSpawnRate - 0.8f) / 0.2f))); // cant go below 0.1
        MoveSpawner();
    }

    void FixedUpdate() {
        // change directions on random
        if (Random.value < changeDirectionChance) {
            if (currentDirection == Direction.X) xDirection *= -1;
            else yDirection *= -1;
        }
    }

    private void MoveSpawner() {
        // basic movement across border
        if (currentDirection == Direction.X) {
            transform.position += new Vector3(xDirection * speed * Time.deltaTime, 0, 0);
            if (Mathf.Abs(transform.position.x) >= xBorder) { // check border
                currentDirection = Direction.Y;
                xDirection *= -1;

                // clamp to border
                if (transform.position.x > xBorder) transform.position = new Vector3(xBorder, transform.position.y, transform.position.z);
                else if (transform.position.x < -xBorder) transform.position = new Vector3(-xBorder, transform.position.y, transform.position.z);
            }
        }
        else if (currentDirection == Direction.Y) {
            transform.position += new Vector3(0, yDirection * speed * Time.deltaTime, 0);
            if (Mathf.Abs(transform.position.y) >= yBorder) { // check border
                currentDirection = Direction.X;
                yDirection *= -1;

                // clamp to border 
                if (transform.position.y > yBorder) transform.position = new Vector3(transform.position.x, yBorder, transform.position.z);
                else if (transform.position.y < -yBorder) transform.position = new Vector3(transform.position.x, -yBorder, transform.position.z);
            }
        }
    }
}
