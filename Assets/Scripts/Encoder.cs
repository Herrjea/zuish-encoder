using UnityEngine;
using System.Collections.Generic;
using System.IO;


public enum Spacing
{
    Standard,
    Compact
}


public class Encoder : MonoBehaviour
{
    [SerializeField] bool loadFile = true;
    [SerializeField] string filePath;
    [SerializeField] Cipher cipher;
    [SerializeField] GameObject letterPrefab;
    [SerializeField] int lineLength;
    [SerializeField] Spacing spacing = Spacing.Standard;
    [SerializeField] ZuishText spacingButtonText;
    [SerializeField] TypingCursor cursor;
    [SerializeField] float firstRemoveCooldown = .5f;
    [SerializeField] float secondRemoveCooldown = .1f;
    [SerializeField] float removeCooldownSpeedIncrease = 1.1f;
    float actualRemoveCooldown;
    List<List<Letter>> encoded;
    List<Letter> rawLetters;
    float spacingSize = 0.2f;
    Color currentGlyphColor = Color.white;

    Dictionary<KeyCode, char> chars;


    void Awake()
    {
        cipher.Init();

        encoded = new List<List<Letter>>();
        encoded.Add(new List<Letter>());
        rawLetters = new List<Letter>();

        spacingButtonText.text = spacing.ToString().ToLower();
        
        if (loadFile)
        {
            string text;

#if UNITY_WEBGL && !UNITY_EDITOR
            text = ProcessString(PlayerPrefs.GetString("file"));
#else
            text = ProcessString(File.ReadAllText("Assets/Resources/" + filePath + ".txt"));
#endif

            for (int i = 0; i < text.Length; i++)
                rawLetters.Add(GenerateLetter(text[i]));

            UpdateVisuals();
            cursor.gameObject.SetActive(false);
        }
        else
        {
            chars = new Dictionary<KeyCode, char>()
            {
                { KeyCode.Q, 'k' },
                { KeyCode.W, 'w' },
                { KeyCode.E, 'e' },
                { KeyCode.R, 'r' },
                { KeyCode.T, 't' },
                { KeyCode.Y, 'y' },
                { KeyCode.U, 'u' },
                { KeyCode.I, 'i' },
                { KeyCode.O, 'o' },
                { KeyCode.P, 'p' },
                { KeyCode.A, 'a' },
                { KeyCode.S, 's' },
                { KeyCode.D, 'd' },
                { KeyCode.F, 'f' },
                { KeyCode.G, 'g' },
                { KeyCode.H, 'h' },
                { KeyCode.J, 'j' },
                { KeyCode.K, 'k' },
                { KeyCode.L, 'l' },
                { KeyCode.Z, 'z' },
                { KeyCode.X, 'x' },
                { KeyCode.C, 'c' },
                { KeyCode.V, 'u' },
                { KeyCode.B, 'b' },
                { KeyCode.N, 'n' },
                { KeyCode.M, 'm' },
                { KeyCode.Alpha0, '0' },
                { KeyCode.Alpha1, '1' },
                { KeyCode.Alpha2, '2' },
                { KeyCode.Alpha3, '3' },
                { KeyCode.Alpha4, '4' },
                { KeyCode.Alpha5, '5' },
                { KeyCode.Alpha6, '6' },
                { KeyCode.Alpha7, '7' },
                { KeyCode.Alpha8, '8' },
                { KeyCode.Alpha9, '9' },
                { KeyCode.Plus, '+' },
                { KeyCode.Equals, '+' },
                { KeyCode.Space, ' ' },
            };

            UpdatePositions();
            cursor.Play();
        }

        GameEvents.NewGlyphColor.AddListener(NewGlyphColor);
    }


    void UpdateVisuals()
    {
        if (loadFile)
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
                encoded[i][j].MoveTo(GetPosition(i, j));
            }

