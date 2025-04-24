using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCaller : MonoBehaviour
{
    public PopEffect target;

    public void ShowObject()
    {
        target.Show();
    }

    public void HideObject()
    {
        target.Hide();
    }
}
