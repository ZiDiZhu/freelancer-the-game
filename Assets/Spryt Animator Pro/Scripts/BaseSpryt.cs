using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(SpriteRenderer))]
public abstract class BaseSpryt : MonoBehaviour
{
    ///<summary>The speed at which the frames are cycled.</summary>
    public float speed = 1f;

    ///<summary>Whether the animation is paused or playing.
    ///<para>You can use the Pause / Resume / Toggle Methods to Set this Property.</para></summary>
    public bool isPaused = false;

    ///<summary>A set of Sine Data used to initialize all 5 Sine Effects on the Spryt.</summary>
    public SSineData sprytSineData;
    
//Properties
    ///<summary>The current index of the frames.
    ///<para>The actual frame being displayed is this value floor-rounded.</para></summary>
    public float Frame { get { return _frame; } set {
        //Ensure the assigned frame is within the currently available frames
            value = Mathf.Clamp(value, 0f, frames.Count - 1f);
            _frame = value;
        //Update the renderer
            myRenderer.sprite = frames[Mathf.FloorToInt(_frame)];
            } }
    private float _frame;

    ///<summary>The number of frames in the current Spryt Index.
    ///<para>Set automatically when a new Spryt is assigned to the instance.</para></summary>
    public int Count { get; protected set; }

    ///<summary>Controls whether the Renderer is enabled or not.
    ///<para>When this property is false, the Update method is not executed (the frame index and Sine Effects are not updated).</para></summary>
    public bool Visible { get { return _visible; } set { _visible = value; myRenderer.enabled = _visible; } }
    private bool _visible;

//Color and Alpha
    ///<summary>Controls the opacity of the Sprites between [0f: Transparent] to [1f: Opaque].
    ///<para>In contrast to Visible, setting the Alpha to 0f will still update the frame index.</para></summary>
    public float Alpha { get { return _alpha; } set { 
            _alpha = Mathf.Clamp(value, 0f, 1f);
            Color newAlphaColor = _color;
            newAlphaColor.a = _alpha;
            myRenderer.color = newAlphaColor;
        } }
    private float _alpha;

    ///<summary>Sets the Color on the Renderer.
    ///<para>This does not effect the Alpha of the Spryt; the Alpha Property must be set independently.</para></summary>
    public Color Color { get { return _color; } set { _color = value; myRenderer.color = new Color(_color.r, _color.g, _color.b, _alpha); } }
    private Color _color;

//Position
    ///<summary>Independent control of the x Position of the Spryt.</summary>
    public float x { get { return _x; } set { _x = value;
            transform.localPosition = new Vector2(_x, transform.localPosition.y);
            originPosition = new Vector2(_x, _y);
        } }
    private float _x;
    
    ///<summary>Independent control of the y Position of the Spryt.</summary>
    public float y { get { return _y; } set { _y = value;
            transform.localPosition = new Vector2(transform.localPosition.x, _y);
            originPosition = new Vector2(_x, _y);
        } }
    private float _y;

//Rotation and Scaling
    ///<summary>Changes the rotation of the Spryt between -180f and 180f.</summary>
    public float Angle { get { return _angle; } set {
        //Bound the value to between -180f to 180f
            while (value >= 180f - Mathf.Epsilon)
                value -= 360f;
            while (value < -180f + Mathf.Epsilon)
                value += 360f;
            _angle = value;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, value);
            originRotation = value;
        } }
    private float _angle;

    ///<summary>Independent control of the x Scale of the Spryt.</summary>
    public float xScale { get { return _xScale; } set { _xScale = value;
            transform.localScale = new Vector2(_xScale, transform.localScale.y);
            originScale = new Vector2(_xScale, _yScale);
        } }
    private float _xScale;
    
    ///<summary>Independent control of the y Scale of the Spryt.</summary>
    public float yScale { get { return _yScale; } set { _yScale = value;
            transform.localScale = new Vector2(transform.localScale.x, _yScale);
            originScale = new Vector2(_xScale, _yScale);
        } }
    private float _yScale;
