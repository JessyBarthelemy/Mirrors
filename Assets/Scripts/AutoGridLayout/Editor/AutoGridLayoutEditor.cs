using UnityEditor.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[CustomEditor(typeof(AutoGridLayout),false)]
[CanEditMultipleObjects]
public class AutoGridLayoutGroupEditor : Editor
{
    SerializedProperty m_Padding;
    SerializedProperty m_Spacing;
    SerializedProperty m_StartCorner;
    SerializedProperty m_StartAxis;
    SerializedProperty m_ChildAlignment;
    SerializedProperty m_ColumnCount;
    SerializedProperty m_Scale;

    protected virtual void OnEnable()
    {
        m_Padding = serializedObject.FindProperty("m_Padding");
        m_Spacing = serializedObject.FindProperty("m_Spacing");
        m_StartCorner = serializedObject.FindProperty("m_StartCorner");
        m_StartAxis = serializedObject.FindProperty("m_StartAxis");
        m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
        m_ColumnCount = serializedObject.FindProperty("m_ColumnCount");
        m_Scale = serializedObject.FindProperty("m_Scale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Padding, true);
        EditorGUILayout.PropertyField(m_Spacing, true);
        EditorGUILayout.PropertyField(m_StartCorner, true);
        EditorGUILayout.PropertyField(m_StartAxis, true);
        EditorGUILayout.PropertyField(m_ChildAlignment, true);
        EditorGUILayout.PropertyField(m_ColumnCount, true);
        EditorGUILayout.PropertyField(m_Scale, true);
        serializedObject.ApplyModifiedProperties();
    }
}