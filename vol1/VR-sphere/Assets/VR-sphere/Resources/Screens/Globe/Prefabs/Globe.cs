//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;

public class Globe : MonoBehaviour, PointController.IPointControllerTarget {

	bool isRotating = true;
	Color orgColor = Color.clear;

	//------------------------------------------------------------------------------------------------------------------------------//
	public void InitPins( CityData city ) {

		var pin = Instantiate ( Resources.Load("Screens/Globe/Prefabs/Pin") ) as GameObject;
		pin.transform.SetParent ( transform, false );
		pin.transform.SetAsLastSibling ();
		pin.transform.FindChild("Title").GetComponent<TextMesh>().text = city.title;
		pin.transform.localPosition = geoToPolar ( city.lat, city.lng );

		pin.GetComponent<Pin> ().city = city; 
	}

	Vector3 geoToPolar( float lat, float lng ) {

		// lat = -90(S)～-90(N) // Lng = -180(W)～180(E)
		float ph = (90f - lat) * Mathf.Deg2Rad;
		float th = lng * Mathf.Deg2Rad;
		float r = 0.66f;

		// x = rsinΦcosΘ
		// y = rcosΦ
		// z = rsinΦsinΘ
		Vector3 point = Vector3.zero;
		point.x = r * Mathf.Sin ( ph ) * Mathf.Cos ( th );
		point.z = r * Mathf.Sin ( ph ) * Mathf.Sin ( th );
		point.y = r * Mathf.Cos ( ph );
		return point;
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void LoadingAnimation( bool isOn ) {

		isRotating = !isOn;

		if( orgColor == Color.clear ){
			orgColor = transform.FindChild ("Land").GetComponent<Renderer> ().material.GetColor ("_EmissionColor");
		}

		// 点滅させる
		if ( isOn ) {
			iTween.ColorTo ( transform.FindChild ("Land").gameObject, iTween.Hash (
				"color", Color.red,
				"namedcolorvalue", "_EmissionColor",
				"looptype", iTween.LoopType.pingPong,
				"time", 1.0f,
				"easing", iTween.EaseType.easeInOutBounce,
				"includechildren", false
			));
		} else {
			iTween.Stop( transform.FindChild ("Land").gameObject );
			transform.FindChild ("Land").GetComponent<Renderer> ().material.SetColor ("_EmissionColor", orgColor) ;
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void Update() {

		// 回転アニメ
		if( isRotating ){ 
			transform.Rotate ( new Vector3( 0, 0.1f, 0 ) );
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void OnPointControllerHit( bool isOn ) {

		transform.FindChild("LatLng").gameObject.SetActive( isOn );

		// 拡大する
		iTween.ScaleTo( gameObject, iTween.Hash(
			"scale", isOn ? new Vector3 ( 3, 3, 3 ) : new Vector3 ( 1.5f, 1.5f, 1.5f ),
			"time", 1.0f,
			"easing", iTween.EaseType.easeInOutElastic
		));
	}

	public void OnPointControllerClick( bool isOn ) {
		isRotating = !isOn;
	}
}
