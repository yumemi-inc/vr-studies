using UnityEngine;
using System.Collections;

namespace VRStudies { namespace SinglePlayer {

public class Player : MonoBehaviour {

	void Start () {

		// カメラをアバターの子にして追従させる
		GameObject avatar = transform.FindChild("Avatar").gameObject;
		GameObject camera = transform.FindChild("Camera").gameObject;
		camera.transform.parent = avatar.transform;
	}
}

}}
