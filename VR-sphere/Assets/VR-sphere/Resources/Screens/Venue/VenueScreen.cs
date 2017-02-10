//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VenueScreen : VRScreen {

	//------------------------------------------------------------------------------------------------------------------------------//
	public void LoadData( CityData city, CategoryData category, VenueData venue ) {

		TextMesh[] texts = transform.GetComponentsInChildren<TextMesh> ();
		foreach( TextMesh text in texts ){
			text.text = "";
		}

		// 基本データを読み込む
		transform.FindChild ("Info/Venue").GetComponent<TextMesh>().text = venue.title;
		transform.FindChild ("Info/City-Category").GetComponent<TextMesh>().text = city.title + " : [ " + category.title + " ]";

		// サムネイルを読み込む
		transform.FindChild ("Venue").GetComponent<Venue>().ClearData();
		transform.FindChild ("Venue").GetComponent<Venue>().LoadData( city, category, venue );

		// API経由で詳細データを読み込む
		FourSquareAPI.Instance.LoadVenueInfo( venue.id, OnLoadData );
	}

	void OnLoadData( LitJson.JsonData json ) {
		
		var venue = json["response"]["venue"];

		//------------------------------------------------------------------------------------------------------------------------------//
		string category = "";
		if( venue["categories"] != null ){
			category = " [ " + venue["categories"][0]["name"] + " ]";
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		string address = "";
		if( venue["location"]["formattedAddress"] != null ){
			address = "" + venue["location"]["formattedAddress"][0];
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		string tags = "";
		for( int i = 0; i < venue["tags"].Count; i++ ){
			tags += venue["tags"][i];
			if ( i != venue["tags"].Count - 1 ) tags += " / ";
		}
		if( tags.Length > 60 ){ tags = tags.Insert ( 60, "\n" ); }
		if( tags.Length > 120 ){ tags = tags.Substring( 0, 120 ); }

		//------------------------------------------------------------------------------------------------------------------------------//
		string rating = "[★] 0.0  [♥] 0";
		try{
			if( venue["rating"]	 != null && venue["likes"] != null ){
				rating = "[★] " + venue["rating"] + "  [♥] " + venue["likes"]["count"];
			}
		}catch( System.Exception e ){
			Debug.Log( e );
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		transform.FindChild ("Info/City-Category").GetComponent<TextMesh>().text += category;
		transform.FindChild ("Info/Address").GetComponent<TextMesh>().text = address;
		transform.FindChild ("Info/Tags").GetComponent<TextMesh>().text = "[ " + tags + " ]";
		transform.FindChild ("Info/Rating").GetComponent<TextMesh>().text = rating;
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public override void OnPushIn () {

		base.OnPushIn ();

		// 左右に散らしてから再度集合
		var panel = transform.FindChild ("Venue/Panel");
		panel.localPosition = new Vector3( -10, 0, 0 );	
		iTween.MoveTo( panel.gameObject, iTween.Hash (
			"x", 0f,
			"time", 2.0f,
			"islocal", true,
			"easing", iTween.EaseType.linear
		));

		var info = transform.FindChild ("Info");
		info.localPosition = new Vector3( 10, 0, 0 );	
		iTween.MoveTo( info.gameObject, iTween.Hash (
			"x", 0.18f,
			"time", 2.0f,
			"islocal", true,
			"easing", iTween.EaseType.linear
		));
	}

	public override void OnPopOut () {

		base.OnPopOut ();

		// 左右に散らす
		var panel = transform.FindChild ("Venue/Panel");
		iTween.MoveTo( panel.gameObject, iTween.Hash (
			"x", -5f,
			"time", 2.0f,
			"islocal", true,
			"easing", iTween.EaseType.easeInCirc
		));

		var info = transform.FindChild ("Info");
		iTween.MoveTo( info.gameObject, iTween.Hash (
			"x", 5f,
			"time", 2.0f,
			"islocal", true,
			"easing", iTween.EaseType.easeInCirc
		));
	}
}
