using UnityEngine;
using System.Collections.Generic;


public enum Kerning
{
    Standard,
    Hardcore
}


public class Encoder : MonoBehaviour
{
    [TextArea, SerializeField] string input;
    [SerializeField] Cipher cipher;
    [SerializeField] GameObject letterPrefab;
    [SerializeField] int lineLength;
    [SerializeField] Kerning kerning = Kerning.Standard;
    List<Transform> encoded;
    float kerningSize = 0.2f;


    void Awake()
    {
        string text = ProcessString(input);

        encoded = new List<Transform>();
        int x = 0, y = 0;
        Transform letter;
        char c;

        for (int i = 0; i < text.Length; i++)
        {

            c = text[i];
            if (c != '\n')
            {
                letter = GenerateLetter(c);
                letter.position = new Vector3(
                    -y - (ApplyKerning ? kerningSize * y : 0), 
                    -x - (ApplyKerning ? kerningSize * x : 0),
                    0
                );
                encoded.Add(letter);
                x++;
            } 
            else
            {
                y++;
                x = 0;
            }

        }

    }


    string ProcessString(string text)
    {
        int shift = 'a' - 'A';
        char previous = ' ';
        string result = "";

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (c >= 'A' && c <= 'Z')
                result += (char)(c + shift);

            else if (c >= 'a' && c <= 'z')
                result += c;

            else if ((c == '.' || c == '\n') && previous != '\n')
                result += '\n';

            else if (previous != ' ' && previous != '\n')
                result += ' ';

            if (result.Length > 0)
                previous = result[result.Length - 1];
        }

        print(result);

        return result;
    }

    Transform GenerateLetter(char c)
    {
        Transform letter = GameObject.Instantiate(letterPrefab).transform;
        Sprite sprite = GetSprite(c);
        letter.GetComponent<SpriteRenderer>().sprite = sprite;
        return letter;
    }

    Sprite GetSprite(char c)
    {
        foreach (CharSpritePair pair in cipher.letters)
            if (pair.letter == c)
                return pair.sprite;

        print("value not found: '" + c + "'");
        return null;
    }

    bool ApplyKerning 
    {
        get => kerning == Kerning.Standard;
    }
}
