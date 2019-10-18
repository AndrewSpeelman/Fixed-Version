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

    private GameObject AttackerVisualparent;
    private GameObject DefenderVisualparent;
    public GameObject AttackerVisual;
    public GameObject DefenderVisual;

    protected Dropdown[] AttackDropdowns;

    public bool HasFlow
    {
        get
        {
            return this.Water != null;
        }
    }

    public bool temp = false;
    private GameObject attackedIndicatorInstance;
    private Canvas rootCanvas;
    public GameObject WaterIndicator;
    [SerializeField]
    private Vector3 visualOffset = new Vector3(0.0f, 1.3f, 0.0f);
    private Vector3 waterIndicatorOffset = new Vector3(0.0f, 1.2f, 0.0f);

    //-------TEMPORARY------
    void Update()
    {
    }

    protected void Start()
    {
        UIManager.current.onHideWaterIndicatorTrigger += onHideWaterIndicator;
        UIManager.current.onShowWaterIndicatorTrigger += onShowWaterIndicator;

        UIManager.current.onAttackerTurnTrigger += onAttackerTurn;
        UIManager.current.onDefenderTurnTrigger += onDefenderTurn;

        WaterFlowController.current.listenercounttest++;

    }

    public virtual bool IsFilter()
    {
        return false;
    }

    public virtual bool IsPump()
    {
        return false;
    }

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
        AttackerVisualparent.SetActive(true);
        DefenderVisualparent.SetActive(false);
    }
    protected virtual void onDefenderTurn()
    {

        AttackerVisualparent.SetActive(false);
        DefenderVisualparent.SetActive(true);
    }

    private void OnDestroy()
    {
        UIManager.current.onHideWaterIndicatorTrigger -= onHideWaterIndicator;
        UIManager.current.onShowWaterIndicatorTrigger -= onShowWaterIndicator;
    }
    //-----------------

    //notes- not sure if displayTextTitle is used anywhere else
    /// <summary>
    /// makes fields
    /// finds canvas, instantiates 
    /// </summary>
    private void Awake()
    {
        id=idCounter;
        idCounter++;
        
        HandleWaterIndicator();
        HandleVisualIndicator();
        rootCanvas = (Canvas)FindObjectOfType(typeof(Canvas));
    }

    private void HandleWaterIndicator()
    {
        WaterIndicator = Instantiate(WaterIndicator, WaterIndicator.transform.position, WaterIndicator.transform.rotation);
        WaterIndicator.transform.SetParent(this.gameObject.transform);
        WaterIndicator.transform.position = transform.position + waterIndicatorOffset;
    }
    private void HandleVisualIndicator()
    {
        //AttackerIndicators
        if (AttackerVisualparent == null)
            AttackerVisualparent = new GameObject();

        AttackerVisualparent.name = "Attacker Visual";
        AttackerVisualparent.transform.SetParent(this.gameObject.transform);
        AttackerVisualparent.transform.position = transform.position + visualOffset;

        AttackerVisualparent.AddComponent(typeof(AttackVisual));

        if(AttackerVisual !=null)
        {
            AttackerVisual = Instantiate(AttackerVisual, AttackerVisual.transform.position,AttackerVisual.transform.rotation);
            
            AttackerVisual.transform.SetParent(AttackerVisualparent.transform);
            AttackerVisual.transform.position = AttackerVisualparent.transform.position;
        }

        //DefenderIndicators
        if (DefenderVisualparent == null)
            DefenderVisualparent = new GameObject();

        DefenderVisualparent = new GameObject();
        DefenderVisualparent.name = "Defender Visual";
        DefenderVisualparent.transform.SetParent(this.gameObject.transform);
         DefenderVisualparent.transform.position = transform.position + visualOffset;
        if(DefenderVisual != null)
        {
            DefenderVisual = Instantiate(DefenderVisual, DefenderVisual.transform.position,DefenderVisual.transform.rotation);            
            DefenderVisual.transform.SetParent(DefenderVisualparent.transform);
            DefenderVisual.transform.position = DefenderVisualparent.transform.position;
        }

    }


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




    /// <summary>
    /// What to do when the attacker attacks the module
    /// </summary>

    public virtual void Attack()
    {
        this.Attacked = !this.Attacked;
        WaterFlowController.current.SimulateWater();

        //Applies to only gameobjects below this. Intended for the scripts
        // attached to the visual particles
        gameObject.BroadcastMessage("AttackedTrigger");

        //resimulate the water

    }

    /// <summary>
    /// Defines interaction with mouse
    /// </summary>
    public virtual void OnMouseOver()
    {
        if (GameController.current.GameState == GameState.AttackerTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("ATTACKED: " + gameObject.name);
                this.Attack();
            }

        }
    }

}