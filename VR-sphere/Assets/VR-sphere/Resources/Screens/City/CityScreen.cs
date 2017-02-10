//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityScreen : VRScreen {

	List<GameObject> categories = new List<GameObject>();

	//------------------------------------------------------------------------------------------------------------------------------//
	void Awake() {

		//最初に全てのオブジェクトを構築しておく
		InitCategory ( 0, "food", "4bf58dd8d48988d1f9941735" );
		InitCategory ( 1, "night", "4d4b7105d754a06376d81259" );
		InitCategory ( 2, "shop", "4d4b7105d754a06378d81259" );
		InitCategory ( 3, "art", "4d4b7104d754a06370d81259" );
		InitCategory ( 4, "outdoor", "4d4b7105d754a06377d81259" );
		InitCategory ( 5, "travel", "4d4b7105d754a06379d81259" );
		InitCategory ( 6, "hotel", "4bf58dd8d48988d1fa931735" );
		InitCategory ( 7, "study", "4d4b7105d754a06372d81259" );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void LoadData( CityData city ) {

		// カテゴリーデータを読み込む
		LoadCategory ( city, "food" );
		LoadCategory ( city, "night" );
		LoadCategory ( city, "shop" );
		LoadCategory ( city, "art" );
		LoadCategory ( city, "outdoor" );
		LoadCategory ( city, "travel" );
		LoadCategory ( city, "hotel" );
		LoadCategory ( city, "study" );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void InitCategory ( int index, string categoryName, string categoryId ) {

		// カテゴリ別にオブジェクトを作成
		var category = Instantiate ( Resources.Load("Screens/City/Prefabs/Category") ) as GameObject;
		category.name = categoryName;
		category.transform.SetParent ( transform, false );
		category.transform.localPosition = new Vector3 ( index * 1.5f - 6.0f + (index >= 4 ? 1.5f : 0), 0, 0 );
		category.GetComponent<Category> ().InitData( categoryName, categoryId );
		categories.Add ( category );
	}

	public void LoadCategory ( CityData city, string categoryName ) {

		// スクリーンタイトルを変更
		transform.FindChild ("Title").GetComponent<TextMesh>().fontSize = city.title.Length > 10 ? 35 : 40;
		transform.FindChild ("Title").GetComponent<TextMesh>().text = city.title;

		var category = transform.FindChild ( categoryName );
		category.GetComponent<Category> ().LoadData( city );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	// 出現時ハンドラ
	public override void OnPushIn () {

		base.OnPushIn ();

		// 写真パネルのみ散らしてから再度集合
		Venue[] venues = transform.GetComponentsInChildren<Venue> ( true );
		for( int i = 0; i < venues.Length; i++ ) {
			
			var panel = venues [i].transform.FindChild ("Panel");
			panel.localPosition = new Vector3( Random.Range( -5f, 5f ), Random.Range( -5f, 5f ), Random.Range( -5f, 5f ) );	

			Vector3[] path = new Vector3[3];
			path[0] = new Vector3 ( panel.localPosition.x, 0, panel.localPosition.z );
			path[1] = new Vector3 ( 0, 0, panel.localPosition.z );
			path[2] = new Vector3 ( 0, 0, 0 );

			iTween.MoveTo( panel.gameObject, iTween.Hash (
				"path", path,
				"time", 3.6f,
				"islocal", true,
				"easing", iTween.EaseType.linear
			));
		}
	}

	// 消滅時ハンドラ
	public override void OnPopOut () {

		base.OnPopOut ();

		// 写真パネルのみ散らしてから離散
		Venue[] venues = transform.GetComponentsInChildren<Venue> ( true );
		for( int i = 0; i < venues.Length; i++ ) {
			var panel = venues [i].transform.FindChild ("Panel");
			var position = new Vector3( Random.Range( -5f, 5f ), Random.Range( -5f, 5f ), Random.Range( 10f, -10f ) );	
			iTween.MoveTo( panel.gameObject, iTween.Hash (
				"position", position,
				"time", 3f,
				"islocal", true,
				"easing", iTween.EaseType.linear
			));
		}
	}
}
