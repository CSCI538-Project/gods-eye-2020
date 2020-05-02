// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Utilities.Editor;
using UnityEngine.Events;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.UI
{
    /// <summary>
    /// A basic focus event receiver
    /// </summary>
    public class InteractableOnFocusReceiver : ReceiverBase
    {
        [InspectorField(Type = InspectorField.FieldTypes.Event, Label = "On Focus Off", Tooltip = "Focus has left the object")]
        public UnityEvent OnFocusOff = new UnityEvent();

        private bool hadFocus;
        private State lastState;

        private float focusTimer = 0;

        public InteractableOnFocusReceiver(UnityEvent ev) : base(ev)
        {
            Name = "OnFocus";
        }

        public override void OnUpdate(InteractableStates state, Interactable source)
        {
            bool hasFocus = state.GetState(InteractableStates.InteractableStateEnum.Focus).Value > 0;

            float focusTime = source.gameObject.GetComponent<ButtonFocusTime>().GetFocusTime();

            //if button is focused for focusTime seconds, change button's focus state to be false
            if (hasFocus && focusTimer < focusTime){
                focusTimer += Time.deltaTime;

                if (focusTimer >= focusTime){
                    source.SetState(InteractableStates.InteractableStateEnum.Focus, false);
                }
            } else {
                focusTimer = 0;
            }


            bool changed = state.CurrentState() != lastState;

            if (hadFocus != hasFocus && changed)
            {
                if (hasFocus)
                {
                    uEvent.Invoke();
                }
                else
                {
                    OnFocusOff.Invoke();
                }
            }

            hadFocus = hasFocus;
            lastState = state.CurrentState();
        }
    }
}
