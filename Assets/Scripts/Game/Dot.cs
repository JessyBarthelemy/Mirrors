using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dot : MonoBehaviour, IPointerDownHandler
{
    public const char STATE_INACTIVE = '0';
    public const char STATE_START = '1';
    public const char STATE_NORMAL = '2';
    public const char STATE_GOAL = '3';
    public const char STATE_GOAL_INV = '4';
    public const char STATE_END = '5';
    public const char STATE_END_INV = '6';
    public const char STATE_INV = '7';
    public const char STATE_TELEPORT = '8';

    private Button button;
    public Image image;
    public char State;

    public bool IsPicked;
    public bool IsTop;
    private Animator animator;

    public int Id;
    public int Order;
    private DotsRenderer dotsRenderer;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();
    }

    public void Init(DotsRenderer dotsRenderer, int id, char dotState, bool isTop)
    {
        this.dotsRenderer = dotsRenderer;
        this.IsTop = isTop;
        State = dotState;
        this.Id = id;

        if(State == STATE_INV)
            animator.runtimeAnimatorController = Resources.Load("Animations/change_color" + dotsRenderer.Daltonian) as RuntimeAnimatorController;

        if (State == STATE_TELEPORT)
            animator.runtimeAnimatorController = Resources.Load("Animations/teleport") as RuntimeAnimatorController;

        UpdateUI(false);
    }

    public void UpdateUI(bool isInversed)
    {
        if (State != STATE_INACTIVE)
        {
            image.enabled = true;
            button.enabled = true;

            string target = null;

            if (IsPicked)
            {
                if (State == STATE_TELEPORT)
                    animator.runtimeAnimatorController = Resources.Load("Animations/Dot" + dotsRenderer.Daltonian) as RuntimeAnimatorController;

                //top
                if ((IsTop && !isInversed) || (!IsTop && isInversed))
                    target = State == STATE_NORMAL? "dot_top_path" : "dot_top_normal";
                else
                    target = State == STATE_NORMAL ? "dot_bottom_path" : "dot_bottom_normal";
            }
            else
            {
                switch (State)
                {
                    case STATE_START:
                        target = IsTop ? "dot_top_normal" : "dot_bottom_normal";
                        break;
                    case STATE_GOAL:
                        target = IsTop ? "dot_top_normal" : "dot_bottom_normal";
                        break;
                    case STATE_GOAL_INV:
                        target = IsTop ? "dot_bottom_normal" : "dot_top_normal";
                        break;
                    case STATE_END:
                        target = IsTop ? "dot_top_end" : "dot_bottom_end";
                        break;
                    case STATE_END_INV:
                        target = IsTop ? "dot_bottom_end" : "dot_top_end";
                        break;
                    case STATE_TELEPORT:
                        target = IsTop ? "dot_top_normal" : "dot_bottom_normal";
                        break;
                    default:
                        target = "dot_inactive";
                        break;
                }
            }
           
            image.sprite = Resources.Load<Sprite>("Images/Game/" + target + dotsRenderer.Daltonian);
        }
        else
        {
            image.enabled = false;
            button.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dotsRenderer != null)
            dotsRenderer.OnDotClick(this);
    }

    public void SetAnimation(DotAnimation anim){
        SetAnimation(anim, false);
    }

    public void SetAnimation(DotAnimation anim, bool updateAnimator)
    {
        if (State == STATE_TELEPORT)
            animator.runtimeAnimatorController = Resources.Load("Animations/Dot" + dotsRenderer.Daltonian) as RuntimeAnimatorController;

        switch (anim)
        {
            case DotAnimation.Selected:
                animator.SetBool("Selected", true);
                break;
            default:
                if (State == STATE_TELEPORT)
                    animator.runtimeAnimatorController = Resources.Load("Animations/teleport") as RuntimeAnimatorController;
                else
                    animator.SetBool("Selected", false);
                break;
        }
    }
}
