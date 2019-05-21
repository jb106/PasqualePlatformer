﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Interaction settings")]
    [SerializeField] private float _distanceToInteract = 3.0f;
    [SerializeField] private float _bodyLevelWhenCrouching = 2.5f;


    [Header("References")]
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _rightHandTarget;
    [SerializeField] private Transform _carryingObjectPosition;

    [SerializeField] private GameObject _interactableTarget = null;
    [SerializeField] private GameObject _currentInteractableObjectCarried = null;


    //Private reference to Player Components (assigned in the start function from the GameManager)
    private PlayerController _playerController = null;

    //Other private variables
    private bool _isCaryingSomething = false;
    private Transform _leftHandHandle, _rightHandHandle;

    //This boolean is useful when we have delays on grabing objects so if the thing is processing something we can't react to player input
    private bool _isProcessing = false;

    private Vector3 _bodyOffsetPosition = new Vector3();
    private float _handsWeight = 0.0f;
    private float _handLerpValue = 0.0f;

    private void Start()
    {
        //Getting all player components needed
        _playerController = GameManager.instance.GetPlayer().GetComponent<PlayerController>();

        StartCoroutine(CheckingAndUpdatingStuff());
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        _playerController.GetFullBodyBipedIK().solver.bodyEffector.positionOffset = _bodyOffsetPosition;

        //Lerp the weights off the hands for grabing animation...
        float lerpSpeed = 6f;

        _handLerpValue = Mathf.Lerp(_handLerpValue, _handsWeight, Time.deltaTime * lerpSpeed);

        _playerController.GetFullBodyBipedIK().solver.leftHandEffector.positionWeight = _handLerpValue;
        _playerController.GetFullBodyBipedIK().solver.rightHandEffector.positionWeight = _handLerpValue;

        _playerController.GetFullBodyBipedIK().solver.leftHandEffector.rotationWeight = _handLerpValue;
        _playerController.GetFullBodyBipedIK().solver.rightHandEffector.rotationWeight = _handLerpValue;

        if (_isProcessing)
            return;

        if(!_isCaryingSomething && (_interactableTarget != null && Input.GetButtonDown("Interact")))
        {
            //Check if the player is facing the object to avoid bug because the player is not turned toward the object
            if(_interactableTarget.GetComponent<InteractableObject>().playerSide != _playerController.GetPlayerFacingDirection())
                StartCoroutine(StartGrabingObject());

        }
        else if(_isCaryingSomething)
        {
            _leftHandTarget.position = _leftHandHandle.position;
            _rightHandTarget.position = _rightHandHandle.position;

            //Key to release an object
            if (Input.GetButtonDown("Interact"))
            {
                ReleaseObject();
                _handsWeight = 0.0f;
            }
        }
    }

    private void GrabObject()
    {
        _currentInteractableObjectCarried = _interactableTarget;
        _isCaryingSomething = true;

        //Assign the hands handles
        _leftHandHandle = _currentInteractableObjectCarried.GetComponent<InteractableObject>().GetLeftHandle();
        _rightHandHandle = _currentInteractableObjectCarried.GetComponent<InteractableObject>().GetRightHandle();

        _handsWeight = 1.0f;
    }
    private void ReleaseObject()
    {
        _currentInteractableObjectCarried.transform.parent = null;
        _currentInteractableObjectCarried.GetComponent<Rigidbody>().isKinematic = false;
        _currentInteractableObjectCarried.GetComponent<Collider>().isTrigger = false;

        //Apply a force relative to the player movement
        _currentInteractableObjectCarried.GetComponent<Rigidbody>().AddForce(_playerController.GetMoveDirection() * 5000f);

        _currentInteractableObjectCarried = null;
        _isCaryingSomething = false;
    }

    IEnumerator CheckingAndUpdatingStuff()
    {
        while (true)
        {
            _interactableTarget = null;
            foreach( InteractableObject obj in GameManager.instance.GetInteractableObjects())
            {
                if (obj.distanceToPlayer <= _distanceToInteract)
                    _interactableTarget = obj.gameObject;
            }

            yield return null;
        }
    }


    IEnumerator StartGrabingObject()
    {
        _isProcessing = true;
        _playerController.SetCanMove(false);
        GrabObject();

        float crouchSpeed = 4f;

        //Lerp the crouch
        float timerLerp = 0.0f;
        while(true)
        {
            float newLevelValue = Mathf.Lerp(_bodyOffsetPosition.y, _bodyLevelWhenCrouching, timerLerp);
            _bodyOffsetPosition = new Vector3(0, newLevelValue, 0);

            _leftHandTarget.position = _leftHandHandle.position;
            _rightHandTarget.position = _rightHandHandle.position;

            timerLerp += Time.deltaTime * crouchSpeed;
            if (timerLerp >= 1.0f)
                break;

            yield return null;
        }

        _currentInteractableObjectCarried.transform.rotation = _currentInteractableObjectCarried.GetComponent<InteractableObject>().defaultRotation;
        _currentInteractableObjectCarried.transform.parent = _carryingObjectPosition;
        
        _currentInteractableObjectCarried.GetComponent<Rigidbody>().isKinematic = true;
        _currentInteractableObjectCarried.GetComponent<Collider>().isTrigger = true;

        //Lerp the stand up
        timerLerp = 0.0f;
        while (true)
        {
            float newLevelValue = Mathf.Lerp(_bodyOffsetPosition.y, 0, timerLerp);
            _bodyOffsetPosition = new Vector3(0, newLevelValue, 0);

            //Smoothly lerp the position and rotation of the grabbed object
            _currentInteractableObjectCarried.transform.localPosition = Vector3.Lerp(_currentInteractableObjectCarried.transform.localPosition, Vector3.zero, timerLerp);

            _leftHandTarget.position = _leftHandHandle.position;
            _rightHandTarget.position = _rightHandHandle.position;

            timerLerp += Time.deltaTime * crouchSpeed;
            if (timerLerp >= 1.0f)
                break;

            yield return null;
        }

        _playerController.SetCanMove(true);
        _isProcessing = false;
    }
}
