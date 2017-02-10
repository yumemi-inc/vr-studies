//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour, PointController.IPointControllerTarget {

	public CityData city = null;

	void Start () {
		
		iTween.ColorTo (transform.FindChild ("Pin").gameObject, iTween.Hash (
			"color", Color.red,
			"namedcolorvalue", "_EmissionColor",
			"looptype", iTween.LoopType.pingPong,
			"time", 1.0f,
			"easing", iTween.EaseType.easeInOutBounce,
			"includechildren", false
		));
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void Update () {

		// Z軸以外のビルボード
		var ca = Camera.main.transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler( new Vector3 ( ca.x, ca.y, 0 ) );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void OnPointControllerHit( bool isOn ) {

		transform.FindChild("Title").GetComponent<TextMesh>().color = isOn ? Color.red : Color.white;

		// 拡大する
		iTween.ScaleTo( gameObject, iTween.Hash(
			"scale", isOn ? new Vector3 ( 2, 2, 2 ) : new Vector3 ( 1f, 1f, 1f ),
			"time", 0.5f
		));

		if( isOn ){
			transform.parent.GetComponent<Globe> ().OnPointControllerHit ( isOn );
		}
	}

	public void OnPointControllerClick( bool isOn ) {

		if( isOn ){

			// 地球を光らせる
			transform.parent.GetComponent<Globe>().LoadingAnimation( true );

			//都市詳細スクリーンをロードする
			VRSphere.Instance.PushCityScreen ( city );
		}
	}
}
