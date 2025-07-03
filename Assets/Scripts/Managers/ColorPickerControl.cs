using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ColorPickerControl : MonoBehaviour
{
    [SerializeField] float initialHue = 1, initialSat = 0, initialVal = 1;
    [HideInInspector] public float currentHue, currentSat, currentVal;

    [SerializeField] RawImage hueImage;
    [SerializeField] RawImage satValImage;
    Texture2D hueTexture, satValTexture;

    [SerializeField] bool glyphColor = true;


    void Awake()
    {
        currentHue = initialHue;
        currentSat = initialSat;
        currentVal = initialVal;

        CreateHueImage();
        CreateSatValImage();

        UpdateColor();

        GameEvents.RandomizeColors.AddListener(RandomizeColors);
    }


    void CreateHueImage()
    {
        hueTexture = new Texture2D(16, 1);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for (int i = 0; i < hueTexture.width; i++)
            hueTexture.SetPixel(
                i, 
                0, 
                Color.HSVToRGB(
                    1f * i / hueTexture.width, 
                    1, 
                    1
                )
            );

        hueTexture.Apply();
        hueImage.texture = hueTexture;
    }

    void CreateSatValImage()
    {
        satValTexture = new Texture2D(16, 16);
        satValTexture.wrapMode = TextureWrapMode.Clamp;
        satValTexture.name = "SatValTexture";

        UpdateSatValImage();

        satValImage.texture = satValTexture;
    }

    void UpdateColor()
    {
        Color color = Color.HSVToRGB(currentHue, currentSat, currentVal);

        (glyphColor ? GameEvents.NewGlyphColor : GameEvents.NewBGColor).Invoke(color);
    }

    public void UpdateSatVal(float sat, float val)
    {
        currentSat = sat;
        currentVal = val;

        UpdateColor();
    }

    public void UpdateHue(float hue)
    {
        currentHue = hue;

        UpdateSatValImage();
        UpdateColor();
    }

    public void UpdateSatValImage()
    {
        for (int y = 0; y < satValTexture.height; y++)
            for (int x = 0; x < satValTexture.width; x++)
                satValTexture.SetPixel(
                    x,
                    y,
                    Color.HSVToRGB(
                        currentHue, 
                        1f * x / satValTexture.width, 
                        1f * y / satValTexture.height
                    )
                );

        satValTexture.Apply();
    }

    void RandomizeColors()
    {
        currentHue = Random.Range(0f, 1f);
        currentSat = Random.Range(0f, 1f);
        currentVal = Random.Range(0f, 1f);
    }
}
