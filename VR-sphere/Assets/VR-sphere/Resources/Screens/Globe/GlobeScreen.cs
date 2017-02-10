//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobeScreen : VRScreen {

	public GameObject globe;
	List<CityData> cities = new List<CityData>();

	//------------------------------------------------------------------------------------------------------------------------------//
	public void LoadData() {

		// 地球儀を作成する
		globe = transform.FindChild ("Globe").gameObject;

		// 都市ピンを作成する
		cities.Add ( InitCityPin( "London", 51.5074f-12f, -0.1278f+2f ) );
		cities.Add ( InitCityPin( "Moscow", 55.7558f-5f, 37.6173f+5f ) );

		cities.Add ( InitCityPin( "Istanbul", 41.0082f-11f, 28.9784f+3f ) );
		cities.Add ( InitCityPin( "Dubai", 25.2048f-10f, 55.2708f+7f ) );
		cities.Add ( InitCityPin( "New Delhi", 28.7041f-12f, 77.1025f+7f ) );

		cities.Add ( InitCityPin( "Cairo", 30.0444f-10f, 31.2357f+5f ) );
		cities.Add ( InitCityPin( "Cape Town", -33.9249f-3f, 18.4241f+4f ) );

		cities.Add ( InitCityPin( "Tokyo", 35.6813f-5f, 139.7662f+5f ) );
		cities.Add ( InitCityPin( "Beijing", 39.9042f-3f, 116.4074f+8f ) );

		cities.Add ( InitCityPin( "Bangkok", 13.7563f-8f, 100.5018f+11f ) );
		cities.Add ( InitCityPin( "Jakarta", -6.1745f-8f, 106.8227f+11f ) );
		cities.Add ( InitCityPin( "Sydney", -33.3688f-12f, 151.2093f+6f ) );

		cities.Add ( InitCityPin( "Vancouver", 49.2827f-3f, -123.1207f+11f ) );
		cities.Add ( InitCityPin( "Los Angeles", 34.0522f-6f, -118.2437f ) );
		cities.Add ( InitCityPin( "New York", 41.7128f-10f, -74.0059f+5f ) );

		cities.Add ( InitCityPin( "Bogota", 4.7110f-7f, -74.0721f-3f ) );
		cities.Add ( InitCityPin( "Rio de Janeiro", -22.9068f-5f, -43.1729f+1f ) );
		cities.Add ( InitCityPin( "Buenos Aires", -34.6037f-5f, -58.3816f+1f ) );

	}

	CityData InitCityPin( string title, float lat, float lng ) {
		var city = new CityData( title, lat, lng );
		globe.GetComponent<Globe>().InitPins( city );
		return city;
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public override void OnPushIn () {
		gameObject.SetActive ( true );
	}

	public override void OnPopOut () {
		gameObject.SetActive ( false );
	}

	public override void OnFront ( bool isOn ) {

		if( isOn ){
			globe.GetComponent<Globe>().LoadingAnimation( false );
		}

		// 非アクティブ時は地球儀のJointを解放する ( しないと移動アニメーションができない )
		base.OnFront ( isOn );
		globe.GetComponent<Rigidbody>().isKinematic = !isOn;
		globe.GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = isOn;
	}
}
