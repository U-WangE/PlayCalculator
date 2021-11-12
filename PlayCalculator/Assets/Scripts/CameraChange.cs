using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraChange : MonoBehaviour
{
    public Camera mainCamera;
    public Camera gameCamera;
    public GameObject game_screen;
    public TextMeshPro gameOnOff;
    public static bool onGame;

    void Start()
    {
        mainCamera.enabled = true;
        gameCamera.enabled = false;
        game_screen.SetActive(false);
        onGame = false;
    }

    void Update()
    {
        string[] click = MousePos.objectName.Split('_');

        if(click[1].Contains("GS") && onGame == false)  // MainCamera -> GameCamera로 전환
        {
            gameOnOff.text = "Exit";
            GameCamera();
        }
        else if(click[1].Contains("GS") && onGame == true)  // GameCamera -> MainCamera로 전환
        {
            gameOnOff.text = "Game";
            CalculatorCamera();
        }

        // 마우스 클릭 초기화
        MousePos.objectName = null;
    }

    // GameCamera On
    void GameCamera()
    {
        mainCamera.enabled = false;
        gameCamera.enabled = true;
        game_screen.SetActive(true);
        onGame = true;
    }

    // MainCamera On
    void CalculatorCamera()
    {
        mainCamera.enabled = true;
        gameCamera.enabled = false;
        game_screen.SetActive(false);
        onGame = false;
    }
}