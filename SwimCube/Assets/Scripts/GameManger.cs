using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameManger : MonoBehaviour {
	public static GameManger me;
	public GameObject gameoverUI;
	public Text deathMsg;
	public Image deathType;
	public Button btn_replay;
	private string deathPath = "Sprite/death";
	void Awake(){
		me = this;
		gameoverUI.SetActive (false);
		btn_replay.onClick.AddListener(Replay);
	}
	public void ShowGameOver(int type){
		gameoverUI.SetActive (true);
		switch(type){
		case 1:
			deathMsg.text = "你被鲨鱼吃掉了";
			break;
		case 2:
			deathMsg.text = "你炸死了";
			break;
		case 3:
			deathMsg.text = "你被憋死了";
			break;
		default:
			deathMsg.text = "你死了";
			break;
		}
		deathType.sprite = Resources.Load<Sprite> (deathPath+type);
		O2Ctrl.me.isOver = true;
		SoundManager.me.isOver = true;
	}
	public void Replay(){
		SceneManager.LoadSceneAsync (0);;
	}
}
