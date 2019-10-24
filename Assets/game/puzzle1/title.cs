using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class title : MonoBehaviour {
	public int score = 0;

    public GUIStyle textStyle;

    // Use this for initialization
    void Start () {
		score = PlayerPrefs.GetInt("maxScore");
	
	}
	
	// Update is called once per frame
	void Update () {
		// クリックされたら
		if (Input.GetMouseButtonDown(0)) {
            //Application.LoadLevel(1); // 2015年の記述方式
            SceneManager.LoadScene("Puzzle");
        }
	}

	void OnGUI(){
		GUI.Label (new
		           Rect (20, 10, 100, 40), "最高得点:" + score.ToString(), textStyle);
	}
}
