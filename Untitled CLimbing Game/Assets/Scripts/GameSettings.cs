using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] public int lockFrameRate = 150;

    private void Awake()
    {

        Application.targetFrameRate = lockFrameRate;
    }
}
