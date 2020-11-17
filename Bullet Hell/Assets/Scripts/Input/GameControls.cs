// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Input
{
    public class @GameControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""d3a56e3d-3992-456d-b2bf-e73a243987bd"",
            ""actions"": [
                {
                    ""name"": ""Direction"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f0bb05eb-2e67-499d-a01c-0d11b2bbb84f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CursorPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f569326c-41aa-461e-9418-c204e3628563"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2DVector"",
                    ""id"": ""08f91d0b-b129-40c9-a114-e40570878f5e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e00e46ca-05d2-4ab4-88da-78b7fb1b0344"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""77565bb7-0138-4cb5-be90-7292da90bdb1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9a19fe95-5a36-438f-acbd-3422eb89ad4d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""32f55e5f-40b3-433e-95c6-7b587d1706d4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""386c6889-9778-4415-9c7f-309ecac2ec57"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Combat"",
            ""id"": ""59ab8bb2-6ec8-4d2c-80a2-f7503ceb246a"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0f0b3f69-8db1-4b79-b362-8fdc7f3f1de4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""47d07dac-cb1b-4e5b-9c82-80095e0eeb5f"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Movement
            m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
            m_Movement_Direction = m_Movement.FindAction("Direction", throwIfNotFound: true);
            m_Movement_CursorPosition = m_Movement.FindAction("CursorPosition", throwIfNotFound: true);
            // Combat
            m_Combat = asset.FindActionMap("Combat", throwIfNotFound: true);
            m_Combat_Shoot = m_Combat.FindAction("Shoot", throwIfNotFound: true);
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

        // Movement
        private readonly InputActionMap m_Movement;
        private IMovementActions m_MovementActionsCallbackInterface;
        private readonly InputAction m_Movement_Direction;
        private readonly InputAction m_Movement_CursorPosition;
        public struct MovementActions
        {
            private @GameControls m_Wrapper;
            public MovementActions(@GameControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Direction => m_Wrapper.m_Movement_Direction;
            public InputAction @CursorPosition => m_Wrapper.m_Movement_CursorPosition;
            public InputActionMap Get() { return m_Wrapper.m_Movement; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
            public void SetCallbacks(IMovementActions instance)
            {
                if (m_Wrapper.m_MovementActionsCallbackInterface != null)
                {
                    @Direction.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnDirection;
                    @Direction.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnDirection;
                    @Direction.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnDirection;
                    @CursorPosition.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnCursorPosition;
                    @CursorPosition.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnCursorPosition;
                    @CursorPosition.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnCursorPosition;
                }
                m_Wrapper.m_MovementActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Direction.started += instance.OnDirection;
                    @Direction.performed += instance.OnDirection;
                    @Direction.canceled += instance.OnDirection;
                    @CursorPosition.started += instance.OnCursorPosition;
                    @CursorPosition.performed += instance.OnCursorPosition;
                    @CursorPosition.canceled += instance.OnCursorPosition;
                }
            }
        }
        public MovementActions @Movement => new MovementActions(this);

        // Combat
        private readonly InputActionMap m_Combat;
        private ICombatActions m_CombatActionsCallbackInterface;
        private readonly InputAction m_Combat_Shoot;
        public struct CombatActions
        {
            private @GameControls m_Wrapper;
            public CombatActions(@GameControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Shoot => m_Wrapper.m_Combat_Shoot;
            public InputActionMap Get() { return m_Wrapper.m_Combat; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CombatActions set) { return set.Get(); }
            public void SetCallbacks(ICombatActions instance)
            {
                if (m_Wrapper.m_CombatActionsCallbackInterface != null)
                {
                    @Shoot.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnShoot;
                    @Shoot.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnShoot;
                    @Shoot.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnShoot;
                }
                m_Wrapper.m_CombatActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Shoot.started += instance.OnShoot;
                    @Shoot.performed += instance.OnShoot;
                    @Shoot.canceled += instance.OnShoot;
                }
            }
        }
        public CombatActions @Combat => new CombatActions(this);
        public interface IMovementActions
        {
            void OnDirection(InputAction.CallbackContext context);
            void OnCursorPosition(InputAction.CallbackContext context);
        }
        public interface ICombatActions
        {
            void OnShoot(InputAction.CallbackContext context);
        }
    }
}
