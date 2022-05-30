using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        [SerializeField] private Text score;
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

            buttonScripts[4].click = () => Restart();
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

            TemporarySwitch();
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
                    endGameTitle.SetActive(true);
                    Time.timeScale = 0;
                }   
            }
            
            if (leader.localPosition == eat.transform.localPosition)
            {
                var bodyElement = Instantiate(bodyPrefab, leaderBodySpawn);
                slaveElement.Add(bodyElement);
                bodyElement.transform.localPosition = position;

                MoveEatAfterAdd();
            }
            
            score.text = Convert.ToString(slaveElement.Count);
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
            MovingEdgesScene();
        }

        private void Restart()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }

        private void MovingEdgesScene()
        {
            if (leader.localPosition.y > 600)
            {
                leader.localPosition = new Vector3(leader.localPosition.x, -600, 0);
            }

            if (leader.localPosition.y < -600)
            {
                leader.localPosition = new Vector3(leader.localPosition.x, 600, 0);
            }

            if (leader.localPosition.x < -800)
            {
                leader.localPosition = new Vector3(1300, leader.localPosition.y, 0);
            }
            
            if (leader.localPosition.x > 1300)
            {
                leader.localPosition = new Vector3(-800, leader.localPosition.y, 0);
            }
        }

        private void TemporarySwitch()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _movementState = MoveState.MoveUp;
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _movementState = MoveState.MoveDown;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _movementState = MoveState.MoveLeft;
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _movementState = MoveState.MoveRight;
            }

            DirectionTemporarySwitch();
        }
        
        void DirectionTemporarySwitch()
        {
            var mov = _movementState;
            
            switch (_movementState)
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
            _movementState = mov;
        }
    }
}
