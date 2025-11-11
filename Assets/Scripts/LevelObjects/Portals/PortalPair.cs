using System.Collections.Generic;
using UnityEngine;

public class PortalPair : MonoBehaviour
{
	private Portal _first;
	private Portal _second;
	[SerializeField] private Color _color;

	private List<Transform> _inPortal1 = new List<Transform>();
	private List<Transform> _inPortal2 = new List<Transform>();

	private void Awake()
	{
		_first = transform.GetChild(0).GetComponent<Portal>();
		_second = transform.GetChild(1).GetComponent<Portal>();
		_first.Initialize(this, _color);
		_second.Initialize(this, _color);
	}

	public void Teleport(Portal start, Transform obj)
	{
		if (ObjectsIn(start).Contains(obj))
			return;
		ObjectsIn(Other(start)).Add(obj);
		obj.position = Other(start).transform.position;
	}

	public void Remove(Portal portal, Transform t)
	{
		ObjectsIn(portal).Remove(t);
	}

	private Portal Other(Portal p) => p == _first ? _second : _first;
	private List<Transform> ObjectsIn(Portal p) => p == _first ? _inPortal1 : (p == _second ? _inPortal2 : null);
}
