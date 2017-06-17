using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : Animal {
	private Transform _target;
	public float damage =1;
	private float timer = 0;
	// Use this for initialization
	void Start () {
		_target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	// Update is called once per frame
	void Update () {
		if(_target){
			transform.Translate(Vector3.forward * Time.deltaTime);
			transform.LookAt (_target);
		}

	}
	public void OnMove(Vector2 v2){
		
	}

	void OnCollisionEnter(Collision target){
		if(target.gameObject.CompareTag("Player")){
			if(timer<Time.time){
				timer += 1;
				target.gameObject.GetComponent<PlayerCtrl> ().GetDamage(damage,1);
			}


			SoundManager.me.PlayAudioAtPosition (SoundType.sharkhit,transform.position);
		}
	}
	protected override void OnDeath ()
	{
		_target = null;
	}
}
