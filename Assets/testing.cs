using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class testing : MonoBehaviour
{
    [HideInInspector]
    public bool showVariable = false;

    [HideInInspector]
    public int variableToHide;

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.BeginVertical("Box");

        showVariable = EditorGUILayout.Toggle("Show Variable", showVariable);

        if (showVariable)
        {
            variableToHide = EditorGUILayout.IntField("Variable To Hide", variableToHide);
        }

        GUILayout.EndVertical();
    }
#endif
}

[CustomEditor(typeof(testing))]
public class ExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        testing _test = (testing)target;

        

        if (GUILayout.Button("Show Variable"))
        {
            _test.showVariable = !_test.showVariable;
            Debug.Log("Custom Button clicked!");
        }

        if (_test.showVariable)
        {
            EditorGUI.indentLevel++;
            _test.variableToHide = EditorGUILayout.IntField("Variable To Hide", _test.variableToHide);
            EditorGUI.indentLevel--;
        }


    }
}
