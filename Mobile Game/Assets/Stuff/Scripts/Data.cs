using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public Skin[] skins;
    public int gems;

    public Data(Skin[] _skins, int _gems)
    {
        skins = _skins;
        gems = _gems;
    }
}
