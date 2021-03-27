using UnityEngine;

[CreateAssetMenu(menuName = "ObjectColorIndex")]
public class ObjectColorIndex : ScriptableObject
{
	public static int GetCurrentObjectIndex(GameObject objectForIndex)
	{
		return (int)char.GetNumericValue(objectForIndex.name[objectForIndex.name.Length - 8]);
	}
}
