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
        [SerializeField] private SpawnBodyButtonScript spawnButton;
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

            buttonScripts[5].click = () => AddBodyAndEat(leaderBodySpawn.localPosition);
            buttonScripts[4].click = () => Restart();
        }

        void Update()
        {
            
            Move();
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
            
            // for (int i = 0; i < slaveElement.Count; i++)
            // {
            //     if (leader.localPosition == slaveElement[i].transform.localPosition)
            //     {
            //         endGameTitle.SetActive(true);
            //         Time.timeScale = 0;
            //     }   
            // }
            
                var bodyElement = Instantiate(bodyPrefab, leaderBodySpawn);
                slaveElement.Add(bodyElement);
                bodyElement.transform.localPosition = position;

                MoveEatAfterAdd();

                score.text = Convert.ToString(slaveElement.Count);
        }
        
        private void GetDirection(Transform lTransform)
        {
            switch (_movementState)
            {
                case MoveState.MoveUp:
                    lTransform.Translate(Vector3.up * stepLength * Time.deltaTime) ;
                    break;
                case MoveState.MoveDown:
                    lTransform.Translate(Vector3.down * stepLength * Time.deltaTime);
                    break;
                case MoveState.MoveLeft:
                    lTransform.Translate(Vector3.left * stepLength * Time.deltaTime);
                    break;
                case MoveState.MoveRight:
                    lTransform.Translate(Vector3.right * stepLength * Time.deltaTime);
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
            foreach (var element in slaveElement)
            {
                var newLoc = element.transform.localPosition;
                element.transform.localPosition = pos;
                pos = newLoc;
            }
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
    }
}
