using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class playerhpbar : MonoBehaviour {
	public static playerhpbar me;
	private Image[] hearts;
	void Awake(){
		me = this;

	}
	// Use this for initialization
	void Start () {
		hearts = GetComponentsInChildren<Image> ();
	}
	public void setValue(float value){
		if(value>1){value = 1;}
		if(value<0){value = 0;}
		int light =(int) Mathf.Floor(hearts.Length*(value));
		float helf = hearts.Length*(value) -light;
		int helfIndex=light;
		for(int index =0;index<hearts.Length;index++){
			if (index <= light-1) {
				hearts [index].fillAmount = 1;
			} else if (index == helfIndex) {
				hearts [index].fillAmount = helf;
			} else {
				hearts [index].fillAmount = 0;
			}
		}
	}
}
