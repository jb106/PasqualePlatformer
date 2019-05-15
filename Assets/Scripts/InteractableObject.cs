using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float distanceToPlayer = 0.0f;
    public Transform leftHandle, rightHandle;

    private GameObject _player;
   

    void Start()
    {
        GameManager._instance.RegisterNewInteractableObject(this);
        _player = PlayerController._instance.gameObject;
    }

    private void Update()
    {
        if (!_player)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
    }

}
