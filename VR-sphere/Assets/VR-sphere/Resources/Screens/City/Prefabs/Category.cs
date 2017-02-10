//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Category : MonoBehaviour {

	CityData city = null;
	CategoryData category = null;
	List<VenueData> venues = new List<VenueData>();

	//------------------------------------------------------------------------------------------------------------------------------//
	public void InitData ( string _category, string _categoryId ) {

		// データを初期化する
		category = new CategoryData ();
		category.title = _category; 
		category.id = _categoryId; 

		foreach( Transform venue in transform ){
			if( venue.name != "Venue" ){ continue; }
			venue.GetComponent<Venue> ().InitData ( Venue.VENUE_MODE.FOR_CITY );
		}

		// Icon部分にカテゴリのアイコンを読み込む
		if ( _category != null && _category != "" ) {
			transform.FindChild ("Icon/Quad").GetComponent<Renderer> ().material.mainTexture = Resources.Load ("Images/Icons/" + _category) as Texture2D;
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void LoadData ( CityData _city ) {

		city = _city;

		// べニューの画像をクリア
		var scripts = transform.GetComponentsInChildren<Venue>();
		foreach( var script in scripts ){
			script.ClearData();
		}

		//API経由でカテゴリ別人気べニューを取得
		venues.Clear();
		FourSquareAPI.Instance.SearchByCity( city.title, category.id, 0, 6, OnLoadData );
	}

	void OnLoadData ( List<VenueData> list ) {

		if ( list != null ) { 
			venues.AddRange (list);
			TaskScheduler.Instance.AddIterator( LoadVenues() );
		}
	}

	IEnumerator LoadVenues() {

		// べニューパネルに情報をセット
		int index = 0;
		foreach( Transform child in transform ){
			if (child.name != "Venue") { continue; }
			child.GetComponent<Venue> ().LoadData( city, category, venues[index] );
			index += 1;
			if( index >= venues.Count ){ break; }
			yield return null;
		}
	}
}
