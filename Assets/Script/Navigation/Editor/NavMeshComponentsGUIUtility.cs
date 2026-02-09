using UnityEditor;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Navigation.Editor
{
    internal static class NavMeshComponentsGUIUtility
    {
        private static readonly int s_allAreaMask;

        static NavMeshComponentsGUIUtility()
        {
            int mask = 0;
            string[] areaNames = NavMesh.GetAreaNames();
            for (int i = 0; i < areaNames.Length; i++)
            {
                int areaValue = NavMesh.GetAreaFromName(areaNames[i]);
                int areaMask = (1 << areaValue);
                mask |= areaMask;
            }

            s_allAreaMask = mask;
        }

        public static void AreaPopup(Rect rect, string labelName, SerializedProperty property)
        {
            var areaIndex = -1;
            var areaNames = NavMesh.GetAreaNames();
            for (var i = 0; i < areaNames.Length; i++)
            {
                int areaValue = NavMesh.GetAreaFromName(areaNames[i]);
                if (areaValue == property.intValue)
                    areaIndex = i;
            }

            ArrayUtility.Add(ref areaNames, "");
            ArrayUtility.Add(ref areaNames, "Open Area Settings...");

            EditorGUI.BeginProperty(rect, GUIContent.none, property);

            EditorGUI.BeginChangeCheck();
            areaIndex = EditorGUI.Popup(rect, labelName, areaIndex, areaNames);

            if (EditorGUI.EndChangeCheck())
            {
                if (areaIndex >= 0 && areaIndex < areaNames.Length - 2)
                    property.intValue = NavMesh.GetAreaFromName(areaNames[areaIndex]);
                else if (areaIndex == areaNames.Length - 1)
                    NavMeshEditorHelpers.OpenAreaSettings();
            }

            EditorGUI.EndProperty();
        }

        public static bool IsAgentSelectionValid(SerializedProperty property)
        {
            int count = NavMesh.GetSettingsCount();
            for (int i = 0; i < count; i++)
            {
                int id = NavMesh.GetSettingsByIndex(i).agentTypeID;
                if (id == property.intValue)
                    return true;
            }

            return false;
        }

        public static void AgentTypePopup(Rect rect, string labelName, SerializedProperty property)
        {
            int index = -1;
            int count = NavMesh.GetSettingsCount();
            string[] agentTypeNames = new string[count + 2];
            for (int i = 0; i < count; i++)
            {
                int id = NavMesh.GetSettingsByIndex(i).agentTypeID;
                string name = NavMesh.GetSettingsNameFromID(id);
                agentTypeNames[i] = name;
                if (id == property.intValue)
                    index = i;
            }

            agentTypeNames[count] = "";
            agentTypeNames[count + 1] = "Open Agent Settings...";

            bool validAgentType = index != -1;
            if (!validAgentType)
            {
                Rect warningRect = rect;
                warningRect.height *= .5f;
                warningRect.y += warningRect.height;
                EditorGUI.HelpBox(warningRect, "Agent Type invalid.", MessageType.Warning);

                rect.height *= .5f;
            }

            EditorGUI.BeginProperty(rect, GUIContent.none, property);

            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(rect, labelName, index, agentTypeNames);
            if (EditorGUI.EndChangeCheck())
            {
                if (index >= 0 && index < count)
                {
                    int id = NavMesh.GetSettingsByIndex(index).agentTypeID;
                    property.intValue = id;
                }
                else if (index == count + 1)
                {
                    NavMeshEditorHelpers.OpenAgentSettings(-1);
                }
            }

            EditorGUI.EndProperty();
        }

        public static void AgentMaskPopup(string labelName, SerializedProperty property)
        {
            // Contents of the dropdown box.
            string popupContent = "";

            if (property.hasMultipleDifferentValues)
                popupContent = "\u2014";
            else
                popupContent = GetAgentMaskLabelName(property);

            var content = new GUIContent(popupContent);
            var popupRect = GUILayoutUtility.GetRect(content, EditorStyles.popup);

            EditorGUI.BeginProperty(popupRect, GUIContent.none, property);
            popupRect = EditorGUI.PrefixLabel(popupRect, 0, new GUIContent(labelName));
            bool pressed = GUI.Button(popupRect, content, EditorStyles.popup);

            if (pressed)
            {
                var show = !property.hasMultipleDifferentValues;
                var showNone = show && property.arraySize == 0;
                var showAll = show && IsAllAgents(property);

                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("None"), showNone, SetAgentMaskNone, property);
                menu.AddItem(new GUIContent("All"), showAll, SetAgentMaskAll, property);
                menu.AddSeparator("");

                var count = NavMesh.GetSettingsCount();
                for (var i = 0; i < count; i++)
                {
                    var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
                    var name = NavMesh.GetSettingsNameFromID(id);

                    var showSelected = show && AgentMaskHasSelectedAgentTypeID(property, id);
                    var userData = new object[] {property, id, !showSelected};
                    menu.AddItem(new GUIContent(name), showSelected, ToggleAgentMaskItem, userData);
                }

                menu.DropDown(popupRect);
            }

            EditorGUI.EndProperty();
        }

        public static GameObject CreateAndSelectGameObject(string suggestedName, GameObject parent)
        {
            var parentTransform = parent != null ? parent.transform : null;
            var uniqueName = GameObjectUtility.GetUniqueNameForSibling(parentTransform, suggestedName);
            var child = new GameObject(uniqueName);

            Undo.RegisterCreatedObjectUndo(child, "Create " + uniqueName);
            if (parentTransform != null)
                Undo.SetTransformParent(child.transform, parentTransform, "Parent " + uniqueName);

            Selection.activeGameObject = child;

            return child;
        }


        public static void AreaMaskPopup(Rect rect, string labelName, SerializedProperty property)
        {
            string popupContent = "";

            if (property.hasMultipleDifferentValues)
                popupContent = "\u2014";
            else
                popupContent = GetAreaMaskLabelName(property);

            GUIContent content = new GUIContent(popupContent);
            Rect popupRect = rect;
            //Rect popupRect = GUILayoutUtility.GetRect(content, EditorStyles.popup);

            EditorGUI.BeginProperty(popupRect, GUIContent.none, property);
            popupRect = EditorGUI.PrefixLabel(popupRect, 0, new GUIContent(labelName));

            if (GUI.Button(popupRect, content, EditorStyles.popup))
            {
                bool show = !property.hasMultipleDifferentValues;
                bool showNone = show && property.intValue == 0;
                bool showAll = show && IsAllAreas(property);

                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("None"), showNone, SetAreaMaskNone, property);
                menu.AddItem(new GUIContent("All"), showAll, SetAreaMaskAll, property);
                menu.AddSeparator("");

                string[] areaNames = NavMesh.GetAreaNames();
                for (var i = 0; i < areaNames.Length; i++)
                {
                    string name = areaNames[i];
                    int id = NavMesh.GetAreaFromName(name);

                    var showSelected = show && AreaMaskHasSelectedAreaValue(property, id);
                    var userData = new object[] {property, id, !showSelected};
                    menu.AddItem(new GUIContent(name), showSelected, ToggleAreaMaskItem, userData);
                }

                menu.DropDown(popupRect);
            }

            EditorGUI.EndProperty();
        }

        private static bool IsAllAgents(SerializedProperty property)
        {
            return property.arraySize == 1 && property.GetArrayElementAtIndex(0).intValue == -1;
        }

        private static bool IsAllAreas(SerializedProperty property)
        {
            return property.intValue == (property.intValue | s_allAreaMask);
        }

        private static void ToggleAgentMaskItem(object userData)
        {
            object[] args = (object[]) userData;
            SerializedProperty agentMask = (SerializedProperty) args[0];
            int agentTypeID = (int) args[1];
            bool value = (bool) args[2];

            ToggleAgentMaskItem(agentMask, agentTypeID, value);
        }

        private static void ToggleAgentMaskItem(SerializedProperty agentMask, int agentTypeID, bool value)
        {
            if (agentMask.hasMultipleDifferentValues)
            {
                agentMask.ClearArray();
                agentMask.serializedObject.ApplyModifiedProperties();
            }

            // Find which index this agent type is in the agentMask array.
            int idx = -1;
            for (var j = 0; j < agentMask.arraySize; j++)
            {
                SerializedProperty elem = agentMask.GetArrayElementAtIndex(j);
                if (elem.intValue == agentTypeID)
                    idx = j;
            }

            // Handle "All" special case.
            if (IsAllAgents(agentMask))
            {
                agentMask.DeleteArrayElementAtIndex(0);
            }

            // Toggle value.
            if (value)
            {
                if (idx == -1)
                {
                    agentMask.InsertArrayElementAtIndex(agentMask.arraySize);
                    agentMask.GetArrayElementAtIndex(agentMask.arraySize - 1).intValue = agentTypeID;
                }
            }
            else
            {
                if (idx != -1)
                {
                    agentMask.DeleteArrayElementAtIndex(idx);
                }
            }

            agentMask.serializedObject.ApplyModifiedProperties();
        }

        private static void ToggleAreaMaskItem(object userData)
        {
            object[] args = (object[]) userData;
            SerializedProperty property = (SerializedProperty) args[0];
            int areaValue = (int) args[1];
            bool value = (bool) args[2];
            ToggleAreaMaskItem(property, areaValue, value);
        }

        private static void ToggleAreaMaskItem(SerializedProperty areaMask, int areaValue, bool value)
        {
            if (areaMask.hasMultipleDifferentValues)
            {
                areaMask.intValue = 0;
                areaMask.serializedObject.ApplyModifiedProperties();
            }

            if (value)
            {
                areaMask.intValue |= (1 << areaValue);
            }
            else
            {
                areaMask.intValue &= ~(1 << areaValue);
            }

            areaMask.serializedObject.ApplyModifiedProperties();
        }

        private static void SetAgentMaskNone(object data)
        {
            SerializedProperty agentMask = (SerializedProperty) data;
            agentMask.ClearArray();
            agentMask.serializedObject.ApplyModifiedProperties();
        }

        private static void SetAgentMaskAll(object data)
        {
            SerializedProperty agentMask = (SerializedProperty) data;
            agentMask.ClearArray();
            agentMask.InsertArrayElementAtIndex(0);
            agentMask.GetArrayElementAtIndex(0).intValue = -1;
            agentMask.serializedObject.ApplyModifiedProperties();
        }

        private static void SetAreaMaskNone(object data)
        {
            SerializedProperty agentMask = (SerializedProperty) data;
            agentMask.intValue = 0;
            agentMask.serializedObject.ApplyModifiedProperties();
        }

        private static void SetAreaMaskAll(object data)
        {
            SerializedProperty agentMask = (SerializedProperty) data;
            agentMask.intValue = NavMesh.AllAreas;
            agentMask.serializedObject.ApplyModifiedProperties();
        }

        private static string GetAgentMaskLabelName(SerializedProperty agentMask)
        {
            if (agentMask.arraySize == 0)
                return "None";

            if (IsAllAgents(agentMask))
                return "All";

            if (agentMask.arraySize <= 3)
            {
                string labelName = "";
                for (int j = 0; j < agentMask.arraySize; j++)
                {
                    SerializedProperty elem = agentMask.GetArrayElementAtIndex(j);
                    string settingsName = NavMesh.GetSettingsNameFromID(elem.intValue);
                    if (string.IsNullOrEmpty(settingsName))
                        continue;

                    if (labelName.Length > 0)
                        labelName += ", ";
                    labelName += settingsName;
                }

                return labelName;
            }

            return "Mixed...";
        }

        private static string GetAreaMaskLabelName(SerializedProperty areaMask)
        {
            if (areaMask.intValue == 0)
                return "None";

            if (IsAllAreas(areaMask))
                return "All";


            string[] areaNames = NavMesh.GetAreaNames();
            string labelName = "";
            int count = 0;
            for (int i = 0; i < areaNames.Length; i++)
            {
                string name = areaNames[i];
                int id = NavMesh.GetAreaFromName(name);
                if (areaMask.intValue == (areaMask.intValue | 1 << id))
                {
                    count++;
                    if (string.IsNullOrEmpty(name))
                        continue;

                    if (labelName.Length > 0)
                        labelName += ", ";
                    labelName += name;
                }
            }

            return count <= 3 ? labelName : "Mixed...";
        }

        private static bool AreaMaskHasSelectedAreaValue(SerializedProperty property, int areaValue)
        {
            return property.intValue == (property.intValue | 1 << areaValue);
        }

        private static bool AgentMaskHasSelectedAgentTypeID(SerializedProperty agentMask, int agentTypeID)
        {
            for (int j = 0; j < agentMask.arraySize; j++)
            {
                SerializedProperty elem = agentMask.GetArrayElementAtIndex(j);
                if (elem.intValue == agentTypeID)
                    return true;
            }

            return false;
        }
    }
}