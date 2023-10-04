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


    [SerializeField] private RoomItem roomItemPrefab;
    [SerializeField] private List<RoomItem> roomItensList = new List<RoomItem>();
    [SerializeField] private Transform contentObj;

    [SerializeField] private float timeBetweenUpdates = 1.5f;
    [SerializeField] private float nextUpdateTime;
  
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
}
