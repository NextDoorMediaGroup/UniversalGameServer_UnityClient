using System;
using ndmg.UniversalGameServer;
using UnityEngine;


public class Example : MonoBehaviour
{
    public UniversalGameServerClient cl;

    private void Start()
    {
        cl.On("chat", handle);
    }


    private void Update()
    {
        cl.Send("chat", "Hi how are you");
    }

    public static void handle(string data)
    {
        Debug.Log(data);
    }
}