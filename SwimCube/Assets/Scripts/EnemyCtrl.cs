using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour {
	private Transform _target;
	// Use this for initialization
	void Start () {
		_target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime);
		transform.LookAt (_target);
	}
	public void OnMove(Vector2 v2){
		
	}


}
