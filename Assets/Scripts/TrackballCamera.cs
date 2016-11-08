using UnityEngine;
using System.Collections;

public class TrackballCamera : MonoBehaviour
{

	public float Distance = 15f;

	public float VirtualTrackballDistance = 0.25f; // distance of the virtual trackball.

	public Transform Target;

	private Vector3? lastMousePosition;

	void Start()
	{
		var startPosn = (this.transform.position - this.Target.transform.position).normalized * this.Distance;
		var position = startPosn + this.Target.transform.position;
		this.transform.position = position;
		this.transform.LookAt(this.Target.transform.position);
	}

	// Update is called once per frame
	void LateUpdate()
	{
		var mousePosn = Input.mousePosition;

		var mouseBtn = Input.GetMouseButton(0);
		if (mouseBtn)
		{
			if (lastMousePosition.HasValue)
			{
				// we are moving from here
				var lastPosn = this.transform.position;
				var targetPosn = this.Target.transform.position;

				// we have traced out this distance on a sphere from lastPosn
				/*
				var rotation = TrackBall(
										lastMousePosition.Value.x, 
										lastMousePosition.Value.y,
										mousePosn.x,
										mousePosn.y );
				*/
				var rotation = FigureOutAxisAngleRotation(lastMousePosition.Value, mousePosn);

				var vecPos = (targetPosn - lastPosn).normalized * -this.Distance;

				this.transform.position = rotation * vecPos + targetPosn;
				this.transform.LookAt(targetPosn);

				this.lastMousePosition = mousePosn;
			}
			else {
				this.lastMousePosition = mousePosn;
			}
		}
		else {
			this.lastMousePosition = null;
		}

	}

	Quaternion FigureOutAxisAngleRotation(Vector3 lastMousePosn, Vector3 mousePosn)
	{
		if (lastMousePosn.x == mousePosn.x && lastMousePosn.y == mousePosn.y)
			return Quaternion.identity;

		Vector3 near = new Vector3(0, 0, Camera.main.nearClipPlane);

		Vector3 p1 = Camera.main.ScreenToWorldPoint(lastMousePosn + near);
		Vector3 p2 = Camera.main.ScreenToWorldPoint(mousePosn + near);

		//WriteLine("## {0} {1}", p1,p2);
		var axisOfRotation = Vector3.Cross(p2, p1);

		var twist = (p2 - p1).magnitude / (2.0f * VirtualTrackballDistance);

		if (twist > 1.0f)
			twist = 1.0f;
		if (twist < -1.0f)
			twist = -1.0f;

		var phi = (2.0f * Mathf.Asin(twist)) * 180 / Mathf.PI;

		//WriteLine("AA: {0} angle: {1}",axisOfRotation, phi);

		return Quaternion.AngleAxis(phi, axisOfRotation);
	}

	Quaternion TrackBall(float p1x, float p1y, float p2x, float p2y, float radius)
	{
		// if there has been no drag, then return "no rotation"
		if (p1x == p2x && p1y == p2y)
		{
			return Quaternion.identity;
		}
		var p1 = ProjectToSphere(radius, p1x, p1y);
		var p2 = ProjectToSphere(radius, p2x, p2y);

		var a = Vector3.Cross(p2, p1); // axis of rotation
									   // how much to rotate around above axis
		var d = p1 - p2;
		var t = d.magnitude / (2.0f * radius);
		// clamp values to stop things going out of control.
		if (t > 1.0f) t = 1.0f;
		if (t < -1.0f) t = -1.0f;
		var phi = 2.0f * Mathf.Asin(t);
		phi = phi * 180 / Mathf.PI; // to degrees

		return Quaternion.AngleAxis(phi, a);
	}

	/// <summary>
	/// Projects an x,y pair onto a sphere of radius distance.
	/// OR onto a hyperbolic sheet if we are away from the center 
	/// of the sphere
	/// </summary>
	/// <returns>
	/// The point on the sphere.
	/// </returns>
	/// <param name='distance'>
	/// Distance.
	/// </param>
	/// <param name='x'>
	/// X.
	/// </param>
	/// <param name='y'>
	/// Y.
	/// </param>
	Vector3 ProjectToSphere(float distance, float x, float y)
	{
		float z;
		float d = Mathf.Sqrt(x * x + y * y);
		if (d < distance * 0.707f)
		{
			// inside sphere
			z = Mathf.Sqrt(distance * distance - d * d);
		}
		else
		{
			// on hyperbola
			var t = distance / 1.4142f;
			z = t * t / d;
		}

		return new Vector3(x, y, z);
	}
}