using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CarAppearance : MonoBehaviour
{
    public string playerName;
    public Color carColor;
    public TMP_Text nameText;
    public Renderer carRenderer;

    public int playerNumber;
    // Start is called before the first frame update
    void Start()
    {
        if (playerNumber == 0)
        {
            playerName = PlayerPrefs.GetString("PlayerName");
            carColor = IntToColor(PlayerPrefs.GetInt("Red"), PlayerPrefs.GetInt("Green"), PlayerPrefs.GetInt("Blue"));
        }
        else
        {
            playerName = "Random " + playerNumber;
            carColor = new Color(Random.Range(0f, 255f) / 255, Random.Range(0f, 255f) / 255, Random.Range(0f, 255f) / 255);
        }

        nameText.text = playerName;
        carRenderer.material.color = carColor;
        nameText.color = carColor;
    }
    public Color IntToColor(int red, int green, int blue)
    {
        float r = (float)red / 255;
        float g = (float)green / 255;
        float b = (float)blue / 255;
        Color col = new Color(r, g, b);
        return col;
    }
}
