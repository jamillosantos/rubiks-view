using UnityEngine;
using System.Collections;
using System;

public class RubiksPiece : MonoBehaviour
{
	private RubiksPieceFace[] faces;

	private Quaternion storedRotation;

	private Vector3 storedPosition;

	void Start()
	{
		this.faces = this.GetComponentsInChildren<RubiksPieceFace>();
	}

	void OnDrawGizmos()
	{
		Vector3 v;
		if (this.transform.position.x != 0)
		{
			v = this.transform.position;
			v.x = 0;
			Gizmos.DrawLine(this.transform.position, this.transform.position + (this.transform.position - v) * 2f);
		}
		if (this.transform.position.y != 0)
		{
			v = this.transform.position;
			v.y = 0;
			Gizmos.DrawLine(this.transform.position, this.transform.position + (this.transform.position - v) * 2f);
		}
		if (this.transform.position.z != 0)
		{
			v = this.transform.position;
			v.z = 0;
			Gizmos.DrawLine(this.transform.position, this.transform.position + (this.transform.position - v) * 2f);
		}
	}

	internal void BeginRotation()
	{
		this.storedRotation = this.transform.localRotation;
		this.storedPosition = this.transform.localPosition;
	}
	public void RotateAround(Vector3 zero, Vector3 up, float angle)
	{
		this.transform.localRotation = this.storedRotation;
		this.transform.localPosition = this.storedPosition;
		this.transform.RotateAround(Vector3.zero, up, angle);
	}


	public void SetColour(Vector3 up, Color colour)
	{
		up *= 0.12f;
		foreach (RubiksPieceFace f in this.faces)
		{
			if (
				   ((up.x != 0) && (Mathf.Abs(up.x - f.transform.localPosition.x) < 0.01))
				|| ((up.y != 0) && (Mathf.Abs(up.y - f.transform.localPosition.y) < 0.01))
				|| ((up.z != 0) && (Mathf.Abs(up.z - f.transform.localPosition.z) < 0.01))
			)
			{
				f.GetComponent<MeshRenderer>().material.color = colour;
			}
		}
	}
}