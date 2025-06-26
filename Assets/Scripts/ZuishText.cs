using UnityEngine;
using System.Collections.Generic;


[ExecuteAlways]
public class ZuishText : MonoBehaviour
{
    [SerializeField] string text = "";
    string previousText = "";
    [SerializeField] int size = 50;
    int previousSize;
    [SerializeField] bool spacing = true;
    bool previousSpacing;
    [SerializeField] GameObject letterPrefab;

    [SerializeField] Cipher cipher;
    Dictionary<char, Sprite> sprites;

    List<UILetter> letters;


    void Init()
    {
        sprites = new Dictionary<char, Sprite>();
        foreach (CharSpritePair pair in cipher.letters)
            sprites.Add(pair.letter, pair.sprite);

        letters = new List<UILetter>();

        previousText = text;
        previousSize = size;
        previousSpacing = spacing;
    }

    void Update()
    {
        if (previousText.Length != text.Length)
        {
            UpdateText();
            previousText = text;
        }

        if (previousSize != size)
        {
            UpdateSizes();
            UpdatePositions();
            previousSize = size;
        }

        if (previousSpacing != spacing)
        {
            UpdatePositions();
            previousSpacing = spacing;
        }
    }

    void UpdateText()
    {
        if (sprites == null)
            Init();

        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);
        letters.Clear();

        UILetter letter;
        foreach (char c in text)
            if (sprites.ContainsKey(c))
            {
                letter = GenerateLetter(c);
                letter.Init();
                letter.Sprite = sprites[c];
                letter.Size = size;
                letter.Position = 
                    1f * letters.Count * size 
                    + 
                    (spacing ? letters.Count * (size / 5) : 0);
                letters.Add(letter);
            }
    }

    void UpdatePositions()
    {
        for (int i = 0; i < letters.Count; i++)
            letters[i].Position = 
                1f * i * size 
                + 
                (spacing ? i * (size / 5) : 0);
    }

    void UpdateSizes()
    {
        foreach (UILetter letter in letters)
            letter.Size = size;
    }

    UILetter GenerateLetter(char c)
    {
        return GameObject.Instantiate(letterPrefab, transform).GetComponent<UILetter>();
    }
}
