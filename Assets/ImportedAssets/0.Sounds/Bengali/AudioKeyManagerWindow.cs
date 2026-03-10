// using UnityEngine;
// using UnityEditor;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Text;
// using System.IO;

// public class AudioLocalizationState : ScriptableObject
// {
// public List<string> singleKeysNames = new List<string>();
// public List<string> singleKeysValues = new List<string>();


// public List<string> arrayFieldNames = new List<string>();
// public List<int> arrayIndex = new List<int>();
// public List<string> arrayValues = new List<string>();


// }

// public class AudioLocalizationBatchTool : EditorWindow
// {
// List<ScriptableObject> targetSOs = new List<ScriptableObject>();


// AudioLocalizationState state;

// Vector2 soScroll;
// Vector2 keyScroll;

// [MenuItem("Tools/Audio/Localization Batch Tool")]
// static void Open()
// {
//     GetWindow<AudioLocalizationBatchTool>("Audio Localization");
// }

// void OnEnable()
// {
//     if (state == null)
//         state = CreateInstance<AudioLocalizationState>();

//     Undo.undoRedoPerformed += Repaint;
// }

// void OnDisable()
// {
//     Undo.undoRedoPerformed -= Repaint;
// }

// void OnGUI()
// {
//     DrawSOSelectionPanel();

//     if (targetSOs.Count == 0)
//         return;

//     EditorGUILayout.Space(10);

//     keyScroll = EditorGUILayout.BeginScrollView(keyScroll);

//     DrawAudioFields();

//     EditorGUILayout.EndScrollView();

//     EditorGUILayout.Space(10);

//     GUILayout.BeginHorizontal();

//     bool valid = !HasDuplicateKeys() && !HasEmptyKeys();

//     GUI.enabled = valid;

//     if (GUILayout.Button("Rename All Clips", GUILayout.Height(35)))
//         RenameAll();

//     GUI.enabled = true;

//     if (GUILayout.Button("Check Array Lengths", GUILayout.Height(35)))
//         CheckArrays();

//     if (GUILayout.Button("Export CSV", GUILayout.Height(35)))
//         ExportCSV();

//     if (GUILayout.Button("Import CSV", GUILayout.Height(35)))
//         ImportCSV();

//     GUILayout.EndHorizontal();

//     if (HasDuplicateKeys())
//         EditorGUILayout.HelpBox("Duplicate keys detected.", MessageType.Error);

//     if (HasEmptyKeys())
//         EditorGUILayout.HelpBox("Keys cannot be empty.", MessageType.Error);
// }


// void DrawSOSelectionPanel()
// {
//     EditorGUILayout.LabelField("Selected ScriptableObjects", EditorStyles.boldLabel);

//     soScroll = EditorGUILayout.BeginScrollView(soScroll, GUILayout.Height(120));

//     EditorGUILayout.BeginVertical("box");

//     for (int i = 0; i < targetSOs.Count; i++)
//     {
//         EditorGUILayout.BeginHorizontal();

//         targetSOs[i] = (ScriptableObject)EditorGUILayout.ObjectField(
//             targetSOs[i],
//             typeof(ScriptableObject),
//             false);

//         if (GUILayout.Button("✖", GUILayout.Width(22)))
//         {
//             targetSOs.RemoveAt(i);
//             return;
//         }

//         EditorGUILayout.EndHorizontal();
//     }

//     EditorGUILayout.EndVertical();

//     EditorGUILayout.EndScrollView();

//     Rect dropArea = GUILayoutUtility.GetRect(0, 40, GUILayout.ExpandWidth(true));
//     GUI.Box(dropArea, "Drag ScriptableObjects Here");

//     Event evt = Event.current;

//     if (dropArea.Contains(evt.mousePosition))
//     {
//         if (evt.type == EventType.DragUpdated)
//             DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

//         if (evt.type == EventType.DragPerform)
//         {
//             DragAndDrop.AcceptDrag();

//             foreach (var obj in DragAndDrop.objectReferences)
//                 if (obj is ScriptableObject so && !targetSOs.Contains(so))
//                     targetSOs.Add(so);

//             Repaint();
//         }
//     }
// }

// void DrawAudioFields()
// {
//     ScriptableObject baseSO = targetSOs[0];

//     var fields = baseSO.GetType().GetFields(
//         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//     foreach (var field in fields)
//     {
//         if (field.FieldType == typeof(AudioClip))
//         {
//             int index = GetSingleIndex(field.Name);

