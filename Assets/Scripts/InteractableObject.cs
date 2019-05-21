using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSide { Left, Right }

public class InteractableObject : MonoBehaviour
{
    public float distanceToPlayer = 0.0f;
    public Quaternion defaultRotation = Quaternion.identity;
    public Vector3 rotationOffset = new Vector3();

    public PlayerSide playerSide = PlayerSide.Left;

    private List<Transform> handles = new List<Transform>();
    private GameObject _player;
   

    void Start()
    {
        GameManager.instance.RegisterNewInteractableObject(this);
        _player = GameManager.instance.GetPlayer();

        foreach (Transform handle in transform)
            handles.Add(handle);

        defaultRotation = transform.rotation * Quaternion.Euler(rotationOffset);
    }

    public List<Transform> GetHandles()
    {
        return handles;
    }

    public Transform GetLeftHandle()
    {
        if (playerSide == PlayerSide.Left)
            return handles[1];
        else
            return handles[0];
    }

    public Transform GetRightHandle()
    {
        if (playerSide == PlayerSide.Left)
            return handles[0];
        else
            return handles[1];
    }

    private void Update()
    {
        if (!_player)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        playerSide = transform.position.x >= _player.transform.position.x ? PlayerSide.Left : PlayerSide.Right;
    }

}
