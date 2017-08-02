using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueStruct))]
public class DialogueStructEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueStruct diaStruct = (DialogueStruct)target;

        diaStruct.scriptToRun = (DialogueScript)EditorGUILayout.ObjectField("Script To Run", diaStruct.scriptToRun, typeof(DialogueScript), true);

        EditorGUILayout.HelpBox("You MUST create a script which extends the DialogueScript class. The currently supported methods are: OnStart(), OnFinish(), OnCharacterTyped()", MessageType.Info);

        diaStruct.secondsPerLetter = EditorGUILayout.FloatField("Seconds Per Letter", diaStruct.secondsPerLetter);

        diaStruct.responseType = (DialogueResponseType)EditorGUILayout.Popup("Response Type", (int)diaStruct.responseType, Enum.GetNames(typeof(DialogueResponseType)));

        if (diaStruct.responseType == DialogueResponseType.YES_NO)
        {
            EditorGUILayout.LabelField("Response Trees", EditorStyles.boldLabel);
            diaStruct.yesResponseTree = (DialogueTrigger)EditorGUILayout.ObjectField("Yes Response Trigger", diaStruct.yesResponseTree, typeof(DialogueTrigger), true);
            diaStruct.noResponseTree = (DialogueTrigger)EditorGUILayout.ObjectField("No Response Trigger", diaStruct.noResponseTree, typeof(DialogueTrigger), true);
        }

        EditorGUILayout.LabelField("Sentence", EditorStyles.boldLabel);

        diaStruct.sentence = EditorGUILayout.TextArea(diaStruct.sentence);
    }
}
