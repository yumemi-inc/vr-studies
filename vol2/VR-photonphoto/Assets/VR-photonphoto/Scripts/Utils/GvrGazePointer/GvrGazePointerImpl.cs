// Copyright 2017 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// GvrReticlePointerImplの拡張。注視クリック機能を付加する。by miuccie-miurror
public class GvrGazePointerImpl : GvrReticlePointerImpl {

	Image indicator = null;
	bool isIndicatorOn = false;
	GameObject indicatedObject = null;


  public void OnStart ( Image _indicator ) {
		indicator = _indicator;
		base.OnStart();
  }

  public override void OnPointerEnter(RaycastResult rayastResult, Ray ray,
			bool isInteractive) {

		// IPointerClickHandlerを実装しているか & Ignoreレイヤーをみる
		var target = rayastResult.gameObject;
		if ( target.layer != LayerMask.NameToLayer ("Ignore Raycast") ) {
			var itarget = target.GetComponent (typeof(IPointerClickHandler)) as IPointerClickHandler;
			if (itarget != null) {
				isIndicatorOn = true;
				indicatedObject = target;
			}
			base.OnPointerEnter (rayastResult, ray, isInteractive);
		}
  }

	public override void OnPointerHover(RaycastResult rayastResult, Ray ray,
		bool isInteractive) {

		// Ignoreレイヤーをみる
		var target = rayastResult.gameObject;
		if ( target.layer != LayerMask.NameToLayer ("Ignore Raycast") ) {
			base.OnPointerHover (rayastResult, ray, isInteractive);
		}
	}

 	public override void OnPointerExit(GameObject previousObject) {
		
		// IPointerClickHandlerを実装しているか & Ignoreレイヤーをみる
		if ( previousObject.layer != LayerMask.NameToLayer ("Ignore Raycast") ) {
			var itarget = previousObject.GetComponent (typeof(IPointerClickHandler)) as IPointerClickHandler;
			if (itarget != null) {
				isIndicatorOn = false;
			}
			base.OnPointerExit (previousObject);
		}

  }

  public void UpdateIndicator() {

		//ローディングアニメ
		if ( indicator && isIndicatorOn ) {
			
			indicator.fillAmount += 0.8f * Time.deltaTime;

			//１周したらクリックイベントを送信する
			if( indicatedObject && indicator.fillAmount >= 1.0f ){

				//IPointerClickHandlerへイベント送信
				ExecuteEvents.Execute<IPointerClickHandler>(indicatedObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
				indicatedObject = null;
				isIndicatorOn = false;
			}

		} else {
			indicator.fillAmount = 0;
		}
  }
}
