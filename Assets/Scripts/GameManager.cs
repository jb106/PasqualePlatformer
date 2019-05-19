using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    [Header("Registered objects")]
    [SerializeField] private GameObject _player = null;
    [SerializeField] private List<InteractableObject> _interactableObjects = new List<InteractableObject>();

    //SETTERS
    //-----------------------------
    //-----------------------------------------------------

    public void RegisterNewInteractableObject(InteractableObject interactableObject)
    {
        _interactableObjects.Add(interactableObject);
    }

    public void RegisterPlayer(GameObject playerObject)
    {
        _player = playerObject;
    }


    //GETTERS
    //-----------------------------
    //-----------------------------------------------------

    public List<InteractableObject> GetInteractableObjects()
    {
        return _interactableObjects;
    }

    public GameObject GetPlayer()
    {
        return _player;
    }
}
