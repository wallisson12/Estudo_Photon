using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameConnection : MonoBehaviourPunCallbacks
{

    [SerializeField] private TextMeshProUGUI _text; 

    void Awake()
    {
        //Nickname
        PhotonNetwork.LocalPlayer.NickName = "Name_" + Random.Range(0,100);

        //Connect to the server
        PhotonNetwork.ConnectUsingSettings(); 
    }


    //Runs when the connection is correct
    public override void OnConnectedToMaster()
    {
        _text.text += "\nConectado ao servidor";

        //Se eu nao estiver em um lobby
        if (PhotonNetwork.InLobby == false)
        {
            _text.text += "\nEntrando em uma lobby";

            //Join into the lobby
            PhotonNetwork.JoinLobby();
        }
    }

    //Runs if the player be in lobby 
    public override void OnJoinedLobby()
    {
        _text.text += "\nEntrou no lobby";
        PhotonNetwork.JoinRoom("TesteP1");
    }

    //Runs when the player try enter in falied room
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _text.text += "\nErro ao entrar na sala: " + message + " | " + returnCode;

        if (returnCode == ErrorCode.GameDoesNotExist)
        {
            _text.text += "\nCriando uma sala";
            RoomOptions roomOp = new RoomOptions { MaxPlayers = 20 };
            PhotonNetwork.CreateRoom("TesteP1", roomOp, null);
        }
    }

    //Runs when the player enters the room
    public override void OnJoinedRoom()
    {
        _text.text += "\nEntrou em uma sala TesteP1! Seu nick eh: " + PhotonNetwork.LocalPlayer.NickName;

        //Instance the player
        Vector2 pos = new Vector2(Random.Range(-30f,30f),Random.Range(-6f,6f));
        PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _text.text += $"\nO player {newPlayer.NickName}, entrou na sala TesteP1!";
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _text.text += $"\nO player {otherPlayer.NickName}, saiu da sala TesteP1!";
    }

    public override void OnLeftRoom()
    {
        _text.text += "\nVoce saiu da sala TesteP1";
    }

    //Switch host 
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        
    }


}
