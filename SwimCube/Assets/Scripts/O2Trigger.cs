using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2Trigger : MonoBehaviour {
	bool isGetTop =false;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter(Collider target){
		if(target.CompareTag("Player")){
			if(!isGetTop){
				isGetTop = true;
				SoundManager.me.PlayAudioAtPosition (SoundType.breath);
				O2Ctrl.me.isCharge = true;
			}
		}
	}
	void OnTriggerExit(Collider target){
		if(target.CompareTag("Player")){
			if(isGetTop){
				isGetTop = false;
				SoundManager.me.PlayAudioAtPosition (SoundType.putong);
				O2Ctrl.me.isCharge = false;
			}
		}
	}
}
