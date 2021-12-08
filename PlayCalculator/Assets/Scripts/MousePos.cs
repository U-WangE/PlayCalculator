using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePos : MonoBehaviour
{
    public Camera[] getCamera;
    public static string objectName;

    private RaycastHit hit;
    int cameraMode;  // 현재 게임을 실행 중인지 판단하여 getCamera를 변경해준다.

    void Start()
    {
        cameraMode = 0;
        objectName = null;
    }

    void Update()
    {
        // 마우스 클릭
        if(Input.GetMouseButtonUp(0))
        {
            if(!CameraChange.onGame)
                cameraMode = 0;  // Calculator 일 때
            else
                cameraMode = 1;  // Game 일 때

            // 마우스 위치
            Ray ray = getCamera[cameraMode].ScreenPointToRay(Input.mousePosition);

            // Ray에 Object가 걸림
            if(Physics.Raycast(ray, out hit))
            {
                // object 이름
                objectName = hit.collider.gameObject.name;
            }
        }
    }
}
