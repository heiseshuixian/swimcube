using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boomCtrl : MonoBehaviour {
	private bool wilBoom =false;
	private float damageRange=2f;
	private float damage=1;
	List<GameObject> animals = new List<GameObject>();

	// Use this for initialization
	void Start () {
		Reset ();
		//boomEvent =letsboom();
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("fish");
		for(int index =0;index<temp.Length;index++){
			animals.Add (temp[index]);
		}
		temp = GameObject.FindGameObjectsWithTag ("Player");
		for(int index =0;index<temp.Length;index++){
			animals.Add (temp[index]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider target){
		if(!wilBoom){
			wilBoom = true;
			StartEffect ();
			StartCoroutine (letsboom());
		}
	}

	IEnumerator letsboom(){

		yield return new WaitForSeconds (1);
		BoomDamage ();
		BoomEffect ();
		Reset ();
		//StopCoroutine (boomEvent);
		StopAllCoroutines();
	}
	void StartEffect(){
		SoundManager.me.PlayAudioAtPosition (SoundType.boomstart,transform.position);
	}
	void BoomEffect(){
		SoundManager.me.PlayAudioAtPosition (SoundType.BOOM);
	}
	void BoomDamage(){
		for(int index =0;index<animals.Count;index++){
			if(Vector3.Distance(animals[index].transform.position,transform.position)<damageRange){
				animals [index].GetComponent<Animal> ().GetDamage (damage,2);
			}
		}
	}
	void Reset(){
		wilBoom = false;
		int newX = Random.Range (-4,4);
		int newY = Random.Range (-4,4);
		int newZ = Random.Range (-4,4);
		transform.position = new Vector3 (newX,newY,newZ);
	}
}
