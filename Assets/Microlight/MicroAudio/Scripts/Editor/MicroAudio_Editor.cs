#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    [CustomEditor(typeof(MicroAudio))]
    public class MicroAudio_Editor : Editor
    {
        [MenuItem("Microlight/Micro Audio/Micro Audio Manager")]
        private static void AddMicroAudioManagerTopMenu() => AddMicroAudioManager();
        [MenuItem("GameObject/Microlight/Micro Audio/Micro Audio Manager")]
        private static void AddMicroAudioManager()
        {
            // Get prefab
            GameObject go = MicrolightAssetUtilities.GetPrefab("MicroAudio");
            if (go == null)
            {
                Debug.LogWarning("MicroAudio: Error when instantiating prefab. You can always drag and drop prefab from Prefabs folder.");
                return;
            }

            PrefabUtility.InstantiatePrefab(go);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Mixers
            SerializedProperty mixer = serializedObject.FindProperty("_mixer");
            SerializedProperty masterMixerGroup = serializedObject.FindProperty("_masterMixerGroup");
            SerializedProperty musicMixerGroup = serializedObject.FindProperty("_musicMixerGroup");
            SerializedProperty soundsMixerGroup = serializedObject.FindProperty("_soundsMixerGroup");
            SerializedProperty sfxMixerGroup = serializedObject.FindProperty("_sfxMixerGroup");
            SerializedProperty uiMixerGroup = serializedObject.FindProperty("_uiMixerGroup");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mixer", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(mixer);
            EditorGUILayout.PropertyField(masterMixerGroup);
            EditorGUILayout.PropertyField(musicMixerGroup);
            EditorGUILayout.PropertyField(soundsMixerGroup);
            EditorGUILayout.PropertyField(sfxMixerGroup);
            EditorGUILayout.PropertyField(uiMixerGroup);

            // Music
            SerializedProperty musicAudioSource1 = serializedObject.FindProperty("musicAudioSource1");
            SerializedProperty musicAudioSource2 = serializedObject.FindProperty("musicAudioSource2");
            SerializedProperty crossfadeTime = serializedObject.FindProperty("crossfadeTime");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Music", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(musicAudioSource1);
            EditorGUILayout.PropertyField(musicAudioSource2);
            EditorGUILayout.PropertyField(crossfadeTime);

            // Music
            SerializedProperty maxSoundsSources = serializedObject.FindProperty("maxSoundsSources");
            SerializedProperty maxInstancesOfSameClip = serializedObject.FindProperty("maxInstancesOfSameClip");
            SerializedProperty soundsContainer = serializedObject.FindProperty("soundsContainer");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sounds", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(maxSoundsSources);
            EditorGUILayout.PropertyField(maxInstancesOfSameClip);
            EditorGUILayout.PropertyField(soundsContainer);

            serializedObject.ApplyModifiedProperties();   // Apply changes
        }
    }
}
#endif