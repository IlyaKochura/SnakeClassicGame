using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

namespace MainScripts
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private List<ButtonScript> buttonScripts;
<<<<<<< Updated upstream
        [SerializeField] private List<GameObject> slaveElement;
=======
>>>>>>> Stashed changes
        [SerializeField] private Transform leader;
        [SerializeField] private float speed;
        [SerializeField] private GameObject endGameTitle;
        [SerializeField] private GameObject eat;
        [SerializeField] private GameObject bodyPrefab;
        [SerializeField] private Transform leaderBodySpawn;
        [SerializeField] private Text score;
        [SerializeField] private SpawnBodyButtonScript spawnButton;
        private List<GameObject> _slaveElement = new();
        private List<Vector3> _nextPosition = new();
        private int _id;
<<<<<<< Updated upstream
=======
        private int _currentId;
>>>>>>> Stashed changes
        private MoveState _movementState = MoveState.MoveUp;
        private Axis _axis = Axis.AxisY;

        private enum MoveState
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight
        }

        private enum Axis
        {
            AxisY,
            AxisX
        }

        void Start()
        {
            for (int i = 0; i < buttonScripts.Count; i++)
            {
                var id = i;
                buttonScripts[i].click = () => Transmitter(id);
            }

            buttonScripts[4].click = () => Restart();
            buttonScripts[5].click = () => AddBodyAndEat();
        }

        void Update()
        {
            switch (_axis)
            {
                case Axis.AxisY:
                    if (leader.localPosition.y % 100 > -1 && leader.localPosition.y % 100 < 1)
                    {
                        Direction(_id);
                        UpdateCashPosition();
                    }

                    break;

                case Axis.AxisX:
                    if (leader.localPosition.x % 100 > -1 && leader.localPosition.x % 100 < 1)
                    {
                        Direction(_id);
                        UpdateCashPosition();
                    }

                    break;
            }
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
            Move();
        }

        private void UpdateCashPosition()
        {
            _nextPosition.Clear();

            if (_slaveElement.Count <= 0) return;
            for (var i = 0; i < _slaveElement.Count; i++)
            {
                if (i <= 0)
                {
                    _nextPosition.Add(leader.localPosition);
                }
                else
                {
                    _nextPosition.Add(_slaveElement[i - 1].transform.localPosition);
                }

                //_slaveElement[i].transform.localPosition = _nextPosition[i];
            }
            
            for (var i = 0; i < _slaveElement.Count; i++)
            {
                _slaveElement[i].transform.localPosition = _nextPosition[i];
            }
        }

        void Direction(int dir)
        {
            var i = (MoveState)dir;

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
<<<<<<< Updated upstream
        
        private void AddBodyAndEat(Vector3 position)
        {
            var bodyElement = Instantiate(bodyPrefab, leaderBodySpawn);
                slaveElement.Add(bodyElement);
                bodyElement.transform.localPosition = position;

                MoveEatAfterAdd();
=======


        private void AddBodyAndEat()
        {
            // for (int i = 0; i < slaveElement.Count; i++)
            // {
            //     if (leader.localPosition == slaveElement[i].transform.localPosition)
            //     {
            //         endGameTitle.SetActive(true);
            //         Time.timeScale = 0;
            //     }   
            // }
            
                var bodyElement = Instantiate(bodyPrefab, leaderBodySpawn);
                _slaveElement.Add(bodyElement);
                bodyElement.transform.localPosition = leader.localPosition;
                
            
>>>>>>> Stashed changes

            score.text = Convert.ToString(_slaveElement.Count);
        }

        private void GetDirection(Transform lTransform)
        {
            int posX;
            int posY;
            switch (_movementState)
            {
                case MoveState.MoveUp:
                    lTransform.Translate(Vector3.up * speed * Time.deltaTime);
                    if (leader.localPosition.x % 100 != 0)
                    {
                        _axis = Axis.AxisY;
                        posX = Convert.ToInt32(Math.Round(leader.localPosition.x / 100) * 100);
                        leader.localPosition = new Vector3(posX, leader.localPosition.y);
                    }

                    break;

                case MoveState.MoveDown:
                    lTransform.Translate(Vector3.down * speed * Time.deltaTime);
                    if (leader.localPosition.x % 100 != 0)
                    {
                        _axis = Axis.AxisY;
                        posX = Convert.ToInt32(Math.Round(leader.localPosition.x / 100) * 100);
                        leader.localPosition = new Vector3(posX, leader.localPosition.y);
                    }

                    break;

                case MoveState.MoveLeft:
                    lTransform.Translate(Vector3.left * speed * Time.deltaTime);
                    if (leader.localPosition.y % 100 != 0)
                    {
                        _axis = Axis.AxisX;
                        posY = Convert.ToInt32(Math.Round(leader.localPosition.y / 100) * 100);
                        leader.localPosition = new Vector3(leader.localPosition.x, posY);
                    }

                    break;

                case MoveState.MoveRight:
                    lTransform.Translate(Vector3.right * speed * Time.deltaTime);
                    if (leader.localPosition.y % 100 != 0)
                    {
                        _axis = Axis.AxisX;
                        posY = Convert.ToInt32(Math.Round(leader.localPosition.y / 100) * 100);
                        leader.localPosition = new Vector3(leader.localPosition.x, posY);
                    }

                    break;
            }
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
            GetDirection(leader);
            var pos = leader.localPosition;
<<<<<<< Updated upstream
            foreach (var element in slaveElement)
            {
                var newLoc = element.transform.localPosition;
                element.transform.localPosition = pos;
                pos = newLoc;
            }
=======
            // foreach (var element in _slaveElement)
            // {
            //     var newLoc = element.transform.localPosition;
            //     element.transform.localPosition = new Vector3(newLoc.x + 100, newLoc.y + 100);
            //     element.transform.localPosition = pos;
            //     pos = newLoc;
            // }

>>>>>>> Stashed changes
            MovingEdgesScene();
            //AddBodyAndEat();
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
        
        private void Transmitter(int id)
        {
            _id = id;
        }
<<<<<<< Updated upstream
=======


        // void DirectionTemporarySwitch()
        // {
        //     var mov = _movementState;
        //     
        //     switch (_movementState)
        //     {
        //         case MoveState.MoveUp:
        //             if (_movementState == MoveState.MoveDown)
        //             {
        //                 return;
        //             }
        //             break;
        //         case MoveState.MoveDown:
        //             if (_movementState == MoveState.MoveUp)
        //             {
        //                 return;
        //             }
        //             break;
        //         case MoveState.MoveLeft:
        //             if (_movementState == MoveState.MoveRight)
        //             {
        //                 return;
        //             }
        //             break;
        //         case MoveState.MoveRight:
        //             if (_movementState == MoveState.MoveLeft)
        //             {
        //                 return;
        //             }
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        //     _movementState = mov;
        // }
>>>>>>> Stashed changes
    }
}