//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//
using UnityEngine;
using System.Collections;

namespace VRStudies {
	
	public class CameraUIController : MonoBehaviour {

		public float smooth = 10f;
		public bool  isFirstPerson = false;

		GameObject avatar = null;
		Transform  defaultPos;
		Transform  frontPos;

		//------------------------------------------------------------------------------------------------------------------------------//
		public void Setup ( GameObject _avatar ) {

			avatar = _avatar;

			// カメラポジションの取得
			frontPos = avatar.transform.Find ("Position/Front").transform;
			if ( isFirstPerson ) {
				defaultPos = avatar.transform.Find("Position/First");
			} else {
				defaultPos = avatar.transform.Find ("Position/Third");
			}
				
			//カメラ位置を変更する
			transform.position = defaultPos.position;	
			transform.forward = defaultPos.forward;
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void FixedUpdate ()	{

			if( avatar == null ){ return; }

			// キーによってカメラ位置を変更
			if ( Input.GetKey (KeyCode.LeftShift) ) {

				//フロント表示
				transform.position = frontPos.position;	
				transform.forward = frontPos.forward;

			} else {

				//バック表示 //補間しながら対象についていく
				transform.position = Vector3.Lerp (transform.position, defaultPos.position, Time.fixedDeltaTime * smooth);	
				transform.forward = Vector3.Lerp (transform.forward, defaultPos.forward, Time.fixedDeltaTime * smooth);
			}
		}
	}
}