//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField(field.Name, GUILayout.Width(200));

//             string value = state.singleKeysValues[index];

//             EditorGUI.BeginChangeCheck();
//             value = EditorGUILayout.TextField(value);

//             if (EditorGUI.EndChangeCheck())
//             {
//                 Undo.RecordObject(state, "Edit Audio Key");
//                 state.singleKeysValues[index] = value;
//             }

//             EditorGUILayout.EndHorizontal();
//         }

//         if (field.FieldType == typeof(AudioClip[]))
//         {
//             AudioClip[] clips = field.GetValue(baseSO) as AudioClip[];
//             if (clips == null) continue;

//             EditorGUILayout.Space(5);
//             EditorGUILayout.LabelField(field.Name, EditorStyles.boldLabel);

//             for (int i = 0; i < clips.Length; i++)
//             {
//                 int index = GetArrayIndex(field.Name, i);

//                 EditorGUILayout.BeginHorizontal();
//                 EditorGUILayout.LabelField(field.Name + "[" + i + "]", GUILayout.Width(200));

//                 string value = state.arrayValues[index];

//                 EditorGUI.BeginChangeCheck();
//                 value = EditorGUILayout.TextField(value);

//                 if (EditorGUI.EndChangeCheck())
//                 {
//                     Undo.RecordObject(state, "Edit Audio Key");
//                     state.arrayValues[index] = value;
//                 }

//                 EditorGUILayout.EndHorizontal();
//             }
//         }
//     }
// }

// int GetSingleIndex(string name)
// {
//     int index = state.singleKeysNames.IndexOf(name);

//     if (index == -1)
//     {
//         state.singleKeysNames.Add(name);
//         state.singleKeysValues.Add(name);
//         index = state.singleKeysNames.Count - 1;
//     }

//     return index;
// }

// int GetArrayIndex(string field, int idx)
// {
//     for (int i = 0; i < state.arrayFieldNames.Count; i++)
//         if (state.arrayFieldNames[i] == field && state.arrayIndex[i] == idx)
//             return i;

//     state.arrayFieldNames.Add(field);
//     state.arrayIndex.Add(idx);
//     state.arrayValues.Add(field + "_" + idx);

//     return state.arrayValues.Count - 1;
// }

// bool HasEmptyKeys()
// {
//     foreach (var v in state.singleKeysValues)
//         if (string.IsNullOrEmpty(v)) return true;

//     foreach (var v in state.arrayValues)
//         if (string.IsNullOrEmpty(v)) return true;

//     return false;
// }

// bool HasDuplicateKeys()
// {
//     HashSet<string> set = new HashSet<string>();

//     foreach (var v in state.singleKeysValues)
//         if (!set.Add(v)) return true;

//     foreach (var v in state.arrayValues)
//         if (!set.Add(v)) return true;

//     return false;
// }

// void RenameAll()
// {
//     foreach (var so in targetSOs)
//     {
//         var fields = so.GetType().GetFields(
//             BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//         foreach (var field in fields)
//         {
//             if (field.FieldType == typeof(AudioClip))
//             {
//                 AudioClip clip = field.GetValue(so) as AudioClip;
//                 if (clip == null) continue;

//                 int index = GetSingleIndex(field.Name);
//                 RenameClip(clip, state.singleKeysValues[index]);
//             }

//             if (field.FieldType == typeof(AudioClip[]))
//             {
//                 AudioClip[] clips = field.GetValue(so) as AudioClip[];

//                 for (int i = 0; i < clips.Length; i++)
//                 {
//                     int index = GetArrayIndex(field.Name, i);
//                     RenameClip(clips[i], state.arrayValues[index]);
//                 }
//             }
//         }
//     }

//     AssetDatabase.SaveAssets();
//     AssetDatabase.Refresh();

//     Debug.Log("Audio Renamed");
// }

// void RenameClip(AudioClip clip, string key)
// {
//     if (clip == null || string.IsNullOrEmpty(key)) return;

//     string path = AssetDatabase.GetAssetPath(clip);
//     AssetDatabase.RenameAsset(path, key);
// }

// void CheckArrays()
// {
//     ScriptableObject baseSO = targetSOs[0];

//     var fields = baseSO.GetType().GetFields(
//         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//     foreach (var field in fields)
//     {
//         if (field.FieldType != typeof(AudioClip[]))
//             continue;

//         int baseLen = ((AudioClip[])field.GetValue(baseSO)).Length;

