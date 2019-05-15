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


    private bool _isCaryingSomething = false;
    [SerializeField] private GameObject _interactableTarget = null;
    [SerializeField] private GameObject _currentInteractableObjectCarried = null;


    private void Start()
    {
        StartCoroutine(Checking());
    }

    private void Update()
    {
        if(!_isCaryingSomething && (_interactableTarget != null && Input.GetButtonDown("Interact")))
        {
            _currentInteractableObjectCarried = _interactableTarget;
            _isCaryingSomething = true;

            _currentInteractableObjectCarried.transform.parent = _carryingObjectPosition;
            _currentInteractableObjectCarried.transform.localPosition = Vector3.zero;
            _currentInteractableObjectCarried.GetComponent<Rigidbody>().isKinematic = true;
            _currentInteractableObjectCarried.GetComponent<Collider>().isTrigger = true;

            PlayerController._instance.GetFullBodyBipedIK().solver.leftHandEffector.positionWeight = 1.0f;
            PlayerController._instance.GetFullBodyBipedIK().solver.rightHandEffector.positionWeight = 1.0f;

            PlayerController._instance.GetFullBodyBipedIK().solver.leftHandEffector.rotationWeight = 1.0f;
            PlayerController._instance.GetFullBodyBipedIK().solver.rightHandEffector.rotationWeight = 1.0f;

            _leftHandTarget.position = _currentInteractableObjectCarried.GetComponent<InteractableObject>().leftHandle.position;
            _rightHandTarget.position = _currentInteractableObjectCarried.GetComponent<InteractableObject>().rightHandle.position;

        }
    }

    IEnumerator Checking()
    {
        while (true)
        {
            foreach( InteractableObject obj in GameManager._instance.GetInteractableObjects())
            {
                if (obj.distanceToPlayer <= _distanceToInteract)
                    _interactableTarget = obj.gameObject;
                else
                    _interactableTarget = null;
            }

            yield return null;
        }
    }
}
