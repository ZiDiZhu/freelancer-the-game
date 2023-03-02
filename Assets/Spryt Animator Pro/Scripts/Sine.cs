using UnityEngine;

[System.Serializable]
///<summary>Utility class used to create smooth transitions utilizing Sine curves.</summary>
public class Sine {
    ///<summary>The divisor which controls how large of small a "slice of Pi" which is added to or subtracted from "index".
    ///<para>A larger divisor results in a slower effect, a smaller divisor results in a faster effect.</para></summary>
    public float divisor = 60f;

    ///<summary>A multiplier on the value of index to alter its range from the default -1 to +1.</summary>
	public float magnitude = 1f;

    ///<summary>Measured in increments of Pi. A complete Sine curve has a duration of 2f while a quarter is 0.5f.
    ///<para>If the effect is supposed to have limited duration (instead of continuing forever) set this to a non-zero number.</para></summary>
	public float duration = 0f;
    
    public enum Type {SineDuration, SinMagnitude, Sine, CosDuration, CosMagnitude, Cos}

    ///<summary>If using the expedited SineUpdate method, this controls the operation passed on the ref float.</summary>
    public Type updateType = Type.SineDuration;

    ///<summary>A boolean to get or set whether the effect is currently playing.
    ///<para>Set this to true if you want an effect to play immediately.</para></summary>
    public bool effectActive = false;

    ///<summary>If using the Sine's Update method, this controls whether the index will count up to its maximum value or down to zero.</summary>
    public bool countDown = false;

    ///<summary>The actual value which tracks the progress of the effect.
    ///<para>If it has a concept of duration, it will either approach zero or its maximum value.</para>
    ///<para>If not, it will be bounded between a range of 0 to 2*Pi.</para></summary>
	public float index = 0f;

    ///<summary>An event fired when effects with limited duration elapse.</summary>
    public event System.EventHandler OnEffectEnd;

    /// <summary>An expedited constructor which accepts a SineData Scriptable Object.</summary>
    /// <param name="sineData">A Scriptable Object which holds all the data for an instance of the Sine Class.</param>
    public Sine(SineData sineData) {
        SetSineData(sineData);
    }

    /// <summary>The full Constructor which defines all fields for the Sine effect.</summary>
    /// <param name="effectActive">A boolean to get or set whether the effect is currently playing.</param>
    /// <param name="divisor">A larger divisor results in a slower effect, a smaller divisor results in a faster effect.</param>
    /// <param name="magnitude">A multiplier on the value of index to alter its range from the default -1 to +1.</param>
    /// <param name="duration">Measured in increments of Pi. A complete Sine curve has a duration of 2f while a quarter is 0.5f.</param>
    /// <param name="index">The actual value which tracks the progress of the effect.</param>
    /// <param name="updateType">If using the expedited SineUpdate method, this controls the operation passed on the ref float.</param>
    /// <param name="countDown">If using the SineUpdate method, this controls the index will count up to its maximum value or down to zero.</param>
    public Sine(bool effectActive, float divisor, float magnitude, float duration, float index, Type updateType, bool countDown) {
        this.divisor = divisor;
        this.magnitude = magnitude;
        this.duration = duration;
        this.index = index;
        this.effectActive = effectActive;
        this.updateType = updateType;
        this.countDown = countDown;
    }

    /// <summary>An expedited Constructor which sets index to 0f, effectActive to false, and countdown to false.</summary>
    /// <param name="divisor">A larger divisor results in a slower effect, a smaller divisor results in a faster effect.</param>
    /// <param name="magnitude">A multiplier on the value of index to alter its range from the default -1 to +1.</param>
    /// <param name="duration">Measured in increments of Pi. A complete Sine curve has a duration of 2f while a quarter is 0.5f.</param>
    /// <param name="updateType">If using the expedited SineUpdate method, this controls the operation passed on the ref float.</param>
    public Sine(float divisor, float magnitude, float duration, Type updateType) {
        this.divisor = divisor;
        this.magnitude = magnitude;
        this.duration = duration;
        index = 0f;
        effectActive = false;
        this.updateType = updateType;
        countDown = false;
    }

//Getters (these are the methods called based on updateType)
    ///<summary>Performs the operation Mathf.Sin on the current value of index.</summary>
    public float GetSine() {
        return Mathf.Sin(index);
    }

    ///<summary>Performs the operation Mathf.Cos on the current value of index.</summary>
    public float GetCos() {
        return Mathf.Cos(index);
    }

    ///<summary>Performs the operation Mathf.Sin on the current value of index multiplied by magnitude.</summary>
    public float GetSineMagnitude() {
        return Mathf.Sin(index) * magnitude;
    }

    ///<summary>Performs the operation Mathf.Cos on the current value of index multiplied by magnitude.</summary>
    public float GetCosMagnitude() {
        return Mathf.Cos(index) * magnitude;
    }

    ///<summary>Returns a value of the Sin of Index (multiplied by magnitude) relative to the overall duration.
    ///<para>As the effect nears completion, this value approaches zero.</para></summary>
    public float GetSineDuration() {
        return Mathf.Sin(index) * magnitude * (1f - (index / (duration * Mathf.PI * 2)));
    }

