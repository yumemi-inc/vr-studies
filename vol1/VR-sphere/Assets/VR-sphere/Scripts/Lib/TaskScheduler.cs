//
// from https://gist.github.com/divide-by-zero/5b2e7f333978c0f501cd
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
	private static TaskScheduler instance;

	private readonly Dictionary<Guid, IEnumerator> iteratorList = new Dictionary<Guid, IEnumerator>();
	public float targetTime = 0.005f;

	public static TaskScheduler Instance
	{
		get
		{
			if (instance != null) return instance;
			instance = FindObjectOfType(typeof(TaskScheduler)) as TaskScheduler;
			if (!instance)
			{
				instance = new GameObject("TaskScheduler").AddComponent<TaskScheduler>();
			}
			return instance;
		}
	}

	public Guid AddIterator(IEnumerator iterator)
	{
		var guid = Guid.NewGuid();
		iteratorList.Add(guid, iterator);
		return guid;
	}

	public void RemoveIterator(Guid guid)
	{
		if (iteratorList.ContainsKey(guid))
		{
			iteratorList.Remove(guid);
		}
	}

	public void StopAllIterator()
	{
		iteratorList.Clear();
	}

	public Guid AddAction(Action act)
	{
		return AddIterator(ActionIterator(act));
	}

	public IEnumerator ActionIterator(Action act)
	{
		if(act != null)act();
		yield return null;
	}

	public void Start()
	{
		StartCoroutine(Iterator());
	}

	private IEnumerator Iterator()
	{
		while (true)
		{
			var ts = Time.realtimeSinceStartup;
			do
			{
				if (iteratorList.Count <= 0) break;
				foreach (var itr in iteratorList.ToList())
				{
					if (itr.Value.MoveNext() == false)
					{
						iteratorList.Remove(itr.Key);
					}
					if (Time.realtimeSinceStartup - ts > targetTime) yield return null;
				}
			} while (Time.realtimeSinceStartup - ts <= targetTime);
			yield return null;
		}
	}
}
