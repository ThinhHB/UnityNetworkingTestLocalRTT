using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Use in menu scene, have some public functions. use them as target for
/// 2 button on screen : StartLocalServer and ConnectLocalServer.
/// </summary>
public class UIMenu : MonoBehaviour
{
	#region Init, config
	#endregion//Init, config

	#region Public, Inspector
	public void IN_PressStartLocalServer()
	{
		var netManager = NetworkManager.singleton;
		netManager.networkAddress = "localhost";
		netManager.networkPort = 7777;
		netManager.StartServer();
	}

	public void IN_PressConnectLocal()
	{
		var netManager = NetworkManager.singleton;
		netManager.networkAddress = "localhost";
		netManager.networkPort = 7777;
		netManager.StartClient();
	}
	#endregion//Public, Inspector
}
