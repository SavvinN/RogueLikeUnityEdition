//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/InputController.inputactions
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

public partial class @InputController: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""AttackMap"",
            ""id"": ""593afda3-96eb-4f8e-b6c9-748012f0cfc4"",
            ""actions"": [
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""38f6cd52-ce9d-46d8-90bf-4ebd1d55b06e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""0ec06402-8fc9-44de-afa8-7df82fa8155f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""34aee713-95e8-4cb0-bb3a-4abc8e0d85da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""d9f03312-a97c-4857-8fa0-1a49cf4af64b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bf6d30e8-d3c0-481e-b8e6-67284d95abf3"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6ea0dba-5519-43f8-a449-5358b7f9ac24"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48fe4ee5-5f23-4b53-a558-827633a9a41b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40021f2f-5a09-46a3-80eb-5641e0dfb40b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MoveMap"",
            ""id"": ""d940f26c-0f56-440e-91ba-93452cbb9c01"",
            ""actions"": [
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""8577c090-6a2d-4cbf-a7bd-f9044f2a7da9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""ab32c8ef-8a3f-4e73-a30f-294e49ecf316"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""5369240e-1aef-4b27-82b4-d0bdd4802300"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""29c54526-c9dc-4306-8699-0d07246b8107"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6c1da3d0-42c5-41be-97f2-529f4bca680f"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d21e2278-3591-46a5-97e2-8e089eb51822"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f184c28b-ee59-417c-b49b-6dc74eba6ceb"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21518252-744f-4ed3-8828-ee5f30dc32d1"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // AttackMap
        m_AttackMap = asset.FindActionMap("AttackMap", throwIfNotFound: true);
        m_AttackMap_Up = m_AttackMap.FindAction("Up", throwIfNotFound: true);
        m_AttackMap_Down = m_AttackMap.FindAction("Down", throwIfNotFound: true);
        m_AttackMap_Left = m_AttackMap.FindAction("Left", throwIfNotFound: true);
        m_AttackMap_Right = m_AttackMap.FindAction("Right", throwIfNotFound: true);
        // MoveMap
        m_MoveMap = asset.FindActionMap("MoveMap", throwIfNotFound: true);
        m_MoveMap_Up = m_MoveMap.FindAction("Up", throwIfNotFound: true);
        m_MoveMap_Down = m_MoveMap.FindAction("Down", throwIfNotFound: true);
        m_MoveMap_Left = m_MoveMap.FindAction("Left", throwIfNotFound: true);
        m_MoveMap_Right = m_MoveMap.FindAction("Right", throwIfNotFound: true);
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

    // AttackMap
    private readonly InputActionMap m_AttackMap;
    private List<IAttackMapActions> m_AttackMapActionsCallbackInterfaces = new List<IAttackMapActions>();
    private readonly InputAction m_AttackMap_Up;
    private readonly InputAction m_AttackMap_Down;
    private readonly InputAction m_AttackMap_Left;
    private readonly InputAction m_AttackMap_Right;
    public struct AttackMapActions
    {
        private @InputController m_Wrapper;
        public AttackMapActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Up => m_Wrapper.m_AttackMap_Up;
        public InputAction @Down => m_Wrapper.m_AttackMap_Down;
        public InputAction @Left => m_Wrapper.m_AttackMap_Left;
        public InputAction @Right => m_Wrapper.m_AttackMap_Right;
        public InputActionMap Get() { return m_Wrapper.m_AttackMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AttackMapActions set) { return set.Get(); }
        public void AddCallbacks(IAttackMapActions instance)
        {
            if (instance == null || m_Wrapper.m_AttackMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_AttackMapActionsCallbackInterfaces.Add(instance);
            @Up.started += instance.OnUp;
            @Up.performed += instance.OnUp;
            @Up.canceled += instance.OnUp;
            @Down.started += instance.OnDown;
            @Down.performed += instance.OnDown;
            @Down.canceled += instance.OnDown;
            @Left.started += instance.OnLeft;
            @Left.performed += instance.OnLeft;
            @Left.canceled += instance.OnLeft;
            @Right.started += instance.OnRight;
            @Right.performed += instance.OnRight;
            @Right.canceled += instance.OnRight;
        }

        private void UnregisterCallbacks(IAttackMapActions instance)
        {
            @Up.started -= instance.OnUp;
            @Up.performed -= instance.OnUp;
            @Up.canceled -= instance.OnUp;
            @Down.started -= instance.OnDown;
            @Down.performed -= instance.OnDown;
            @Down.canceled -= instance.OnDown;
            @Left.started -= instance.OnLeft;
            @Left.performed -= instance.OnLeft;
            @Left.canceled -= instance.OnLeft;
            @Right.started -= instance.OnRight;
            @Right.performed -= instance.OnRight;
            @Right.canceled -= instance.OnRight;
        }

        public void RemoveCallbacks(IAttackMapActions instance)
        {
            if (m_Wrapper.m_AttackMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IAttackMapActions instance)
        {
            foreach (var item in m_Wrapper.m_AttackMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_AttackMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public AttackMapActions @AttackMap => new AttackMapActions(this);

    // MoveMap
    private readonly InputActionMap m_MoveMap;
    private List<IMoveMapActions> m_MoveMapActionsCallbackInterfaces = new List<IMoveMapActions>();
    private readonly InputAction m_MoveMap_Up;
    private readonly InputAction m_MoveMap_Down;
    private readonly InputAction m_MoveMap_Left;
    private readonly InputAction m_MoveMap_Right;
    public struct MoveMapActions
    {
        private @InputController m_Wrapper;
        public MoveMapActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Up => m_Wrapper.m_MoveMap_Up;
        public InputAction @Down => m_Wrapper.m_MoveMap_Down;
        public InputAction @Left => m_Wrapper.m_MoveMap_Left;
        public InputAction @Right => m_Wrapper.m_MoveMap_Right;
        public InputActionMap Get() { return m_Wrapper.m_MoveMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveMapActions set) { return set.Get(); }
        public void AddCallbacks(IMoveMapActions instance)
        {
            if (instance == null || m_Wrapper.m_MoveMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MoveMapActionsCallbackInterfaces.Add(instance);
            @Up.started += instance.OnUp;
            @Up.performed += instance.OnUp;
            @Up.canceled += instance.OnUp;
            @Down.started += instance.OnDown;
            @Down.performed += instance.OnDown;
            @Down.canceled += instance.OnDown;
            @Left.started += instance.OnLeft;
            @Left.performed += instance.OnLeft;
            @Left.canceled += instance.OnLeft;
            @Right.started += instance.OnRight;
            @Right.performed += instance.OnRight;
            @Right.canceled += instance.OnRight;
        }

        private void UnregisterCallbacks(IMoveMapActions instance)
        {
            @Up.started -= instance.OnUp;
            @Up.performed -= instance.OnUp;
            @Up.canceled -= instance.OnUp;
            @Down.started -= instance.OnDown;
            @Down.performed -= instance.OnDown;
            @Down.canceled -= instance.OnDown;
            @Left.started -= instance.OnLeft;
            @Left.performed -= instance.OnLeft;
            @Left.canceled -= instance.OnLeft;
            @Right.started -= instance.OnRight;
            @Right.performed -= instance.OnRight;
            @Right.canceled -= instance.OnRight;
        }

        public void RemoveCallbacks(IMoveMapActions instance)
        {
            if (m_Wrapper.m_MoveMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMoveMapActions instance)
        {
            foreach (var item in m_Wrapper.m_MoveMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MoveMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MoveMapActions @MoveMap => new MoveMapActions(this);
    public interface IAttackMapActions
    {
        void OnUp(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
    }
    public interface IMoveMapActions
    {
        void OnUp(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
    }
}