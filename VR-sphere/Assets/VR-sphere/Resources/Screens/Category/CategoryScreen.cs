//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CategoryScreen : VRScreen {

	CityData city = null;
	CategoryData category = null;
	List<VenueData> venues = new List<VenueData>();

	int PHOTO_NUM_X = 0;
	int PHOTO_NUM_Y = 0;
	int PHOTO_NUM = 0;

	//------------------------------------------------------------------------------------------------------------------------------//
	void Awake() {

		// 縦横最大取得写真数を取得
		PHOTO_NUM_X = VRSphere.Instance.photo_max_num_x;
		PHOTO_NUM_Y = VRSphere.Instance.photo_max_num_y;
		PHOTO_NUM = PHOTO_NUM_X * PHOTO_NUM_Y;

		//最初に全てのオブジェクトを構築しておく
		for( int i = 0; i < PHOTO_NUM; i++ ){
			if( i >= PHOTO_NUM_X * (PHOTO_NUM_Y/2) + (PHOTO_NUM_X-4)/2+1 && i < PHOTO_NUM_X * (PHOTO_NUM_Y/2+1) - (PHOTO_NUM_X-4)/2-1 ){ continue; }
			var venue = Instantiate ( Resources.Load("Screens/Category/Prefabs/Venue") ) as GameObject;
			venue.name = "Venue";
			venue.transform.SetParent ( transform, false );
			venue.transform.localPosition = new Vector3 ( (i % PHOTO_NUM_X) * 1.2f - (1.2f * (int)(PHOTO_NUM_X/2)), (1.2f * (int)(PHOTO_NUM_Y/2) -0.3f) - (i / PHOTO_NUM_X) * 1.2f, 0 );
			venue.GetComponent<Venue> ().InitData ( Venue.VENUE_MODE.FOR_CATEGORY );
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void LoadData( CityData _city, CategoryData _category ) {

		city = _city;
		category = _category;

		// スクリーンタイトルを変更
		transform.FindChild ("Title").GetComponent<TextMesh>().text = city.title + " \n[ " + category.title + " ]";

		// べニューの画像をクリア
		var scripts = transform.GetComponentsInChildren<Venue>();
		foreach( var script in scripts ){
			script.ClearData();
		}

		//API経由でカテゴリ別人気べニューを取得
		venues.Clear();
		FourSquareAPI.Instance.SearchByCity( city.title, category.id, 0, PHOTO_NUM, OnLoadData );
	}

	void OnLoadData ( List<VenueData> list ) {

		if ( list != null ) { venues.AddRange (list); }
		if( list != null && list.Count != 0 && venues.Count < PHOTO_NUM ){

			//API経由でカテゴリ別人気べニューを取得
			FourSquareAPI.Instance.SearchByCity( city.title, category.id, venues.Count, PHOTO_NUM - venues.Count, OnLoadData );

		}else{

			TaskScheduler.Instance.AddIterator( LoadVenues() );
		}
	}

	IEnumerator LoadVenues() {
	
		// べニューパネルに情報をセット
		int index = 0;
		foreach( Transform child in transform ){

			if (child.name != "Venue") { continue; }

			if ( index < venues.Count ) { 
				child.GetComponent<Venue> ().LoadData (city, category, venues [index]);
			}else{
				
				// フェードアウト
				iTween.FadeTo( child.gameObject, iTween.Hash(
					"alpha", 0.0f,
					"time", 1.5f,
					"includechildren", true,
					"easing", iTween.EaseType.easeInCirc
				));
			}

			index += 1;
			yield return null;
		}
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
