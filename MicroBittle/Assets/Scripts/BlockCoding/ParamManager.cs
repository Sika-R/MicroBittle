using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamManager : MonoBehaviour
{
    public enum Obstacle
    {
        rock,
        mouse,
        waterfall,
        spiderweb,
        wall
    }
    private static ParamManager _instance;
    public static ParamManager Instance { get { return _instance; } }
    Dictionary<Obstacle, List<float>> allParams = new Dictionary<Obstacle, List<float>>();
    Dictionary<int, Obstacle> pinToObstacle = new Dictionary<int, Obstacle>();
    Dictionary<FunctionType, Obstacle> functionToObstacle = new Dictionary<FunctionType, Obstacle>();
    public List<ParamController> allControllers = new List<ParamController>();
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<float> GetParam(Obstacle o)
    {
        if(allParams.ContainsKey(o))
        {
            return allParams[o];
        }
        return null;
    }
    public List<float> GetParamByPin(int i)
    {
        if(pinToObstacle.ContainsKey(i))
        {
            if(allParams.ContainsKey(pinToObstacle[i]))
            {
                return allParams[pinToObstacle[i]];
            }
        }
        return null;
    }

    public List<float> GetParamByFunction(FunctionType f)
    {
        if(functionToObstacle.ContainsKey(f))
        {
            if(allParams.ContainsKey(functionToObstacle[f]))
            {
                return allParams[functionToObstacle[f]];
            }
        }
        return null;
    }


    

    public void SetParam(Obstacle o, List<float> l)
    {
        allParams[o] = l;
    }

    public void SetPin(int i, Obstacle o)
    {
        pinToObstacle[i] = o;
        PinValueCheck();
        // return pinToObstacle[i].Count != 1;
    }

    public ObstacleType GetObstacleByPin(int i)
    {
        if(!pinToObstacle.ContainsKey(i))
        {
            return ObstacleType.None;
        }
        ParamManager.Obstacle o = pinToObstacle[i];
        if(o == ParamManager.Obstacle.mouse)
        {
            return ObstacleType.Light;
        }
        if (o == ParamManager.Obstacle.spiderweb)
        {
            return ObstacleType.Vacuum;
        }
        if (o == ParamManager.Obstacle.wall)
        {
            return ObstacleType.Slider;
        }
        if (o == ParamManager.Obstacle.rock)
        {
            return ObstacleType.Knob;
        }
        return ObstacleType.None;
    }

    private bool PinValueCheck()
    {
        pinToObstacle.Clear();
        bool result = true;
        for (int i = 0; i < allControllers.Count; i++)
        {
            allControllers[i].ChangePinColor(Color.white);
        }
        for (int i = 0; i < allControllers.Count; i++)
        {
            bool hasConflicted = false;
            for (int j = i + 1; j < allControllers.Count; j++)
            {
                if(allControllers[i].pinNum == allControllers[j].pinNum)
                {
                    hasConflicted = true;
                    allControllers[i].ChangePinColor(Color.red);
                    allControllers[j].ChangePinColor(Color.red);
                    result = false;
                }
            }
            if(!hasConflicted)
            {
                pinToObstacle[allControllers[i].pinNum] = allControllers[i].obstacle;
            }
        }
        return result;
    }

    public bool paramValidationCheck(ParamManager.Obstacle o)
    {
        foreach(ParamController c in allControllers)
        {
            if(c.obstacle == o)
            {
                return c.isAllInputValid();
            }
        }
        return false;
    }
    
    public bool allParamValidationCheck()
    {
        foreach (ParamController c in allControllers)
        {
            if(!c.isAllInputValid())
            {
                return false;
            }
        }
        return true;
    }

    public void SetFunction(FunctionType f, Obstacle o)
    {
        functionToObstacle[f] = o;
    }


    void SetParamForWiringTest()
    {
        foreach(var type in functionToObstacle.Keys)
        {
            List<float> param = GetParamByFunction(type);
            if(type == FunctionType.jackhammer)
            {
                programUI.Instance.setValueforSlider(type, param[0], param[1]);

            }
            else if(type == FunctionType.headlamp)
            {
                programUI.Instance.setValueforSlider(type, param[0], param[5]);
            }
            /*else if(type == FunctionType.divinggear)
            {
                programUI.Instance.setValueforSlider(type, 0, param[1]);
            }*/
        }
    }

    public int GetObstacleCnt()
    {
        return allParams.Keys.Count;
    }

    public void AddController(ParamController ctrl)
    {
        allControllers.Add(ctrl);
    }
}
