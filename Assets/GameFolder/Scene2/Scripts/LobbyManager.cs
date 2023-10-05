using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private GameObject lobbyPainel,roomPainel;
    [SerializeField] private TextMeshProUGUI roomName;

    [Header("Room List")]
    [SerializeField] private RoomItem roomItemPrefab;
    [SerializeField] private List<RoomItem> roomItensList = new List<RoomItem>();
    [SerializeField] private Transform contentObj;

    [SerializeField] private float timeBetweenUpdates = 1.5f;
    [SerializeField] private float nextUpdateTime;

    [Header("Player List")]
    [SerializeField] private List<PlayerItem> playerItemsList = new List<PlayerItem>();
    [SerializeField] private PlayerItem playerItemPrefab;
    [SerializeField] private Transform contentObjPI;
  
    void Start()
    {
        //Enter the lobby
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreateRoom()
    {
        if (roomNameInput.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomNameInput.text);
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPainel.SetActive(false);
        roomPainel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;

        UpdatePlayerList();
    }

    //Called for any change room List (Create,modify,Deleted)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        //Remove old
        foreach (RoomItem item in roomItensList)
        {
            Destroy(item.gameObject);
        }

        roomItensList.Clear();


        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObj);
            newRoom.SetRoomName(room.Name);
            roomItensList.Add(newRoom);
        }
    }

    public void JoinRoom(string room)
    {
        PhotonNetwork.JoinRoom(room);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        lobbyPainel.SetActive(true);
        roomPainel.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }


    void UpdatePlayerList()
    {
        foreach (PlayerItem playerItem in playerItemsList)
        {
            Destroy(playerItem.gameObject);
        }

        playerItemsList.Clear();


        if (PhotonNetwork.CurrentRoom != null)
        {
            foreach (KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
            {

                PlayerItem newPlayerItem = Instantiate(playerItemPrefab, contentObjPI);
                newPlayerItem.SetAttributesPlayer(player.Value);

                if (player.Value == PhotonNetwork.LocalPlayer)
                {
                    newPlayerItem.ApplyLocalChanges();
                }

                playerItemsList.Add(newPlayerItem);

            }
        }
        else
        {
            return;
        }
    }
}
