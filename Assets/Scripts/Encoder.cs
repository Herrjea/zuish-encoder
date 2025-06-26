using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;


public enum Kerning
{
    Standard,
    Compact
}


public class Encoder : MonoBehaviour
{
    [SerializeField] string filePath;
    [SerializeField] Cipher cipher;
    [SerializeField] GameObject letterPrefab;
    [SerializeField] int lineLength;
    [SerializeField] Kerning kerning = Kerning.Standard;
    [SerializeField] TMP_Text kerningButtonText;
    List<List<Letter>> encoded;
    List<Letter> rawLetters;
    float kerningSize = 0.2f;


    //int x, y;
    void Awake()
    {
        string text = ProcessString(File.ReadAllText("Assets/Resources/" + filePath + ".txt"));

        encoded = new List<List<Letter>>();
        rawLetters = new List<Letter>();

        for (int i = 0; i < text.Length; i++)
            rawLetters.Add(GenerateLetter(text[i], i));

        UpdateVisuals();
        kerningButtonText.text = kerning.ToString().ToLower();

        GameEvents.NewGlyphColor.AddListener(NewGlyphColor);
    }


    void UpdateVisuals()
    {
        UpdateLining();
        UpdatePositions();
        UpdateTextBox();
    }

    void UpdateLining()
    {
        // clean old lining
        foreach (List<Letter> line in encoded)
            line.Clear();
        encoded.Clear();
        encoded.Add(new List<Letter>());

        Letter letter;
        int x = 0, y = 0;

        for (int i = 0; i < rawLetters.Count; i++)
        {
            letter = rawLetters[i];

            // linebreak
            if (letter.IsLinebreak)
            {
                NewLine();
            }

            // space
            else if (letter.IsSpace)
            {
                if (x >= lineLength)
                    NewLine();
                else
                {
                    encoded[y].Add(letter);
                    x++;
                }
            }

            // actual letter
            else
            {
                if (x >= lineLength && LineHasSpace(y))
                    MoveCurrentWordToNextLine();

                encoded[y].Add(letter);
                x++;
            } 
        }

        // Remove last line if it remained empty in the end
        if (encoded[encoded.Count - 1].Count == 0)
            encoded.RemoveAt(encoded.Count - 1);


        void NewLine()
        {
            y++;
            x = 0;
            encoded.Add(new List<Letter>());
        }

        void MoveCurrentWordToNextLine()
        {
            List<Letter> word = new List<Letter>();

            int line = encoded.Count - 1;
            int letter = encoded[line].Count - 1;
            while (!encoded[line][letter].IsSpace)
            {
                word.Insert(0, encoded[line][letter]);
                encoded[line].RemoveAt(encoded[line].Count - 1);

                letter--;
            }

            NewLine();
            foreach (Letter l in word)
                encoded[y].Add(l);
            x = encoded[y].Count;
        }
    }

    void UpdatePositions()
    {
        for (int i = 0; i < encoded.Count; i++)
            for (int j = 0; j < encoded[i].Count; j++)
            {
                encoded[i][j].MoveTo(new Vector3(
                    -i - (ApplyKerning ? kerningSize * i : 0), 
                    -j - (ApplyKerning ? kerningSize * j : 0),
                    0
                ));
            }
    }
    

    void UpdateTextBox()
    {
        float width = encoded.Count - 1 + (ApplyKerning ? kerningSize * (encoded.Count - 2) : 0);
        float height = 0;

        foreach (List<Letter> line in encoded)
            if (line.Count > height)
                height = line.Count;
        height -= 1;

        if (ApplyKerning)
            height += kerningSize * (height - 1);

        GameEvents.NewTextSize.Invoke(width, height);
    }


    void Update()
    {
        // make the text always be centered on origin

        float width = encoded[encoded.Count - 1][0].Position.x;
        float height = 0;

        foreach (List<Letter> line in encoded)
            if (line[line.Count - 1].Position.y < height)
                height = line[line.Count - 1].Position.y;

        transform.position = new Vector3(
            -width / 2, 
            -height / 2, 
            0
        );
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
            {
                if (c == 'Q')
                    result += 'k';
                else if (c == 'V')
                    result += 'u';
                else
                    result += (char)(c + shift);
            }

            else if (c >= 'a' && c <= 'z')
            {
                if (c == 'q')
                    result += 'k';
                else if (c == 'v')
                    result += 'u';
                else
                    result += c;
            }

            else if ((c == '.' || c == '\n') && previous != '\n')
                result += '\n';

            else if (previous != ' ' && previous != '\n')
                result += ' ';

            if (result.Length > 0)
                previous = result[result.Length - 1];
        }

        return result;
    }

    Letter GenerateLetter(char c, int i)
    {
        Letter letter = GameObject.Instantiate(letterPrefab, transform).GetComponent<Letter>();
        Sprite sprite = GetSprite(c);
        letter.GetComponent<SpriteRenderer>().sprite = sprite;

        if (c == ' ')
            letter.Type = LetterType.Space;
        else if (c == '\n')
            letter.Type = LetterType.Linebreak;

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


    public void ToggleKerning()
    {
        if (kerning == Kerning.Standard)
            kerning = Kerning.Compact;
        else
            kerning = Kerning.Standard;

        kerningButtonText.text = kerning.ToString().ToLower();

        UpdatePositions();
        UpdateTextBox();
    }

    public void ShorterLine()
    {
        lineLength--;
        UpdateVisuals();
    }

    public void LongerLine()
    {
        lineLength++;
        UpdateVisuals();
    }


    bool ApplyKerning 
    {
        get => kerning == Kerning.Standard;
    }

    bool LineHasSpace(int y)
    {
        foreach (Letter letter in encoded[y])
            if (letter.IsSpace)
                return true;

        return false;
    }


    void NewGlyphColor(Color color)
    {
        foreach (Letter letter in rawLetters)
            letter.Color = color;
    }
}
