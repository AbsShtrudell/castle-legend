//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/Player/InputSystem/PlayerControlActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControlActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControlActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControlActions"",
    ""maps"": [
        {
            ""name"": ""Build"",
            ""id"": ""65c1f73b-76df-42b3-b558-7e1517fdfb53"",
            ""actions"": [
                {
                    ""name"": ""Place"",
                    ""type"": ""Button"",
                    ""id"": ""626f896c-7cb4-4365-a7e6-f713436988e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""801bfa0c-3210-4f88-b6d4-b6ff8e997c05"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""DisableBuildMode"",
                    ""type"": ""Button"",
                    ""id"": ""64a8f9b0-b99e-49a3-ae70-dc7ff9bfb0aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""771c28dd-0010-4dcf-af2a-f82d74abf395"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Place"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""45e0cccd-fcf1-45f9-b5fc-083cf35f13d5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""73fb07e4-1aeb-4f44-90b1-6ae696d8a1f7"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5d92ddf4-b58f-4a5c-996a-2ca86f36d430"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c2e29d6b-4b86-458d-993c-2143a9425433"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DisableBuildMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Controll"",
            ""id"": ""4f52c26d-c693-4ae7-983d-d65d7ce51103"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""b2b7391a-08e0-4cc9-af13-09bd688fde83"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Order"",
                    ""type"": ""Button"",
                    ""id"": ""578caba9-577d-4992-8b52-3e019a73c84d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""5aa0d6a2-518f-49d2-aaf7-0cd6cb23fee9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""315433f0-484c-4183-bb29-3a39b54400ed"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee7116aa-973f-4edc-b6e4-6ce03a0efd28"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Order"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ac1d96c-5781-48f5-8e68-b9e40c786db9"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Build
        m_Build = asset.FindActionMap("Build", throwIfNotFound: true);
        m_Build_Place = m_Build.FindAction("Place", throwIfNotFound: true);
        m_Build_Rotate = m_Build.FindAction("Rotate", throwIfNotFound: true);
        m_Build_DisableBuildMode = m_Build.FindAction("DisableBuildMode", throwIfNotFound: true);
        // Controll
        m_Controll = asset.FindActionMap("Controll", throwIfNotFound: true);
        m_Controll_Select = m_Controll.FindAction("Select", throwIfNotFound: true);
        m_Controll_Order = m_Controll.FindAction("Order", throwIfNotFound: true);
        m_Controll_Delete = m_Controll.FindAction("Delete", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Build
    private readonly InputActionMap m_Build;
    private IBuildActions m_BuildActionsCallbackInterface;
    private readonly InputAction m_Build_Place;
    private readonly InputAction m_Build_Rotate;
    private readonly InputAction m_Build_DisableBuildMode;
    public struct BuildActions
    {
        private @PlayerControlActions m_Wrapper;
        public BuildActions(@PlayerControlActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Place => m_Wrapper.m_Build_Place;
        public InputAction @Rotate => m_Wrapper.m_Build_Rotate;
        public InputAction @DisableBuildMode => m_Wrapper.m_Build_DisableBuildMode;
        public InputActionMap Get() { return m_Wrapper.m_Build; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BuildActions set) { return set.Get(); }
        public void SetCallbacks(IBuildActions instance)
        {
            if (m_Wrapper.m_BuildActionsCallbackInterface != null)
            {
                @Place.started -= m_Wrapper.m_BuildActionsCallbackInterface.OnPlace;
                @Place.performed -= m_Wrapper.m_BuildActionsCallbackInterface.OnPlace;
                @Place.canceled -= m_Wrapper.m_BuildActionsCallbackInterface.OnPlace;
                @Rotate.started -= m_Wrapper.m_BuildActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_BuildActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_BuildActionsCallbackInterface.OnRotate;
                @DisableBuildMode.started -= m_Wrapper.m_BuildActionsCallbackInterface.OnDisableBuildMode;
                @DisableBuildMode.performed -= m_Wrapper.m_BuildActionsCallbackInterface.OnDisableBuildMode;
                @DisableBuildMode.canceled -= m_Wrapper.m_BuildActionsCallbackInterface.OnDisableBuildMode;
            }
            m_Wrapper.m_BuildActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Place.started += instance.OnPlace;
                @Place.performed += instance.OnPlace;
                @Place.canceled += instance.OnPlace;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @DisableBuildMode.started += instance.OnDisableBuildMode;
                @DisableBuildMode.performed += instance.OnDisableBuildMode;
                @DisableBuildMode.canceled += instance.OnDisableBuildMode;
            }
        }
    }
    public BuildActions @Build => new BuildActions(this);

    // Controll
    private readonly InputActionMap m_Controll;
    private IControllActions m_ControllActionsCallbackInterface;
    private readonly InputAction m_Controll_Select;
    private readonly InputAction m_Controll_Order;
    private readonly InputAction m_Controll_Delete;
    public struct ControllActions
    {
        private @PlayerControlActions m_Wrapper;
        public ControllActions(@PlayerControlActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Controll_Select;
        public InputAction @Order => m_Wrapper.m_Controll_Order;
        public InputAction @Delete => m_Wrapper.m_Controll_Delete;
        public InputActionMap Get() { return m_Wrapper.m_Controll; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControllActions set) { return set.Get(); }
        public void SetCallbacks(IControllActions instance)
        {
            if (m_Wrapper.m_ControllActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_ControllActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_ControllActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_ControllActionsCallbackInterface.OnSelect;
                @Order.started -= m_Wrapper.m_ControllActionsCallbackInterface.OnOrder;
                @Order.performed -= m_Wrapper.m_ControllActionsCallbackInterface.OnOrder;
                @Order.canceled -= m_Wrapper.m_ControllActionsCallbackInterface.OnOrder;
                @Delete.started -= m_Wrapper.m_ControllActionsCallbackInterface.OnDelete;
                @Delete.performed -= m_Wrapper.m_ControllActionsCallbackInterface.OnDelete;
                @Delete.canceled -= m_Wrapper.m_ControllActionsCallbackInterface.OnDelete;
            }
            m_Wrapper.m_ControllActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Order.started += instance.OnOrder;
                @Order.performed += instance.OnOrder;
                @Order.canceled += instance.OnOrder;
                @Delete.started += instance.OnDelete;
                @Delete.performed += instance.OnDelete;
                @Delete.canceled += instance.OnDelete;
            }
        }
    }
    public ControllActions @Controll => new ControllActions(this);
    public interface IBuildActions
    {
        void OnPlace(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnDisableBuildMode(InputAction.CallbackContext context);
    }
    public interface IControllActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnOrder(InputAction.CallbackContext context);
        void OnDelete(InputAction.CallbackContext context);
    }
}
