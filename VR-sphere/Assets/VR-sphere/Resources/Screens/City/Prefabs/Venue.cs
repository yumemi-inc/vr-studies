//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.IO;  

public class Venue : MonoBehaviour, PointController.IPointControllerTarget {

	public enum VENUE_MODE { FOR_CITY, FOR_CATEGORY, FOR_VENUE };
	VENUE_MODE mode = VENUE_MODE.FOR_VENUE;

	//------------------------------------------------------------------------------------------------------------------------------//
	CityData city = null;
	CategoryData category = null;
	VenueData venue = null;

	GameObject photo;
	GameObject loader;
	Texture    no_photo;

	//------------------------------------------------------------------------------------------------------------------------------//
	static string SAVE_DIR = Directory.GetCurrentDirectory() + "/Photos/";
	static int    PHOTO_SIZE_S = 250;
	static int    PHOTO_SIZE_L = 500;
	static string PHOTO_PATH = SAVE_DIR + "{0}-{1}.jpg"; 
	string CreatePhotoPath( string venueId ){
		int size = mode == VENUE_MODE.FOR_VENUE ? PHOTO_SIZE_L : PHOTO_SIZE_S;
		return string.Format ( PHOTO_PATH, venueId, size + "x" + size );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void Awake() {
		photo = transform.FindChild ("Panel/Photo").gameObject;
		loader = transform.FindChild ("Panel/Loader").gameObject;
		no_photo = Resources.Load ("Images/No-Photo") as Texture2D;
	}

	void Update() {

		// ローダーアニメ
		if( loader.activeSelf ){
			loader.transform.Rotate ( new Vector3( 0, 0, 3f ) );
		}

		// ビルボードアニメ
		if( mode != VENUE_MODE.FOR_VENUE ){
			var ca = Camera.main.transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler( new Vector3 ( 0, ca.y, 0 ) );
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void OnPointControllerHit( bool isOn ) {

		if( mode == VENUE_MODE.FOR_VENUE ){	return;	} 

		// 手前に出す
		iTween.MoveTo( gameObject, iTween.Hash(
			"z", isOn ? -0.6f : 0f,
			"time", 0.5f,
			"islocal", true
		));
	}

	public void OnPointControllerClick( bool isOn ) {

		if( mode == VENUE_MODE.FOR_VENUE ){	return;	} 

		// 縮小する
		iTween.ScaleTo( gameObject, iTween.Hash(
			"scale", isOn ? new Vector3 ( 0.75f, 0.75f, 0.75f ) : new Vector3 ( 1f, 1f, 1f ),
			"time", 0.5f,
			"easing", iTween.EaseType.easeOutElastic
		));

		if( isOn ){

			// カテゴリ詳細スクリーンをロードする
			if( mode == VENUE_MODE.FOR_CITY ){
				VRSphere.Instance.PushCategoryScreen( city, category );
			
			// べニュー詳細スクリーンをロードする
			}else if( mode == VENUE_MODE.FOR_CATEGORY ){
				VRSphere.Instance.PushVenueScreen( city, category, venue );
			}
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public void InitData( VENUE_MODE _mode ){
		mode = _mode;
	}

	public void ClearData(){

		photo.SetActive( false );
		loader.SetActive( true );

		// 以前のサムネイルを削除
		Texture tex = photo.GetComponent<Renderer>().material.mainTexture;
		photo.GetComponent<Renderer> ().material.mainTexture = null;
		if( tex && tex.name != "No-Photo" ){ MonoBehaviour.Destroy( tex ); }
	}

	public void LoadData( CityData _city, CategoryData _category, VenueData _venue ){

		city = _city;
		category = _category;
		venue = _venue;

		// APIもしくはローカルキャッシュからサムネイルを取得
		LoadVenuePhoto ();
	}

	void LoadVenuePhoto(){

		int size = mode == VENUE_MODE.FOR_VENUE ? PHOTO_SIZE_L : PHOTO_SIZE_S;
		string path = CreatePhotoPath( venue.id );
		Directory.CreateDirectory ( SAVE_DIR );

		if( File.Exists( path ) ){

			// ローカルからサムネイルをロードする
			TextureLoadFromLocal( path, size );

		}else{

			// ExploreAPIから直接取得したURLでロードする
			var photoURL = venue.photo_prefix + size + "x" + size + venue.photo_suffix;
			OnLoadFromAPI ( photoURL );
		}
	}

	void OnLoadFromAPI( string url ){
		TaskScheduler.Instance.AddIterator( TextureLoadFromWeb( url ) );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void TextureLoadFromLocal( string path, int size ){

		byte[] bytes = File.ReadAllBytes( path );
		Texture2D texture = new Texture2D( size, size );

		if ( texture.LoadImage (bytes) ) {
			photo.GetComponent<Renderer> ().material.mainTexture = texture;
			//yield return null;
		}

		photo.SetActive( true );
		loader.SetActive( false );
		iTween.Stop ( loader );
	}

	IEnumerator TextureLoadFromWeb( string url ){

		using (WWW www = new WWW (url)) {

			while (www.isDone == false) {
				yield return null;
			}

			if (www.error == null) {

				photo.GetComponent<Renderer> ().material.mainTexture = www.texture;
				yield return null;

				// ローカルへキャッシュする
				string path = CreatePhotoPath (venue.id);
				if ( File.Exists (path) == false ) {
					File.WriteAllBytes( path, www.bytes );
					yield return null;
				}

			} else {

				// NO-IMAGEをアタッチする
				photo.GetComponent<Renderer> ().material.mainTexture = no_photo;
			}
		}

		photo.SetActive (true);
		loader.SetActive (false);
		iTween.Stop (loader);
	}
}
