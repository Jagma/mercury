﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionState : MonoBehaviour {
    
    public static int level = 0; // 2 - 4;

    public static string environmentName = "Mars";

    public static void NextLevel () {
        level++;
        if (level <= 1) {
            environmentName = "Mars";
        }
        else if (level <= 3)
        {
            environmentName = "Venus";
        }
        else if (level <= 5) {
            environmentName = "Mercury";
        }
    }

    public static void Reset () {
        level = 0;
        environmentName = "Mars";
    }
}
