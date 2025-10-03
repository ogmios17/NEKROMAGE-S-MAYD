using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    StateNode current;
    Dictionary<Type, StateNode> nodes = new();
    HashSet<TransitionInterface> anyTransition = new();

    public void Update()
    {
        var transition = GetTransition();
        if(transition != null)
        {
            ChangeState(transition.to);
        }

        current.state?.Update();
    }

    public void FixedUpdate()
    {
        current.state?.FixedUpdate();
    }

    public void SetState(StateInterface state)
    {
        current = nodes[state.GetType()];
        current.state?.OnEnter();
    }

    void ChangeState(StateInterface state)
    {
        if (state == current.state) return;
        var previousState = current.state;
        var nextState = nodes[state.GetType()].state;
        previousState?.OnExit();
        nextState?.OnEnter();
        current = nodes[state.GetType()];
    }

    TransitionInterface GetTransition()
    {
        foreach(var transition in anyTransition) 
            if(transition.condition.Evaluate())
                return transition;
        foreach(var transition in current.transitions)
            if (transition.condition.Evaluate())
                return transition;
        return null;
    }

    public void AddTransition(StateInterface from, StateInterface to, Predicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).state, condition);
    }

    public void AddAnyTransition(StateInterface to, Predicate condition)
    {
        anyTransition.Add(new Transition(GetOrAddNode(to).state,condition));
    }

    StateNode GetOrAddNode(StateInterface state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());  
        
        if(node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }
}