//         foreach (var so in targetSOs)
//         {
//             AudioClip[] arr = field.GetValue(so) as AudioClip[];

//             if (arr.Length != baseLen)
//                 Debug.LogWarning(so.name + " mismatch in " + field.Name);
//         }
//     }
// }

// void ExportCSV()
// {
//     string path = EditorUtility.SaveFilePanel("Export CSV", "", "AudioKeys.csv", "csv");
//     if (string.IsNullOrEmpty(path)) return;

//     StringBuilder sb = new StringBuilder();

//     sb.AppendLine("Variable,Key");

//     for (int i = 0; i < state.singleKeysNames.Count; i++)
//         sb.AppendLine(state.singleKeysNames[i] + "," + state.singleKeysValues[i]);

//     for (int i = 0; i < state.arrayFieldNames.Count; i++)
//         sb.AppendLine(state.arrayFieldNames[i] + "[" + state.arrayIndex[i] + "]," + state.arrayValues[i]);

//     File.WriteAllText(path, sb.ToString());

//     Debug.Log("CSV Exported");
// }

// bool IsDuplicate(string value)
// {
//     if (string.IsNullOrEmpty(value))
//         return false;

//     int count = 0;

//     foreach (var v in state.singleKeysValues)
//         if (v == value) count++;

//     foreach (var v in state.arrayValues)
//         if (v == value) count++;

//     return count > 1;
// }

// void ImportCSV()
// {
//     string path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
//     if (string.IsNullOrEmpty(path)) return;

//     Undo.RecordObject(state, "Import CSV");

//     var lines = File.ReadAllLines(path);

//     for (int i = 1; i < lines.Length; i++)
//     {
//         var split = lines[i].Split(',');

//         if (split.Length < 2) continue;

//         string variable = split[0];
//         string key = split[1];

//         if (variable.Contains("["))
//         {
//             string name = variable.Substring(0, variable.IndexOf("["));
//             int index = int.Parse(variable.Substring(variable.IndexOf("[") + 1).Replace("]", ""));

//             int idx = GetArrayIndex(name, index);
//             state.arrayValues[idx] = key;
//         }
//         else
//         {
//             int idx = GetSingleIndex(variable);
//             state.singleKeysValues[idx] = key;
//         }
//     }

//     Repaint();
// }


// }

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;

public class AudioLocalizationState : ScriptableObject
{
    public List<string> singleKeysNames = new List<string>();
    public List<string> singleKeysValues = new List<string>();

    public List<string> arrayFieldNames = new List<string>();
    public List<int> arrayIndex = new List<int>();
    public List<string> arrayValues = new List<string>();
}

public class AudioLocalizationBatchTool : EditorWindow
{
    List<ScriptableObject> targetSOs = new List<ScriptableObject>();

    AudioLocalizationState state;

    Vector2 soScroll;
    Vector2 keyScroll;

    [MenuItem("Tools/Audio/Localization Batch Tool")]
    static void Open()
    {
        GetWindow<AudioLocalizationBatchTool>("Audio Localization");
    }

    void OnEnable()
    {
        if (state == null)
            state = CreateInstance<AudioLocalizationState>();

        Undo.undoRedoPerformed += Repaint;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= Repaint;
    }

    void OnGUI()
    {
        DrawSOSelectionPanel();

        if (targetSOs.Count == 0)
            return;

        keyScroll = EditorGUILayout.BeginScrollView(keyScroll);

        DrawSORecursive(targetSOs[0]);

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(10);

        GUILayout.BeginHorizontal();

        bool valid = !HasDuplicateKeys() && !HasEmptyKeys();

        GUI.enabled = valid;

        if (GUILayout.Button("Rename All Clips", GUILayout.Height(35)))
            RenameAll();

        GUI.enabled = true;

        if (GUILayout.Button("Export CSV", GUILayout.Height(35)))
            ExportCSV();

        if (GUILayout.Button("Import CSV", GUILayout.Height(35)))
            ImportCSV();

        GUILayout.EndHorizontal();

        if (HasDuplicateKeys())
            EditorGUILayout.HelpBox("Duplicate keys detected.", MessageType.Error);

        if (HasEmptyKeys())
            EditorGUILayout.HelpBox("Keys cannot be empty.", MessageType.Error);
    }

