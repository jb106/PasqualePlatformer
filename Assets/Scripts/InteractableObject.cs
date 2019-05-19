using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float distanceToPlayer = 0.0f;
    private List<Transform> handles = new List<Transform>();
    private GameObject _player;
   

    void Start()
    {
        GameManager.instance.RegisterNewInteractableObject(this);
        _player = GameManager.instance.GetPlayer();

        foreach (Transform handle in transform)
            handles.Add(handle);
    }

    public List<Transform> GetHandles()
    {
        return handles;
    }

    private void Update()
    {
        if (!_player)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
    }

}
