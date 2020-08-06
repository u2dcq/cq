using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAni : MonoBehaviour {

	List<Texture2D> t2ds = new List<Texture2D>();
	SpriteRenderer sr;
	int index;
	int timer;

	// Use this for initialization
	void Start () {
		//加载图片;
		for (int i = 0; i < 7; i++) {
			Texture2D t2d = Resources.Load<Texture2D> ("human/new_dco004/0_attack_"+i);
			t2ds.Add (t2d);
		}

		//精灵组件;
		sr = this.transform.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		timer++;
		if(timer == 5){
			timer = 0;

			index++;
			if(index > 6){
				index = 0;
			}
			Texture2D t2d = t2ds [index];
			sr.sprite = Sprite.Create (t2d, new Rect(0,0,t2d.width,t2d.height), new Vector2(0.5f, 0.5f));
		}
	}
}
