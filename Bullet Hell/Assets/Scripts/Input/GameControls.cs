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
        },
        {
            ""name"": ""WeaponBar"",
            ""id"": ""6944fa41-921d-442e-b61e-a41ab326ba9e"",
            ""actions"": [
                {
                    ""name"": ""Cast_1"",
                    ""type"": ""Button"",
                    ""id"": ""b0c65a32-ec46-47e5-9b28-0b77a1cdc21e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cast_2"",
                    ""type"": ""Button"",
                    ""id"": ""950b9659-8b5d-479d-91e2-0464dc4c3738"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cast_3"",
                    ""type"": ""Button"",
                    ""id"": ""01755fe2-4ffe-4b80-9cc2-c237545dfbc4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f4157c4b-4039-447a-adb5-ec9f5a7fdef5"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cast_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""597e8bb6-45f8-4e4c-886c-8f5db7b278d5"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cast_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29be4adf-0e1c-4a64-80d0-11257e046a20"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cast_3"",
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
            // WeaponBar
            m_WeaponBar = asset.FindActionMap("WeaponBar", throwIfNotFound: true);
            m_WeaponBar_Cast_1 = m_WeaponBar.FindAction("Cast_1", throwIfNotFound: true);
            m_WeaponBar_Cast_2 = m_WeaponBar.FindAction("Cast_2", throwIfNotFound: true);
            m_WeaponBar_Cast_3 = m_WeaponBar.FindAction("Cast_3", throwIfNotFound: true);
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

        // WeaponBar
        private readonly InputActionMap m_WeaponBar;
        private IWeaponBarActions m_WeaponBarActionsCallbackInterface;
        private readonly InputAction m_WeaponBar_Cast_1;
        private readonly InputAction m_WeaponBar_Cast_2;
        private readonly InputAction m_WeaponBar_Cast_3;
        public struct WeaponBarActions
        {
            private @GameControls m_Wrapper;
            public WeaponBarActions(@GameControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Cast_1 => m_Wrapper.m_WeaponBar_Cast_1;
            public InputAction @Cast_2 => m_Wrapper.m_WeaponBar_Cast_2;
            public InputAction @Cast_3 => m_Wrapper.m_WeaponBar_Cast_3;
            public InputActionMap Get() { return m_Wrapper.m_WeaponBar; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(WeaponBarActions set) { return set.Get(); }
            public void SetCallbacks(IWeaponBarActions instance)
            {
                if (m_Wrapper.m_WeaponBarActionsCallbackInterface != null)
                {
                    @Cast_1.started -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_1;
                    @Cast_1.performed -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_1;
                    @Cast_1.canceled -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_1;
                    @Cast_2.started -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_2;
                    @Cast_2.performed -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_2;
                    @Cast_2.canceled -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_2;
                    @Cast_3.started -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_3;
                    @Cast_3.performed -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_3;
                    @Cast_3.canceled -= m_Wrapper.m_WeaponBarActionsCallbackInterface.OnCast_3;
                }
                m_Wrapper.m_WeaponBarActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Cast_1.started += instance.OnCast_1;
                    @Cast_1.performed += instance.OnCast_1;
                    @Cast_1.canceled += instance.OnCast_1;
                    @Cast_2.started += instance.OnCast_2;
                    @Cast_2.performed += instance.OnCast_2;
                    @Cast_2.canceled += instance.OnCast_2;
                    @Cast_3.started += instance.OnCast_3;
                    @Cast_3.performed += instance.OnCast_3;
                    @Cast_3.canceled += instance.OnCast_3;
                }
            }
        }
        public WeaponBarActions @WeaponBar => new WeaponBarActions(this);
        public interface IMovementActions
        {
            void OnDirection(InputAction.CallbackContext context);
            void OnCursorPosition(InputAction.CallbackContext context);
        }
        public interface ICombatActions
        {
            void OnShoot(InputAction.CallbackContext context);
        }
        public interface IWeaponBarActions
        {
            void OnCast_1(InputAction.CallbackContext context);
            void OnCast_2(InputAction.CallbackContext context);
            void OnCast_3(InputAction.CallbackContext context);
        }
    }
}
