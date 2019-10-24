using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class score : MonoBehaviour {
	private int maxScore = 0;
	private int currentScore = 0;

    public GUIStyle textStyle;

    // Use this for initialization
    void Start () {
		maxScore = PlayerPrefs.GetInt("maxScore");
		currentScore = PlayerPrefs.GetInt("score");
        ParticleSystem parsys = GameObject.Find("Particle").GetComponent<ParticleSystem>();


        //GameObject.Find("Particle").GetComponent<ParticleSystem>().enableEmission = false;
        // 2015年の記述
        parsys.Stop(true);
        if (maxScore == currentScore) {
            //GameObject.Find ("Particle").GetComponent<ParticleSystem>().enableEmission = true;
            parsys.Play(true);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
            // Application.LoadLevel(0);
            SceneManager.LoadScene("title");
        }
	
	}

	void OnGUI(){
        /*GUI.Label (new
		           Rect (100, 120, 100, 200), "得点:" + currentScore.ToString() + "\n" + "最高得点:" + maxScore.ToString());
	    */
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "得点:" + currentScore.ToString() + "\n" + "最高得点:" + maxScore.ToString(), textStyle);
        // スクリーンサイズ/2-画像サイズ/2だと中央に来るらしいので
    }
}
