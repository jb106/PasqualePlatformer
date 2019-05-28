using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class PlayerCombat : MonoBehaviour
{
    //Private reference to Player Components (assigned in the start function from the GameManager)
    private PlayerController _playerController = null;
    private PlayerInteractions _playerInteractions = null;

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
            if(Input.GetButtonDown("Fire1"))
            {
                //_playerInteractions._carryingObjectAnimator.SetTrigger("fire");
                _playerController.GetPlayerModel().gameObject.GetComponent<Recoil>().Fire(1f);
            }

            yield return null;
        }
    }
}
