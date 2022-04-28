using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PassiveBase : ItemBase
{
    public string currentValue;

    private void Start()
    {
        InitializeItemFromScriptableObject();
    }

}
