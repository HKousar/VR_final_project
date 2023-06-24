using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DistributedGrabbable : MonoBehaviourPun
{
    #region Member Variables

    private bool isGrabbed = false;

    #endregion

    #region Custom Methods

    public bool RequestGrab()
    {
        if (!isGrabbed) {
            photonView.RPC("GrabRPC", RpcTarget.All, true);
            return true;
        }
        else {
            
            return false;
        }
    }

    public void Release()
    {
        photonView.RPC("GrabRPC", RpcTarget.All, false);
    }

    #endregion

    #region RPCS

    [PunRPC]
    public void GrabRPC(bool b)
    {
        isGrabbed = b;
    }

    #endregion
}
