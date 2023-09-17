using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Sprite Trail/Data")]
public class SpriteTrailData : ScriptableObject
{

    public float trailLifetime;
    public SpriteTrail.SpawnType spawnType;

    public float timeBetweenTrailSpawn;
    public float distanceBetweenTrailSpawn;

    public bool alphaUpdateOn;
    public float maxAlpha;
    public float minAlpha;

    public bool useSolidColors;
    public bool rainbowMode;

    public List<Color32> userSelectedColorPalette;
}
