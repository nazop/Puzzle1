using UnityEngine;
using System.Collections;

/* ballの動きを規定するスクリプト。ballプレハブに適用されている
 * ボールの色変更（＝画像変更）とか、押された時の挙動とかを管理
 * 画像指定とか、動き・画像処理はプレハブの方に丸投げ */
public class ball : MonoBehaviour {
	public SpriteRenderer MainSpriteRenderer; // 画像描写するクラス。インスペクターで設定
	public Sprite[] SpriteBall; // inspectorにて設定
	public int color = 0; // どの色か

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // rigidbody2D.WakeUp(); // 止まると衝突判定が消滅して困るので動かし続ける
        // (2015年当時そう書いてあった)
	
	}


	public void ChangeColor(int id) { // idで指定された画像に変更
		// SpriteRenderのspriteを設定済みの他のspriteに変更
		MainSpriteRenderer.sprite = SpriteBall[id];
		color = id;
	} 

	// 押された時の処理
	public void tap() { 
		// もし４つ以上同色がくっついてたら爆弾に

		// 画面内に入るブロック数の最大数を上限として、同色だったゲームオブジェクトを入れる配列を作成
		GameObject[] sameBlock = new GameObject[GameObject.Find("box").GetComponent<puzzle1>().maxBall];
		sameBlock [0] = this.gameObject; // 最初は自分
		sameBlock = same(sameBlock); // ひとまず自分のを入れておく
		int colorCount = 1; // 同色の数

		// 同色のを探して行く
		for (int i = 1; i < sameBlock.Length; i++) {
			if (sameBlock[i] == null) { // 無くなったら終了
				break;
			}
			sameBlock = sameBlock[i].GetComponent<ball>().same(sameBlock); // 同色で被ってないのを後ろに追加して貰う
			colorCount++; // 同色の数一個増やす
			
		}

		// 4つ以上の時のみ得点処理＋ジェム作成
		if (colorCount >= 4) {

			// ジェム作成（消したのがジェムではない場合）
			if (color != 0) {
				// ボム作って
				// GameObject bomb = GameObject.Find ("box").GetComponent<puzzle1>().CreateBomb(colorCount);
				// ball作って
				GameObject gem = GameObject.Find ("box").GetComponent<puzzle1> ().CreateBall ();
				// Gemに変更
				gem.GetComponent<ball> ().ChangeColor (0);
				// 場所移動
				//bomb.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
				gem.transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
			}

			for (int i = 1; i < sameBlock.Length; i++) {
					if (sameBlock [i] == null) { // 無くなったら終了
							break;
					}
					Destroy (sameBlock [i].gameObject); // 用済みなので削除
			}


			// 得点処理。Gemなら高め
			int score = colorCount * colorCount;
			if (color == 0) {
					score *= colorCount;
			}
			GameObject.Find ("box").GetComponent<puzzle1> ().score += score;

		}

		// 存在している数減らす
		GameObject.Find ("box").GetComponent<puzzle1> ().exist -= colorCount;
		Destroy (this.gameObject);

	}

	// ゲームオブジェクトと被ってない、このゲームオブジェクトと隣接している同色ブロックを、与えられたゲームオブジェクト末尾に追加
	public GameObject[] same(GameObject[] sameBlock) {

		GameObject[] block = sameColor(); // このオブジェクトに隣接している同色ブロック一覧を取得
		for (int i = 0; i < block.Length; i++) { // 同色ブロックを見て行く

			if (block[i] == null) {
				break; // nullの可能性があるので、nullだったら最後と見なして終わる
			}
			// 与えられたゲームオブジェクトと比較して無いのあったら追加
			for (int i2 = 0; i2 < sameBlock.Length; i2++) {
				if (sameBlock[i2] == null) { // 被ってなかったら
					sameBlock[i2] = block[i]; // 追加
					break;
				} else if (block[i] == sameBlock[i2]) { // 被ってたら次へ
					break;
				}
			}
		}
		return sameBlock;

	}

	// このボールと隣接していて同色なのを返す
	public GameObject[] sameColor() {
		// 衝突判定の半径使って、その4倍の半径を持つ円内のボール無いか探してる。衝突判定見つけたら入る
		Collider2D[] block = Physics2D.OverlapCircleAll (transform.position, GetComponent<CircleCollider2D>().radius * 4);
		GameObject[] sameBlock = new GameObject[block.Length]; // 同色の接してるブロック入れるとこ。こいつを返り値にする
	 	for (int i = 0; i < block.Length; i++) {
			// boxが入ってる可能性もあるのでblockだった時だけ判定
			if (block[i].gameObject.tag == "block") {
				// 同色なら入れるとこ探す
				if (block[i].gameObject.GetComponent<ball>().color == this.color) {
					for (int i2 = 0; i2 < sameBlock.Length; i2++) {

						if (sameBlock[i2] == null) {// 空き場所見つけたら
							sameBlock [i2] = block [i].gameObject; // 突っ込んで						
							break; // 終了
						}
					}
				}
			}
		}
		return sameBlock;
	}

	public void erase() {
		// 存在している数減らしつつ削除
		GameObject.Find ("box").GetComponent<puzzle1> ().exist--;
		Destroy (this.gameObject);
	}


}
