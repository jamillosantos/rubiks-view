using UnityEngine;
using System.Collections;
using System;

public class RubiksFace
{
	private RubiksCube cube;

	public Vector3 Up;

	public RubiksFace(RubiksCube cube, Vector3 up)
	{
		this.cube = cube;
		this.Up = up;
	}

	public bool IsOn(RubiksPiece piece)
	{
		return
			   ((this.Up.x != 0) && (Mathf.Abs(this.Up.x - piece.transform.position.x) < 0.1))
			|| ((this.Up.y != 0) && (Mathf.Abs(this.Up.y - piece.transform.position.y) < 0.1))
			|| ((this.Up.z != 0) && (Mathf.Abs(this.Up.z - piece.transform.position.z) < 0.1));
	}

	public void BeginRotation()
	{
		foreach (RubiksPiece p in cube.pieces)
		{
			if (this.IsOn(p))
				p.BeginRotation();
		}
	}

	protected void rotate(float multiplier)
	{
		float a = (90) * multiplier;
		foreach (RubiksPiece p in cube.pieces)
		{
			if (this.IsOn(p))
			{
				p.RotateAround(Vector3.zero, this.Up, a);
			}
		}
	}

	public void RotateCW(float time)
	{
		this.rotate(1 * time);
	}

	public void RotateCCW(float time)
	{
		this.rotate(-1 * time);
	}

	public void SetColour(Color colour)
	{
		foreach (RubiksPiece p in cube.pieces)
		{
			if (this.IsOn(p))
			{
				p.SetColour(this.Up, colour);
			}
		}
	}
}