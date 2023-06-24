using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MoleControl : MonoBehaviour
{
    public Mole[] moles;
    private int randomMoleIndex;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateRandomMoleIndex());
    }

    // Update is called once per frame
    void Update()
    {

        

     
        if (!moles[randomMoleIndex].isHit && PhotonNetwork.IsMasterClient) {

            StartCoroutine(moles[randomMoleIndex].MoleMoveUp());
            StartCoroutine(moles[randomMoleIndex].MoleMoveDown());

        }
       

    

    }

    IEnumerator GenerateRandomMoleIndex()
    {
        while (true)
        {
            randomMoleIndex = Random.Range(0, 15);
            yield return new WaitForSeconds(1);
        }
        
    }



    }
