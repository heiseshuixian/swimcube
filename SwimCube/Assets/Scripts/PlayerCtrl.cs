using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : Animal {
	private bool isDown =false;
	bool isCtrl =false;
	Vector3 dir;
	float Y=0;
	Vector3 final ;
	public int hitType=1;
	private int speed=1;
	// Use this for initialization
	void Start () {
		_currentHP = _HPMax;
	}
	
	// Update is called once per frame
	void Update () {

	
		if (isDown) {
			if (Y < 1) {
				Y+=Time.deltaTime;
			}
		} else {
			if (Y > -1) {
				Y-=Time.deltaTime;
			}
		}
		dir.y = Y + transform.position.y;
		if(!isCtrl){
			//transform.LookAt (transform.position+ transform.forward);
		}
		transform.LookAt (dir);
		final = Vector3.forward * Time.deltaTime * 2;
		final.x = Mathf.Clamp (final.x,-4.5f,4.5f);
		final.y = Mathf.Clamp (final.y,-4.5f,4.5f);
		final.z = Mathf.Clamp (final.z,-4.5f,4.5f);
		transform.Translate(final*speed);
		Vector3 place = transform.position;
		place.x = Mathf.Clamp (place.x,-4.5f,4.5f);
		place.y = Mathf.Clamp (place.y,-4.5f,4.5f);
		place.z = Mathf.Clamp (place.z,-4.5f,4.5f);
		transform.position=place;
	}
	public void OnMove(Vector2 v2){
		dir.x = v2.x+transform.position.x;
		dir.z = v2.y+transform.position.z;
	
	}
	public void GetCtrl(){
		isCtrl = true;
	}
	public void LostCtrl(){
		isCtrl = false;
	}
	public void SwimDown(){
		isDown = true;
		speed = 2;
	}
	public void SwimUp(){
		isDown = false;
		speed = 1;
	}
	protected override void OnDeath ()
	{
		base.OnDeath ();
		GameManger.me.ShowGameOver (hitType);
	}
	public override void GetDamage (float _damage,int hittype)
	{
		hitType = hittype;
		base.GetDamage (_damage,hittype);
		playerhpbar.me.setValue (_currentHP/_HPMax);
	}
}
