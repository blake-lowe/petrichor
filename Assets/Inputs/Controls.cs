// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""3bed5020-2aa2-4e9a-8dbb-076f1c81a790"",
            ""actions"": [
                {
                    ""name"": ""sprint"",
                    ""type"": ""Button"",
                    ""id"": ""be6cb3a7-d89a-4040-a72e-00521d23f2bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""crouch"",
                    ""type"": ""Button"",
                    ""id"": ""1074f229-a434-4ac4-b317-a29e367ab3b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""rightHand"",
                    ""type"": ""Button"",
                    ""id"": ""91bb2edf-551e-4821-b22b-663cc05c2826"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""leftHand"",
                    ""type"": ""Button"",
                    ""id"": ""07708318-de15-4f75-8080-f423f595f547"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""rightLeg"",
                    ""type"": ""Button"",
                    ""id"": ""dc6db2cd-12cc-459d-b94d-fcf97772b518"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""leftLeg"",
                    ""type"": ""Button"",
                    ""id"": ""e1367cf9-bca5-418d-8e56-52046beb2a30"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""pause"",
                    ""type"": ""Button"",
                    ""id"": ""ec4c0600-4420-40dc-b512-42c4b34aebbd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""direction"",
                    ""type"": ""Button"",
                    ""id"": ""5e62b521-d7b5-41db-b9ae-1945c2f14e89"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""6e482a35-818a-4a4c-89e9-11b8bc3d6700"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""Normalize(max=1)"",
                    ""groups"": """",
                    ""action"": ""direction"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f5b36038-b306-4769-8273-82b20bf12643"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ce0caa78-c5dc-4fa7-8f72-abe606ebc8cc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""08bd25c7-387c-4109-a407-f93a5a71d552"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""73ad1e56-a96e-403b-90b0-b3a23fd4f7d5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0e6a999a-7037-4447-b26b-165c9fd9a68e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""321322b6-1e70-42f9-985f-7eb800b28bb4"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f7b513f-c237-4f20-885c-1ddcf10a3422"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""leftLeg"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb4053f9-d20f-4f8c-9883-7c5f24e480c0"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""rightLeg"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5856bb5e-2568-4f0c-a2e1-9063c1791fbe"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""leftHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3686c6a-e107-4442-a339-a69758fe99bb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""rightHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""faf5ca9b-81e3-419c-ae2b-5f159bb6b8b8"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3342a7e3-be47-4a84-9dfd-b71dca036d16"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KBM"",
                    ""action"": ""sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KBM"",
            ""bindingGroup"": ""KBM"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_sprint = m_Player.FindAction("sprint", throwIfNotFound: true);
        m_Player_crouch = m_Player.FindAction("crouch", throwIfNotFound: true);
        m_Player_rightHand = m_Player.FindAction("rightHand", throwIfNotFound: true);
        m_Player_leftHand = m_Player.FindAction("leftHand", throwIfNotFound: true);
        m_Player_rightLeg = m_Player.FindAction("rightLeg", throwIfNotFound: true);
        m_Player_leftLeg = m_Player.FindAction("leftLeg", throwIfNotFound: true);
        m_Player_pause = m_Player.FindAction("pause", throwIfNotFound: true);
        m_Player_direction = m_Player.FindAction("direction", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_sprint;
    private readonly InputAction m_Player_crouch;
    private readonly InputAction m_Player_rightHand;
    private readonly InputAction m_Player_leftHand;
    private readonly InputAction m_Player_rightLeg;
    private readonly InputAction m_Player_leftLeg;
    private readonly InputAction m_Player_pause;
    private readonly InputAction m_Player_direction;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @sprint => m_Wrapper.m_Player_sprint;
        public InputAction @crouch => m_Wrapper.m_Player_crouch;
        public InputAction @rightHand => m_Wrapper.m_Player_rightHand;
        public InputAction @leftHand => m_Wrapper.m_Player_leftHand;
        public InputAction @rightLeg => m_Wrapper.m_Player_rightLeg;
        public InputAction @leftLeg => m_Wrapper.m_Player_leftLeg;
        public InputAction @pause => m_Wrapper.m_Player_pause;
        public InputAction @direction => m_Wrapper.m_Player_direction;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @sprint.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @sprint.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @sprint.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSprint;
                @crouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @crouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @crouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @rightHand.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightHand;
                @rightHand.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightHand;
                @rightHand.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightHand;
                @leftHand.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftHand;
                @leftHand.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftHand;
                @leftHand.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftHand;
                @rightLeg.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightLeg;
                @rightLeg.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightLeg;
                @rightLeg.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightLeg;
                @leftLeg.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftLeg;
                @leftLeg.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftLeg;
                @leftLeg.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftLeg;
                @pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @direction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDirection;
                @direction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDirection;
                @direction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDirection;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @sprint.started += instance.OnSprint;
                @sprint.performed += instance.OnSprint;
                @sprint.canceled += instance.OnSprint;
                @crouch.started += instance.OnCrouch;
                @crouch.performed += instance.OnCrouch;
                @crouch.canceled += instance.OnCrouch;
                @rightHand.started += instance.OnRightHand;
                @rightHand.performed += instance.OnRightHand;
                @rightHand.canceled += instance.OnRightHand;
                @leftHand.started += instance.OnLeftHand;
                @leftHand.performed += instance.OnLeftHand;
                @leftHand.canceled += instance.OnLeftHand;
                @rightLeg.started += instance.OnRightLeg;
                @rightLeg.performed += instance.OnRightLeg;
                @rightLeg.canceled += instance.OnRightLeg;
                @leftLeg.started += instance.OnLeftLeg;
                @leftLeg.performed += instance.OnLeftLeg;
                @leftLeg.canceled += instance.OnLeftLeg;
                @pause.started += instance.OnPause;
                @pause.performed += instance.OnPause;
                @pause.canceled += instance.OnPause;
                @direction.started += instance.OnDirection;
                @direction.performed += instance.OnDirection;
                @direction.canceled += instance.OnDirection;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KBMSchemeIndex = -1;
    public InputControlScheme KBMScheme
    {
        get
        {
            if (m_KBMSchemeIndex == -1) m_KBMSchemeIndex = asset.FindControlSchemeIndex("KBM");
            return asset.controlSchemes[m_KBMSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnSprint(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnRightHand(InputAction.CallbackContext context);
        void OnLeftHand(InputAction.CallbackContext context);
        void OnRightLeg(InputAction.CallbackContext context);
        void OnLeftLeg(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnDirection(InputAction.CallbackContext context);
    }
}
