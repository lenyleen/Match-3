using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameEditor : EditorWindow
{
        [MenuItem("Custom Editor/Game Editor")]
        public static void GameEditorWindow()
        {
                GetWindowWithRect(typeof(GameEditor), new Rect(new Vector2(0,0),new Vector2(700,700)), false, " Game Editor");   
        }
        void OnInspectorUpdate() { Repaint(); }
        private readonly List<IGameEditorWindow> editors = new List<IGameEditorWindow>();
        private int selectedEditorIndex = -1;
        private void OnEnable()
        {
            editors.Add(new EditorTab());
            editors.Add(new ScoreEditor());
            editors.Add(new StorePricesSettings());
            editors.Add(new DailyGiftsEditor());
            editors.Add(new GameConfigurationEditor());
        }

        private void OnGUI()
        {
            selectedEditorIndex =
                GUILayout.Toolbar(selectedEditorIndex, editors.Select(editor => editor.Name).ToArray());    
            DrawSelectedEditor();     
        }

        private void DrawSelectedEditor()
        {
            if(selectedEditorIndex > -1 && selectedEditorIndex < editors.Count)
                editors[selectedEditorIndex].DrawWindow();
        }
        
}
