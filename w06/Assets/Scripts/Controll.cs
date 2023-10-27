using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
public class Controll : MonoBehaviour,ISceneController,IUserAction
{
    public Role[] roles;
    public Boat boat;
    public Bank left_bank;
    public Bank right_bank;

    UserGUI User;
    public MySceneActionManager actionManager;
    public Judge judge;

    void Start()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        User = gameObject.AddComponent<UserGUI>() as UserGUI;
        LoadResources();

        actionManager = gameObject.AddComponent<MySceneActionManager>() as MySceneActionManager;
        judge = gameObject.AddComponent<Judge>() as Judge;
    }

    public void LoadResources(){
        GameObject water = Instantiate(Resources.Load("Water", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        boat = new Boat();
        roles = new Role[6];
        left_bank = new Bank("left");
        right_bank = new Bank("right");
        for(int i=0;i<3;++i){
            Role role = new Role("Priest");
            role.SetName("Priest"+i);
            role.SetPosition(right_bank.GetPos());
            right_bank.AddRole(role);
            roles[i] = role;
        }
        for(int i=0;i<3;++i){
            Role role = new Role("Devil");
            role.SetName("Devil"+i);
            role.SetPosition(right_bank.GetPos());
            right_bank.AddRole(role);
            roles[3+i] = role;
        }
    }

    public void MoveBoat(){
        if(boat.IsEmpty())return;
        if(actionManager.Moving > 0)return;
        actionManager.move_boat(boat,boat.BoatMoveToPosition());
        User.check = judge.check();
    }

    public void MoveRole(Role role){
        
        if(role.GetOnBank() == -boat.Get_boat_bank()) return;
        if(actionManager.Moving > 0)return;
        
        Vector3 pos;
        if(role.GetOnBank() == 1 || role.GetOnBank() == -1){ //move role to boat
            int temp = 0;
            temp = boat.SetRoles(role);       //返回一个空位置，若没有则返回-1
            if(temp == -1)return;             //boat is full
            pos = boat.GetBoatPosition();
            if(temp == 1 && role.GetOnBank() == 1) pos.x += 1;
            if(temp == 0 && role.GetOnBank() == -1) pos.x -= 1;
            //role.RoleMove(pos);
            actionManager.move_role_to_boat(role.getGameObject(),pos);  //动作分离版本

            if(role.GetOnBank() == 1) right_bank.DeleteRole(role);
            else left_bank.DeleteRole(role);
            role.SetOnBank(2);
        }
        else if(role.GetOnBank() == 2){                      //move role to bank
            if(boat.Get_boat_bank() == 1){
                pos = right_bank.GetPos();
                role.SetOnBank(1);
                right_bank.AddRole(role);
                boat.DeleteRole(role);
                //role.RoleMove2(pos);
                actionManager.move_role_to_bank(role.getGameObject(),pos);//动作分离版本
            }
            else {
                pos = left_bank.GetPos();
                role.SetOnBank(-1);
                left_bank.AddRole(role);
                boat.DeleteRole(role);
                //role.RoleMove2(pos);
                actionManager.move_role_to_bank(role.getGameObject(),pos);//动作分离版本
            }
            User.check = judge.check();
        }
    }

    public void Restart(){
        actionManager.Moving = 0;
        left_bank.reset();
        right_bank.reset();
        for(int i=0;i<6;++i){
            roles[i].SetOnBank(1);
            roles[i].SetPosition(right_bank.GetPos());
            right_bank.AddRole(roles[i]);
            if(roles[i].GetIsMove() == true) roles[i].ChangeIsMove();
            roles[i].Setcon();
        }
        boat.reset();
        actionManager.Moving = 0;
    }
}
