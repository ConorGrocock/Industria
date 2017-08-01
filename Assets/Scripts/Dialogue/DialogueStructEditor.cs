using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueStruct))]
public class DialogueStructEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueStruct diaStruct = (DialogueStruct)target;

        diaStruct.responseType = (DialogueResponseType)EditorGUILayout.Popup("Response Type", (int)diaStruct.responseType, Enum.GetNames(typeof(DialogueResponseType)));

        if (diaStruct.responseType == DialogueResponseType.YES_NO)
        {
            EditorGUILayout.LabelField("Response Trees", EditorStyles.boldLabel);
            diaStruct.yesResponseTree = (DialogueTrigger)EditorGUILayout.ObjectField("Yes Reponse Trigger", diaStruct.yesResponseTree, typeof(DialogueTrigger), true);
            diaStruct.noResponseTree = (DialogueTrigger)EditorGUILayout.ObjectField("No Reponse Trigger", diaStruct.noResponseTree, typeof(DialogueTrigger), true);
        }

        EditorGUILayout.LabelField("Sentence", EditorStyles.boldLabel);

        diaStruct.sentence = EditorGUILayout.TextArea(diaStruct.sentence);
    }
}
