using UnityEngine;
using System.Collections;

public enum SoundType{
	sharkhit,
	BOOM,
	boomstart,
	coin,
	breath,
	putong
}
public class SoundManager : MonoBehaviour {
	public static SoundManager me;
	public AudioClip[] ClipArray;
	private AudioSource Source;
	public bool isOver=false;
	void Awake(){
		me = this;
	}

	// Use this for initialization
	void Start () {
		Source = GetComponent<AudioSource> ();
	}
	void Update(){
		if(isOver){
			Source.volume -= Time.deltaTime*0.4f;
		}

	}
	public void PlayAudioAtPosition(SoundType type){
		AudioClip playClip = null;
		ChooseSoundType (out playClip,type);
		if(playClip){
			AudioSource.PlayClipAtPoint (playClip,Camera.main.transform.position);
		}
	}
	public void PlayAudioAtPosition(SoundType type,Vector3 pos){
		AudioClip playClip = null;
		ChooseSoundType (out playClip, type);
		if (playClip) {
			AudioSource.PlayClipAtPoint (playClip, pos);
		}
	}
	void ChooseSoundType(out AudioClip newClip,SoundType type){
		newClip = ClipArray[0];
		switch(type){
		case SoundType.sharkhit:
			newClip = ClipArray[0];
			break;
		case SoundType.BOOM:
			newClip = ClipArray[1];
			break;
		case SoundType.boomstart:
			newClip = ClipArray[2];
			break;
		case SoundType.coin:
			newClip = ClipArray[3];
			break;
		case SoundType.breath:
			newClip = ClipArray[4];
			break;
		case SoundType.putong:
			newClip = ClipArray[5];
			break;
		}
	}
}
