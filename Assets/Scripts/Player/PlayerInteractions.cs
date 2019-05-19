using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Interaction settings")]
    [SerializeField] private float _distanceToInteract = 3.0f;


    [Header("References")]
    [SerializeField] private Transform _leftHandTarget, _rightHandTarget;
    [SerializeField] private Transform _carryingObjectPosition;

    [SerializeField] private GameObject _interactableTarget = null;
    [SerializeField] private GameObject _currentInteractableObjectCarried = null;


    //Private reference to Player Components (assigned in the start function from the GameManager)
    private PlayerController _playerController = null;

    //Other private variables
    private bool _isCaryingSomething = false;
    private Transform _leftHandHandle, _rightHandHandle;

    private void Start()
    {
        StartCoroutine(Checking());

        //Getting all player components needed
        _playerController = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
    }

    private void LateUpdate()
    {
        if(!_isCaryingSomething && (_interactableTarget != null && Input.GetButtonDown("Interact")))
        {
            _currentInteractableObjectCarried = _interactableTarget;
            _isCaryingSomething = true;

            _currentInteractableObjectCarried.transform.parent = _carryingObjectPosition;
            _currentInteractableObjectCarried.transform.localPosition = Vector3.zero;
            _currentInteractableObjectCarried.GetComponent<Rigidbody>().isKinematic = true;
            _currentInteractableObjectCarried.GetComponent<Collider>().isTrigger = true;

            _playerController.GetFullBodyBipedIK().solver.leftHandEffector.positionWeight = 1.0f;
            _playerController.GetFullBodyBipedIK().solver.rightHandEffector.positionWeight = 1.0f;

            _playerController.GetFullBodyBipedIK().solver.leftHandEffector.rotationWeight = 1.0f;
            _playerController.GetFullBodyBipedIK().solver.rightHandEffector.rotationWeight = 1.0f;

            //Choose two handlers from the carried object (the nearest handler from each hand)
            Transform leftChoosenHandle = null;
            Transform rightChoosenHandle = null;
            float nearestPosition = 0.0f;

            //Get the nearest handle from the left hand
            foreach(Transform handle in _currentInteractableObjectCarried.GetComponent<InteractableObject>().GetHandles())
            {
                if(!leftChoosenHandle)
                {
                    leftChoosenHandle = handle;
                    nearestPosition = Vector3.Distance(_playerController.GetFullBodyBipedIK().solver.leftHandEffector.position, handle.position);
                }
                else
                {
                    float newPosition = Vector3.Distance(_playerController.GetFullBodyBipedIK().solver.leftHandEffector.position, handle.position);
                    if(newPosition<nearestPosition)
                    {
                        leftChoosenHandle = handle;
                        nearestPosition = newPosition;
                    }
                }
            }

            //Assign the left hand handle
            _leftHandHandle = leftChoosenHandle;

            //We need to avoid taking two time the same handle because he can't handle one point with two hands
            //Get the nearest handle from the right hand
            foreach (Transform handle in _currentInteractableObjectCarried.GetComponent<InteractableObject>().GetHandles())
            {
                if (handle != _leftHandHandle)
                {

                    if (!rightChoosenHandle)
                    {
                        rightChoosenHandle = handle;
                        nearestPosition = Vector3.Distance(_playerController.GetFullBodyBipedIK().solver.rightHandEffector.position, handle.position);
                    }
                    else
                    {
                        float newPosition = Vector3.Distance(_playerController.GetFullBodyBipedIK().solver.rightHandEffector.position, handle.position);
                        if (newPosition < nearestPosition)
                        {
                            rightChoosenHandle = handle;
                            nearestPosition = newPosition;
                        }
                    }

                }
            }

            //Assign the right hand handle
            _rightHandHandle = rightChoosenHandle;


        }
        else if(_isCaryingSomething)
        {
            _leftHandTarget.position = _leftHandHandle.position;
            _rightHandTarget.position = _rightHandHandle.position;


            //Key to release an object
            if(Input.GetButtonDown("Interact"))
            {
                _currentInteractableObjectCarried.transform.parent = null;
                _currentInteractableObjectCarried.GetComponent<Rigidbody>().isKinematic = false;
                _currentInteractableObjectCarried.GetComponent<Collider>().isTrigger = false;

                _playerController.GetFullBodyBipedIK().solver.leftHandEffector.positionWeight = 0.0f;
                _playerController.GetFullBodyBipedIK().solver.rightHandEffector.positionWeight = 0.0f;

                _playerController.GetFullBodyBipedIK().solver.leftHandEffector.rotationWeight = 0.0f;
                _playerController.GetFullBodyBipedIK().solver.rightHandEffector.rotationWeight = 0.0f;

                //Apply a force relative to the player movement
                _currentInteractableObjectCarried.GetComponent<Rigidbody>().AddForce(_playerController.GetMoveDirection() * 5000f);

                _currentInteractableObjectCarried = null;
                _isCaryingSomething = false;
            }
        }
    }

    IEnumerator Checking()
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
}
