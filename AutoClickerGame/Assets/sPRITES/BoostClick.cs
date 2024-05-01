using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoostClick : MonoBehaviour
{
    public Slider ProgressBar;
    public float Velocidad;

    private StateMultiplier currentState = StateMultiplier.None;

    public void Click()
    {
        ProgressBar.value += 1f;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            PointerUp();
        }
        UpdateBarProgress();
    }

    public void PointerUp()
    {
        ProgressBar.value -= Time.deltaTime * Velocidad;
    }

    public void UpdateBarProgress()
    {
        StateMultiplier newState = GetNewState();

        // Si el nuevo estado es diferente del estado actual, cambia el estado en AutoClicker.
        if (newState != currentState)
        {
            AutoClicker.instance.ChangeState(newState);
            currentState = newState;
        }
    }

    private StateMultiplier GetNewState()
    {
        if (ProgressBar.value >= 15f && AutoClicker.instance.BoostCinco)
        {
            return StateMultiplier.Cinco;
        }
        else if (ProgressBar.value >= 15f)
        {
            return StateMultiplier.Dos;
        }
        else
        {
            return StateMultiplier.None;
        }
    }
}

