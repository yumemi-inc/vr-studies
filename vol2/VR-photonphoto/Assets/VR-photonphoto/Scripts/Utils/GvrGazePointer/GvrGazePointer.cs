// Copyright 2015 Google Inc. All rights reserved.
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

/// GvrReticlePointerの拡張。注視クリック機能を付加する。
/// なぜか継承できないダメ設計になっているのでコピー上書きとする　by miuccie-miurror
//[AddComponentMenu("GoogleVR/UI/GvrReticlePointer")]
[RequireComponent(typeof(Renderer))]
public class GvrGazePointer : MonoBehaviour {
	
	private GvrGazePointerImpl gazePointerImpl;

	/// Number of segments making the reticle circle.
	public int reticleSegments = 20;

	/// Growth speed multiplier for the reticle/
	public float reticleGrowthSpeed = 8.0f;

	void Awake() {
		gazePointerImpl = new GvrGazePointerImpl();
	}

	void Start() {

		Image indicator = transform.FindChild ("Canvas/Indicator").GetComponent<Image>();
		gazePointerImpl.OnStart( indicator );

		gazePointerImpl.MaterialComp = gameObject.GetComponent<Renderer>().material;
		gazePointerImpl.ReticleGrowthSpeed = reticleGrowthSpeed;
		gazePointerImpl.PointerTransform = transform;
	}

	void Update() {
		if( gazePointerImpl != null ){
			gazePointerImpl.UpdateIndicator();
		}
		SetAsMainPointer (); //これを毎回呼ばないとイベントシステムに乗らない模様
	}

	public void SetAsMainPointer() {
		GvrPointerManager.Pointer = gazePointerImpl;
	}
}
