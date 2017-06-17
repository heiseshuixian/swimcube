using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeMap : MonoBehaviour {
	Vector3 rotation = Vector3.zero;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnMove(Vector2 v2){
		
		v2.y=v2.x;
		v2.x = 0;
		transform.Rotate (v2*0.5F);
	}
}
