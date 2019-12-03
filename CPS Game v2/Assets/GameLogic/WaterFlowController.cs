using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the water flow between modules
/// </summary>
public class WaterFlowController : MonoBehaviour
{
    public static WaterFlowController current;
    public Reservoir Reservoir;

    [SerializeField]
    private Module firstModule;
    [SerializeField]
    private Module endModule;
    
    public List<Module> ModuleList = new List<Module>();
    public List<Module> initialModuleList = new List<Module>();
    [SerializeField]
    private bool listhasbeenCompiled = false;

    public int listenercounttest = 0;

    private int index=0;
    private void Start()
    {
    }

    /// <summary>
    /// Makes time move (tick) forward for the modules.  Ticking time forward allows for the water to flow through
    /// the system.
    /// </summary>
    
    private void Awake()
    {
        if(current == null)
        {
            current=this;

        }
        else
         Destroy(gameObject);
    }
    public void CompileModuleList()
    {
        //only flows if water is available from previous and not blocked
        ModuleList.Clear();
        ModuleList.Add(firstModule);
         if(!listhasbeenCompiled)
        {
            index=0;
            firstModule.name = index + "--" + firstModule.name;
        }
        foreach(Module next in firstModule.NextModule)
        {
            if(next.Attacked == false) //attacked modules do not let water through.
            {   
                Debug.Log("HELLO");      
                
                if(!listhasbeenCompiled)
                {                    
                    index++;                
                    next.name = index + "--" + next.name;
                }       
                AddNextModule(next);
            }
        }
    }

   
     
    private void AddNextModule(Module currentModule)
    {
        //only flows if water is available from previous and not blocked
        if(!(ModuleList.Contains(currentModule)))
        {
            index++;
            if(!listhasbeenCompiled)
                currentModule.name = index + "--" + currentModule.name; //fix later            
            ModuleList.Add(currentModule);
        }
        foreach(Module next in currentModule.NextModule)
        {
            if(next.Attacked == false) //attacked modules do not let water through.
            {
                AddNextModule(next);
            }
        }
    }

    //update later to be more efficient, bruteforce for now
     public void UpdateWater()
    {
        foreach(Module m in initialModuleList)
        {
            m.WaterDisable();
        }
        foreach(Module m in ModuleList)
        {            
            m.Water = new WaterObject(); //unsure what this is needed but keeping it for now
            if(GameController.current.GameState == GameState.AttackerTurn)            
                m.WaterActivate();
        }
        //update indicators
        //Debug.LogError("STOP");
        //there is a bug on this, doesn't hide or show properly because listeners have not been added all properly
        //UIManager.current.HideWaterIndicatorTrigger();
        //UIManager.current.ShowWaterIndicatorTrigger();
        
    }
    public void SimulateWater()
    {
        //asumes no diverging paths for now
        //clearwater
        CompileModuleList();
        if(listhasbeenCompiled ==false)
        {
            initialModuleList.AddRange(ModuleList);
            listhasbeenCompiled=true;
        }
        UpdateWater();


    }
}