    void DrawSOSelectionPanel()
    {
        EditorGUILayout.LabelField("Selected ScriptableObjects", EditorStyles.boldLabel);

        soScroll = EditorGUILayout.BeginScrollView(soScroll, GUILayout.Height(120));

        EditorGUILayout.BeginVertical("box");

        for (int i = 0; i < targetSOs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            targetSOs[i] = (ScriptableObject)EditorGUILayout.ObjectField(
                targetSOs[i],
                typeof(ScriptableObject),
                false);

            if (GUILayout.Button("✖", GUILayout.Width(22)))
            {
                targetSOs.RemoveAt(i);
                return;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();

        Rect dropArea = GUILayoutUtility.GetRect(0, 40, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag ScriptableObjects Here");

        Event evt = Event.current;

        if (dropArea.Contains(evt.mousePosition))
        {
            if (evt.type == EventType.DragUpdated)
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (var obj in DragAndDrop.objectReferences)
                    if (obj is ScriptableObject so && !targetSOs.Contains(so))
                        targetSOs.Add(so);

                Repaint();
            }
        }
    }

    void DrawSORecursive(object obj)
    {
        if (obj == null) return;

        var fields = obj.GetType().GetFields(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var type = field.FieldType;

            if (type == typeof(AudioClip))
            {
                DrawSingleField(field, obj);
            }
            else if (type == typeof(AudioClip[]))
            {
                DrawArrayField(field, obj);
            }
            else if (type.Name == "ObjectSound[]")
            {
                DrawEnumMapping(field, obj);
            }
            else if (typeof(ScriptableObject).IsAssignableFrom(type))
            {
                DrawSORecursive(field.GetValue(obj));
            }
        }
    }

    void DrawSingleField(FieldInfo field, object obj)
    {
        int index = GetSingleIndex(field.Name);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(field.Name, GUILayout.Width(200));

        string value = state.singleKeysValues[index];

        Color prev = GUI.color;

        if (string.IsNullOrEmpty(value))
            GUI.color = Color.red;
        else if (IsDuplicate(value))
            GUI.color = Color.yellow;

        EditorGUI.BeginChangeCheck();
        value = EditorGUILayout.TextField(value);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(state, "Edit Key");
            state.singleKeysValues[index] = value;
        }

        GUI.color = prev;

        EditorGUILayout.EndHorizontal();
    }

    void DrawArrayField(FieldInfo field, object obj)
    {
        AudioClip[] clips = field.GetValue(obj) as AudioClip[];

        if (clips == null) return;

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField(field.Name, EditorStyles.boldLabel);

        for (int i = 0; i < clips.Length; i++)
        {
            int index = GetArrayIndex(field.Name, i);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(field.Name + "[" + i + "]", GUILayout.Width(200));

            string value = state.arrayValues[index];

            Color prev = GUI.color;

            if (string.IsNullOrEmpty(value))
                GUI.color = Color.red;
            else if (IsDuplicate(value))
                GUI.color = Color.yellow;

            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.TextField(value);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(state, "Edit Key");
                state.arrayValues[index] = value;
            }

            GUI.color = prev;

            EditorGUILayout.EndHorizontal();
        }
    }

