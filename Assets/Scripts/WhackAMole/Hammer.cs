using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class Hammer : MonoBehaviourPun
{
    [Header("Collider")]
    public HandColliderWhack rightHandCollider;
    public HandColliderWhack leftHandCollider;

 
    private GameObject rightHitObject;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()

    {

        if (rightHandCollider.isColliding && photonView.IsMine)
        {
            Debug.Log("Hit");
            rightHitObject = rightHandCollider.collidingObject;

            if (PhotonNetwork.IsMasterClient) { 
            rightHitObject.GetComponent<Mole>().goalDoneByPlayerID = 1;
            }
            else
            {
                rightHitObject.GetComponent<Mole>().goalDoneByPlayerID = 2;
            }

            rightHitObject.GetComponent<Mole>().isHit = true;
        }

            
        }
    }



