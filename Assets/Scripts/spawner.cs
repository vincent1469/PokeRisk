using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // pokemon variables
    [SerializeField] private GameObject oppPrefab;
    private string oppSelect;
    private float defaultSpawnRate = 2f;
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
        StartCoroutine(spawn());
    }

    // function that spawns opps
    private bool initial = true;
    private IEnumerator spawn() {
        while (true) {
            // wait one second before spawning
            if (initial) {
                yield return new WaitForSeconds(1f);
                initial = false;
            }
            
            // geodude spawns on wave 5
            if (waveTracker.getWave() < 5) {
                if (Random.value < 0.5f) oppSelect = "016"; // pidgey
                else oppSelect = "019"; // rattata
            } else {
                if (Random.value < 0.33f) oppSelect = "019"; // rattata
                else if (Random.value < 0.66f) oppSelect = "016"; // pidgey
                else {
                    oppSelect = "074"; // geodude
                    spawnOne(); // geodude brings a friend
                }
            }

            // instantiate prefab with some randomness to the position of the spawns
            GameObject spawning = Instantiate(oppPrefab);
            if (Random.value < 0.25f) spawning.transform.position = new Vector3(-transform.position.x, -transform.position.y, transform.position.z);
            else if (Random.value < 0.5f) spawning.transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            else if (Random.value < 0.75f) spawning.transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
            else spawning.transform.position = transform.position;
            if (Mathf.Abs(transform.position.x) == xBorder) spawning.transform.position = new Vector3(spawning.transform.position.x, spawning.transform.position.y * Random.value, transform.position.z);
            else spawning.transform.position = new Vector3(spawning.transform.position.x * Random.value, spawning.transform.position.y, transform.position.z);

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
                    case "074":
                        oppScript.spawnGeodude();
                        break;
                }
            }

            // wait to spawn again
            yield return new WaitForSeconds(spawnrate);
        }
    }
    private void spawnOne() {
        GameObject spawning = Instantiate(oppPrefab);
        spawning.transform.position = transform.position;
        oppcontroller oppScript = spawning.GetComponent<oppcontroller>();
        if (Random.value < 0.5) oppScript.spawnPidgey(); 
        else oppScript.spawnRattata();
    }

    void Update() {
        // adjust spawnrate based on wave
        if (spawnrate > 1) spawnrate = defaultSpawnRate - ((waveTracker.getWave() - 1) * 0.2f);
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
