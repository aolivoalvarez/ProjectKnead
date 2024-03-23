/*-----------------------------------------
Creation Date: 3/21/2024 1:53:33 PM
Author: theco
Description: A custom attribute that calls a function when a specific serialized value is changed in the editor.
-----------------------------------------*/

// SCRIPT TAKEN FROM USERS INDIEGAMEDEV AND A. DELRAN ON STACKOVERFLOW
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace CustomAttributes
{
    public class OnChangedCallAttribute : PropertyAttribute
    {
        public string methodName;
        public OnChangedCallAttribute(string methodNameNoArguments)
        {
            methodName = methodNameNoArguments;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(OnChangedCallAttribute))]
    public class OnChangedCallAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            if (!EditorGUI.EndChangeCheck()) return;

            var targetObject = property.serializedObject.targetObject;

            var callAttribute = attribute as OnChangedCallAttribute;
            var methodName = callAttribute?.methodName;

            var classType = targetObject.GetType();
            var methodInfo = classType.GetMethods().FirstOrDefault(info => info.Name == methodName);

            // Update the serialized field
            property.serializedObject.ApplyModifiedProperties();

            // If we found a public function with the given name that takes no parameters, invoke it
            if (methodInfo != null && !methodInfo.GetParameters().Any())
            {
                methodInfo.Invoke(targetObject, null);
            }
            else
            {
                // TODO: Create proper exception
                Debug.LogError($"OnChangedCall error : No public function taking no " +
                               $"argument named {methodName} in class {classType.Name}");
            }
        }
    }

#endif
}
