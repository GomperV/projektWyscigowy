using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
public class RaceLauncher : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerName;
    byte maxPlayersPerRoom = 3;
    bool isConnecting;
    public TMP_Text networkText;
    string gameVersion = "2";
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public void StartTrial()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnConnectedToMaster()
    {
        if(isConnecting)
        {
            networkText.text += "OnConnectToMaster...\n";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        networkText.text += "Failed to join the room. \n";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
    }

    public override void OnDisconnected(DisconnectCause disconnectCause)
    {
        networkText.text += "Disconnected because " + disconnectCause + "/n";
        isConnecting = false;
    }

    public override void OnJoinedRoom()
    {
        networkText.text = "Joined room with " + PhotonNetwork.CurrentRoom.PlayerCount + " players in total.";
        PhotonNetwork.LoadLevel("TestTrack");
    }

    public void ConnectNetwork()
    {
        networkText.text = "";
        isConnecting = true;
        PhotonNetwork.NickName = playerName.text;
        if(PhotonNetwork.IsConnected)
        {
            networkText.text = "Joining room...";
            PhotonNetwork.JoinRandomRoom();
        } else
        {
            networkText.text = "Connecting...";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

    }
}

