using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIInteraction : MonoBehaviour
{
    VisualElement root;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>("B1").clicked += () => Debug.Log("Testing");
        root.Q<Button>("B2").clicked += TestFunc;

    }

    void TestFunc()
    {
        Debug.Log("Testing function");
    }
}
