// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/CinemachineCustomInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CinemachineCustomInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CinemachineCustomInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CinemachineCustomInputs"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""b8e7a6a2-a1f2-4673-813e-d60b1c5bd8c8"",
            ""actions"": [
                {
                    ""name"": ""VerticalRigChange"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7b8e9960-7bb6-4639-8351-085101027816"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""15dfdbe6-0764-425a-b9b1-2e4eb65c5c03"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalRigChange"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c1b385b5-e217-4475-961c-a9abf7dccfec"",
                    ""path"": ""<Mouse>/forwardButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalRigChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7959bb6e-2fb2-46fe-836e-51e52b48559b"",
                    ""path"": ""<Mouse>/backButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalRigChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_VerticalRigChange = m_Mouse.FindAction("VerticalRigChange", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_VerticalRigChange;
    public struct MouseActions
    {
        private @CinemachineCustomInputs m_Wrapper;
        public MouseActions(@CinemachineCustomInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @VerticalRigChange => m_Wrapper.m_Mouse_VerticalRigChange;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @VerticalRigChange.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnVerticalRigChange;
                @VerticalRigChange.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnVerticalRigChange;
                @VerticalRigChange.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnVerticalRigChange;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @VerticalRigChange.started += instance.OnVerticalRigChange;
                @VerticalRigChange.performed += instance.OnVerticalRigChange;
                @VerticalRigChange.canceled += instance.OnVerticalRigChange;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    public interface IMouseActions
    {
        void OnVerticalRigChange(InputAction.CallbackContext context);
    }
}
