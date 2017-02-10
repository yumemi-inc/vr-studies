//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using System.Collections;

///<summary>
/// 階層スクリーン1枚を表現する抽象クラス. 
/// 継承して使用してください.
///</summary>
public class VRScreen : MonoBehaviour {

	// 出現時ハンドラ
	public virtual void OnPushIn () {

		gameObject.SetActive ( true );

		// 全子要素のalphaを0にした後フェードイン
		Renderer[] children = transform.GetComponentsInChildren<Renderer> ( true );
		for( int i = 0; i < children.Length; i++ ) {
			children[i].material.color = new Color( 1, 1, 1, 0 );	
		}

		iTween.FadeTo( gameObject, iTween.Hash (
			"alpha", 1f,
			"time", 1.5f,
			"includechildren", true,
			"easing", iTween.EaseType.easeInCirc
		));
	}

	// 消滅時ハンドラ
	public virtual void OnPopOut () {

		// フェードアウトした後非表示
		iTween.FadeTo( gameObject, iTween.Hash(
			"alpha", 0.0f,
			"time", 1.5f,
			"includechildren", true,
			"easing", iTween.EaseType.easeInCirc,
			"oncomplete", (System.Action)( () => {
				gameObject.SetActive ( false );
			})
		));
	}

	// 最前面時ハンドラ
	public virtual void OnFront ( bool isOn ) {

		gameObject.layer = isOn ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Ignore Raycast");
		var children = gameObject.GetChildren( true );
		foreach( GameObject child in children ) {
			child.layer = gameObject.layer;
		}
	}
}
