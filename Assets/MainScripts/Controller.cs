using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainScripts
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private List<ButtonScript> buttonScripts;
        [SerializeField] private int speed = 1;
        [SerializeField] private List<Transform> slaveElement;
        [SerializeField] private Transform leader;
        [SerializeField] private float stepLength;
        private float _time = 1;
        private MoveState _movementState = MoveState.MoveUp;

        private enum MoveState
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight
        }

        void Start()
        {
            for (int i = 0; i < buttonScripts.Count; i++)
            {
                var id = i;
                buttonScripts[i].click = () => Direction(id);
            }
        }

        void Update()
        {
            if (_time > 0)
            {
                _time -= Time.deltaTime * speed;
            }
            else
            {
                Move();
                //SlaveMove();
                _time = 1;
            }
        }

        void Direction(int dir)
        {
            var i = (MoveState) dir;

            switch (i)
            {
                case MoveState.MoveUp:
                    if (_movementState == MoveState.MoveDown)
                    {
                        return;
                    }
                    break;
                case MoveState.MoveDown:
                    if (_movementState == MoveState.MoveUp)
                    {
                        return;
                    }
                    break;
                case MoveState.MoveLeft:
                    if (_movementState == MoveState.MoveRight)
                    {
                        return;
                    }
                    break;
                case MoveState.MoveRight:
                    if (_movementState == MoveState.MoveLeft)
                    {
                        return;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _movementState = i;
        }

        private Vector3 GetDirection()
        {
            var position = leader.localPosition;

            switch (_movementState)
            {
                case MoveState.MoveUp:
                    
                    return new Vector3(position.x, position.y + stepLength);

                case MoveState.MoveDown:
                    return new Vector3(position.x, position.y - stepLength);

                case MoveState.MoveLeft:
                    return new Vector3(position.x - stepLength, position.y);

                case MoveState.MoveRight:
                    return new Vector3(position.x + stepLength, position.y);
            }

            return new Vector3();
        }

        private void Move()
        {
            var pos = leader.localPosition;
            leader.localPosition = GetDirection();
            foreach (var element in slaveElement)
            {
                var newLoc = element.localPosition;
                element.localPosition = pos;
                pos = newLoc;
            }
            
        }

        private void SlaveMove()
        {
            for (int i = slaveElement.Count; i > 0; i--)
            {
                var position = slaveElement[i - 1].localPosition;
                slaveElement[i].transform.position = new Vector3(position.x, position.y);
            }
        }
    }
}