    ///<summary>Returns a value of the Cos of Index (multiplied by magnitude) relative to the overall duration.
    ///<para>As the effect nears completion, this value approaches zero.</para></summary>
    public float GetCosDuration() {
        return Mathf.Cos(index) * magnitude * (1f - (index / (duration * Mathf.PI * 2)));
    }

//Utility Methods
    /// <summary>A helper method which unpacks a SineData Scriptable Object and assigns it to the various fields of this class.</summary>
    /// <param name="sineData">A Scriptable Object which holds all the data for an instance of the Sine Class.</param>
    public void SetSineData(SineData sineData) {
        divisor = sineData.divisor;
        magnitude = sineData.magnitude;
        duration = sineData.duration;
        index = sineData.index;
        effectActive = sineData.effectActive;
        updateType = sineData.updateType;
        countDown = sineData.countDown;
    }
    
    /// <summary>Updates the floatRef parameter based on the operation specified in updateType.</summary>
    /// <param name="floatRef">A variable to be passed by reference which will only be set to 0f if the effectActive is not true.</param>
    /// <param name="updateType">Controls the Get Sine operation passed on the ref float.</param>
    /// <returns>Whether effectActive is true. Use this as a check if you only wish for something to occur if floatRef was changed by this method.</returns>
    public bool Update(out float floatRef, Type updateType) {
        if (Mathf.Abs(divisor) < Mathf.Epsilon) {
    //Deactivate the effect if the Divisor is zero since this will cause a Divide by Zero error
            effectActive = false;
        }
        floatRef = 0f; //If the effect is not active, floatRef is simply set to 0f
        if (effectActive) {
        //Increment / Decrement Index
            if (countDown) {
                Decrement();
            } else {
		        Increment();
            }
        //Prevent attempt to use Sine / Cos Duration if duration is 0
            if (duration < Mathf.Epsilon) {
                if (updateType == Type.SineDuration) {
                    updateType = Type.SinMagnitude;
                    Debug.LogError("Warning: Attempted to use SineDuration with duration set to a value of 0. Substituting SinMagnitude, but either duration should not be zero, or you should use SinMagnitude intentionally");
                } else if (updateType == Type.CosDuration) {
                    updateType = Type.CosMagnitude;
                    Debug.LogError("Warning: Attempted to use CosDuration with duration set to a value of 0. Substituting CosMagnitude, but either duration should not be zero, or you should use CosMagnitude intentionally");
                }
            }
        //Return a value based on the Sine type requested
            switch(updateType) {
                case Type.Sine: floatRef = GetSine(); break;
                case Type.Cos: floatRef = GetCos(); break;
                case Type.SinMagnitude: floatRef = GetSineMagnitude(); break;
                case Type.CosMagnitude: floatRef = GetCosMagnitude(); break;
                case Type.SineDuration: floatRef = GetSineDuration(); break;
                case Type.CosDuration: floatRef = GetCosDuration(); break;
            }
        }
        return effectActive;
    }

    /// <summary>Updates the floatRef parameter based on the updateType stored within the instance of the Sine Class.</summary>
    /// <param name="floatRef">A variable to be passed by reference which will only be set to 0f if the effectActive is not true.</param>
    /// <returns>Whether effectActive is true. Use this as a check if you only wish for something to occur if floatRef was changed by this method.</returns>
    public bool Update(out float floatRef) {
        return Update(out floatRef, updateType);
    }

    ///<summary>Increases index by the "slice of Pi" and handles the concept of duration.
    ///<para>With no set duration, keeps index bound between 0 and 2*Pi.</para></summary>
    public void Increment() {
        if (effectActive) {
		    index += Mathf.PI / divisor;
            if (duration > 0f) {
                if (index > duration * Mathf.PI * 2) {
                    effectActive = false;
                    index = duration * Mathf.PI * 2;
                    OnEffectEnd?.Invoke(this, System.EventArgs.Empty);
                }
            } else {
                if (index > Mathf.PI * 2) {
                    index -= Mathf.PI * 2;
                }
            }
        }
	}

    ///<summary>Decreases index by the "slice of Pi" and handles the concept of duration.
    ///<para>With no set duration, keeps index bound between 0 and 2*Pi.</para></summary>
	public void Decrement() {
        if (effectActive) {
		    index -= Mathf.PI / divisor;
            if (duration > 0f) {
                if (index < Mathf.PI / divisor) {
                    effectActive = false;
                    index = 0;
                    OnEffectEnd?.Invoke(this, System.EventArgs.Empty);
                }
            } else {
                if (index < Mathf.PI * 2) {
                    index += Mathf.PI * 2;
                }
            }
        }
	}

    /// <summary>A zero-parameter alternative to Activate which sets Index to zero.</summary>
    public void Activate() {
        Activate(false);
    }

    /// <summary>Sets index back to a value of zero and sets durationElapsed to false.</summary>
    /// <param name="max">Maximizes the value of index, useful when counting down instead of up.</param>
    public void Activate(bool max) {
        if (duration > 0f)
            index = max ? Mathf.PI * 2 * duration : 0f;
        else
            index = max ? Mathf.PI * 2 : 0f;
        effectActive = true;
    }

    ///<summary>Used to set value to anywhere between a range of 0 to 2*Pi
    ///<para>Automatically sets duration to 0.</para></summary>
    public void Randomize() {
        index = Random.Range(0f, Mathf.PI * 2);
        duration = 0f;
    }
}