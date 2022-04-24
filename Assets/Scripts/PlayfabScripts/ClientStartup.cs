using PlayFab;
using PlayFab.ClientModels;
using Mirror;
using UnityEngine;
using PlayFab.MultiplayerModels;
using System.Collections.Generic;
using PlayFab.Networking;
using System;

public class ClientStartup : MonoBehaviour
{
    public void StartPlayFabClient()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = SystemInfo.deviceUniqueIdentifier
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabLoginSuccess, OnPlayFabError);
    }
    private void OnPlayFabLoginSuccess(LoginResult loginresult) {

        Debug.Log("Login Success!");

        RequestMultiplayerServer();
    }

    private void OnPlayFabError(PlayFabError playfaberror) {
        Debug.Log("Login Error!");
    }

    public void RequestMultiplayerServer()
    {
        Debug.Log("-Client Startup-RequestMultiplayerServer");

        RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest
        {
            BuildId = "3b65856d-d6c4-4ebf-9f7c-b54455d89b93",
            SessionId = "1a1388bc-4245-4b2c-87bd-746fafdb603d",
            PreferredRegions = new List<string> { "EastUS" }
        };

        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
    }

    private void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
    {
        if (response == null) return;
        Debug.Log("-Details- IP:" + response.IPV4Address + " PORT: " + (ushort)response.Ports[0].Num);
        UnityNetworkServer.Instance.networkAddress = response.IPV4Address;
        UnityNetworkServer.Instance.GetComponent<kcp2k.KcpTransport>().Port = (ushort)response.Ports[0].Num;
        UnityNetworkServer.Instance.StartClient();

    }

    private void OnRequestMultiplayerServerError(PlayFabError error)
    {
        Debug.Log("An error occurred while creating your multiplayer Server: "+ error);
    }

}