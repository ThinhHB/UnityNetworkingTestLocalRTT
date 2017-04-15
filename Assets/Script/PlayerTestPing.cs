using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[NetworkSettings(channel = 1)]
public class PlayerTestPing : NetworkBehaviour
{
	#region Config
	[SerializeField] KeyCode testCmdRpcKey = KeyCode.A;
	[SerializeField] KeyCode testMessageKey = KeyCode.S;
	#endregion//config


	#region Network callbacks
	public override void OnStartLocalPlayer ()
	{
		// this enable the Update() function
		enabled = true;
		// reigster to receive event
		NetworkManager.singleton.client.RegisterHandler(PING_MESSAGE, OnLocalReceiveCheckPingMessage);
	}

	public override void OnStartClient ()
	{
		enabled = false;
	}

	public override void OnStartServer ()
	{
		enabled = false;
		NetworkServer.RegisterHandler(PING_MESSAGE, OnServerReceiveCheckPingMessage);
	}
	#endregion // Network callbacks


	#region Local
	void LocalCheckPingViaCmdRpc()
	{
		_sentTime = Time.time;
		CmdServerReceiveCheckingPingRequest();
	}

	void LocalCheckPingViaMessage()
	{
		_sentTime = Time.time;
		var client = NetworkManager.singleton.client;
		var message = new PingMessage();
		client.Send(PING_MESSAGE, message);
	}

	void OnLocalReceiveCheckPingMessage(NetworkMessage message)
	{
		var RTT = (Time.time - _sentTime) * 1000f;
		Debug.LogFormat("NetworkMessag -> RTT : {0}", RTT);
	}
	#endregion


	#region Server
	void OnServerReceiveCheckPingMessage(NetworkMessage message)
	{
		var connectionID = message.conn.connectionId;
		var reply = new PingMessage();
		NetworkServer.SendToClient(connectionID, PING_MESSAGE, reply);
	}
	#endregion


	#region Private
	void Update()
	{
		if (Input.GetKeyDown(testCmdRpcKey))
		{
			LocalCheckPingViaCmdRpc();
		}

		if (Input.GetKeyDown(testMessageKey))
		{
			LocalCheckPingViaMessage();
		}
	}
	#endregion//Private


	#region CheckPing via Cmd RPC
	float _sentTime;
	[Command]
	void CmdServerReceiveCheckingPingRequest()
	{
		RpcClientReceiveCheckingPingMessageFromServer();
	}

	[ClientRpc]
	void RpcClientReceiveCheckingPingMessageFromServer()
	{
		if (isLocalPlayer)
		{
			var RTT = (Time.time - _sentTime) * 1000f;
			Debug.LogFormat("Cmd, Rpc -> RTT : {0}", RTT);
		}
	}
	#endregion//CheckPing via Cmd RPC


	#region Custom classes
	const short PING_MESSAGE = 1000;

	[System.Serializable]
	public class PingMessage : MessageBase
	{
	}
	#endregion//Custom classes
}

