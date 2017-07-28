//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;

///<summary>
/// VR-Sphereのエントリクラス. 
/// 各階層スクリーンの生成とアニメーションを担当する.
///</summary>
public class VRSphere : VRScreenManager {

	public int photo_max_num_x = 21;
	public int photo_max_num_y = 9;

	GameObject globeScreen;
	GameObject cityScreen;
	GameObject categoryScreen;
	GameObject venueScreen;

	//------------------------------------------------------------------------------------------------------------------------------//
	private static VRSphere instance;
	public static VRSphere Instance {
		get {
			if (instance != null) return instance;
			instance = FindObjectOfType(typeof(VRSphere)) as VRSphere;
			return instance;
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void Start(){

		// SteamVR_TrackedControllerをアタッチして、トリガーイベントハンドラを登録
		SteamVR_TrackedController handTO = GameObject.Find("[CameraRig]").GetComponent<SteamVR_ControllerManager>().right.AddComponent<SteamVR_TrackedController> ();
		handTO.PadClicked += new ClickedEventHandler ( OnPadClicked );

		// スクリーンの参照を保持する
		globeScreen = transform.FindChild("GlobeScreen").gameObject;
		cityScreen = transform.FindChild("CityScreen").gameObject;
		categoryScreen = transform.FindChild("CategoryScreen").gameObject;
		venueScreen = transform.FindChild("VenueScreen").gameObject;

		// 一度すべてのスクリーンを非表示にする
		globeScreen.SetActive (false);
		cityScreen.SetActive (false);
		categoryScreen.SetActive (false);
		venueScreen.SetActive (false);

		// 最初のスクリーンをロードする
		PushGlobeScreen ();
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void PushGlobeScreen () {

		//地球スクリーンをロードする
		globeScreen.GetComponent<GlobeScreen> ().LoadData ();
		base.PushScreen ( globeScreen );
	}

	public void PushCityScreen ( CityData city ) {

		//都市詳細スクリーンをロードする
		cityScreen.GetComponent<CityScreen> ().LoadData ( city );
		base.PushScreen ( cityScreen );
	}

	public void PushCategoryScreen ( CityData city, CategoryData category ) {

		//カテゴリ詳細スクリーンをロードする
		categoryScreen.GetComponent<CategoryScreen> ().LoadData ( city, category );
		base.PushScreen ( categoryScreen );
	}

	public void PushVenueScreen ( CityData city, CategoryData category, VenueData venue ) {

		//べニュー詳細スクリーンをロードする
		venueScreen.GetComponent<VenueScreen> ().LoadData ( city, category, venue );
		base.PushScreen ( venueScreen );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void OnPadClicked( object sender, ClickedEventArgs e ) {

		//最前面のスクリーンを削除して、ひとつ前のスクリーンに戻る
		base.PopScreen();
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	protected override void OnPushScreen ( GameObject frontScreen ) {

		//全スクリーンをひとつ奥へスライドさせる
		for( int i = 0; i < screens.Count; i++ ){

			GameObject screen = screens[i];
			if( screen == frontScreen ){ continue; }

			iTween.MoveTo( screen, iTween.Hash(
				"z",  10f * (screens.Count - (i+1)),
				//"z",  10f + screen.transform.localPosition.z,
				"time", 3.6f,
				"easing", iTween.EaseType.easeOutExpo,
				"includechildren", false,
				"islocal", true
			));
		}
	}

	protected override void OnPopScreen ( GameObject frontScreen, System.Action onComplete ) {

		//全スクリーンをひとつ手前へスライドさせる
		for( int i = 0; i < screens.Count; i++ ){

			GameObject screen = screens[i];
			iTween.MoveTo( screen, iTween.Hash(
				"z",  10f * (screens.Count - (i+1)),
				//"z",  -10f + screen.transform.localPosition.z,
				"time", 3.0f,
				"easing", iTween.EaseType.easeOutExpo,
				"includechildren", false,
				"islocal", true,
				"oncomplete", onComplete
			));
		}
	}

}
