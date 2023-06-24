using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviourPunCallbacks
{
    #region Member Variables

    public List<Light> lightSources;
    public InputActionProperty lightToggleAction;

    private bool lightOn = true;
    
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        foreach (var light in lightSources)
        {
            light.intensity = lightOn ? 1f : 0f;
        }
    }

    private void Update()
    {
        foreach (var light in lightSources)
        {
            light.intensity = lightOn ? 1f : 0f;
        }

        if (lightToggleAction.action.WasPressedThisFrame()) { 
            //ToggleLight();
            photonView.RPC("ToggleLightRPC", RpcTarget.All);
        }
    }

    #endregion

    #region Custom Methods

    private void ToggleLight()
    {
        if (lightOn == true)
        {
            lightOn = false;
        }
        else
        {
            lightOn = true;
        }
    }

    #endregion

    #region PUN Callbacks

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("SendStateRPC", RpcTarget.All, lightOn);
    }

    #endregion

    #region RPCs

    [PunRPC]
    public void ToggleLightRPC()
    {
        ToggleLight();
    }

    [PunRPC]
    public void SendStateRPC(bool lightOn)
    {
        // use to inform late joined users
        this.lightOn = lightOn;
    }

    #endregion
}