//Events
    ///<summary>An Event which fires when the animation reaches the end of its frame count, forward or backwards.</summary>
    public event System.EventHandler<Spryt> OnAnimationEnd;

//Hidden public variables
    ///<summary>A reference to the Spryt's Rect Transform for use in manipulating its position if it exists in the context of a Canvas</summary>
    [HideInInspector] public RectTransform rect;

//Sine Effect Variables, these are all publicly accessible so you can hook into their events, but to easily manipulate them, there are a number of Helper Methods
    /// <summary>A Sine Effect which changes the Spryt's x Position.</summary>
    [HideInInspector] public Sine sineX;
    
    /// <summary>A Sine Effect which changes the Spryt's y Position.</summary>
    [HideInInspector] public Sine sineY;
    
    /// <summary>A Sine Effect which changes the Spryt's x Scale.</summary>
    [HideInInspector] public Sine sineScaleX;
    
    /// <summary>A Sine Effect which changes the Spryt's y Scale.</summary>
    [HideInInspector] public Sine sineScaleY;
    
    /// <summary>A Sine Effect which changes the Spryt's Rotation.</summary>
    [HideInInspector] public Sine sineAngle;

//Protected
    ///<summary>myRenderer is dynamically typed so that it can be used with EITHER UI Images OR Sprite Renderers</summary>
    protected dynamic myRenderer;
    
    ///<summary>The actual List of frames passed to the renderer</summary>
    protected List<Sprite> frames = new List<Sprite>();

    ///<summary>This is declared in BaseSpryt so it can be passed along with its Animation End event, but only MultiSpryt defines it, so for SprytSingle it will be null</summary>
    protected Spryt _index;

    ///<summary>Boolean set to determine if the Spryt is being used in a UI or on a Sprite Renderer</summary>
    protected bool isUI;

//Private
    ///<summary>The original local position offset of the Spryt</summary>
    private Vector2 originPosition;

    ///<summary>The original local scale of the Spryt</summary>
    private Vector2 originScale;

    ///<summary>The original local scale of the Spryt</summary>
    private float originRotation;

    ///<summary>If true, plays a Spryt's animation only once, then pauses it. Set by PlayOneShot Animation Method</summary>
    private bool playOneShot = false;

    ///<summary>If playOneShot is used, this controls whether the Spryt pauses on its last frame or loops back to its first.</summary>
    private bool pauseOnLastFrame = false;

