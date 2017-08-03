using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueStruct))]
public class DialogueStructEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DialogueStruct diaStruct = (DialogueStruct)target;

        SerializedProperty diaStruScripts = serializedObject.FindProperty("scriptsToRun");

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(diaStruScripts, true);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        //diaStruct.scriptToRun = (DialogueScript)EditorGUILayout.ObjectField("Script To Run", diaStruct.scriptToRun, typeof(DialogueScript), true);

        EditorGUILayout.HelpBox("To be able to attach scripts, the scripts MUST extend the DialogueScript class.", MessageType.Info);

        diaStruct.secondsPerCharacter = EditorGUILayout.FloatField("Seconds Per Character", diaStruct.secondsPerCharacter);

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
