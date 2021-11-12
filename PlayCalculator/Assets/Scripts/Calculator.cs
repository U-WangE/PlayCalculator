using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Calculator : MonoBehaviour
{
    public TextMeshPro screen_main;  // 현재 계산 중인 과정, 결과를 보여주는 screen
    public TextMeshPro screen_sub;  // 이전에 계산한 과정, 결과를 보여주는 screen

    public string input;  // 현재 입력한 값
    public List<String> course;  // 현재까지 입력된 기록
    public string result; // 결과 값
    public double output;

    void Start()
    {
        course = new List<string>();
        input = "";
        course.Add("0");  // 부호가 먼저 입력 된 경우를 위해
        result = "";
    }

    void Update()
    {
        string[] btn = {"number", "math", "etc"};  // 눌린 버튼의 대분류
        string[] click = MousePos.objectName.Split('_'); // _ 단위로 자름 : num_1 -> num, 1

        if(!CameraChange.onGame)
        {
            // 숫자가 눌렸을 경우
            if (btn[0].Contains(click[0]))
            {
                GetNumber(click[1]);
            }
            // 연산 기호가 눌렸을 경우
            else if (btn[1].Contains(click[0]))
            {
                GetMath(click[1]);
            }
            // 숫자, 연산 기호 이외의 버튼이 눌렸을 경우
            else if (btn[2].Contains(click[0]))
            {
                GetEtc(click[1]);
            }
        }

        // 마우스 클릭 초기화
        MousePos.objectName = null;
    }

    // 숫자 입력
    void GetNumber(string click)   // -> 루트와 제곱 이후 바로 숫자가 나올 수 없다. (수정 바람)
    {
        if(course[course.Count - 1] != "^" || course[course.Count - 1] != "r")
        {
            input += click;
            result += click;
            // 현재 입력한 값 출력
            screen_main.text = result;
        }
    }

    // 부호 입력
    void GetMath(string click)
    {
        // 처음 입력시 아무 입력 없이 "="이 들어온 경우
        if(result == "" && click.Substring(0, 2) == "Eq")
        {
            result = output.ToString();
            input = result;
            screen_main.text = result;
            return;
        }

        // 숫자 입력 없이 입력에 부호만 들어왔을 때
        if(input == "")
        {
            // 첫 입력에 연산 기호가 들어온 경우 -> 이전 기록의 결과를 가져온다.
            if(course.Count == 1)
            {
                input = output.ToString();
                result = input;
                course.Add(input);
                screen_main.text = input;
            }
            // 연산 기호가 연속으로 들어온 경우, 나중에 들어온 연산 기호로 변경
            else if(course.Count > 1)
            {
                // 현재 list에 들어온 입력이 숫자인지 연산 기호인지 판단
                try
                {
                    int.Parse(course[course.Count - 1]);
                }
                // 연산 기호 인경우
                catch (Exception ex)
                {
                    // 직전 입력이 루트와 제곱인 경우 제외
                    if(course[course.Count - 1] != "r" && course[course.Count - 1] != "^")
                    {
                        course.RemoveAt(course.Count - 1);
                        
                        result = result.Substring(0, result.Length - 1);
                    }
                }
            }
        }
        else if(input != "")  // 부호 전에 입력된 숫자를 course에 기록
        {
            if(course[course.Count - 1] == ".")
            {
                course.Add(input);
                FindDot();
            }
            else
                course.Add(input);
        }

        // 부호 판별
        switch(click.Substring(0, 2))
        {
        case "Pl":
            course.Add("+");
            result += "+";
            break;
        case "Mi":
            course.Add("-");
            result += "-";
            break;
        case "Mu":
            course.Add("*");
            result += "*";
            break;
        case "Di":
            course.Add("/");
            result += "/";
            break;
        case "Do":
            course.Add(".");
            result += ".";
            break;
        case "Ro":
            course.Add("r");
            result += "(root)";
            break;
        case "Sq":
            course.Add("^");
            result += "^2";
            break;
        case "Eq":
            PrintResult();
            break;
        default:
            break;
        }

        if(result != "")
            screen_main.text = result;

        input = "";
    }


    void PrintResult()  // 계산 결과를 반환 한다.
    {
        if (result != "" && course.Count == 2)
            return;
        
        output = 0;

        // Root ^2
        for(int i = 0; i < course.Count; i++)
        {
            if(course[i] == "r")
            {
                course[i - 1] = Math.Sqrt(double.Parse(course[i - 1])).ToString();  // 루트
                
                course.RemoveAt(i);
                i -= 1;
            }
            else if(course[i] == "^")
            {
                course[i - 1] = Math.Pow(double.Parse(course[i - 1]), 2).ToString();  // 제곱

                course.RemoveAt(i);
                i -= 1;
            }
        }

        // 연산 기호 앞, 뒤의 수를 기호에 맞게 계산 후, 현제 위치 list를 2번 삭제후, 연산 기호 앞에 계산 결과를 넣고, i를 1감소
        
        // * /
        for(int i = 0; i < course.Count - 1; i++)
        {
            if(course[i] == "*" || course[i] == "/")
            {
                if(course[i] == "*")
                    output = double.Parse(course[i - 1]) * double.Parse(course[i + 1]);
                else if(course[i] == "/")
                    output = double.Parse(course[i - 1]) / double.Parse(course[i + 1]);

                course.RemoveAt(i);
                course.RemoveAt(i);

                course[i - 1] = output.ToString();

                i -= 1;
            }
        }

        // + -
        for(int i = 0; i < course.Count - 1; i++)
        {
            if(course[i] == "+" || course[i] == "-")
            {
                if(course[i] == "+"){
                    output = double.Parse(course[i - 1]) + double.Parse(course[i + 1]);
                }
                else if(course[i] == "-")
                    output = double.Parse(course[i - 1]) - double.Parse(course[i + 1]);

                course.RemoveAt(i);
                course.RemoveAt(i);
                
                course[i - 1] = output.ToString();

                i -= 1;
            }
        }

        // 기호 먼저 입력한 list의 count : 1, 숫자 먼저 입력한 list의 count : 2
        if(course.Count < 2)
            output = double.Parse(course[0]);
        else
            output = double.Parse(course[1]);
            
        // list 초기화
        course.Clear();
        course.Add("0");
        result += "=";
        result += output.ToString();
        screen_sub.text = result;  // 이전 계산 과정 및 결과 기록
        screen_main.text = result;
        result = "";
        return;
    }

    // 소수점 .
    void FindDot()
    {
        string dot = "";
        int i = course.Count - 2;

        dot = course[i - 1] + "." + course[i + 1];

        course.RemoveAt(i);
        course.RemoveAt(i);

        course[i - 1] = dot;

        i -= 1;
    }

    // CE or AC 입력
    void GetEtc(string click)
    {
        switch(click.Substring(0, 2))
        {
        case "CE":
            ClearEnter();
            break;
        case "AC":
            AllClearEnter();
            break;
        default:
            break;
        }
    }

    // CE
    void ClearEnter()
    {
        // 지워야 할 부분이 기호인 경우
        if (input == "")
        {
            // 마지막 연산 기호가 root : 루트
            if (course[course.Count - 1] == "r")
                result = result.Substring(0, result.Length - 6);  // 6(root) -> 6
            // 마지막 연산 기호가 ^2 : 제곱
            else if (course[course.Count - 1] == "^")
                result = result.Substring(0, result.Length - 2);
            // 마지막 연산 기호가 root, ^2 외인 경우
            else
                result = result.Substring(0, result.Length - 1);
                
            // 마지막 연산 기호 이전의 숫자
            if (course[course.Count - 2] != "r" || course[course.Count - 2] != "^")
                input = course[course.Count - 2];

            course.RemoveAt(course.Count - 1);
            if(result == "")
                screen_main.text = course[0];
            else
                screen_main.text = result;
        }
        // 지워야 할 부분이 숫자인 경우
        else
        {
            input = input.Substring(0, input.Length - 1);
            result = result.Substring(0, result.Length - 1);
            
            if(result == "")
                screen_main.text = course[0];
            else
                screen_main.text = result;
        }
    }

    // AC
    void AllClearEnter()
    {
        input = "";
        course.Clear();
        course.Add("0");
        result = "";
        screen_main.text = "0";
        screen_sub.text = "0";
        output = 0;
    }


}
