using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Level
{
    [Range(1, 11)]
    public int normalParts;
    [Range(0, 11)]
    public int deathParts;

    public Level(int normalParts, int deathParts)
    {
        this.normalParts = normalParts;
        this.deathParts = deathParts;
    }

    public Level() { }
}

[CreateAssetMenu(fileName = "New Stage")]
[Serializable]
public class Stage : ScriptableObject
{
    public Color BackgroundColor = Color.white;
    public Color BallColor = Color.white;
    public Color PartsColor = Color.white;
    public List<Level> Levels;

    public Stage(Color backgroundColor, Color ballColor, Color partsColor, List<Level> levels)
    {
        BackgroundColor = backgroundColor;
        BallColor = ballColor;
        PartsColor = partsColor;
        Levels = levels;
    }
}
