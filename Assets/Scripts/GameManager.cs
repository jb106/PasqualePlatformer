using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    private void Awake()
    {
        _instance = this;
    }

    [Header("Registered objects")]
    [SerializeField] private List<InteractableObject> _interactableObjects = new List<InteractableObject>();

    public void RegisterNewInteractableObject(InteractableObject interactableObject)
    {
        _interactableObjects.Add(interactableObject);
    }

    public List<InteractableObject> GetInteractableObjects()
    {
        return _interactableObjects;
    }
}
