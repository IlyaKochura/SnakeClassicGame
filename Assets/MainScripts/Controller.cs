using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace MainScripts
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private List<ButtonScript> buttonScripts;
        [SerializeField] private int speed = 1;
        [SerializeField] private List<GameObject> slaveElement;
        [SerializeField] private Transform leader;
        [SerializeField] private float stepLength;
        [SerializeField] private GameObject endGameTitle;
        [SerializeField] private GameObject eat;
        [SerializeField] private GameObject bodyPrefab;
        [SerializeField] private Transform leaderBodySpawn;
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

        

        private void AddBodyAndEat(Vector3 position)
        {
            
            for (int i = 0; i < slaveElement.Count; i++)
            {
                if (leader.localPosition == slaveElement[i].transform.localPosition)
                {
                    // endGameTitle.SetActive(true);
                    Debug.Break();
                }   
            }
            
            if (leader.localPosition == eat.transform.localPosition)
            {
                var bodyElement = Instantiate(bodyPrefab, leaderBodySpawn);
                slaveElement.Add(bodyElement);
                bodyElement.transform.localPosition = position;

                MoveEatAfterAdd();
            }
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

        private void MoveEatAfterAdd()
        {
            Random rnd = new Random();
            var posY = rnd.Next(-6, 6);
            var posX = rnd.Next(-8, 13);

            eat.transform.localPosition = new Vector3(posX * 100, posY * 100, 0);
        }
        
        private void Move()
        {
            var pos = leader.localPosition;
            leader.localPosition = GetDirection();
            foreach (var element in slaveElement)
            {
                var newLoc = element.transform.localPosition;
                element.transform.localPosition = pos;
                pos = newLoc;
            }
            AddBodyAndEat(pos);
        }
    }
}
