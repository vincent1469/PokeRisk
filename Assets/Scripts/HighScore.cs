using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScore : MonoBehaviour {
    [Header("Dynamic")]
    static private int highScore = 0;
    static private TextMeshProUGUI hs;
    private TextMeshProUGUI txtCom;

    void Awake() {
        hs = GetComponent<TextMeshProUGUI>();

        if (PlayerPrefs.HasKey("HighScore")) {
            scoring = PlayerPrefs.GetInt("HighScore");
            PlayerPrefs.SetInt("HighScore", scoring);
        }
    }

    static public int scoring {
        get { return highScore; }
        private set {
            highScore = value;
            PlayerPrefs.SetInt("HighScore", value);
            if (hs != null) hs.SetText("High Score: \n" + value.ToString("#,0")); 
        }
    }

    static public void TRY_SET_HIGH_SCORE(int scoreToTry) {
        if (scoreToTry <= scoring) return;
        scoring = scoreToTry;
    }

    public bool resetHighScoreNow = false;

    void OnDrawGizmos() {
        if (resetHighScoreNow) {
            resetHighScoreNow = false;
            PlayerPrefs.SetInt("HighScore", 0);
            Debug.LogWarning("PlayerPrefs HighScore reset to 0.");
        }
    }
}