    void DrawEnumMapping(FieldInfo field, object obj)
    {
        var array = field.GetValue(obj) as System.Array;

        if (array == null) return;

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField(field.Name + " (Enum Mapping)", EditorStyles.boldLabel);

        for (int i = 0; i < array.Length; i++)
        {
            var element = array.GetValue(i);

            var enumField = element.GetType().GetField("objectName");
            var clipField = element.GetType().GetField("respectiveAudioClip");

            if (enumField == null || clipField == null)
                continue;

            string enumName = enumField.GetValue(element).ToString();

            int index = GetArrayIndex(enumName, i);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(enumName + "[" + i + "]", GUILayout.Width(200));

            string value = state.arrayValues[index];

            Color prev = GUI.color;

            if (string.IsNullOrEmpty(value))
                GUI.color = Color.red;
            else if (IsDuplicate(value))
                GUI.color = Color.yellow;

            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.TextField(value);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(state, "Edit Key");
                state.arrayValues[index] = value;
            }

            GUI.color = prev;

            EditorGUILayout.EndHorizontal();
        }
    }

    int GetSingleIndex(string name)
    {
        int index = state.singleKeysNames.IndexOf(name);

        if (index == -1)
        {
            state.singleKeysNames.Add(name);
            state.singleKeysValues.Add(name);
            index = state.singleKeysNames.Count - 1;
        }

        return index;
    }

    int GetArrayIndex(string field, int idx)
    {
        for (int i = 0; i < state.arrayFieldNames.Count; i++)
            if (state.arrayFieldNames[i] == field && state.arrayIndex[i] == idx)
                return i;

        state.arrayFieldNames.Add(field);
        state.arrayIndex.Add(idx);
        state.arrayValues.Add(field + "_" + idx);

        return state.arrayValues.Count - 1;
    }

    bool HasEmptyKeys()
    {
        foreach (var v in state.singleKeysValues)
            if (string.IsNullOrEmpty(v)) return true;

        foreach (var v in state.arrayValues)
            if (string.IsNullOrEmpty(v)) return true;

        return false;
    }

    bool HasDuplicateKeys()
    {
        HashSet<string> set = new HashSet<string>();

        foreach (var v in state.singleKeysValues)
            if (!set.Add(v)) return true;

        foreach (var v in state.arrayValues)
            if (!set.Add(v)) return true;

        return false;
    }

    bool IsDuplicate(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        int count = 0;

        foreach (var v in state.singleKeysValues)
            if (v == value) count++;

        foreach (var v in state.arrayValues)
            if (v == value) count++;

        return count > 1;
    }

    void RenameAll()
    {
        foreach (var so in targetSOs)
        {
            RenameRecursive(so);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Audio Renamed");
    }

    void RenameRecursive(object obj)
    {
        if (obj == null) return;

        var fields = obj.GetType().GetFields(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var type = field.FieldType;

            if (type == typeof(AudioClip))
            {
                AudioClip clip = field.GetValue(obj) as AudioClip;

                int index = GetSingleIndex(field.Name);

                RenameClip(clip, state.singleKeysValues[index]);
            }
            else if (type == typeof(AudioClip[]))
            {
                AudioClip[] clips = field.GetValue(obj) as AudioClip[];

                for (int i = 0; i < clips.Length; i++)
                {
                    int index = GetArrayIndex(field.Name, i);
                    RenameClip(clips[i], state.arrayValues[index]);
                }
            }
            else if (type.Name == "ObjectSound[]")
            {
                var array = field.GetValue(obj) as System.Array;

                for (int i = 0; i < array.Length; i++)
                {
                    var element = array.GetValue(i);

                    var enumField = element.GetType().GetField("objectName");
                    var clipField = element.GetType().GetField("respectiveAudioClip");

                    if (enumField == null || clipField == null)
                        continue;

                    string enumName = enumField.GetValue(element).ToString();

                    int index = GetArrayIndex(enumName, i);

                    AudioClip clip = clipField.GetValue(element) as AudioClip;

                    RenameClip(clip, state.arrayValues[index]);
                }
            }
            else if (typeof(ScriptableObject).IsAssignableFrom(type))
            {
                RenameRecursive(field.GetValue(obj));
            }
        }
    }

    void RenameClip(AudioClip clip, string key)
    {
        if (clip == null || string.IsNullOrEmpty(key)) return;

        string path = AssetDatabase.GetAssetPath(clip);
        AssetDatabase.RenameAsset(path, key);
    }

    void ExportCSV()
    {
        string path = EditorUtility.SaveFilePanel("Export CSV", "", "AudioKeys.csv", "csv");
        if (string.IsNullOrEmpty(path)) return;

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Variable,Key");

        for (int i = 0; i < state.singleKeysNames.Count; i++)
            sb.AppendLine(state.singleKeysNames[i] + "," + state.singleKeysValues[i]);

        for (int i = 0; i < state.arrayFieldNames.Count; i++)
            sb.AppendLine(state.arrayFieldNames[i] + "[" + state.arrayIndex[i] + "]," + state.arrayValues[i]);

        File.WriteAllText(path, sb.ToString());

        Debug.Log("CSV Exported");
    }

    void ImportCSV()
    {
        string path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
        if (string.IsNullOrEmpty(path)) return;

        Undo.RecordObject(state, "Import CSV");

        var lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            var split = lines[i].Split(',');

            if (split.Length < 2) continue;

            string variable = split[0];
            string key = split[1];

            if (variable.Contains("["))
            {
                string name = variable.Substring(0, variable.IndexOf("["));
                int index = int.Parse(variable.Substring(variable.IndexOf("[") + 1).Replace("]", ""));

                int idx = GetArrayIndex(name, index);
                state.arrayValues[idx] = key;
            }
            else
            {
                int idx = GetSingleIndex(variable);
                state.singleKeysValues[idx] = key;
            }
        }

        Repaint();
    }
}