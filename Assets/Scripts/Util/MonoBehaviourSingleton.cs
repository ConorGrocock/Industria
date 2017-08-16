using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MonoBehaviourSingleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
{
    private static TSelfType m_Instance = null;
    public static TSelfType _instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (TSelfType)FindObjectOfType(typeof(TSelfType));
                if (m_Instance == null)
                    m_Instance = (new GameObject(typeof(TSelfType).Name)).AddComponent<TSelfType>();
                //DontDestroyOnLoad(m_Instance.gameObject);
            }

            return m_Instance;
        }
    }
}
