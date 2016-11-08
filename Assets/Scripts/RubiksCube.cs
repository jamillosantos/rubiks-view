using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Rotation
{
	public RubiksFace Face;

	public int Times;

	public float Duration;

	public float StartedAt;

	public void Begin()
	{
		this.StartedAt = Time.timeSinceLevelLoad;
		this.Face.BeginRotation();
	}
}

public class RubiksCube : MonoBehaviour
{

	public RubiksPiece[] pieces;

	public GameObject PieceModel;

	public RubiksFace Front;

	public RubiksFace Back;

	public RubiksFace Right;

	public RubiksFace Left;

	public RubiksFace Up;

	public RubiksFace Down;

	private Queue<Rotation> Rotations;

	public RubiksCube()
		: base()
	{
		this.Rotations = new Queue<Rotation>();
	}

	void Start()
	{
		this.pieces = new RubiksPiece[27];
		for (int i = 0; i < this.pieces.Length; ++i)
		{
			GameObject piece = GameObject.Instantiate(this.PieceModel);
			this.pieces[i] = piece.GetComponent<RubiksPiece>();
			piece.name = "Piece" + i;
			Vector3 position = new Vector3((i % 3) - 1, ((i / 3) % 3) - 1, ((i / 9) % 3) - 1);
			piece.transform.position = position;
			piece.transform.SetParent(this.transform, false);
		}

		this.Front = new RubiksFace(this, Vector3.forward);
		this.Back = new RubiksFace(this, Vector3.back);
		this.Right = new RubiksFace(this, Vector3.right);
		this.Left = new RubiksFace(this, Vector3.left);
		this.Up = new RubiksFace(this, Vector3.up);
		this.Down = new RubiksFace(this, Vector3.down);

		this.Invoke("Reset", 0.1f);
	}

	private Rotation runningRotation = null;

	void Update()
	{
		RubiksFace face = null;
		if (Input.GetKeyDown(KeyCode.F))
		{
			face = this.Front;
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			face = this.Back;
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			face = this.Right;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			face = this.Left;
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			face = this.Up;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			face = this.Down;
		}
		if (face != null)
		{
			this.Rotations.Enqueue(new Rotation()
			{
				Duration = 0.5f,
				Face = face,
				Times = 1 * (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? -1 : 1)
			});
		}

		if ((this.runningRotation == null) && (this.Rotations.Count > 0))
		{
			this.runningRotation = this.Rotations.Dequeue();
			this.runningRotation.Begin();
		}

		if (this.runningRotation != null)
		{
			float time = Mathf.Min(1f, (Time.timeSinceLevelLoad - this.runningRotation.StartedAt) / this.runningRotation.Duration);
			this.runningRotation.Face.RotateCW(time * this.runningRotation.Times);
			if (time == 1f)
				this.runningRotation = null;
		}
	}

	public void Rotate(Rotation rotation)
	{
		this.Rotations.Enqueue(rotation);
	}

	public void Reset()
	{
		this.Up.SetColour(Color.gray);
		this.Down.SetColour(Color.yellow);
		this.Right.SetColour(Color.red);
		this.Left.SetColour(new Color(1f, 0.55f, 0));
		this.Front.SetColour(Color.green);
		this.Back.SetColour(Color.blue);
	}
}
