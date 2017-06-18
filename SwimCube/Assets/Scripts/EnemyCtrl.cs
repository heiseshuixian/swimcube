using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : Animal {
	private Transform _target;
	public float damage =1;
	private float timer = 0;
    private bool isIdle =true;
	// Use this for initialization
	void Start () {
		
	}
    private void OnEnable()
    {
        TriggerManager.GetInstance().AddTrigger(E_TriggerType.on_game_start,FindPlayer);
        TriggerManager.GetInstance().AddTrigger(E_TriggerType.on_game_over, DelectPlayer);
    }
    private void OnDisable()
    {
        TriggerManager.GetInstance().RemoveTrigger(E_TriggerType.on_game_start, FindPlayer);
        TriggerManager.GetInstance().RemoveTrigger(E_TriggerType.on_game_over, DelectPlayer);
    }
    // Update is called once per frame
    void Update () {
        if (_target)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            transform.LookAt(_target);
            if (isIdle) {
                if (Vector3.Distance(transform.position,_target.position)<1) {
                    ChangeTarget();
                }
            }
        }
        else {
            ChangeTarget();
        }

	}
    public void ChangeTarget() {
        _target = GameObject.FindGameObjectWithTag("targetball").transform;
        Vector3 vec = Vector3.zero;
        vec.x = Random.Range(-5,5);
        vec.y = Random.Range(-5, 5);
        vec.z = Random.Range(-5, 5);
        _target.position = vec;
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
    private void FindPlayer(object obj) {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        isIdle = false;
    }
    private void DelectPlayer(object obj)
    {
        _target = null;
        isIdle = true;
    }
    protected override void OnDeath ()
	{
		_target = null;
	}
}
