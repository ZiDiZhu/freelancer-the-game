using UnityEngine;

///<summary>To be used on instances which will need to dynamically switch between sets of frames at runtime through script.
///<para>Spryts can be assigned to the Index Property of MultiSpryt directly to switch between sets of frames.</para>
///<para>For instances which do not need to switch between Spryts, SprytSingle can be used instead.</para></summary>
public class MultiSpryt : BaseSpryt
{
    ///<summary>The Inspector-accessible Spryt which is assigned on Start. It is otherwise unused.</summary>
    [SerializeField] private Spryt index;
    /// <summary>Index accepts a Spryt Scriptable Object which is simply a List of Sprites.
    /// <para>You can safely assign the same Spryt to Index multiple times and it will ignore subsequent attempts.</para></summary>
    public Spryt Index { get { return _index; } set { 
            if (_index != value) {
        //If the Spryt is already this Spryt, prevent assigning the same one
                _index = value;
                PerformAssignFrames();
            }
        } }

    public override void Start() {
        base.Start(); //Call the Start method in BaseSpryt to handle setting all private members
        _index = index; //Grab the index provided in the Inspector
        PerformAssignFrames(); //This will handle actually assigning the frames and ensuring that index is not null
    }

    /// <summary>Used when MultiSpryt is initialized and when Index is assigned to.
    /// <para>Ensures index is not null, and if MultiSpryt is being used in a Canvas, sets the Native size of the new Spryt.</para></summary>
    protected void PerformAssignFrames() {
        if (_index == null) {
            Debug.LogError("MultiSpryt requires Index to be assigned a value which is not null. Please ensure a Spryt has been assigned to Index in the Inspector for " + gameObject.name);
        } else {
            AssignFrames(_index.frames); //Assign the frames in the Spryt Scriptable Object to the internal list of Frames
            if (isUI)
                myRenderer.SetNativeSize(); //If this is a UI Object, set the size on the GameObject
        }
    }

    /// <summary>In a single method, assign a new Spryt, assign a new SprytSine Data and Activate it</summary>
    /// <param name="newSpryt">The new Spryt to assign</param>
    /// <param name="ssData">The new SprytSine Data to activate</param>
    public void Combo(Spryt newSpryt, SSineData ssData) {
        Index = newSpryt;
        SineSetData(ssData, true);
    }

    /// <summary>Returns a string of the current Sprite Index asset name.</summary>
    /// <returns>A string of the current Sprite Index asset name.</returns>
    public override string ToString() { return GetName(_index); }
}