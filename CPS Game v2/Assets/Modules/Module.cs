using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Abstract class with implementation that is common to all modules.  Unless the virtual functions are overriden for custom functionality,
/// the modules will pass water through them iff their corresponding pump is on and they have the capacity to take in more water.
/// </summary>
public abstract class Module : MonoBehaviour
{
    public GameObject popupPrefab;
    public GameObject AttackedIndicator;

    public Module PreviousModule; //must be added in
    public List<Module> NextModule = new List<Module>(); //must be added in

    public Pump InFlowingPump;

    public bool Attacked = false;

    public WaterObject Water;

    public List<WaterObject> WaterList = new List<WaterObject>();
    [SerializeField]

    //BY DEFAULT, ALL MODULES HAVE 1 CAPACITY

    private int capacity;
    public virtual int Capacity
    {
        get { return capacity; }
        set { capacity = value; }
    }

    // public virtual int WaterAmount
    // {
    //     get { return WaterList.Count; }
    // }

    protected List<string> displayFields;
    private GameObject popupInstance;
    private Text displayTextTitle;
    private Text displayTextContent;

    protected Dropdown[] AttackDropdowns;

    
    public bool HasFlow {
        get {
            return this.Water != null;
        }
    }
    private GameObject attackedIndicatorInstance;
    private Canvas rootCanvas;
    public GameObject WaterIndicator;

    //-------TEMPORARY------
     void Update()
    {
    }

    protected void Start()
    {
        UIManager.current.onHideWaterIndicatorTrigger += onHideWaterIndicator;
        UIManager.current.onShowWaterIndicatorTrigger += onShowWaterIndicator;
        WaterFlowController.current.listenercounttest ++;

    } 

    //UIwaterIndicator event system
    private void onHideWaterIndicator()
    {
        WaterIndicator.SetActive(false);        
    }
    private void onShowWaterIndicator()
    {        
        if(!HasFlow)
            WaterIndicator.SetActive(true);        
    }

    private void OnDestroy()
    {        
        UIManager.current.onHideWaterIndicatorTrigger -= onHideWaterIndicator;
        UIManager.current.onShowWaterIndicatorTrigger -= onShowWaterIndicator;
    }
    //-----------------

    public virtual bool IsFilter()
    {
        return false;
    }

    public virtual bool IsPump()
    {
        return false;
    }

    //notes- not sure if displayTextTitle is used anywhere else
    /// <summary>
    /// makes fields
    /// finds canvas, instantiates 
    /// </summary>
    private void Awake() {
        //temporary for debug purposes

        WaterIndicator = Instantiate(WaterIndicator, this.WaterIndicator.transform.position, this.WaterIndicator.transform.rotation);
        WaterIndicator.transform.SetParent(this.gameObject.transform);
        WaterIndicator.transform.position = transform.position;
        /* 
        //This is attached to update populdisplay
        this.displayFields = new List<string>
        {
            "Attacked",
            "Capacity",
            "HasFlow",
            "WaterAmount"
        };

        Capacity = 1;*/
        rootCanvas = (Canvas)FindObjectOfType(typeof(Canvas));
	}


    /// <summary>
    /// Moves water through system if specified pump is on. Then calls Tick for previous module.
    /// ----------obselete-------
    /// </summary>
    public virtual void Tick()
    {

        if (this.InFlowingPump.On)
        {
            this.OnFlow();
        }

        /*if (Water.Amount > this.Capacity)
        {
            this.OnOverflow();
        }*/

        //this.UpdatePopupDisplay();
        Debug.Log("I AM: "+gameObject.name);
        if (this.PreviousModule)
        {
            
            Debug.Log("Calling : "+PreviousModule.name);
            this.PreviousModule.Tick();
        }
    }

    /// <summary>
    /// Recursive. Water flows. Stops if current systems is blocked (override this depending on type)
    /// </summary>   
    public virtual void WaterFlow()
    {
        //only flows if water is available from previous and not blocked

        bool blocked= false; //make this public later
        if(PreviousModule.Water != null && !blocked) 
        { 
            //take water from previous then moved to new modules
            this.Water = PreviousModule.Water;           
            if(NextModule != null)
            {
                foreach(Module next in NextModule)
                {
                    next.WaterFlow();
                }
            }

        }
    }

  


    /// <summary>
    /// Only called when the pump is on.  Brings as much water as it can from the previous module into this one.
    /// Override for custom functionality in modules!
    /// </summary>
    protected virtual void OnFlow()
    {
        //previous module has to exist
        if (this.PreviousModule && this.Water == null)
        {
            //basically just moves the water into this module
            this.Water = this.PreviousModule.Water;
            this.PreviousModule.Water = null;
        }
    }

    /// <summary>
    /// Override to specify how the module behaves when fill exceeds capacity.  (Can only occur if OnFlow is overritten)
    /// </summary>
    protected virtual void OnOverflow()
    {

    }

    /// <summary>
    /// What to do when the attacker attacks the module
    /// </summary>
    public virtual void Attack()
    {
        this.Attacked = !this.Attacked;
        WaterFlowController.current.SimulateWater();
        
        //resimulate the water

    }

    /// <summary>
    /// what to do when the module is no longer being attacked
    /// </summary>
    public void Fix()
    {
        if (this.Attacked)
        {
            this.Attacked = false;
            this.attackedIndicatorInstance.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// Adds back an attack and fixes attacked module.
    /// </summary>
    public void ReverseAttack()
    {
        if (this.Attacked)
        {
            GameController.current.NumAvailableAttacks++;
            this.Fix();
        }
    }

    /// <summary>
    /// Defines interaction with mouse
    /// </summary>
    private void OnMouseOver()
    {
        if(GameController.current.GameState == GameState.AttackerTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("ATTACKED: "+ gameObject.name);
                this.Attack();            }

        }
    }

    /// <summary>
    /// True if the lhs module appears earlier in the system than the rhs
    /// </summary>
    /// <param name="lhs">first module to compare</param>
    /// <param name="rhs">second module to compare</param>
    /// <returns></returns>
    public static bool operator <(Module lhs, Module rhs)
    {
        Module currMod = rhs.PreviousModule;
        while (currMod)
        {
            if (currMod == lhs)
            {
                return true;
            }

            currMod = currMod.PreviousModule;
        }

        return false;
    }

    /// <summary>
    /// True if the lhs module appears later in the system than the rhs
    /// </summary>
    /// <param name="lhs">first module to compare</param>
    /// <param name="rhs">second module to compare</param>
    /// <returns></returns>
    public static bool operator >(Module lhs, Module rhs)
    {
        return (!(lhs < rhs) && lhs != rhs);
    }
}