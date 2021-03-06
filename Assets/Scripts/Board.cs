﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform playerEntranceSpawnPosition = null;
    public Transform playerExitSpawnPosition = null;
    public Transform cameraPointOfView = null;
    public ObjectCollisionStatus entrance, exit;

    private void Update()
    {
        //Entrance
        if (entrance.isColliding)
<<<<<<< HEAD
            GameManager.instance.SetPreviousBoard();

        //Exit
        if (exit.isColliding)
            GameManager.instance.SetNextBoard();
=======
            GameManager.Instance.SetPreviousBoard();

        //Exit
        if (exit.isColliding)
            GameManager.Instance.SetNextBoard();
>>>>>>> Upgrade v2019.1 Unity
    }

    public void SetActiveBoard(bool active)
    {
        foreach (Transform t in transform)
            t.gameObject.SetActive(active);

        //We must reset these values, because otherwise when we disable all the objects from a board, then the isColliding remain true and when we re-enable it cause bug
        entrance.isColliding = false;
        exit.isColliding = false;
    }
}
