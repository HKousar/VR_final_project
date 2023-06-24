using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PointingRay : MonoBehaviourPun, IPunObservable
{
    #region Member Variables

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private LineRenderer lineRendererDist;

    [Header("Ray parameter")] 
    public float rayWidth;
    public float idleLength = 10f;
    public float maxDistance = 100f;
    public Color idleColor;
    public Color highlightColor;
    public LayerMask layersToInclude;
    public InputActionProperty rayActivation;

    private bool isHitting = false;
    private Vector3 startPosition;
    private Vector3 endPosition;


    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        InitializeRay();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            UpdateRay();
        }
    }

    #endregion

    #region Custom Methods

    private void InitializeRay()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        
        lineRenderer.positionCount = 2;

        lineRenderer.startColor = idleColor;
        lineRenderer.endColor = idleColor;
    }

    private void UpdateRay()
    {
        startPosition = transform.position;
        if (rayActivation.action.WasPressedThisFrame() && lineRenderer.enabled == false)
        {
            lineRenderer.enabled = true;
        }
        else if (rayActivation.action.WasPressedThisFrame() && lineRenderer.enabled == true)
        {
            lineRenderer.enabled = false;
        }


        Vector3 endPointIdle = transform.position + (transform.forward * idleLength);

        Vector3 rayVector = endPointIdle - startPosition;

        lineRenderer.SetPositions(new Vector3[] { transform.position, endPosition });

        RaycastHit hit;
        if (Physics.Raycast(startPosition, rayVector.normalized, out hit, idleLength, layersToInclude))
        {
            isHitting = true;
            lineRenderer.startColor = highlightColor;
            lineRenderer.endColor = highlightColor;
            endPosition = hit.point;
        }
        else
        {
            isHitting = false;
            lineRenderer.startColor = idleColor;
            lineRenderer.endColor = idleColor;
            endPosition = endPointIdle;
        }
    }

    #endregion

    #region IPunObservable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (photonView.IsMine && stream.IsWriting)
        {
            stream.SendNext(lineRenderer.enabled);
            stream.SendNext(isHitting);
            stream.SendNext(startPosition);
            stream.SendNext(endPosition);
        }
        else if (stream.IsReading)
        {
            var isEnabled = (bool)stream.ReceiveNext();
            var isHittingDist = (bool)stream.ReceiveNext();
            var startPosition = (Vector3)stream.ReceiveNext();
            var endPosition = (Vector3)stream.ReceiveNext();

            lineRendererDist = GetComponent<LineRenderer>();
            lineRendererDist.enabled = isEnabled;

            lineRendererDist.startWidth = rayWidth;
            lineRendererDist.endWidth = rayWidth;

            lineRendererDist.positionCount = 2;

            lineRendererDist.SetPositions(new Vector3[] { startPosition, endPosition });

            if (isHittingDist)
            {
                lineRenderer.startColor = highlightColor;
                lineRenderer.endColor = highlightColor;
            }
            else
            {
                lineRenderer.startColor = idleColor;
                lineRenderer.endColor = idleColor;
            }


        }
    }

    #endregion

}
