//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class RacketSample : MonoBehaviour {

	public GameObject ball;

	float timePast = 0f;
	float timeSpan = 0.8f;

	void Update () {

		// 一定時間ごとにボールを発射する
		timePast += Time.deltaTime;
		if ( timeSpan < timePast ) {
			timePast = 0;
			var _ball = Instantiate ( ball ) as GameObject;
			_ball.transform.localScale *= 2.0f;
			_ball.transform.position = transform.TransformPoint ( new Vector3 ( 1.1f, 0.45f, 0.6f ) );
			_ball.transform.parent = transform;
			var rigid = _ball.GetComponent<Rigidbody>();
			rigid.AddForce( new Vector3 ( 0.7f, 0, -0.25f ), ForceMode.Impulse );
		}
	}
}
