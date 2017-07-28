//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace VRAcademy {

	public class Avatar : Photon.MonoBehaviour {

		//------------------------------------------------------------------------------------------------------------------------------//
		float animeSpeed = 1.5f;
		float forwardSpeed = 2.0f;
		float rotateSpeed = 2.5f;

		Animator 	animator;
		bool isAutoMoving = false;

		//------------------------------------------------------------------------------------------------------------------------------//
		void Start () {

			//アニメーターを取得する
			animator = GetComponent<Animator> ();
			animator.speed = animeSpeed;
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void Update () {

			// 自分の所有じゃなければスルー
			if( photonView.isMine == false ){
				return;
			}

			// 別コントローラが操作している時はスルー
			if( animator == null || Input.GetKey (KeyCode.LeftAlt) ){
				return;
			}

			// キーボード入力による移動処理 //VRの時は使用不可
			MoveByVector( Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical") );
		}
			
		//------------------------------------------------------------------------------------------------------------------------------//
		// ストリームによる状態の同期 
		//------------------------------------------------------------------------------------------------------------------------------//
		public void ChangeName( string name ) {

			//自クライアント所有の名前を変更する
			this.gameObject.name = name;
			transform.FindChild ("Name").gameObject.GetComponent<TextMesh>().text = name;
			//transform.FindChild ("Name").gameObject.GetComponent<TextMesh>().fontSize = 25;
			transform.FindChild ("Name").gameObject.GetComponent<TextMesh>().color = new Color( 0.8f, 0, 1 );
		}

		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

			if ( stream.isWriting ) {

				// 自クライアント所有のオブジェクトの状態変更を送信
				string myName = this.gameObject.name;
				stream.SendNext( myName );

			} else {

				// 他クライアント所有のオブジェクトの状態変更を受信
				string otherName = (string)stream.ReceiveNext();

				// 名前の変更を反映する
				this.gameObject.name = otherName;
				transform.FindChild ("Name").gameObject.GetComponent<TextMesh>().text = otherName;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public void ChangeAction( string animeId ){

			// 全てのクライアントにアニメキーを送信
			PhotonView.Get(this).RPC( "OnChangeAction", PhotonTargets.All, animeId );
		}

		[PunRPC]
		void OnChangeAction( string animeId ){
			animator.CrossFade ( animeId, 0.15f, 0, 0f);
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public void MoveByVector ( float h, float v ) {

			if( isAutoMoving ){ return; }

			// アニメーターとの連携
			animator.SetFloat ("Speed", v);	

			// 回転と移動
			transform.Rotate (0, h * rotateSpeed, 0);
			if ( v > 0.1 || v < -0.1 ) {
				
				Vector3 velocity = new Vector3 (0, 0, v);
				velocity = transform.TransformDirection (velocity);
				velocity *= forwardSpeed;
				transform.localPosition += velocity * Time.fixedDeltaTime;
				if (v > 0.1)animator.Play ("Walk");
				else animator.Play ("BackWalk");
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public void MoveToPosition ( Vector3 pos ) {

			//特定地点へ向かってトゥイーンで移動する
			Sequence sequence = DOTween.Sequence ();
			sequence.Append( transform.DOLocalMove(pos, 4.0f)).SetEase( Ease.Linear );

			sequence.OnStart (
				()=>{
					isAutoMoving = true;
					transform.LookAt ( pos );
					animator.SetFloat ("Speed", 1.0f);
					ChangeAction ("Walk");
				}
			);
			sequence.OnComplete (
				()=>{
					animator.SetFloat ("Speed", 0.0f);
					ChangeAction ("Idle");
					transform.LookAt ( new Vector3( 0, 0, 5 ) );
					isAutoMoving = false;
				}
			);

			sequence.Play();
		}
	}
}