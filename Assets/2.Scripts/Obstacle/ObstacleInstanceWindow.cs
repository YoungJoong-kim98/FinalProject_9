#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObstacleInstanceWindow : EditorWindow
{
    private Vector2 _scroll;

    // ① 타입-단 Foldout 상태      ② 인스턴스-단 Foldout 상태
    private readonly Dictionary<System.Type, bool> _typeFoldouts = new();
    private readonly Dictionary<Object, bool> _instFoldouts = new();
    private readonly Dictionary<Object, Editor> _editorCache = new();

    [MenuItem("Tools/Obstacle Instance Editor %#o")]
    private static void Open() => GetWindow<ObstacleInstanceWindow>("Obstacle Instance");

    private void OnGUI()
    {
        // IObstacle 구현체만 찾기
        var obstacles = Resources.FindObjectsOfTypeAll<MonoBehaviour>()
                                 .Where(o => o is IObstacle && o.gameObject.scene.IsValid());

        // ────────────────────────── 타입별 그룹
        var grouped = obstacles.GroupBy(o => o.GetType())
                               .OrderBy(g => g.Key.Name);

        using var sv = new EditorGUILayout.ScrollViewScope(_scroll);
        _scroll = sv.scrollPosition;

        foreach (var g in grouped)
        {
            // ─── 1단계: 타입 Foldout ───
            if (!_typeFoldouts.TryGetValue(g.Key, out var open))
                _typeFoldouts[g.Key] = open = true;          // 기본 펼침

            _typeFoldouts[g.Key] = EditorGUILayout.Foldout(
                open, $"▶ {g.Key.Name}  ({g.Count()})", true, EditorStyles.foldoutHeader);

            if (!_typeFoldouts[g.Key]) continue;

            EditorGUI.indentLevel++;
            foreach (var inst in g.OrderBy(o => o.name))
            {
                DrawInstanceFoldout(inst);
                EditorGUILayout.Space(2);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(6);
        }
    }

    // ────────────────────────── 2단계: 인스턴스 Foldout
    private void DrawInstanceFoldout(Object target)
    {
        if (!_instFoldouts.TryGetValue(target, out var open))
            _instFoldouts[target] = open = false;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        _instFoldouts[target] = EditorGUILayout.Foldout(open, target.name, true);

        if (_instFoldouts[target])
        {
            EditorGUI.indentLevel++;
            DrawInspector(target);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawInspector(Object target)
    {
        if (!_editorCache.TryGetValue(target, out var ed) || ed == null)
        {
            ed = Editor.CreateEditor(target);
            _editorCache[target] = ed;
        }
        ed.OnInspectorGUI();
    }

    private void OnDisable()
    {
        foreach (var ed in _editorCache.Values) DestroyImmediate(ed);
        _editorCache.Clear();
        _instFoldouts.Clear();
        _typeFoldouts.Clear();
    }
}
#endif
