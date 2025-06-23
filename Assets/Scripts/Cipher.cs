using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public struct CharSpritePair{
    public char letter;
    public Sprite sprite;
}


[CreateAssetMenu(fileName = "Cipher", menuName = "Cipher")]
public class Cipher : ScriptableObject
{
    public List<CharSpritePair> letters;

    public CharSpritePair this[int index]
    {
        get => letters[index];
        set => letters[index] = value;
    }
}
