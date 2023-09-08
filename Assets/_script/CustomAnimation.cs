using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public enum AnimationMode
{
    PlayOnHover,
    Loop,
    PlayOnce
}

public enum MovementType
{
    Rotate,
    Bounce,
    Translate
}

public class CustomAnimation : MonoBehaviour
{
    public AnimationMode animationMode = AnimationMode.PlayOnHover; // Select the default animation mode in the Inspector
    public MovementType movementType = MovementType.Bounce; // Select the default movement type in the Inspector

    // Parameters for various animations
    public float bounceHeight = 20.0f; // Bounce height
    public float bounceDuration = 1.0f; // Bounce duration
    public Ease bounceEase = Ease.OutBounce;
    public Vector3 translateEndPosition = new Vector3(0, 0, 0); // End position for translation
    public float translateDuration = 1.0f;
    public RotateMode rotateMode = RotateMode.FastBeyond360; // Rotation mode
    public Vector3 rotationAngles = new Vector3(0, 0, 90); // Rotation angles

    private RectTransform rectTransform;
    private bool isPlaying = false; // Boolean to track if the animation is currently playing

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        switch (animationMode)
        {
            case AnimationMode.PlayOnHover:
                // Attach a hover event listener
                AddHoverListener();
                break;
            case AnimationMode.Loop:
                AnimateLoop();
                break;
            case AnimationMode.PlayOnce:
                AnimatePlayOnce();
                break;
        }
    }

    private void AddHoverListener()
    {
        // Attach hover event listeners to start and stop the animation
        var eventTrigger = gameObject.AddComponent<EventTrigger>();

        // Add PointerEnter event to start the animation
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        pointerEnter.callback.AddListener((data) => { AnimateHover(); });
        eventTrigger.triggers.Add(pointerEnter);

        // Add PointerExit event to stop the animation
        EventTrigger.Entry pointerExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        pointerExit.callback.AddListener((data) => { StopAnimation(); });
        eventTrigger.triggers.Add(pointerExit);
    }

    private void AnimateLoop()
    {
        switch (movementType)
        {
            case MovementType.Rotate:
                rectTransform.DORotate(rotationAngles, translateDuration, rotateMode)
                    .SetLoops(-1, LoopType.Restart); // Endlessly rotate
                break;
            case MovementType.Bounce:
                Vector3 endPosition = rectTransform.anchoredPosition + Vector2.up * bounceHeight;
                var bounceTween = rectTransform.DOAnchorPosY(endPosition.y, bounceDuration);
                bounceTween.SetEase(bounceEase);
                bounceTween.SetLoops(-1, LoopType.Yoyo); // Endlessly bounce
                break;
            case MovementType.Translate:
                var translateTween = rectTransform.DOAnchorPos(translateEndPosition, translateDuration);
                // Customize the translation animation here if needed
                break;
        }
    }

    private void AnimatePlayOnce()
    {
        switch (movementType)
        {
            case MovementType.Rotate:
                rectTransform.DORotate(rotationAngles, translateDuration, rotateMode)
                    .SetLoops(1); // Play the rotation animation once
                break;
            case MovementType.Bounce:
                Vector3 endPosition = rectTransform.anchoredPosition + Vector2.up * bounceHeight;
                var bounceTween = rectTransform.DOAnchorPosY(endPosition.y, bounceDuration);
                bounceTween.SetEase(bounceEase);
                // Play the bounce animation once
                break;
            case MovementType.Translate:
                var translateTween = rectTransform.DOAnchorPos(translateEndPosition, translateDuration);
                // Customize the translation animation here if needed
                break;
        }
    }

    private void AnimateHover()
    {
        switch (movementType)
        {
            case MovementType.Rotate:
                rectTransform.DORotate(rotationAngles, translateDuration, rotateMode)
                    .SetLoops(-1, LoopType.Restart); // Endlessly rotate
                break;
            case MovementType.Bounce:
                Vector3 endPosition = rectTransform.anchoredPosition + Vector2.up * bounceHeight;
                var bounceTween = rectTransform.DOAnchorPosY(endPosition.y, bounceDuration);
                bounceTween.SetEase(bounceEase);
                bounceTween.SetLoops(-1, LoopType.Yoyo); // Endlessly bounce
                break;
            case MovementType.Translate:
                var translateTween = rectTransform.DOAnchorPos(translateEndPosition, translateDuration);
                // Customize the translation animation here if needed
                break;
        }
    }

    private void StopAnimation()
    {
        // Stop the animation
        rectTransform.DOKill();
    }
}
