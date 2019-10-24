using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/* Main関数のような代物
 * boxに設置しprefabBallにballのプレハブを適用して使う
 * ボール落っことしたり、得点表示したり、どこ押したか管理するとこ
 */
public class puzzle1 : MonoBehaviour {
	
	private int time = 0; // 時間.生成の為に使用
	public int dropTime = 10; // 落ちて来る為に必要な時間。段々減少する
	public int accelerate = 10; // この回数ブロック落ちたら1加速する
	private int count = 0; // 1加速するまでに何個ブロック落としたか
	public GameObject prefabBall; // ボールのプレハブ用.入れるのはインスペクターから
	public int colorPattern = 4; // 何種類の色を使用するか
	public int exist = 0; // 画面内に何個ボールがあるか
	public int maxBall = 75; // 画面内に何個までボール入れられるようにするか

	public int score = 0; // 得点.利便性の為に他関数から弄れるようにしているので,事実上グローバル変数と化しているが,まぁ小規模ゲームだし多少はね？
	private int startTime = 0; // 開始時間
	public int maxTime = 30; // 1ゲーム何秒か
	private int leftTime = 0; // 経過時間
	public GameObject prefabGUI; // 最終得点表示時の背景部分＋テキスト表示する部分.入れるのはインスペクターから
	//private int result = 0; // ゲーム終了したら1になる。リザルト画面表示した後、その処理の為に

	public int ballSizeX = Screen.width / 8; // ボックス含め
	public int ballSizeY = Screen.height / 13;

	// 画面の範囲を指定。ボール生成用に
	private float minX = -1.9f; // Randam.Range(float min, float max)に使用
	private float maxX = 1.9f; 
	private float Y = 4f; // Vector3

    //public GameObject prefabBomb; // 爆弾。インスペクターでセット

    public GUIStyle textStyle;

    // 開始処理
    void Start () {
		startTime = (int)Time.time;
		for (int i = 0; i <= 15; i++) {
			CreateBall ();
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		// 時間経過処理
		time++; // 時間増やす
		if (time >= dropTime) { // 一定時間経ったら
			if (exist < maxBall) { // 最大数までボールが無かったら
				CreateBall(); //ボール作って
			}
			time = 0; // カウントリセット
		}
		if (count >= accelerate) { // 一定個数落としたら
			if (dropTime > 0) { // 落ちて来る為に必要な時間が0より上の時
				dropTime--; // 必要な時間を1減らす
				count = 0; // countリセット
			}
		}

		leftTime = maxTime - ((int)Time.time - startTime);
		// ゲームが終了したか（＝時間制限が来たか）判定
		if ((int)Time.time - startTime >= maxTime) {
		//if (result == 1) {
			//GameObject GUI = (GameObject)Instantiate(prefabGUI); // 背景画面をプレハブで作ってあるのでゲームオブジェクトとして作成
			//GUI.GetComponent<UITextList>().text = score.ToString();
			//GetComponent<> = score.ToString();

			// 最高得点上回ってたら更新
			if (score > PlayerPrefs.GetInt("maxScore")) {
				PlayerPrefs.SetInt("maxScore", score);
			}
			PlayerPrefs.SetInt("score", score); // 今回の得点も保存
            // Application.LoadLevel(2); // 過去の記述
            SceneManager.LoadScene("score");
        }

		// タッチ処理
		if(Input.touchCount == 1) { // 複数押してる時はガン無視.一個押した時のみ反応
			
			// タッチ情報を取得する
			Touch touch = Input.GetTouch (0);
			
			// タッチ直後であればtrueを返す。
			if (touch.phase == TouchPhase.Began) {
				//tap();
			}
		}
		// マウスクリックされたらtrueを返す。
		if (Input.GetMouseButtonDown(0)) {
			// マウスの場所を探す
			Vector3    aTapPoint   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint); // コライダー取得
			if (aCollider2d) {
				if (aCollider2d.gameObject.tag == "block") {
					aCollider2d.gameObject.GetComponent<ball>().tap();
				}
				/*
				if (aCollider2d.gameObject.tag == "Bomb") {
					aCollider2d.gameObject.GetComponent<bomb>().tap();
				} */

			}
		}
		
	}
	
	// ボール作成。一定以上積もっちゃったらゲームオーバーにする
	public GameObject CreateBall() {
		GameObject ball = (GameObject)Instantiate(prefabBall); // 複製。Object型で生成されるのでGameObject型に変換
		ball.transform.position = new Vector3(Random.Range (minX, maxX), Y, 0); // 横の位置をランダムに変更
		ball color = ball.GetComponent<ball>(); // ballスクリプト取って来る
		color.ChangeColor(Random.Range(1, colorPattern + 1)); // ランダムに色変更
		count++; // 落とした個数増やす
		exist++; // 存在する数増やす
		return ball; // 操作出来るようにオブジェクト返しておく
	}

	// GameOver
	void GameOver(){
	}

	/*
	// 指定した個数に応じたボム作成、ボムを返す
	public GameObject CreateBomb(int sum) {
		GameObject bomb = (GameObject)Instantiate(prefabBomb);
		exist++; // 存在する数増やす
		return bomb;

	}*/
	
	void OnGUI(){

		GUI.Box(new Rect(0, 0, 100, 60), "残り時間:" + leftTime.ToString() + "\n" + "得点:" + score.ToString(), textStyle);
		/*

		if ((int)Time.time - startTime >= maxTime) {

			GUI.Box(new Rect(0, 0, 100, 150), score.ToString());
			result = 1;

		} */

	} 

}
