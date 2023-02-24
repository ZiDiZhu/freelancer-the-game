using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUtility : MonoBehaviour
{

    public void ToggleTargetObjectIsActive(GameObject targetObject)
    {
        targetObject.SetActive(!targetObject.activeSelf);
    }

}
