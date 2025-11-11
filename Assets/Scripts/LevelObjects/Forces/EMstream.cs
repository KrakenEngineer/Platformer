using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class EMstream : MonoBehaviour
{
	[SerializeField] private float _velocity;
	[SerializeField] private float _horizontalForce;
	[SerializeField] private float _verticalForce;

	private BoxCollider2D _trigger;
	private LineEquation _equation;
	private List<Magnet> _objects = new List<Magnet>();

	private void Awake()
	{
		_trigger = GetComponent<BoxCollider2D>();
		_trigger.isTrigger = true;
		_trigger.autoTiling = true;
		_equation = new LineEquation(transform);
		//Debug.Log($"{gameObject.name} {_equation}");
	}

	private void FixedUpdate()
	{
		foreach (var obj in _objects)
		{
			Transform t = obj.transform;
			var r = obj.GetComponent<Rigidbody2D>();
			float forcex = (-Physics2D.gravity.x + _horizontalForce * SignedSquare(_velocity * _equation.b - r.velocityX)) * r.mass;
			float forcey = (-Physics2D.gravity.y + _verticalForce * SignedSquare(_velocity * _equation.a - r.velocityY)) * r.mass;
			//Debug.Log($"{forcex} {forcey}");
			r.AddForce(new Vector2(forcex, forcey));
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Magnet m) && collision.TryGetComponent(out Rigidbody2D r))
			_objects.Add(m);
		else return;
		if (m is Player p)
			p.EnterEMStream(this);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.TryGetComponent(out Magnet m))
			_objects.Remove(m);
		else return;
		if (m is Player p)
			p.ExitEMStream(this);
	}

	private static float SignedSquare(float f) => Mathf.Abs(f) * f;
}

