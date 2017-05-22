using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class O2Ctrl : MonoBehaviour {
	public static O2Ctrl me;
	private Image img_O2;
	public bool isCharge = false;
	public float O2Cur = 10;
	public float O2Max=10;
	private bool isDie = false;
	public bool isOver = false;
	void Awake(){
		me = this;
	}
	// Use this for initialization
	void Start () {
		img_O2 = GameObject.Find ("O2").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(isOver){
			return;
		}
		if (isCharge) {
			O2Cur = O2Max;
		} else {
			if (O2Cur > 0) {
				O2Cur -= Time.deltaTime*0.8f;
			} else {
				O2Cur = 0;
				if(!isDie){
					GameManger.me.ShowGameOver (3);
				}
			}
		}
		SetValue (O2Cur/O2Max);
	}
	public void SetValue(float value){
		img_O2.fillAmount = value;
	}
}
