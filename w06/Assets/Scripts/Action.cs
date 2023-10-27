using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class SSAction : ScriptableObject
{
    public bool enable = true;
    public bool destroy = false;
    public GameObject gameobject {get;set;}
    public Transform transform {get;set;}
    public ISSActionCallback callback {get;set;}
    protected SSAction() {}
    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}

public class CCMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            //waiting for destroy
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }

    public override void Start(){}
}

public class CCSequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1;
    public int start = 0;

    public static CCSequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update();
        }
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        source.destroy = false;
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0){this.destroy = true;this.callback.SSActionEvent(this);}
        }
    }

    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    void OnDestroy()
    {
        //TODO: something
    }
}

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, 
        string strParam = null, 
        Object objectParam = null);
}

public class SSActionManager : MonoBehaviour, ISSActionCallback                      //action管理器
{

    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    //将执行的动作的字典集合,int为key，SSAction为value
    private List<SSAction> waitingAdd = new List<SSAction>();                       //等待去执行的动作列表
    private List<int> waitingDelete = new List<int>();                              //等待删除的动作的key                
    public int Moving = 0;
    protected void Update()
    {
        foreach (SSAction ac in waitingAdd)
        {
            actions[ac.GetInstanceID()] = ac;                                      //获取动作实例的ID作为key
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }
        
        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
        
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
        
        Moving++;
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        Moving--;
    }
}

public class MySceneActionManager : SSActionManager
{

    private CCMoveToAction moveBoatToEndOrStart;
    private CCSequenceAction moveRoleToBank;
    private CCSequenceAction moveRoleToBoat;

    public Controll sceneController;

    protected new void Start()
    {
        sceneController = (Controll)SSDirector.GetInstance().CurrentScenceController;
        sceneController.actionManager = this;
    }
    public void moveBoat(GameObject boat, Vector3 target, float speed)
    {
        moveBoatToEndOrStart = CCMoveToAction.GetSSAction(target, speed);
        this.RunAction(boat, moveBoatToEndOrStart, this);
    }

    public void move_boat(Boat boat, Vector3 pos){
        moveBoatToEndOrStart = CCMoveToAction.GetSSAction(pos, 3);
        this.RunAction(boat.getGameObject(), moveBoatToEndOrStart, this);
        for(int i=0;i<2;++i){
            if(boat.roles[i] != null){
                Vector3 p = boat.roles[i].GetRolePos();
                move_role_to_bank(boat.roles[i].getGameObject(),new Vector3(-p.x + 2*i-1, p.y, p.z));
            }
        }
    }

    public void move_role_to_boat(GameObject role,Vector3 pos){
        Vector3 pos2 = new Vector3(role.transform.position.x, role.transform.position.y + 0.25f, role.transform.position.z);
        Vector3 pos1 = new Vector3(pos.x, pos2.y, role.transform.position.z);
        SSAction action1 = CCMoveToAction.GetSSAction(pos2, 3);
        SSAction action2 = CCMoveToAction.GetSSAction(pos1, 3);
        moveRoleToBoat = CCSequenceAction.GetSSAcition(1,0,new List<SSAction> {action1,action2});
        this.RunAction(role, moveRoleToBoat, this);
    }
    public void move_role_to_bank(GameObject role,Vector3 pos){
        SSAction action1 = CCMoveToAction.GetSSAction(pos, 3);
        moveRoleToBank = CCSequenceAction.GetSSAcition(1,0,new List<SSAction> {action1});
        this.RunAction(role, moveRoleToBank, this);
    }
}