using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giftCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider target){
		if(target.CompareTag("Player")){
			SoundManager.me.PlayAudioAtPosition (SoundType.coin);
			Reset ();
			CoinBar.me.AddCoin ();
		}
	}
	void Reset(){

		int newX = Random.Range (-3,3);
		int newY = -5;
		int newZ = Random.Range (-3,3);
		transform.position = new Vector3 (newX,newY,newZ);
	}
}
