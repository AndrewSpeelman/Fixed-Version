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
    //----
    [SerializeField]
    private int id = 0;
    private static int idCounter = 0;
    //
    public GameObject popupPrefab;
    public GameObject AttackedIndicator;

    public Module PreviousModule; //must be added in
    public List<Module> NextModule = new List<Module>(); //must be added in

    public Pump InFlowingPump;

    public bool Attacked = false;

    public WaterObject Water;

    public List<WaterObject> WaterList = new List<WaterObject>();
    [SerializeField]
    protected bool canbeAttacked = true;

    //VISUALS SHOULD JUST BE VISUALS. NO GAMEPLAY ACTIONS
    //GAMEPLAY SHUOLD CONTINUE STILL IF VISUAL INDICATORS ARE BLANK
    public GameObject AttackerVisual;
    public GameObject DefenderVisual;

    protected Dropdown[] AttackDropdowns;
    // this just means it has water in it
    public bool HasFlow 
    {
        get
        {
            return this.Water != null;
        }
    }
    private Canvas rootCanvas;
    public GameObject WaterIndicator;
    [SerializeField]
    protected Vector3 visualOffset = new Vector3(0.0f, 1.3f, 0.0f);
    private Vector3 waterIndicatorOffset = new Vector3(0.0f, 1.2f, 0.0f);

    //-------TEMPORARY------
    void Update()
    {
    }

    #region setup
   private void Awake()
    {
        id=idCounter;
        idCounter++;
        
        rootCanvas = (Canvas)FindObjectOfType(typeof(Canvas));
        
        SetUpVariables();
        
    }

    private void Start()
    {
        
        SetUpVisuals();
        HandleWaterIndicator();
        HandleVisualIndicator();

    
        //Add Listeners// IMPORTANT--remember to destroy
        UIManager.current.onHideWaterIndicatorTrigger += onHideWaterIndicator;
        UIManager.current.onShowWaterIndicatorTrigger += onShowWaterIndicator;

        UIManager.current.onAttackerTurnTrigger += onAttackerTurn;
        UIManager.current.onDefenderTurnTrigger += onDefenderTurn;        
        
        WaterFlowController.current.listenercounttest++;

        
        AfterSetup();

    }

    

    //add setup variables here
    protected virtual void SetUpVariables()
    {
        //this is where the variables for each type of module gets placed.
        //override this
    }

    //this is setup for the visualization of the modules when a player interacts with it
    protected virtual void SetUpVisuals()
    {
         DefenderVisual= UIManager.current.DefendVisual_Generic;
         AttackerVisual= UIManager.current.AttackVisual_Generic;
    }

    
    #endregion
    public virtual bool IsFilter()
    {
        return false;
    }

    public virtual bool IsPump()
    {
        return false;
    }

    #region Indicators
    //UIwaterIndicator event system
    private void onHideWaterIndicator()
    {
        WaterIndicator.SetActive(false);
    }
    private void onShowWaterIndicator()
    {
        if (HasFlow)
            WaterIndicator.SetActive(true);
    }
    protected virtual void onAttackerTurn()
    {
    
        gameObject.BroadcastMessage("SwitchingTurn", AttackerVisual.name,SendMessageOptions.DontRequireReceiver);
    }
    protected virtual void onDefenderTurn()
    {

        gameObject.BroadcastMessage("SwitchingTurn", DefenderVisual.name,SendMessageOptions.DontRequireReceiver);
    }

    

  
    protected virtual void AfterSetup()
    {
        //Anything Additional Added on after initial setup
    }
    private void HandleWaterIndicator()
    {
        WaterIndicator = Instantiate(WaterIndicator, WaterIndicator.transform.position, WaterIndicator.transform.rotation);
        WaterIndicator.transform.SetParent(this.gameObject.transform);
        WaterIndicator.transform.position = transform.position + waterIndicatorOffset;
    }
    private void HandleVisualIndicator()
    {
        HandleAttackVisualIndicator();
        HandleDefendVisualIndicator();       

    }

    protected virtual void HandleAttackVisualIndicator()
    {        
        //AttackerIndicators    
        if (AttackerVisual == null)
            AttackerVisual = new GameObject();
        else
        {            
            AttackerVisual = Instantiate(AttackerVisual, AttackerVisual.transform.position,     AttackerVisual.transform.rotation);
        }
        AttackerVisual.name = "Attacker Visual";
        AttackerVisual.transform.SetParent(this.gameObject.transform);
        AttackerVisual.transform.position = transform.position + visualOffset;

    }
    protected virtual void HandleDefendVisualIndicator()
    {        
        //DefenderIndicators        
        if (DefenderVisual == null)
            DefenderVisual = new GameObject();
        else
        {            
            DefenderVisual = Instantiate(DefenderVisual, DefenderVisual.transform.position,     DefenderVisual.transform.rotation);
        }
        DefenderVisual.name = "Defender Visual";
        DefenderVisual.transform.SetParent(this.gameObject.transform);
        DefenderVisual.transform.position = transform.position + visualOffset;
    }

    
    private void OnDestroy()
    {
        UIManager.current.onHideWaterIndicatorTrigger -= onHideWaterIndicator;
        UIManager.current.onShowWaterIndicatorTrigger -= onShowWaterIndicator;

        
        UIManager.current.onAttackerTurnTrigger -= onAttackerTurn;
        UIManager.current.onDefenderTurnTrigger -= onDefenderTurn;
    }
    #endregion
    
    #region WaterActivation/Disable
    public virtual void WaterDisable()
    {        
        Water = null;       
        WaterIndicator.SetActive(false);
        gameObject.BroadcastMessage("SetWater",false, SendMessageOptions.DontRequireReceiver);
    }
    public virtual void WaterActivate()
    {
        
        WaterIndicator.SetActive(true);
        gameObject.BroadcastMessage("SetWater",true,SendMessageOptions.DontRequireReceiver);
    }
    #endregion
    /// <summary>
    /// Recursive. Water flows. Stops if current systems is blocked (override this depending on type)
    /// </summary>   
    public virtual void WaterFlow()
    {
        //only flows if water is available from previous and not blocked

        bool blocked = false; //make this public later
        if (PreviousModule.Water != null && !blocked)
        {
            //take water from previous then moved to new modules
            this.Water = PreviousModule.Water;
            if (NextModule != null)
            {
                foreach (Module next in NextModule)
                {
                    next.WaterFlow();
                }
            }

        }
    }


    #region PlayerActions --Anything to do with actions by any party on a module--
    
    /// <summary>
    /// What to do when the attacker attacks the module
    /// </summary>
    public virtual void Attack()
    {
        this.Attacked = !this.Attacked;
        //tally up number of attacks
        
        //resimulate water
        WaterFlowController.current.SimulateWater();


        //Applies to only gameobjects below this. Intended for the scripts
        // attached to the visual particles
        gameObject.BroadcastMessage("AttackedTrigger",SendMessageOptions.DontRequireReceiver);


    }


    /// <summary>
    /// What to do when defender interacts with this module
    /// </summary>
    protected virtual void DefenderAction()
    {
        //Default is nothing
    }

    /// <summary>
    /// What to do when attacker interacts with this module
    /// </summary>
    public virtual void AttackerAction()
    {
        Debug.Log("ATTACKED: " + gameObject.name);
        this.Attack();
    }

    /// <summary>
    /// Actions to take when mouse it hovered// replace later for taps
    /// </summary>
    public virtual void OnMouseOver()
    {
        if (GameController.current.GameState == GameState.AttackerTurn)
        {
            if (canbeAttacked && Input.GetMouseButtonDown(0))
            {
                AttackerAction();
            }

        }
        else if(GameController.current.GameState == GameState.DefenderTurn)
        {            
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click!");
                DefenderAction();
            }
        }
    }
    #endregion  

}