﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class AIState : ScriptableObject
{
    public AIAction[] Actions;
    public AITransition[] Transitions;

    public void EvaluateState(AIStateController controller)
    {
        DoActions(controller);
        EvaluateTransitions(controller);
    }

    public void DoActions(AIStateController controller)
    {
        foreach (var action in Actions)
        {
            action.Act(controller);
        }
    }


    public void EvaluateTransitions(AIStateController controller)
    {
        if (Transitions != null || Transitions.Length > 1)
        {
            for (int i = 0; i < Transitions.Length; i++)
            {
                bool decisionResult = Transitions[i].Decision.Decide(controller);

                if (decisionResult)
                    controller.TransitionToState(Transitions[i].TrueState);
                else
                    controller.TransitionToState(Transitions[i].FalseState);
            }
        }
    }
}
