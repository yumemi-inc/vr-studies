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

using Photon.Pun;
using Photon.Realtime;

namespace VRStudies {

	public class CountDownUI : MonoBehaviourPunCallbacks, IInRoomCallbacks {

		//------------------------------------------------------------------------------------------------------------------------------//
		public int shutterSec = 10;

		public List<AudioClip>  sounds = new List<AudioClip>();
		AudioSource   			audioSource = null;
		TextMesh  	  			counter = null;

		float timeElapsed = 0;
		int   secElapsed = 0;
		bool  isActiveCounter = false;

		//------------------------------------------------------------------------------------------------------------------------------//
		public void Setup () {

			// 撮影カウンターの準備
			isActiveCounter = true;
			audioSource = GetComponent<AudioSource> ();
			counter = GetComponent<TextMesh> ();
			UpdateCounter( shutterSec );
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void FixedUpdate () {

			//マスタークライアントだけが更新する
			if( PhotonNetwork.IsMasterClient == false ){ return; }
			if( isActiveCounter == false ){ return; }

			//一定時間ごとにカウントダウン & シャッターを切る
			timeElapsed += Time.deltaTime;
			if( timeElapsed >= 1.0f ) {

				timeElapsed = 0f;
				secElapsed += 1;

				//カウンターの表示を変更
				SendRoomCount ( secElapsed );
			}
		}

		public void SendRoomCount( int _secElapsed ) {
			
			// ルームプロパティでカウント送信
			var properties  = new ExitGames.Client.Photon.Hashtable();
			properties.Add( "secElapsed", _secElapsed );
			PhotonNetwork.CurrentRoom.SetCustomProperties( properties );
		}

		public void OnRoomPropertiesUpdate( ExitGames.Client.Photon.Hashtable property ){

			// ルームプロパティでカウント更新
			object value = null;
			if( property.TryGetValue( "secElapsed", out value ) ){
				secElapsed = (int)value;
				UpdateCounter ( shutterSec - secElapsed );
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void ResetCounter() {
			isActiveCounter = true;
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void UpdateCounter( int count ) {

			if( counter == null ){
				Setup();
			}

			if ( count > 0 ) {

				counter.text = count.ToString();
				audioSource.PlayOneShot (sounds [0]);

			} else {

				counter.text = "Shot!";
				audioSource.PlayOneShot ( sounds[1] );

				// フラッシュエフェクトをカメラに付加する
				float val = 0;
				ScreenOverlay effect = GameObject.Find("Main Camera").GetComponent<ScreenOverlay>();

				var tween1 = DOTween.To (() => val, x => val = x, 2f, 0.6f).OnUpdate (() => {
					effect.intensity = val;
				});
				var tween2 = DOTween.To (() => val, x => val = x, 0f, 0.8f).OnUpdate (() => {
					effect.intensity = val;
				});

				Sequence sequence = DOTween.Sequence ();
				sequence.Append( tween1 );
				sequence.Append( tween2 );
				sequence.Play();

				//写真を撮影する
				secElapsed = -1;
				var routine = StudioManager.Instance.UpdatePhoto();
				StartCoroutine ( routine );
				isActiveCounter = false;
				Invoke( "ResetCounter", 5f );
			}
		}
	}
}