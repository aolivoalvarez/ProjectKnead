/*-----------------------------------------
Creation Date: 3/15/2024 6:43:09 PM
Author: theco
Description: A custom attribute that allows a field to only be serialized in the inspector if it fulfills certain conditions.
-----------------------------------------*/

// SCRIPT TAKEN FROM USER CHOOPTWISK ON STACKOVERFLOW
using System;
using UnityEngine;

namespace CustomAttributes
{
    public enum ConditionOperator
    {
        // A field is visible/enabled only if all conditions are true.
        And,
        // A field is visible/enabled if at least ONE condition is true.
        Or,
    }

    public enum ActionOnConditionFail
    {
        // If condition(s) are false, don't draw the field at all.
        DontDraw,
        // If condition(s) are false, just set the field as disabled.
        JustDisable,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public ActionOnConditionFail Action { get; private set; }
        public ConditionOperator Operator { get; private set; }
        public string[] Conditions { get; private set; }

        public ShowIfAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator, params string[] conditions)
        {
            Action = action;
            Operator = conditionOperator;
            Conditions = conditions;
        }
    }
}