//"Original" values as defined across various Components and used in Reset method.
    private bool oVisible;
    private float oSpeed;
    private float oAlpha;
    private Color oColor;
    private float oAngle;
    private float oX;
    private float oY;
    private float oxScale;
    private float oyScale;
    private float oScale;

    private void Awake() {
    //Unpack Sine Data in Awake so it is available for attempted Event Subscriptions
        if (sprytSineData == null) {
        //If no data has been supplied, new up a bunch of empty Sine instances
            sineX = new Sine(0f, 0f, 0f, Sine.Type.SineDuration);
            sineY = new Sine(0f, 0f, 0f, Sine.Type.SineDuration);
            sineScaleX = new Sine(0f, 0f, 0f, Sine.Type.SineDuration);
            sineScaleY = new Sine(0f, 0f, 0f, Sine.Type.SineDuration);
            sineAngle = new Sine(0f, 0f, 0f, Sine.Type.SineDuration);
        } else {
        //If data has been supplied, unpack it from the Scriptable Object, otherwise the data on the Scriptable Object will be changed!
            SineSetData(sprytSineData, false);
        }
    }
    public virtual void Start() {
    //Assign either a SpriteRenderer or an Image Component to myRenderer depending on which is found on the GameObject
        originPosition = transform.localPosition;
        originScale = transform.localScale;
        isUI = false;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            myRenderer = spriteRenderer;
        } else {
            Image image = GetComponent<Image>();
            if (image != null) {
                myRenderer = image;
                rect = GetComponent<RectTransform>();
                originPosition = rect.anchoredPosition;
                originScale = rect.localScale;
                isUI = true;
            } else {
        //Neither are found, display an error but add a SpriteRenderer as a fallback.
                Debug.LogError("Spryt requires either a Component of type SpriteRenderer or Image to operate! SpriteRenderer has been added as default.");
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                myRenderer = spriteRenderer;
            }
        }
    //Set original values
        Visible = myRenderer.enabled;
        oVisible = Visible;
        oSpeed = speed;
    //Set original values from Sprite Renderer & Transform
        oAlpha = myRenderer.color.a;
        _alpha = oAlpha;
        oColor = myRenderer.color;
        _color = oColor;
        oAngle = transform.rotation.eulerAngles.z;
        _angle = oAngle;
    //scaleOrigin is used for "relative transformation"
        oX = originPosition.x;
        _x = oX;
        oY = originPosition.y;
        _y = oY;
    //scaleOrigin is used for "relative transformation"
        oxScale = originScale.x;
        _xScale = oxScale;
        oyScale = originScale.y;
        _yScale = oyScale;
    }

    protected void Update() {
    //Only update the frame if the Spryt is Visible, is not paused, its frames are not null (and there is more than one frame), and the magnitude of its speed is not zero. 
        if (_visible && !isPaused && frames != null) {
            if (Mathf.Abs(speed) > Mathf.Epsilon && frames.Count > 1) {
                if (speed > 0) {
            //Animation speed is positive
                    if (_frame + speed + 0.01 < frames.Count) { //0.01 because floating point arithmatic is unreliable
                        _frame += speed;
                    } else {
                //Loop back to the beginning
                        OnAnimationEnd?.Invoke(this, _index);
                        if (playOneShot) {
                    //If this is a one-shot animation, freeze the frame
                            Pause();
                            if (!pauseOnLastFrame)
                                _frame = 0;
                        } else {
                    //Go back to the beginning of the animation
                            _frame = 0;
                        }
                    }
                } else {
            //Animation speed is Negative
                    if (_frame + speed > Mathf.Epsilon)
                        _frame += speed;
                    else {
                //Loop back to the end
                        OnAnimationEnd?.Invoke(this, _index);
                        if (playOneShot) {
                    //If this is a one-shot animation, freeze the frame
                            Pause();
                            if (!pauseOnLastFrame)
                                _frame = frames.Count + speed;
                        } else {
                    //Go back to the end of the animation
                            _frame = frames.Count + speed;
                        }
                    }
                }
            //Update the renderer
                myRenderer.sprite = frames[Mathf.FloorToInt(_frame)];
            }
        //Handle Sprite transformations
            float rX, rY, rXScale, rYScale, rAngle; //These variables could be declared in-line, but to ensure compatibility with Unity v2017x, they are not
            if (sineX.Update(out rX) | sineY.Update(out rY)) //Avoid using short-circuit evaluation to ensure both x / y update
                RelativePosition(rX, rY);
            if (sineScaleX.Update(out rXScale) | sineScaleY.Update(out rYScale)) //Avoid using short-circuit evaluation to ensure both x / y update
                RelativeScale(rXScale, rYScale);
            if (sineAngle.Update(out rAngle))
                RelativeRotate(rAngle);
        }
    }

