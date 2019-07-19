<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    public Vector3 movement = new Vector3();

    public float timerSwitch = 2.0f;
    public float platformSpeed = 5f;

    private float _currentTimer = 0.0f;
    private float _horizontalMovement = 1f;

    private void Start()
    {
        StartCoroutine(Move());
    }

=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    public Vector3 movement = new Vector3();

    public float timerSwitch = 2.0f;
    public float platformSpeed = 5f;

    private float _currentTimer = 0.0f;
    private float _horizontalMovement = 1f;

    private void Start()
    {
        StartCoroutine(Move());
    }

>>>>>>> Upgrade v2019.1 Unity
    IEnumerator Move()
    {
        while(true)
        {
<<<<<<< HEAD
            _currentTimer += Time.deltaTime;

            if (_currentTimer > timerSwitch)
            {
                _horizontalMovement = -_horizontalMovement;
                _currentTimer = 0;
            }

            movement = new Vector3(_horizontalMovement, 0, 0);
            movement = movement * platformSpeed;

            transform.Translate(movement * Time.deltaTime);

            yield return null;
        }
    }
}
=======
            _currentTimer += Time.deltaTime;

            if (_currentTimer > timerSwitch)
            {
                _horizontalMovement = -_horizontalMovement;
                _currentTimer = 0;
            }

            movement = new Vector3(_horizontalMovement, 0, 0);
            movement = movement * platformSpeed;

            transform.Translate(movement * Time.deltaTime);

            yield return null;
        }
    }
}
>>>>>>> Upgrade v2019.1 Unity
