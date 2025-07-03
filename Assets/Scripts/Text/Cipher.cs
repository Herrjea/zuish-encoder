using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class Sprites
{
    public List<Sprite> sprites;
    int current = 0;

    public Sprite Sprite
    {
        get
        {
            current = ++current % sprites.Count;
            return sprites[current];
        }
    }
}


[System.Serializable]
public struct CharSpritePair
{
    public char letter;
    public Sprites sprites;
}


[CreateAssetMenu(fileName = "Cipher", menuName = "Cipher")]
public class Cipher : ScriptableObject
{
    public List<CharSpritePair> letters = null;
    Dictionary<char, Sprites> dictionary = null;

    bool initialized = false;

    public CharSpritePair this[int index]
    {
        get => letters[index];
        set => letters[index] = value;
    }

    public void Init()
    {
        if (initialized)
            return;

        dictionary = new Dictionary<char, Sprites>();
        foreach (CharSpritePair pair in letters)
            dictionary.Add(pair.letter, pair.sprites);
        
        initialized = true;
    }

    public bool Contains(char c)
    {
        if (dictionary == null)
        {
            initialized = false;
            Init();
        }

        return dictionary.ContainsKey(c);
    }

    public Sprite this[char c]
    {
        get
        {
            if (dictionary == null)
            {
                initialized = false;
                Init();
            }

            return dictionary[c].Sprite;
        }
    }
}
