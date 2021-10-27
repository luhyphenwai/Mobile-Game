using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public bool[] ownedSkins;
    public int gems;
    public int highScore;
    public Data(Skin[] _skins, int _gems, int _highScore)
    {
        ownedSkins = new bool[_skins.Length];
        for (int i = 0; i < _skins.Length; i++)
        {
            ownedSkins[i] = _skins[i].owned;
        }

        gems = _gems;
        highScore = _highScore;
    }
}
