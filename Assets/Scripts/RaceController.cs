using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class RaceController : MonoBehaviourPunCallbacks
{
    public static bool racing = false;
    public static int totalLaps = 1;
    public int timer = 3;
    public CheckPointController[] carsController;
    public GameObject endPanel;
    public TMP_Text startText;
    AudioSource audioSource;
    public AudioClip startSound;
    public AudioClip countdownSound;

    public GameObject carPrefab;
    public Transform[] spawnPos;
    public int playerCount;

    public GameObject startRace;
    public GameObject waitText;

    [PunRPC]
    public void StartGame()
    {
        InvokeRepeating("CountDown", 3, 1);
        startRace.SetActive(false);
        waitText.SetActive(false);
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        carsController = new CheckPointController[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            carsController[i] = cars[i].GetComponent<CheckPointController>();
        }
    }
    public void BeginGame()

    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGame", RpcTarget.All, null);
        }
    }
    private void CountDown()
    {
        startText.gameObject.SetActive(true);
        if(timer != 0)
        {
            startText.text = timer.ToString();
            audioSource.PlayOneShot(countdownSound);
            //print("Start wyœcigu za: " + timer);
            timer--;
        } else
        {
            startText.text = "START!!!";
            audioSource.PlayOneShot(startSound);
            //print("START!!!");
            racing = true;
            CancelInvoke("CountDown");
            Invoke("HideStartText", 1);
        }
    }

    void HideStartText()
    {
        startText.gameObject.SetActive(false);
    }

    void Start()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        endPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        startText.gameObject.SetActive(false);
        startRace.SetActive(false);
        waitText.SetActive(false);
        int randomStartPosition = Random.Range(0, spawnPos.Length);
        Vector3 startPos = spawnPos[randomStartPosition].position;
        Quaternion startRot = spawnPos[randomStartPosition].rotation;
        GameObject playerCar = null;
        if (PhotonNetwork.IsConnected)
        {
            startPos = spawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].position;
            startRot = spawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation;
            if (OnlinePlayer.LocalPlayerInstance == null)
            {
                playerCar = PhotonNetwork.Instantiate(carPrefab.name, startPos, startRot,

                0);

            }
            if (PhotonNetwork.IsMasterClient)
            {
                startRace.SetActive(true);
            }
            else
            {
                waitText.SetActive(true);
            }
        }
        playerCar.GetComponent<DrivingScript>().enabled = true;
        playerCar.GetComponent<PlayerController>().enabled = true;

    }

    private void LateUpdate()
    {
        int finishedLap = 0;

        foreach(CheckPointController controller in carsController)
        {
            if (controller.lap == totalLaps + 1) finishedLap++;

            if(finishedLap == carsController.Length && racing)
            {
                print("===== RACE FINISHED!!! =====");
                endPanel.SetActive(true);
                racing = false;
            }

        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
