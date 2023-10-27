using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public interface ISceneController
    {
        void LoadResources();
    }
    public interface IUserAction
    {
        void MoveRole(Role role);
        void MoveBoat();
        void Restart();
        //int check();
    }
    public class SSDirector : System.Object
    {
        private static SSDirector _instance;
        public ISceneController CurrentScenceController { get; set; }
        public static SSDirector GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }
    }

    public class Role
    {
        GameObject role;
        int on_bank;    //right 1,left -1, on_boat 2
        //Move move;
        Click click;
        string RoleType;
        bool IsMove = false;
        public Role(string name){
            if(name == "Priest"){
                role = Object.Instantiate(Resources.Load("Priest", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
            }
            else if(name == "Devil"){
                role = Object.Instantiate(Resources.Load("Devil", typeof(GameObject)), Vector3.zero, Quaternion.Euler(0, -90, 0)) as GameObject;
            }
            click = role.AddComponent(typeof(Click)) as Click;
            //move = role.AddComponent(typeof(Move)) as Move;
            on_bank = 1;
            click.SetRole(this);
            //move.SetRole(this);
            RoleType = name;
        }

        public void SetPosition(Vector3 pos){
            role.transform.position = pos;
        }

        public void SetName(string name){
            role.name = name;
        }
        public string GetName(){return role.name;}
        public string GetRoleName(){return RoleType;}
        public int GetOnBank(){return on_bank;}
        public void SetOnBank(int ob){on_bank = ob;}
        public Vector3 GetRolePos(){return role.transform.position;}
        public bool GetIsMove(){return IsMove;}
        public void ChangeIsMove(){
            IsMove = !IsMove;
        }
        public void Setcon(){
            //move.Setcon();
        }
        public void Moving(){
            IsMove = true;
        }
        public void NotMoving(){
            IsMove = false;
        }
        public GameObject getGameObject() { return role; } //
    }

    public class Bank
    {
        GameObject bank;
        int flag;  //1,right  -1,left
        Role[] roles = new Role[6];
        Vector3[] pos;

        public Bank(string bankname){
            pos = new Vector3[] {new Vector3(6.5f,1.5f,0), new Vector3(7,1.5f,0), new Vector3(7.5f,1.5f,0),
                new Vector3(8,1.5f,0), new Vector3(8.5f,1.5f,0), new Vector3(9,1.5f,0)};
            if(bankname == "right"){
                flag = 1;
                bank = Object.Instantiate(Resources.Load("Bank", typeof(GameObject)), new Vector3(8, 0, 0), Quaternion.identity) as GameObject;
            }
            else if(bankname == "left"){
                flag = -1;
                bank = Object.Instantiate(Resources.Load("Bank", typeof(GameObject)), new Vector3(-8, 0, 0), Quaternion.identity) as GameObject;
            }
        }

        public void AddRole(Role role){
            for(int i=0;i<6;++i){
                if(roles[i] == null){
                    roles[i] = role;
                    break;
                }
            }
        }

        public void DeleteRole(Role role){
            for(int i=0;i<6;++i){
                if(roles[i] == null) continue;
                if(roles[i].GetName() == role.GetName()){
                    roles[i] = null;
                    break;
                }
            }
        }

        public void reset(){
            for(int i=0;i<6;++i){
                roles[i] = null;
            }
        }

        public Vector3 GetPos(){
            int i = 0;
            for(i=0;i<6;++i){
                if(roles[i] == null){
                    break;
                }
            }
            Vector3 p = pos[i];
            p.x = flag * p.x;
            return p;
        }

        public int GetDevilSum(){
            int sum = 0;
            for(int i=0;i<6;++i){
                if(roles[i] != null && roles[i].GetRoleName() == "Devil")++sum;
            }
            return sum;
        }
        public int GetPriestSum(){
            int sum = 0;
            for(int i=0;i<6;++i){
                if(roles[i] != null && roles[i].GetRoleName() == "Priest")++sum;
            }
            return sum;
        }
    }

    public class Boat
    {
        GameObject boat;
        //Move move;
        Click click;
        int boat_bank; //1 right -1 left
        public Role[] roles = new Role[2];
        bool IsMove = false;
        public GameObject getGameObject() { return boat; }
        public Boat()
        {
            boat = Object.Instantiate(Resources.Load("Boat", typeof(GameObject)), new Vector3(4, 1, 0), Quaternion.identity) as GameObject;
            //move = boat.AddComponent(typeof(Move)) as Move;
            click = boat.AddComponent(typeof(Click)) as Click;
            click.SetBoat(this);
            //move.SetBoat(this);
            boat_bank = 1;
        }

        public Vector3 BoatMoveToPosition()                   //动作分离版改变
        {
            boat_bank = -boat_bank;
            if (boat_bank == -1)
            {
                return new Vector3(-4, 1, 0);  
            }
            else
            {
                return new Vector3(4, 1, 0);
            }          
        }

        public Vector3 GetBoatPosition(){return boat.transform.position;}
        public int Get_boat_bank(){return boat_bank;}
        public int SetRoles(Role role){
            if(roles[0] == null){
                roles[0] = role;
                return 0;
            }
            else if(roles[1] == null){
                roles[1] = role;
                return 1;
            }
            else return -1;
        }
        public void DeleteRole(Role role){
            if(roles[0] != null && roles[0].GetName() == role.GetName()) roles[0] = null;
            else if(roles[1] != null && roles[1].GetName() == role.GetName()) roles[1] = null;
        }
        public bool IsEmpty(){
            if(roles[0] == null && roles[1] == null) return true;
            else return false;
        }
        public int GetPriestSum(){
            int sum = 0;
            for(int i=0;i<2;++i){
                if(roles[i] != null && roles[i].GetRoleName() == "Priest")++sum;
            }
            return sum;
        }
        public int GetDevilSum(){
            int sum = 0;
            for(int i=0;i<2;++i){
                if(roles[i] != null && roles[i].GetRoleName() == "Devil")++sum;
            }
            return sum;
        }
        public void reset(){
            boat_bank = 1;
            boat.transform.position = new Vector3(4, 1, 0);
            for(int i=0;i<2;++i){
                roles[i] = null;
            }
            if(IsMove) IsMove = !IsMove;
            //move.Setcon();
        }
        public bool GetIsMove(){
            return IsMove;
        }
        public void ChangeIsMove(){
            IsMove = !IsMove;
        }
        public void ChangeRoleIsMove(){
            if(roles[0] != null)roles[0].ChangeIsMove();
            if(roles[1] != null)roles[1].ChangeIsMove();
        }
        public bool RoleISGetBoat(){
            if(roles[0] != null && roles[0].GetIsMove() == true)return false;
            if(roles[1] != null && roles[1].GetIsMove() == true)return false;
            return true;
        }
    }


    public class Click : MonoBehaviour
    {
        Judge judge;
        IUserAction action;
        Role role = null;
        Boat boat = null;
        public void SetRole(Role role)
        {
            this.role = role;
        }
        public void SetBoat(Boat boat)
        {
            this.boat = boat;
        }
        void Start()
        {
            action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
            judge = gameObject.AddComponent<Judge>() as Judge;
        }
        void OnMouseDown()
        {
            //if(action.check() != 0)return;
            if(judge.check() != 0)return;
            
            
            if (boat == null && role == null) return;
            //if (boat != null && !boat.RoleISGetBoat()) return;//
            /*if (boat != null && boat.GetIsMove() == false && !boat.IsEmpty()){
                boat.ChangeIsMove();                            //修改船的移动状态
                action.MoveBoat();                              //开船
            }*/
            if (boat != null && !boat.IsEmpty()){
                boat.ChangeIsMove();                            //修改船的移动状态
                action.MoveBoat();                              //开船
            }
            else if (role != null){
                role.Moving();                                 //表示角色正在移动
                action.MoveRole(role);                         //移动角色
            }
            else return;
        }
    }
}
