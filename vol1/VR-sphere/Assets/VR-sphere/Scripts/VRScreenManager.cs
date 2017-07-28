//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///<summary>
/// 複数の階層スクリーンを維持管理する抽象クラス. 
/// 継承して使用してください.
///</summary>
public class VRScreenManager : MonoBehaviour {

	protected List<GameObject> screens = new List<GameObject>();

	//------------------------------------------------------------------------------------------------------------------------------//
	//最前面にスクリーンを追加する
	protected void PushScreen ( GameObject frontScreen ) {

		if ( screens.Count > 0 ) { 
			var preScreen = screens [screens.Count - 1];
			if( frontScreen == preScreen ){ return; }
			preScreen.GetComponent<VRScreen> ().OnFront (false);
		}

		screens.Add ( frontScreen ); 
		frontScreen.GetComponent<VRScreen>().OnPushIn ();
		frontScreen.GetComponent<VRScreen>().OnFront (true);

		OnPushScreen ( frontScreen );
	}

	//最前面のスクリーンを削除する
	protected void PopScreen () {

		if( screens.Count > 1 ){
			
			var screen = screens[ screens.Count - 1 ];
			screens.RemoveAt ( screens.Count - 1 );
			screen.GetComponent<VRScreen>().OnPopOut ();
			screen.GetComponent<VRScreen>().OnFront ( false );

			// 削除時はアニメーション終了後にハンドラを呼ぶ
			var frontScreen = screens[ screens.Count - 1 ];
			var onComplete = (System.Action)( () => {
				frontScreen.layer = LayerMask.NameToLayer("Ignore Raycast");
				frontScreen.GetComponent<VRScreen>().OnFront (true);
			});

			OnPopScreen ( frontScreen, onComplete );
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	protected virtual void OnPushScreen ( GameObject frontScreen ) {
		// 下位クラスで実装する
	}

	protected virtual void OnPopScreen ( GameObject frontScreen, System.Action onComplete ) {
		// 下位クラスで実装する
	}
}