//Animation Methods
    ///<summary>Pauses the animation, freezing the Spryt on the current frame.</summary>
    public void Pause() {
        isPaused = true;
    }

    /// <summary>Pauses the animation on a specified frame.</summary>
    /// <param name="_frame">The frame of the animation to pause on.</param>
    public void Pause(int _frame) { //Assign a specific frame and Pause
        Pause();
        this._frame = Mathf.FloorToInt(_frame);
        myRenderer.sprite = frames[Mathf.FloorToInt(this._frame)];
    }
    
    ///<summary>Resumes playing the animation and disables "playOneShot" behaviour.</summary>
    public void Resume() {
        isPaused = false;
        playOneShot = false;
    }
    
    ///<summary>Pauses the animation or Resumes it depending on its current state</summary>
    public void Toggle() {
        if (isPaused) {
            Resume();
        } else {
            Pause();
        }
    }

    ///<summary>Pause on the Last frame</summary>
    public void Last() { Pause(frames.Count - 1); }

    ///<summary>Pause on the First frame</summary>
    public void First() { Pause(0); }

    ///<summary>Inverts the animation speed.
    ///<para>If the animation is on the first or last frame, it jumps to the end or beginning.</para></summary>
    public void Reverse() {
        speed *= -1;
        if (_frame < Mathf.Epsilon) {
            _frame = frames.Count + speed;
        } else if (_frame + speed + 0.01 > frames.Count) { //0.01 because floating point arithmatic is unreliable
            _frame = 0;
        }
    }

    /// <summary>Plays the animation once through, then pauses it.
    /// <para>Automatically jumps to the first frame if speed is positive, or the last frame if speed is negative.</para>
    /// <para>Also automatically unpauses the animation.</para></summary>
    /// <param name="_pauseOnLastFrame">When the animation ends, should it stop on the last frame or cycle back to the beginning?</param>
    public void PlayOneShot(bool _pauseOnLastFrame) {
        isPaused = false;
        playOneShot = true;
        pauseOnLastFrame = _pauseOnLastFrame;
        if (speed < -Mathf.Epsilon)
            _frame = frames.Count + speed;
        else
            _frame = 0f;
    }

//Sine Effect Helper Methods
    public enum SineType {x, y, xScale, yScale, angle}

    /// <summary>Allows for independent activation of any of the 5 Sine effects on the Spryt.</summary>
    /// <param name="sineType">Which of the 5 Sine Effects to activate.</param>
    /// <param name="max">Set to true for Sine Effects which are meant to play in reverse.</param>
    private void ActivateSine(int sineType, bool max) {
        switch (sineType) {
            case 0: sineX.Activate(max); break;
            case 1: sineY.Activate(max); break;
            case 2: sineScaleX.Activate(max); break;
            case 3: sineScaleY.Activate(max); break;
            case 4: sineAngle.Activate(max); break;
        }
    }

    /// <summary>Allows for independent activation of any of the 5 Sine effects on the Spryt.</summary>
    /// <param name="sineType">Which of the 5 Sine Effects to activate.</param>
    /// <param name="max">Set to true for Sine Effects which are meant to play in reverse.</param>
    public void ActivateSine(SineType sineType, bool max) {
        ActivateSine((int)sineType, max);
    }
    
    /// <summary>A shortcut method to Activate only the x / y Position Sine Effects on the Spryt.</summary>
    public void ActivateSinePosition() {
        ActivateSine((int)SineType.x, false);
        ActivateSine((int)SineType.y, false);
    }

    /// <summary>A shortcut method to Activate only the x / y Scale Sine Effects on the Spryt.</summary>
    public void ActivateSineScale() {
        ActivateSine((int)SineType.xScale, false);
        ActivateSine((int)SineType.yScale, false);
    }

    /// <summary>A shortcut method to Activate only the rotation Sine Effect on the Spryt.</summary>
    public void ActivateSineRotation() {
        ActivateSine((int)SineType.angle, false);
    }

    /// <summary>A shortcut method to Activate all 5 Sine Effects on the Spryt.</summary>
    public void ActivateSineAll() {
        ActivateSine((int)SineType.xScale, false);
        ActivateSine((int)SineType.yScale, false);
        ActivateSine((int)SineType.x, false);
        ActivateSine((int)SineType.y, false);
        ActivateSine((int)SineType.angle, false);
    }

    /// <summary>Sets all 5 Sine Effect on the Spryt to a new set from the supplied Spryt Sine Data.</summary>
    /// <param name="ssData">A Spryt Sine Data Scriptable Object which has all the data to replace all 5 Sine Effects on the Spryt.</param>
    /// <param name="activateAll">Whether or not to immediately activate the newly supplied effects.</param>
    public void SineSetData(SSineData ssData, bool activateAll) {
//Extract all the data from the SprytSineData object, if this is not done, the Scriptable Object itself could be modified
        sineX = ConstructSine(ssData.xSine);
        sineY = ConstructSine(ssData.ySine);
        sineScaleX = ConstructSine(ssData.xScaleSine);
        sineScaleY = ConstructSine(ssData.yScaleSine);
        sineAngle = ConstructSine(ssData.angleSine);
        if (activateAll) {
            ActivateSineAll();
        }
    }

    /// <summary>Allows replacement of individual Sine Effects on the Spryt.</summary>
    /// <param name="sineData">A Sine Data Scriptable Object which has the data to replace 1 of the 5 Sine Effects on the Spryt.</param>
    /// <param name="sineType">Which of the 5 Sine Effects to update.</param>
    /// <param name="activate">Whether or not to immediately activate the newly supplied effect.</param>
    public void SineSetData(SineData sineData, SineType sineType, bool activate) {
        int _sineType = (int)sineType;
        switch (_sineType) {
            case 0: sineX.SetSineData(sineData); break;
            case 1: sineY.SetSineData(sineData); break;
            case 2: sineScaleX.SetSineData(sineData); break;
            case 3: sineScaleY.SetSineData(sineData); break;
            case 4: sineAngle.SetSineData(sineData); break;
        }
        if (activate) {
            ActivateSine(_sineType, false);
        }
    }

    /// <summary>Internal method used in SineSetData to construct new Sine instances to be applied to each of the 5 effects in Spryt.</summary>
    /// <param name="_sine">The sine data provided.</param>
    /// <returns>A newly constructed Sine instance made from the components of the supplied Sine data</returns>
    private Sine ConstructSine(Sine _sine) {
        return new Sine(_sine.effectActive, _sine.divisor, _sine.magnitude, _sine.duration, _sine.index, _sine.updateType, _sine.countDown);
    }

