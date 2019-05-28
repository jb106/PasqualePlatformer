using UnityEngine;
using System.Collections;

public enum InteractableObjectType { Weight, Weapon}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InteractableObjectData", order = 1)]
public class InteractableObjectData : ScriptableObject
{
    public InteractableObjectType interactableObjectType;

    public Vector3 leftHandleDefaultRotation = Vector3.zero;
    public Vector3 rightHandleDefaultRotation = Vector3.zero;

    public Vector3 leftHandleDefaultPosition = Vector3.zero;
    public Vector3 rightHandleDefaultPosition = Vector3.zero;
}