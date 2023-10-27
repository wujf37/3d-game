using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    Judge judge;
    public int check = 0;
    void Start()
    {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        judge = gameObject.AddComponent<Judge>() as Judge;
    }

    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.fontSize = 40;
        GUIStyle fontStyle2 = new GUIStyle();
        fontStyle2.fontSize = 20;
        fontStyle2.normal.textColor= new Color(1, 1, 1);
        GUI.Label(new Rect(Screen.width / 2 - 300, 10, 420, 50), "红色为恶魔，白色为牧师。", fontStyle2);
        GUI.Label(new Rect(Screen.width / 2 - 300, 40, 420, 50), "游戏目标：让牧师活着到对岸！", fontStyle2);
        if(GUI.Button (new Rect (Screen.width-100, Screen.height-100, 65, 65), "Exit")){
            Application.Quit();
        }
        if(check == 1){    //success
            GUI.Label(new Rect(Screen.width / 2 - 50, 150, 60, 40), "You Win!", fontStyle);
            if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height-200, 70, 40), "Restart")){
                action.Restart();
                check = 0;
            }
        }
        else if(check == 2){   //fail
            GUI.Label(new Rect(Screen.width / 2 - 50, 150, 60, 40), "Game Over!", fontStyle);
            //if(action.check() == 3){
            if(judge.check() == 3){
                if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height-200, 70, 40), "Restart")){
                    action.Restart();
                    check = 0;
                }
            }
        }
    }
}
