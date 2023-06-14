using UnityEngine;
using System.Collections;

public class UpdatableData : ScriptableObject
{
	public event System.Action OnValuesUpdated;
	public bool auto_update;

	#if UNITY_EDITOR

	protected virtual void OnValidate()
	{
		if (auto_update)
		{
			UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
		}
	}

	public void NotifyOfUpdatedValues()
	{
		UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
		if (OnValuesUpdated != null)
		{
			OnValuesUpdated();
		}
	}

	#endif

}