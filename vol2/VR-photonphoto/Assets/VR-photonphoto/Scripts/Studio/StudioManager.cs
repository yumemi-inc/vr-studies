//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

namespace VRAcademy {

	public class StudioManager : MonoBehaviour {

		//------------------------------------------------------------------------------------------------------------------------------//
		private static StudioManager instance = null;
		public static StudioManager Instance {
			get {
				if (instance != null) return instance;
				instance = FindObjectOfType(typeof(StudioManager)) as StudioManager;
				return instance;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public string slackApiToken = "";
		RenderTexture renderTex = null;

		//------------------------------------------------------------------------------------------------------------------------------//
		#if UNITY_EDITOR
		static string SAVE_DIR = Directory.GetCurrentDirectory() + "/Photos/";
		#elif UNITY_ANDROID || UNITY_IOS
		static string SAVE_DIR = Application.persistentDataPath + "/Photos/";
		#endif

		//------------------------------------------------------------------------------------------------------------------------------//
		public void Setup () {

			// ステージオブジェクトとしてインスタンシエイト
//			var counter = PhotonNetwork.InstantiateSceneObject("Prefabs/Studio/Countdown UI", Vector3.zero, Quaternion.identity, 0, null) as GameObject;
//			counter.transform.parent = transform;
//			counter.transform.position = new Vector3( 0, 5.4f, 8.8f );
//			counter.GetComponent<CountDownUI>().Setup ();

			// Photonログイン後にタイマーを開始
			var counter = GameObject.Find ("Countdown UI").GetComponent<CountDownUI>();
			counter.Setup ();
		}

		public IEnumerator UpdatePhoto () {

			// レンダーテクスチャをテクスチャに焼き込む
			renderTex = transform.Find ("Monitor/Monitor Camera").GetComponent<Camera> ().targetTexture;
			Texture2D tex0 = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGB24, true);
			RenderTexture.active = renderTex;
			tex0.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
			tex0.Apply();

			// リサイズする
			Texture2D tex = new Texture2D(tex0.width/2, tex0.height/2, TextureFormat.RGB24, false);
			tex.SetPixels(tex0.GetPixels(1));
			tex.Apply();

			yield return null;

			// 左右反転させる
//			Color[] px = tex.GetPixels(0);
//			int w = tex.width; int h = tex.height;
//			Color[] fliped = new Color[w * h]; 
//			for (int y = 0; y < h; y++) {
//				for (int x = 0; x < w; x++){
//					fliped[x + y*w] = px[ (w-1)-x + y*w ];
//				}
//			}
//			tex.SetPixels(fliped);
//			tex.Apply();
//
			yield return null;

			// 写真を保存する
			string fileName = "Photo-" + System.DateTime.Now.ToString( "yyyyMMdd-HHmmss" ) + ".png";
			string savePath = SAVE_DIR + fileName;
			Directory.CreateDirectory( SAVE_DIR );
			byte[] bytes = tex.EncodeToPNG();
			File.WriteAllBytes( savePath, bytes );
			Object.Destroy(tex);
			//Debug.Log ("記念写真を保存しました: " + savePath);

			yield return null;

			// 写真を読み込む ( 一度読み込み直さないとテクスチャとしてセットできない　)
			bytes = File.ReadAllBytes( savePath );
			Texture2D tex2 = new Texture2D( renderTex.width, renderTex.height );
			if ( tex2.LoadImage ( bytes ) ) {
				
				// 写真プレファブに写真を読み込んで上から落とす
				var prefab = Resources.Load("Prefabs/Studio/Polaroid") as GameObject;
				var polaroid = GameObject.Instantiate( prefab );
				polaroid.transform.parent = transform;

				GameObject photo = polaroid.transform.FindChild("Photo").gameObject;
				//GameObject photo = transform.FindChild("Polaroid/Photo").gameObject;

				if( Random.value < 0.5f ){
					photo.transform.parent.position = new Vector3 ( -10, 10, 10 );
				}else{
					photo.transform.parent.position = new Vector3 ( 10, 10, 10 );
				}

				photo.GetComponent<Renderer>().material.color = Color.white;
				photo.GetComponent<Renderer> ().material.mainTexture = tex2;

				photo.transform.parent.LookAt ( Vector3.zero );
				photo.transform.parent.eulerAngles =new Vector3( 0, photo.transform.parent.eulerAngles.y, 0 );
			}

			// Slackへのアップロード
			// マスタークライアントだけが送信を行う
			if ( PhotonNetwork.isMasterClient ) {
				UploadToSlack (fileName, bytes);
			}
		}

		void UploadToSlack ( string fileName, byte[] bytes ) {

			var data = new SlackAPI.UploadData {
				token = slackApiToken, 
				filename = fileName, 
				filedata = bytes, 
				title = "VRAcademy-VRxNet",
				initial_comment = "VR空間で記念撮影！！ - " + System.DateTime.Now.ToString ("yyyy/MM/dd-HH:mm:ss"),
				channels = "#general", 
			};

			var routine = SlackAPI.Upload (data);
			StartCoroutine (routine);
		}
	}
}