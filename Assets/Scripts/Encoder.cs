using UnityEngine;
using System.Collections.Generic;
using System.IO;


public enum Kerning
{
    Standard,
    Hardcore
}


public class Encoder : MonoBehaviour
{
    [SerializeField] string filePath;
    [SerializeField] Cipher cipher;
    [SerializeField] GameObject letterPrefab;
    [SerializeField] int lineLength;
    [SerializeField] Kerning kerning = Kerning.Standard;
    List<List<Transform>> encoded;
    float kerningSize = 0.2f;




    void Awake()
    {
        string text = ProcessString(File.ReadAllText("Assets/Resources/" + filePath + ".txt"));

        encoded = new List<List<Transform>>();
        encoded.Add(new List<Transform>());
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
                encoded[y].Add(letter);
                x++;
            } 
            else
            {
                NewLine();
            }
        }

        void NewLine()
        {
            y++;
            x = 0;
            encoded.Add(new List<Transform>());
        }


        /*
        GameEvents.NewTextCenter.Invoke(encoded[encoded.Count - 1].position / 2);
        GameEvents.NewTextSize.Invoke(
            -encoded[encoded.Count - 1].position.x,
            -encoded[encoded.Count - 1].position.y
        );
        */

        NewTextBox();

        // Remove last line if it remained empty in the end
        if (encoded[encoded.Count - 1].Count == 0)
            encoded.RemoveAt(encoded.Count - 1);
    }
    

    void NewTextBox()
    {
        float width = encoded.Count - 1 + (ApplyKerning ? kerningSize * (encoded.Count - 2) : 0);
        float height = 0;

        foreach (List<Transform> line in encoded)
            if (line.Count > height)
                height = line.Count;
        height -= 1;
        print(height);

        if (ApplyKerning)
            height += kerningSize * (height - 1);

        GameEvents.NewTextSize.Invoke(width, height);
        GameEvents.NewTextCenter.Invoke(new Vector3(
            -width / 2, 
            -height / 2 + 0.5f, 
            0
        ));
        print("width: " + width);
        print("height: " + height);
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
        Transform letter = GameObject.Instantiate(letterPrefab, transform).transform;
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
