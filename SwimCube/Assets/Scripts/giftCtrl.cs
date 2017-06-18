using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giftCtrl : MonoBehaviour {
	public GameObject giftFX;
	private GameObject entryFX;
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
			StartCoroutine (ShowFX());
			Reset ();
			CoinBar.me.AddCoin ();
            TriggerManager.GetInstance().Trigger( E_TriggerType.on_get_gift);
		}
	}
	IEnumerator ShowFX(){
		entryFX = GameObject.Instantiate<GameObject> (giftFX);
		entryFX.transform.position = transform.position;
		yield return new WaitForSeconds (1);
		Destroy (entryFX);
	}
	void Reset(){

		int newX = Random.Range (-3,3);
		int newY = -5;
		int newZ = Random.Range (-3,3);
		transform.position = new Vector3 (newX,newY,newZ);
	}
}
