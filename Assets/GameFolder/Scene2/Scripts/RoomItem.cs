using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private LobbyManager lobbyManager;


    void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();     
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void OnClickItem()
    {
        lobbyManager.JoinRoom(roomName.text);
    }

}
