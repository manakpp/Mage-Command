using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenPositionOffset))]
public class TweenPositionOffsetEditor : UITweenerEditor
{
	public override void OnInspectorGUI()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(120f);

		TweenPositionOffset tw = target as TweenPositionOffset;
		GUI.changed = false;

		Vector3 offset = EditorGUILayout.Vector3Field("From", tw.offset);

		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Tween Change", tw);
			tw.Offset = offset;
			NGUITools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
