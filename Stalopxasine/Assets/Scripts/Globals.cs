using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static bool StartEnding = false;
    public static int Character = 0; //1-Caramello, 2-fireKS, 3-Fridman, 4-Viseman
    public static GameObject CreatedCharacter;
    public static Dictionary<int, Vector2> CharPositions = new Dictionary<int, Vector2>();
} 
