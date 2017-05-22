using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinBar : MonoBehaviour {
	public static CoinBar me;
	public Text coin;
	private int _coin=0;
	void Awake(){
		me = this;
	}
	public void AddCoin(){
		_coin++;
		coin.text = "财宝："+_coin;
	}
}
