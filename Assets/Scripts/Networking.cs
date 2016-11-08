using UnityEngine;
using System.Collections;
using server;
using System.Net;

public class Networking : MonoBehaviour
{
	public string Address = "0.0.0.0";

	public int Port = 4572;

	public RubiksCube Cube;

	private UdpServer server;

	void Start ()
	{
		this.server = new UdpServer(new CommandsHandler(this.Cube));
		this.server.Start(new IPEndPoint(IPAddress.Parse(this.Address), this.Port));
	}

	void OnGUI()
	{
		GUILayout.Label("Bind to: " + this.Address + ":" + this.Port);
	}
}
