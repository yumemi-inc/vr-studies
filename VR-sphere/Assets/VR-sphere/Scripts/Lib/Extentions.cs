using System.Linq;
using UnityEngine;

public static class GameObjectExtensions {
	
	public static GameObject[] GetChildren ( this GameObject self, bool includeInactive = false ) {
		return self.GetComponentsInChildren<Transform>( includeInactive )
			.Where( c => c != self.transform )
			.Select( c => c.gameObject )
			.ToArray();
	}
}

public static class ComponentExtensions {
	
	public static GameObject[] GetChildren ( this Component self, bool includeInactive = false ) {
		return self.GetComponentsInChildren<Transform>( includeInactive )
			.Where( c => c != self.transform )
			.Select( c => c.gameObject )
			.ToArray();
	}
}