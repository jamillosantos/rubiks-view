using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Message : MonoBehaviour {

	private static Message _instance;
	private Text _text;

	public static Message Instance
	{
		get
		{
			return _instance;
		}
	}

	void Start ()
	{
		_instance = this;
		this._text = this.GetComponent<Text>();
	}

	void Update()
	{
		this._text.text = this.Text;
	}

	public string Text;
	

}
