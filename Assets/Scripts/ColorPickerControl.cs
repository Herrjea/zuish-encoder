using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ColorPickerControl : MonoBehaviour
{
    public float currentHue = 0, currentSat = 1, currentVal = 1;

    [SerializeField] RawImage hueImage;
    [SerializeField] RawImage satValImage;
    [SerializeField] Slider hueSlider;
    Texture2D hueTexture, satValTexture, outputTexture;


    void Awake()
    {
        CreateHueImage();
        CreateSatValImage();

        UpdateColor();
    }


    void CreateHueImage()
    {
        hueTexture = new Texture2D(1, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for (int i = 0; i < hueTexture.height; i++)
            hueTexture.SetPixel(
                0, 
                i, 
                Color.HSVToRGB(
                    1f * i / hueTexture.height, 
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

        UpdateSatVatImage();

        satValImage.texture = satValTexture;
    }

    void UpdateColor()
    {
        Color color = Color.HSVToRGB(currentHue, currentSat, currentVal);

        GameEvents.NewGlyphColor.Invoke(color);
    }

    public void UpdateSatVal(float sat, float val)
    {
        currentSat = sat;
        currentVal = val;
        UpdateColor();
    }

    public void UpdateSatVatImage()
    {
        currentHue = hueSlider.value;

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
}
