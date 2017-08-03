using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Upgrades))]
public class UpgradesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("To add upgrades to the manager, create scripts which extend the UpgradeScript class. They will add themselves automatically to the manager.", MessageType.Info);
    }
}
