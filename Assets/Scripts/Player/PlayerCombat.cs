using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class PlayerCombat : MonoBehaviour
{
    //Private reference to Player Components (assigned in the start function from the GameManager)
    private PlayerController _playerController = null;
    private PlayerInteractions _playerInteractions = null;

    //Other private variables
    private float _currentFireTimer = 0.0f;

    private void Start()
    {
        //Getting all player components needed
        _playerController = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
        _playerInteractions = GameManager.instance.GetPlayer().GetComponent<PlayerInteractions>();

        StartCoroutine(WeaponUpdate());
    }


    //The main loop for combat script
    IEnumerator WeaponUpdate()
    {
        while(true)
        {
            //Always increment the fire timer, it is used to get the fire rate of each weapon
            _currentFireTimer += Time.deltaTime;

            //If the player is not carrying any weapon we don't need to go any further
            if (_playerInteractions.CheckIfPlayerIsCarryingSomething())
            {
                if (GetInteractableObject().interactableObjectData.interactableObjectType == InteractableObjectType.Weapon)
                {
                    //When we are here this mean that the player is holding a weapon

                    if (Input.GetButtonDown("Fire1"))
                    {
                        if (_currentFireTimer >= GetInteractableObject().interactableObjectData.fireRate)
                        {
                            _playerInteractions._carryingObjectAnimator.SetTrigger("fire");
                            _playerController.GetPlayerModel().gameObject.GetComponent<Recoil>().Fire(1f);

                            GameObject newBullet = Instantiate(GetInteractableObject().interactableObjectData.bulletPrefab);
                            newBullet.transform.position = GetInteractableObject().bulletSpawner.position;
                            newBullet.transform.rotation = GetInteractableObject().bulletSpawner.rotation;

                            //Reset the timer for the next shoot
                            _currentFireTimer = 0.0f;
                        }
                    }


                }
            }

            yield return null;
        }
    }

    private InteractableObject GetInteractableObject()
    {
        if (!_playerInteractions.CheckIfPlayerIsCarryingSomething())
            return null;

        return _playerInteractions.GetCurrentInteractableObjectCarried().GetComponent<InteractableObject>();
    }
}
