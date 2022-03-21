using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Guanomancer.EventRouting
{
    public class GameEventDispatcher : MonoBehaviour
    {
        [SerializeField]
        [Header("Event Options")]
        private GameEvent _gameEvent;
        private GameEventContextType _contextType;

        [Header("Context Info")]
        private string _stringContext;
        private int _integerContext;
        private float _decimalContext;
        private bool _booleanContext;
        private Vector2 _vector2Context;
        private Vector3 _vector3Context;

        public void Dispatch()
        {
            _gameEvent.Dispatch(this, GetContext());
        }

        public object GetContext()
        {
            switch(_contextType)
            {
                case GameEventContextType.String:
                    return _stringContext;
                case GameEventContextType.Integer:
                    return _integerContext;
                case GameEventContextType.Decimal:
                    return _decimalContext;
                case GameEventContextType.Boolean:
                    return _booleanContext;
                case GameEventContextType.Vector2:
                    return _vector2Context;
                case GameEventContextType.Vector3:
                    return _vector3Context;
                default:
                    return null;
            }
        }
    }

    public enum GameEventContextType
    {
        String,
        Integer,
        Decimal,
        Boolean,
        Vector2,
        Vector3,
    }
}
