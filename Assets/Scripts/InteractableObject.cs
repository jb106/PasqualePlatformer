using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeftRight { Left, Right }

public class InteractableObject : MonoBehaviour
{
    public float distanceToPlayer = 0.0f;
    public Vector3 rotationOffset = new Vector3();

    public LeftRight playerSide = LeftRight.Left;

    private List<Transform> handles = new List<Transform>();
    private Quaternion _defaultRotation = Quaternion.identity;
    private GameObject _player;
   

    void Start()
    {
        GameManager.instance.RegisterNewInteractableObject(this);
        _player = GameManager.instance.GetPlayer();

        foreach (Transform handle in transform)
            handles.Add(handle);

        _defaultRotation = transform.rotation * Quaternion.Euler(rotationOffset);
    }

    public List<Transform> GetHandles()
    {
        return handles;
    }

    public Transform GetLeftHandle()
    {
        return handles[1];

    }

    public Transform GetRightHandle()
    {
        return handles[0];
    }

    public Quaternion GetDefaultRotation()
    {
        Quaternion defaultRotation = _defaultRotation;

        if (playerSide == LeftRight.Right)
        {
            Vector3 convertedDefaultRotation = defaultRotation.eulerAngles;
            convertedDefaultRotation.y += 180;
            defaultRotation = Quaternion.Euler(convertedDefaultRotation);
        }

        return defaultRotation;
    }

    private void Update()
    {
        if (!_player)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        playerSide = transform.position.x >= _player.transform.position.x ? LeftRight.Left : LeftRight.Right;
    }

}
