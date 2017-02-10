//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml;
using System.IO;

//------------------------------------------------------------------------------------------------------------------------------//
public class CityData {
	public string title;
	public float lat;
	public float lng;
	public CityData( string _title, float _lat, float _lng ){
		title = _title; lat = _lat; lng = _lng;
	}
}
public class CategoryData {
	public string title;
	public string id;
}
public class VenueData {
	public string title;
	public string id;
	public string photo_prefix;
	public string photo_suffix;
}

//------------------------------------------------------------------------------------------------------------------------------//
///<summary>
/// FourSquare-APIを利用して、各都市の各カテゴリのスポット情報を取得する
///</summary>
///<remarks>
/// 下記のページよりFourSquare-API-IDとSecret-Keyを取得して設定してください.
/// https://developer.foursquare.com/
///</remarks>
public class FourSquareAPI : MonoBehaviour {

	// FourSquare-API-keyを設定してください
	public string foursquare_crient_id = "";
	public string foursquare_crient_secret = "";
	static string version = "&v=20160101";
	static string base_url = "https://api.foursquare.com/v2/venues/";
	static string api_key = "?client_id={0}" + "&client_secret={1}" + version;

	//------------------------------------------------------------------------------------------------------------------------------//
	private static FourSquareAPI instance;
	public static FourSquareAPI Instance {
		get {
			if (instance != null) return instance;
			instance = FindObjectOfType(typeof(FourSquareAPI)) as FourSquareAPI;
			api_key = string.Format ( api_key, instance.foursquare_crient_id, instance.foursquare_crient_secret );
			return instance;
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void SearchByCity( string city, string categoryId, int offset, int limit, System.Action<List<VenueData>> oncomplete ) {

		var near = "&near=" + city.Replace(" ", "+");
		var category = "&categoryId=" + categoryId;
		var option = "&limit=" + limit + "&offset=" + offset + "&venuePhotos=1";
		var url = base_url + "explore" + api_key + option + category + near;

		System.Action<LitJson.JsonData> onload = (LitJson.JsonData json) => {
			if( json != null ){
				oncomplete( CreateVenues( json ) );
			}else{
				oncomplete( null );
			}
		};

		TaskScheduler.Instance.AddIterator( RequestAPI( url, onload ) );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	List<VenueData> CreateVenues ( LitJson.JsonData json ) {

		List<VenueData> venues = new List<VenueData> ();
		var venuesJ = json["response"]["groups"][0]["items"];

		for( int i = 0; i < venuesJ.Count; i++ ){

			var venueJ = venuesJ[i]["venue"];

			VenueData data = new VenueData ();
			data.id = (string)venueJ["id"];
			data.title = (string)venueJ["name"];

			try{
				data.photo_prefix = (string)venueJ["featuredPhotos"]["items"][0]["prefix"];
				data.photo_suffix= (string)venueJ["featuredPhotos"]["items"][0]["suffix"];
			}catch( System.Exception e ){
				Debug.Log (e);
			}

			venues.Add ( data );
		}

		return venues;
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void LoadVenueInfo( string venueId, System.Action<LitJson.JsonData> oncomplete ) {
		var url = base_url + venueId + api_key;
		TaskScheduler.Instance.AddIterator( RequestAPI( url, oncomplete ) );
	}

	public void LoadVenuePhoto( string venueId, string size, System.Action<string> oncomplete ) {

		var url = base_url + venueId + "/photos" + api_key + "&limit=1";

		System.Action<LitJson.JsonData> onload = (LitJson.JsonData json) => {
			if( json != null && json["response"]["photos"]["items"].Count > 0 ){
				var photo = json["response"]["photos"]["items"][0];
				var photoURL = photo["prefix"] + size + photo["suffix"];
				oncomplete( photoURL );
			}else{
				oncomplete( null );
			}
		};

		TaskScheduler.Instance.AddIterator( RequestAPI( url, onload ) );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	IEnumerator RequestAPI( string url, System.Action<LitJson.JsonData> oncomplete ) {

		using (WWW www = new WWW (url)) {
			
			while (www.isDone == false) {
				yield return null;
			}

			if (www.error == null) {
				var result = System.Text.Encoding.UTF8.GetString (www.bytes, 0, www.bytes.Length);
				var dict = LitJson.JsonMapper.ToObject (result);
				oncomplete (dict);
			} else {
				Debug.Log (www.error + ":" + url);
				oncomplete (null);
			}
		}
	}
}