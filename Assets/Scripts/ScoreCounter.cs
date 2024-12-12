using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCounter : MonoBehaviour {
    [Header("Dynamic")]
    private int score = 0;
    TextMeshProUGUI gt;

    // Start is called before the first frame update
    void Start() {
        gt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update() {
        gt.SetText("Score: " + score.ToString("#,0"));
    }

    public int getScore() { return score; }
    public void addScore(int adding) { score += adding; }
}