//Utility Methods
    /// <summary>A publicly accessible Method which can be used to assign a List of Sprites to the internal List used by Spryt.
    /// <para>This method is called each time a new Spryt is assigned to the Index of this instance.</para></summary>
    /// <param name="sprites">A List of type Sprite, in the order the frames should be shown.</param>
    public  void AssignFrames(List<Sprite> sprites) {
        frames.Clear(); //Clear the existing frames stored in the List
        Count = sprites.Count; //Set the Count property to match the new total of frames
        if (Count == 0) {
            Debug.LogError("Call to 'AssignFrames' with an Empty List");
        } else {
        //Iterate through the list of Sprites and add them to the internal list
            foreach (Sprite _frame in sprites) {
                frames.Add(_frame);
            }
        //Reset the frame index based on whether speed is positive or negative
            _frame = 0f;
            if (speed < -Mathf.Epsilon)
                _frame = frames.Count + speed;
        //Update the renderer
            myRenderer.sprite = frames[Mathf.FloorToInt(_frame)];
        }
    }

    /// <summary>Resets all properties of the Spryt to default values as defined on the GameObject in the Inspector.</summary>
    public virtual void ResetSprite() {
        Visible = oVisible;
        speed = oSpeed;
        Alpha = oAlpha;
        Color = oColor;
        Angle = oAngle;
        xScale = oxScale;
        yScale = oyScale;
        x = oX;
        y = oY;
        if (speed < -Mathf.Epsilon)
            _frame = frames.Count + speed;
        else
            _frame = 0f;
    }

    /// <summary>Allows compound assignment of both the x / y Scale Properties simultaneously.</summary>
    /// <param name="newScaleFloat">This single float will be applied to both the x / y Scale of the Spryt.</param>
    public void Scale(float newScaleFloat) {
        xScale = newScaleFloat;
        yScale = newScaleFloat;
    }

    /// <summary>Allows compound assignment of both the x / y Scale Properties simultaneously</summary>
    /// <param name="xS">The float to be applied to the Spryt's xScale</param>
    /// <param name="yS">The float to be applied to the Spryt's yScale</param>
    public void Scale(float xS, float yS) {
        xScale = xS;
        yScale = yS;
    }
    
    /// <summary>Allows compound assignment of both the x / y Scale Properties simultaneously</summary>
    /// <param name="newScaleVector">The x component of this Vector will be applied to the Spryt's xScale,
    /// <para>the y component of this Vector will be applied to the Spryt's yScale</para></param>
    public void Scale(Vector2 newScaleVector) {
        xScale = newScaleVector.x;
        yScale = newScaleVector.y;
    }

    /// <summary>Allows for scale changes to be performed on the Spryt without directly changing its X / Y Scale properties.
    /// <para>Especially useful for Sine Effect animations.</para></summary>
    /// <param name="rX">The relative x scale to apply to the Spryt.</param>
    /// <param name="rY">The relative y scale to apply to the Spryt.</param>
    public void RelativeScale(float rX, float rY) {
        transform.localScale = new Vector2(originScale.x + rX * Mathf.Sign(originScale.x), originScale.y + rY * Mathf.Sign(originScale.y));
    }

    /// <summary>Allows for position changes to be performed on the Spryt without directly changing its X / Y properties.
    /// <para>Especially useful for Sine Effect animations.</para></summary>
    /// <param name="rX">The relative x position to move the Spryt.</param>
    /// <param name="rY">The relative y position to move the Spryt.</param>
    public void RelativePosition(float rX, float rY) {
        if (isUI)
            rect.anchoredPosition = new Vector2(originPosition.x + rX * Mathf.Sign(originPosition.x), originPosition.y + rY * Mathf.Sign(originPosition.y));
        else
            transform.localPosition = new Vector2(originPosition.x + rX * Mathf.Sign(originPosition.x), originPosition.y + rY * Mathf.Sign(originPosition.y));
    }

    /// <summary>Allows for rotations to be performed on the Spryt without directly changing its Angle property.
    /// <para>Especially useful for Sine Effect animations.</para></summary>
    /// <param name="rZ">The relative z rotation to be applied to the Spryt.</param>
    public void RelativeRotate(float rZ) {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, originRotation + rZ);
    }

    /// <summary>Relative Position transformations are performed relative to a Spryt's origin.
    /// <para>This method lets you specify a new Vector2 origin for the Spryt.</para></summary>
    public void PositionOriginRecalibrate(Vector2 newOrigin) {
        originPosition = newOrigin;
        oX = originPosition.x;
        _x = oX;
        oY = originPosition.y;
        _y = oY;
    }

    /// <summary>Relative Position transformations are performed relative to a Spryt's origin.
    /// <para>This method lets you specify a new origin for the Spryt using x / y floats.</para></summary>
    public void PositionOriginRecalibrate(float originX, float originY) {
        PositionOriginRecalibrate(new Vector2(originX, originY));
    }

    /// <summary>Relative Position transformations are performed relative to a Spryt's origin.
    /// <para>This method treats the Spryt's current position as the new origin for the Spryt.</para></summary>
    public void PositionOriginRecalibrate() {
        if (isUI) {
            PositionOriginRecalibrate(rect.anchoredPosition);
        } else {
            PositionOriginRecalibrate(transform.localPosition);
        }
    }
    
    /// <summary>Returns string of the supplied scriptable object</summary>
    /// <param name="scriptableObject">A Scriptable Object</param>
    /// <returns>A string of the supplied scriptable object</returns>
    public static string GetName(ScriptableObject scriptableObject) {
        using (UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(scriptableObject)) {
            return serializedObject.FindProperty("m_Name").stringValue;
        }
    }
}