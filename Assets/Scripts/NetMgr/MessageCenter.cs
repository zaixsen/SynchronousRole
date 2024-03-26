using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter<T> : Singleton<MessageCenter<T>>
{
    Dictionary<int, Action<T>> msgs_Dic = new Dictionary<int, Action<T>>();

    public void AddListener(int msgId, Action<T> action)
    {
        if (msgs_Dic.ContainsKey(msgId))
        {
            msgs_Dic[msgId] += action;
        }
        else
        {
            msgs_Dic.Add(msgId, action);
        }
    }
    public void RemoveListener(int msgId, Action<T> action)
    {
        if (msgs_Dic.ContainsKey(msgId))
        {
            msgs_Dic[msgId] -= action;
            if (msgs_Dic[msgId] == null)
            {
                msgs_Dic.Remove(msgId);
            }
        }
    }

    public void Dispatch(int msgId, T t)
    {
        if (msgs_Dic.ContainsKey(msgId))
        {
            msgs_Dic[msgId]?.Invoke(t);
        }
    }
}

