using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {
	protected float _currentHP=3;
	protected float _HPMax=3;
	protected enum ActionType
	{
		Move,
		Death
	}
	protected ActionType curType = ActionType.Move;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public virtual void GetDamage(float _damage,int hittype){
		_currentHP -= _damage;
		if(_currentHP<=0){
			OnTypeChange (ActionType.Death);
		}
	}
	private void OnTypeChange(ActionType _type){
		if(_type==curType){
			return;
		}
		switch(_type){
		case ActionType.Move:
			OnMove ();
			break;
		case ActionType.Death:
			OnDeath ();
			break;

		}
		curType = _type;
	}
	protected void OnMove(){

	}
	protected virtual void OnDeath(){
		
	}
}
