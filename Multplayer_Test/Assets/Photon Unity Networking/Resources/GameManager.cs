using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public Text PingText;
    public GameObject disconnectUI;
    private bool Off = false;

    public GameObject PlayerFeed;
    public GameObject FeedGrid;

    private void Awake()
    {
        GameCanvas.SetActive(true);
    }
    private void Update()
    {
        CheckInput();
        PingText.text = "Ping : " + PhotonNetwork.GetPing();
    }
    private void CheckInput()
    {
        if(Off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUI.SetActive(false);
            Off = false;
        }
        else if(!Off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUI.SetActive(true);
            Off = true;
        }
    }
    public void SpawnPlayer()
    {
        float randomvalue = Random.Range(-1f, 1f);

        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(this.transform.position.x * randomvalue, this.transform.position.y), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(true);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }
    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject Obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        Obj.transform.SetParent(FeedGrid.transform, false);
        Obj.GetComponent<Text>().text = player.name + " Joined The Game.";
        Obj.GetComponent<Text>().color = Color.green;
    }
    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject Obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        Obj.transform.SetParent(FeedGrid.transform, false);
        Obj.GetComponent<Text>().text = player.name + " Left The Game.";
        Obj.GetComponent<Text>().color = Color.red;
    }
}
