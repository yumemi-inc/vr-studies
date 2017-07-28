//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class LeverSample : MonoBehaviour {

	public GameObject water;
	GameObject lever = null;

	float timePast = 0f;
	float timeSpan = 0.8f;

	void Start () {
		lever = transform.FindChild ("Pipes/Lever").gameObject;
	}

	void Update () {

		// 水球を生成
		var angleY = lever.transform.localRotation.eulerAngles.y; 
		var angleZ = 360f - lever.transform.localRotation.eulerAngles.z; 
		if( angleZ <= 30f  && angleZ > 1f ){

			// Z軸の角度によって水流を変更
			if( angleZ > 20f ){ angleZ = 20f; }
			timeSpan = (1.0f - angleZ / 20f) + 0.1f;
			timePast += Time.deltaTime;

			if( timeSpan < timePast ){

				timePast = 0;
				//GameObject _water = Instantiate ( water, new Vector3 ( 1.08f, 1.03f, 0.2f ), Quaternion.identity ) as GameObject;
				GameObject _water = Instantiate ( water ) as GameObject;
				_water.transform.position = transform.TransformPoint ( new Vector3 ( -0.21f, 0.22f, -0.25f ) );
				_water.transform.parent = transform;

				// X軸の角度によって色を変更
				if( angleY < 45 ){angleY += 360f;}
				angleY -= 315f;
				float red = 0f; 
				float blue = 0f;
				red = angleY / 90f;
				blue = 1.0f - angleY / 90f;
				_water.GetComponent<Renderer> ().material.color = new Color( red, 0, blue );
			}
		}
	}
}
