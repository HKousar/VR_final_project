using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviourPunCallbacks
{
    #region Member Variables
    public int player1Score = 0;
    public int player2Score = 0;

    public TextMeshPro ScoreDisplay1;
    public TextMeshPro ScoreDisplay2;
    public TextMeshPro Timer;

    public float targetTime;
    private float startTime = 0;
    #endregion

    #region MonoBehaviour Callbacks
    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        Timer.text = ((int)startTime).ToString();
        //TODO put name of player
        if (PhotonNetwork.PlayerListOthers.Length > 0) {
            ScoreDisplay1.text =  "1:   " + player1Score.ToString();
            ScoreDisplay2.text =  "2:   "+ player2Score.ToString();
        }
        else
        {
            ScoreDisplay1.text = "1:   " + player1Score.ToString(); 
            
        }

        if (startTime < targetTime) {
            startTime += Time.deltaTime;
        }

        if (startTime >= targetTime)
        {
            timerEnded();
        }
    }
    #endregion

    #region Custom Methods


    void timerEnded()
    {
        //do your stuff here.
    }

    public void IncrementPlayer1Score()
    {
        player1Score = player1Score + 5;
    }


    public void IncrementPlayer2Score()
    {
        player2Score = player2Score + 5;
    }
    #endregion



    #region PUN Callbacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
{
    photonView.RPC("InformOthersOnScoreInit", RpcTarget.Others, player1Score, player2Score, startTime);
}
    #endregion


    #region RPCs

    [PunRPC]
    public void InformOthersOnScoreInit(int player1Score, int player2Score, float startTime)
    {
        this.player1Score = player1Score;
        this.player2Score = player2Score;
        this.startTime = startTime;
    }

    [PunRPC]
    public void InformOthersOnScore(int player1Score, int player2Score)
    {
            this.player1Score = player1Score;
            this.player2Score = player2Score;
    }

    #endregion
}
