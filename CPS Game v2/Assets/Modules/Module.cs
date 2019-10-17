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

    private GameController gameController;

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
    public bool isHide=false;

    //-------TEMPORARY------
     void Update()
    {

    }

    protected void Start()
    {
        UIManager.current.onHideWaterIndicatorTrigger += onHideWaterIndicator;
        UIManager.current.onShowWaterIndicatorTrigger += onShowWaterIndicator;

        
        this.gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    } 

    //UIwaterIndicator event system
    private void onHideWaterIndicator()
    {
        WaterIndicator.SetActive(false);        
    }
    private void onShowWaterIndicator()
    {
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
        
        //This is attached to update populdisplay
        this.displayFields = new List<string>
        {
            "Attacked",
            "Capacity",
            "HasFlow",
            "Purity1",
            "Purity2",
            "Purity3",
            "WaterAmount"
        };

        Capacity = 1;
        rootCanvas = (Canvas)FindObjectOfType(typeof(Canvas));

        //Instantiate the popup that displays the display fields
        this.popupInstance = Instantiate (this.popupPrefab, this.popupPrefab.transform.position, this.popupPrefab.transform.rotation);
		this.popupInstance.transform.SetParent(this.rootCanvas.transform, false);
		var texts = this.popupInstance.GetComponentsInChildren<Text>();
        popupInstance.name = this.gameObject.name + "_popupInstance";
        //Debug.Log("Transform " + popupInstance.name + ": " + this.popupInstance.transform.position);


        if (texts.Length == 2) { //unsure if ref or value passed
			this.displayTextContent = texts[1];
			this.displayTextTitle = texts[0];
		}
		this.displayTextTitle.text = this.gameObject.name;

		//this.CloseInfoPopup();

        //initiate attacker instance at cursor // Camera.main.WorldToScreenPoint places the indicators correctly
        //https://docs.unity3d.com/ScriptReference/Camera.WorldToScreenPoint.html
        this.attackedIndicatorInstance = Instantiate(this.AttackedIndicator,
            this.AttackedIndicator.transform.position,
            this.AttackedIndicator.transform.rotation);
        attackedIndicatorInstance.name = this.gameObject.name + "_attackedIndicatorInstance";
        //Debug.Log("Transform "+ attackedIndicatorInstance.name + ": " + this.AttackedIndicator.transform.position);
        // weird y alignment. Don't know how it happened.

        //sets parents to first gameobject tag
        this.attackedIndicatorInstance.transform.SetParent(GameObject.FindGameObjectWithTag("AttackerIndicatorInstance").transform);
        this.attackedIndicatorInstance.SetActive(false);

        this.AttackDropdowns = this.attackedIndicatorInstance.GetComponentsInChildren<Dropdown>();
        var cancelAttackButton = this.attackedIndicatorInstance.GetComponentInChildren<Button>();
        if (cancelAttackButton)
        {
            //https://youtu.be/h9ye2lU4lhw Unity Memory Game Tutorial - 4 - Adding Listeners To Our Buttons - Memory Game In Unity
            cancelAttackButton.onClick.AddListener(delegate { this.ReverseAttack(); });
        }
	}


    /// <summary>
    /// Moves water through system if specified pump is on. Then calls Tick for previous module.
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
        //CURRENTLY WILL ASSUME LIMITS TO MAX CAPACITY

    }

    /// <summary>
    /// What to do when the attacker attacks the module
    /// </summary>
    public virtual void Attack()
    {
        this.Attacked = true;
        this.attackedIndicatorInstance.SetActive(true);

        RectTransform UITransform = this.attackedIndicatorInstance.GetComponent<RectTransform>();        
        UITransform.position = Camera.main.WorldToScreenPoint(this.transform.position);

        this.gameController.NumAvailableAttacks--;// maybe not put it here, have the attacks decrease only in the dropdown menu
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
            this.gameController.NumAvailableAttacks++;
            this.Fix();
        }
    }

    /// <summary>
    /// Defines interaction with mouse
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (this.gameController && this.gameController.GameState == GameState.AttackerTurn)
            {
                if (!this.Attacked && this.gameController.NumAvailableAttacks > 0)
                {
                    this.Attack();
                }
            }
        }
        /* 
        if (Input.GetMouseButtonDown(1))
        {
            if (this.popupInstance.activeSelf)
            {
                this.CloseInfoPopup();
            }
            else
            {
                this.OpenInfoPopup(Input.mousePosition);
            }
        }*/
    }

    /// <summary>
    /// Updates the popup display by getting the values of the fields and changing the popup text to display
    /// the current values of the fields
    /// </summary>
    /* 
    public void UpdatePopupDisplay() {
		var bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		var fields = new List<FieldInfo>();
        var props = new List<PropertyInfo>();
		foreach (string fieldName in displayFields) {
            var info = this.GetType().GetField(fieldName, bindings);
            if (info != null)
            {
                fields.Add(this.GetType().GetField(fieldName, bindings));
            }
            else
            {
                props.Add(this.GetType().GetProperty(fieldName, bindings));
            }
		}

		var displayStrings = new List<string>();
		foreach(FieldInfo field in fields) {
			displayStrings.Add(field.Name + ": " + field.GetValue(this));
		}
        foreach(PropertyInfo prop in props)
        {
            displayStrings.Add(prop.Name + ": " + prop.GetValue(this, null));
        }

		this.displayTextContent.text = string.Join("\n", displayStrings.ToArray());
	}
*/
	/// <summary>
	/// Opens the info popup at the given location
	/// </summary>
	/// <param name="position">The position to place the popup at.</param>
    /*/
	protected void OpenInfoPopup(Vector2 position) {
		this.CloseInfoPopup();
		this.UpdatePopupDisplay();
		RectTransform UITransform = this.popupInstance.GetComponent<RectTransform>();

		//UITransform.position = position + new Vector2((UITransform.rect.width / 2), (UITransform.rect.height / 2)); --old--
        UITransform.position = Camera.main.WorldToScreenPoint(this.transform.position);

        this.popupInstance.SetActive(true);
	}

    /// <summary>
    /// Closes the info popup
    /// </summary>
	protected void CloseInfoPopup() {
		this.popupInstance.SetActive(false);
	}
*/
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