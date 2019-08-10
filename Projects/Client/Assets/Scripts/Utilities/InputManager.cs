using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public delegate void KeyPressedDelegate();
    public delegate void MousePressedDelegate(Vector3 Position);

    private class InputMap : Dictionary<KeyCode, KeyPressedDelegate>
    { }

    private class MouseInputMap : Dictionary<KeyCode, MousePressedDelegate>
    { }

    private InputMap inputMap = new InputMap();
    private MouseInputMap mouseInputMap = new MouseInputMap();
    private Dictionary<KeyCode, KeyPressedDelegate>.Enumerator kepadMap;
    private Dictionary<KeyCode, MousePressedDelegate>.Enumerator mouseMap;

    public static InputManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        kepadMap = inputMap.GetEnumerator();
        while (kepadMap.MoveNext())
        {
            var current = kepadMap.Current;
            if (Input.GetKeyDown(current.Key))
                current.Value.Invoke();
        }

        mouseMap = mouseInputMap.GetEnumerator();
        while (mouseMap.MoveNext())
        {
            var current = mouseMap.Current;
            if (Input.GetKeyDown(current.Key))
                current.Value(Input.mousePosition);
        }
    }


    public void AddInput(KeyCode KeyCode, KeyPressedDelegate Action)
    {
        if (inputMap.ContainsKey(KeyCode))
            inputMap[KeyCode] += Action;
        else
            inputMap.Add(KeyCode, Action);
    }

    public void AddInput(KeyCode KeyCode, MousePressedDelegate Action)
    {
        if (mouseInputMap.ContainsKey(KeyCode))
            mouseInputMap[KeyCode] += Action;
        else
            mouseInputMap.Add(KeyCode, Action);
    }

    public void RemoveInput(KeyCode KeyCode, KeyPressedDelegate Action)
    {
        if (!inputMap.ContainsKey(KeyCode) || Action == null)
            return;

        inputMap[KeyCode] -= Action;
    }

    public void RemoveInput(KeyCode KeyCode, MousePressedDelegate Action)
    {
        if (!mouseInputMap.ContainsKey(KeyCode) || Action == null)
            return;

        mouseInputMap[KeyCode] -= Action;
    }

    public void ClearMap()
    {
        kepadMap = inputMap.GetEnumerator();
        while (kepadMap.MoveNext())
        {
            var current = kepadMap.Current;

            Delegate[] d = current.Value.GetInvocationList();
            for (int i = 0; i < d.Length; ++i)
                d[i] = null;

        }

        mouseMap = mouseInputMap.GetEnumerator();
        while (mouseMap.MoveNext())
        {
            var current = mouseMap.Current;
            Delegate[] d = current.Value.GetInvocationList();
            for (int i = 0; i < d.Length; ++i)
                d[i] = null;

        }
        inputMap.Clear();
        mouseInputMap.Clear();
    }

}
