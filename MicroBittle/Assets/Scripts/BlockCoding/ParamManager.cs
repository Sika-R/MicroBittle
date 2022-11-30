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
}
