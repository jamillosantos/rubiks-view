
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text;
using UnityEngine;

namespace server
{
	public class CommandsHandler
		: IServerHandler
	{

		public UdpServer Server
		{
			get
			{
				return this._server;
			}

			set
			{
				this._server = value;
			}
		}

		private UdpServer _server;

		public RubiksCube cube;

		public CommandsHandler(RubiksCube cube)
		{
			this.cube = cube;
		}

		public void Received(byte[] data, EndPoint endpoint)
		{
			string json = Encoding.UTF8.GetString(data);
			Debug.Log("RECEIVED: " + json);
			try
			{
				JObject obj = JObject.Parse(json);
				this.command(obj);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		private void command(JObject obj)
		{
			string commandName = (string)obj["name"];
			if (commandName == "multi")
			{
				JArray commands = (JArray)obj["params"];
				foreach (var o in commands)
				{
					this.command((JObject)o);
				}
			}
			else if (commandName == "rotate")
			{
				this.rotate((JObject)obj["params"]);
			}
			else if (commandName == "reset")
			{
				this.reset();
			}
		}

		private void rotate(JObject obj)
		{
			Rotation rotation = new Rotation()
			{
				Times = 1,
				Duration = 0.5f
			};
			foreach (var p in obj)
			{
				if (p.Key == "face")
				{
					switch (p.Value.ToString())
					{
						case "front":
							rotation.Face = this.cube.Front;
							break;
						case "back":
							rotation.Face = this.cube.Back;
							break;
						case "right":
							rotation.Face = this.cube.Right;
							break;
						case "left":
							rotation.Face = this.cube.Left;
							break;
						case "up":
							rotation.Face = this.cube.Up;
							break;
						case "down":
							rotation.Face = this.cube.Down;
							break;
						default:
							throw new Exeption("Cannot find face: " + p.Value.ToString());
					}
				}
				else if (p.Key == "times")
				{
					rotation.Times = (int)p.Value;
				}
				else if (p.Key == "duration")
				{
					rotation.Duration = (float)p.Value;
				}
			}
			this.cube.Rotate(rotation);
		}

		private void reset()
		{
			this.cube.Reset();
		}
	}
}
