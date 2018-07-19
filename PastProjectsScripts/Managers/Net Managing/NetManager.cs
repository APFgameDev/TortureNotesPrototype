using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Managers.NS_GameManagement;

public class NetManager : SingletonPunBehaviour<NetManager> 
{
    [SerializeField]
    string m_VersionID;
    [SerializeField]
    float reconnectWait = 5;
    [SerializeField]
    UnityEngine.UI.Text m_text;
    //[SerializeField]
    ////UnityEngine.UI.

	void Awake ()
    {
        InitSingleton(this);
        StartCoroutine(StayConnectedCoroutine());
    }

    IEnumerator StayConnectedCoroutine()
    {
        while(PhotonNetwork.connected == false)
        {
            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.ConnectUsingSettings(m_VersionID);
            yield return new WaitForSeconds(reconnectWait);
        }
    }

    public override void OnConnectedToPhoton()
    {
        m_text.text = "Online";

        base.OnConnectedToPhoton();
    }

    public override void OnDisconnectedFromPhoton()
    {
        m_text.text = "Offline";

        StartCoroutine(StayConnectedCoroutine());
        base.OnDisconnectedFromPhoton();
    }
}
