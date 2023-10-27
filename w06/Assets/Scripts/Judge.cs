using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class Judge : MonoBehaviour{
    public Controll mainController;
    public Boat boat;
    public Bank left_bank;
    public Bank right_bank;
    public MySceneActionManager actionManager;
    void Start()
    {
        mainController = (Controll)SSDirector.GetInstance().CurrentScenceController;
        this.boat = mainController.boat;
        this.left_bank = mainController.left_bank;
        this.right_bank = mainController.right_bank;
        this.actionManager = mainController.actionManager;
    }

    public int check(){
        if(left_bank.GetPriestSum() == 3 && left_bank.GetDevilSum() == 3)return 1;
        if(boat.Get_boat_bank() == -1){
            if((left_bank.GetDevilSum() + boat.GetDevilSum() > left_bank.GetPriestSum() + boat.GetPriestSum()) && (left_bank.GetPriestSum() + boat.GetPriestSum()) != 0 && actionManager.Moving > 0)return 2;
            else if((left_bank.GetDevilSum() + boat.GetDevilSum() > left_bank.GetPriestSum() + boat.GetPriestSum()) && (left_bank.GetPriestSum() + boat.GetPriestSum()) != 0 && actionManager.Moving == 0)return 3;
            if(right_bank.GetDevilSum() > right_bank.GetPriestSum() && right_bank.GetPriestSum() != 0 && actionManager.Moving > 0) return 2;
            else if(right_bank.GetDevilSum() > right_bank.GetPriestSum() && right_bank.GetPriestSum() != 0 && actionManager.Moving == 0) return 3;
        }
        else if(boat.Get_boat_bank() == 1){
            if((right_bank.GetDevilSum() + boat.GetDevilSum() > right_bank.GetPriestSum() + boat.GetPriestSum()) && (right_bank.GetPriestSum() + boat.GetPriestSum()) != 0 && actionManager.Moving > 0)return 2;
            else if((right_bank.GetDevilSum() + boat.GetDevilSum() > right_bank.GetPriestSum() + boat.GetPriestSum()) && (right_bank.GetPriestSum() + boat.GetPriestSum()) != 0 && actionManager.Moving == 0)return 3;
            if(left_bank.GetDevilSum() > left_bank.GetPriestSum() && left_bank.GetPriestSum() != 0 && actionManager.Moving > 0) return 2;
            else if(left_bank.GetDevilSum() > left_bank.GetPriestSum() && left_bank.GetPriestSum() != 0 && actionManager.Moving == 0) return 3;
        }
        return 0;
    }

}