using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviourPunCallbacks
{
    #region Member Variables
    public float speed;
    public bool isHit=false;
    public Material Red;
    public Material Blue;

    private bool isIncremented=false;

    public Score ScoreOwner;

    public int goalDoneByPlayerID;

    public bool isMovedUp=false;

    public float duration = 0.5f;
    private Vector3 upPosition;
    private Vector3 downPosition;
    #endregion

    #region MonoBehaviour Callbacks
    // Start is called before the first frame update
    void Start()
    {
        upPosition = new Vector3(transform.position.x, 0.8f, transform.position.z);
        downPosition = new Vector3(transform.position.x, 0, transform.position.z);

        transform.position = downPosition;

    }

    // Update is called once per frame
    void Update()
    {

        if (!isHit) {
            if (photonView.IsMine) { 
            photonView.RPC("InformOnHit", RpcTarget.Others, false);
            }
            transform.GetComponent<MeshRenderer>().material = Blue;
            isIncremented = false;


        }
        else if(isHit && !isIncremented)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("InformOnHit", RpcTarget.Others, true);
            }
            StartCoroutine(HitBehavior());
            

                //TODO check w other method than 1 and 2
                if (goalDoneByPlayerID == 1)
                {
                    ScoreOwner.IncrementPlayer1Score();
                    photonView.RPC("InformOthersOnScore", RpcTarget.Others, ScoreOwner.player1Score, ScoreOwner.player2Score);

                  
                }
               

                //TODO we are never increasing player 1 score
                
                else if (goalDoneByPlayerID == 2)
                {
                    ScoreOwner.IncrementPlayer2Score();
                    photonView.RPC("InformOthersOnScore", RpcTarget.Others, ScoreOwner.player1Score, ScoreOwner.player2Score);
                }

                isIncremented = true;
            
        }

    }
    #endregion

    public IEnumerator HitBehavior()
    {
        transform.GetComponent<MeshRenderer>().material = Red;
        transform.position = new Vector3(transform.position.x, 0.8f, transform.position.z);
        yield return new WaitForSeconds(2);
        isHit = false;
    }

    
    public IEnumerator MoleMoveUp()
    {
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, upPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = upPosition;

    }

    public IEnumerator MoleMoveDown()
    {
        yield return new WaitForSeconds(3);
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, downPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = downPosition;
        

    }

    //TODO method is not founnd on other player
    [PunRPC]
    public void InformOnHit(bool isHit)
    {
        this.isHit = isHit;
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (photonView.IsMine && stream.IsWriting)
    //    {
    //stream.SendNext(isHit);
    //stream.SendNext(isIncremented);
    //stream.SendNext(player1);
    //}
    //else if (stream.IsReading)
    //{
    //isHit = (bool)stream.ReceiveNext();
    //isIncremented = (bool)stream.ReceiveNext();
    //player1 = (bool)stream.ReceiveNext();
    //    }
    //}



}