        if (!loadFile)
        {
            int x = encoded.Count - 1;
            int y = encoded[encoded.Count - 1].Count;

            cursor.transform.localPosition = GetPosition(x, y);
            cursor.Restart();
        }
    }

    Vector3 GetPosition(int line, int letter)
    {
        return new Vector3(
            -line - (ApplySpacing ? spacingSize * line : 0), 
            -letter - (ApplySpacing ? spacingSize * letter : 0),
            0
        );
    }
    

    void UpdateTextBox()
    {
        float width = encoded.Count - 1 + (ApplySpacing ? spacingSize * (encoded.Count - 2) : 0);
        float height = 0;

        foreach (List<Letter> line in encoded)
            if (line.Count > height)
                height = line.Count;
        height -= 1;

        if (ApplySpacing)
            height += spacingSize * (height - 1);

        GameEvents.NewTextSize.Invoke(width, height);
    }


    float backspaceTime;
    bool onlyRemovedOne;
    void Update()
    {
        if (!loadFile)
        {
            // add letters
            foreach (KeyCode keyCode in chars.Keys)
                if (Input.GetKeyDown(keyCode))
                {
                    Letter letter = GenerateLetter(chars[keyCode]);
                    letter.Color = currentGlyphColor;

                    if (encoded.Count > 0)
                    {
                        // already something in last line
                        if (encoded[encoded.Count - 1].Count > 0)
                            letter.InstaMoveTo(GetPosition(encoded.Count - 1, encoded[encoded.Count - 1].Count - 1));

                        // last line empty and last line != first line
                        else if (encoded.Count > 1)
                            letter.InstaMoveTo(GetPosition(encoded.Count - 2, 0));

                        // first character in canvas
                        else
                            letter.InstaMoveTo(GetPosition(0, -1));
                    }
                    encoded[encoded.Count - 1].Add(letter);
                    UpdatePositions();
                    UpdateTextBox();
                }

            // add line
            if (Input.GetKeyDown(KeyCode.Return))
            {
                encoded.Add(new List<Letter>());

                UpdatePositions();
                UpdateTextBox();
            }


            // remove letters or go back

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                // empty page
                if (encoded.Count == 0 || (encoded.Count == 1 && encoded[0].Count == 0))
                {
                    GameObject.FindFirstObjectByType<Manager>().LoadModeSelectionScene();
                }
                else
                {
                    BackspaceTick();

                    backspaceTime = 0;
                    onlyRemovedOne = true;
                    actualRemoveCooldown = firstRemoveCooldown;
                }
            }

            if (Input.GetKey(KeyCode.Backspace))
            {
                backspaceTime += Time.deltaTime;

                if (backspaceTime >= actualRemoveCooldown)
                {
                    BackspaceTick();

                    backspaceTime = 0;
                    if (onlyRemovedOne)
                    {
                        actualRemoveCooldown = secondRemoveCooldown;
                        onlyRemovedOne = false;
                    }
                    else
                    {
                        actualRemoveCooldown /= removeCooldownSpeedIncrease;
                    }
                }
            }
        }

        if (encoded.Count == 0 ||encoded[encoded.Count - 1].Count == 0)
            return;


        // make the text always be centered on origin

        float width = encoded[encoded.Count - 1][0].Position.x;
        float height = 0;

        foreach (List<Letter> line in encoded)
            if (line.Count > 0 && line[line.Count - 1].Position.y < height)
                height = line[line.Count - 1].Position.y;

        transform.position = new Vector3(
            -width / 2, 
            -height / 2, 
            0
        );
    }

    void BackspaceTick()
    {
        // empty page
        if (encoded.Count == 0 || (encoded.Count == 1 && encoded[0].Count == 0))
        {
            return;
        }

        // last line has something in it
        if (encoded[encoded.Count - 1].Count > 0)
        {
            Destroy(encoded[encoded.Count - 1][encoded[encoded.Count - 1].Count - 1].gameObject);
            encoded[encoded.Count - 1].RemoveAt(encoded[encoded.Count - 1].Count - 1);
        }

        // last line is empty
        else
        {
            encoded.RemoveAt(encoded.Count - 1);
        }
                    
        UpdatePositions();
        UpdateTextBox();
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

    Letter GenerateLetter(char c)
    {
        Letter letter = GameObject.Instantiate(letterPrefab, transform).GetComponent<Letter>();
        letter.GetComponent<SpriteRenderer>().sprite = cipher.Contains(c) ? cipher[c] : null;

        if (c == ' ')
            letter.Type = LetterType.Space;
        else if (c == '\n')
            letter.Type = LetterType.Linebreak;

        return letter;
    }


    public void ToggleSpacing()
    {
        if (spacing == Spacing.Standard)
            spacing = Spacing.Compact;
        else
            spacing = Spacing.Standard;

        spacingButtonText.text = spacing.ToString().ToLower();

        UpdatePositions();
        UpdateTextBox();
    }

    public void ShorterLine()
    {
        if (lineLength <= 1 || !loadFile)
            return;

        lineLength--;
        UpdateVisuals();
    }

    public void LongerLine()
    {
        if (!loadFile)
            return;

        lineLength++;
        UpdateVisuals();
    }


    bool ApplySpacing 
    {
        get => spacing == Spacing.Standard;
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
        currentGlyphColor = color;

        if (loadFile)
            foreach (Letter letter in rawLetters)
                letter.Color = currentGlyphColor;
        else
        {
            foreach (List<Letter> line in encoded)
                foreach (Letter letter in line)
                    letter.Color = currentGlyphColor;

            cursor.SetColor(currentGlyphColor);
        }
    }
}
