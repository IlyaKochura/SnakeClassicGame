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
        [SerializeField] private List<GameObject> slaveElement;
        [SerializeField] private List<Vector3> slaveElementPosition;
        [SerializeField] private Transform leader;
        [SerializeField] private float speed;
        [SerializeField] private GameObject endGameTitle;
        [SerializeField] private GameObject eat;
        [SerializeField] private GameObject bodyPrefab;
        [SerializeField] private Transform leaderBodySpawn;
        [SerializeField] private Text score;
        [SerializeField] private float sidelenght;
        [SerializeField] private float mb;
        private int _id;
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
            
            slaveElementPosition.Add(leader.position);
            // buttonScripts[5].click = () => AddBodyAndEat();
            buttonScripts[4].click = () => Restart();
        }

        void Update()
        {
            switch (_axis)
            {
                case Axis.AxisY :
                    if (leader.localPosition.y % 100 > -1 && leader.localPosition.y % 100 < 1)
                    {
                        Direction(_id);
                    }
                    break;
                
                case Axis.AxisX :
                    if (leader.localPosition.x % 100 > -1 && leader.localPosition.x % 100 < 1)
                    {
                        Direction(_id);
                    }
                    break;
            }

            
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
        
        private void AddBodyAndEat()
        {
            var eatPosY = eat.transform.localPosition.y;
            var eatPosX = eat.transform.localPosition.x;
            if ( (leader.localPosition.x - eatPosX > -mb && leader.localPosition.x - eatPosX < mb) &&
                 (leader.localPosition.y - eatPosY > -mb && leader.localPosition.y - eatPosY < mb))
            {
                var bodyElement = Instantiate(bodyPrefab, slaveElementPosition[slaveElement.Count], Quaternion.identity,
                    leaderBodySpawn);
                slaveElement.Add(bodyElement);
                slaveElementPosition.Add(bodyElement.transform.localPosition);
                MoveEatAfterAdd();
            }

            score.text = Convert.ToString(slaveElement.Count);
        }
        
        private void GetDirection(Transform lTransform, MoveState moveState)
        {
            int posX;
            int posY;
            switch (moveState)
            {
                case MoveState.MoveUp:
                    lTransform.Translate(Vector3.up * speed * Time.deltaTime);
                    if (lTransform.localPosition.x % 100 != 0)
                    {
                        _axis = Axis.AxisY;
                        posX = Convert.ToInt32(Math.Round(lTransform.localPosition.x / 100) * 100);
                        lTransform.localPosition = new Vector3(posX, lTransform.localPosition.y);
                    }
                    break;
                
                case MoveState.MoveDown:
                    lTransform.Translate(Vector3.down * speed * Time.deltaTime);
                    if (lTransform.localPosition.x % 100 != 0)
                    {
                        _axis = Axis.AxisY;
                       posX = Convert.ToInt32(Math.Round(lTransform.localPosition.x / 100) * 100);
                       lTransform.localPosition = new Vector3(posX, lTransform.localPosition.y);
                    }
                    
                    break;
                
                case MoveState.MoveLeft:
                    lTransform.Translate(Vector3.left * speed * Time.deltaTime);
                    if (lTransform.localPosition.y % 100 != 0)
                    {
                        _axis = Axis.AxisX;
                        posY = Convert.ToInt32(Math.Round(lTransform.localPosition.y / 100) * 100);
                        lTransform.localPosition = new Vector3(lTransform.localPosition.x, posY);
                    }
                    
                    break;
                
                case MoveState.MoveRight:
                    lTransform.Translate(Vector3.right * speed * Time.deltaTime);
                    if (lTransform.localPosition.y % 100 != 0)
                    {
                        _axis = Axis.AxisX;
                        posY = Convert.ToInt32(Math.Round(lTransform.localPosition.y / 100) * 100);
                        lTransform.localPosition = new Vector3(lTransform.localPosition.x, posY);
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
            GetDirection(leader, _movementState);
            var distance = (leader.localPosition - slaveElementPosition[0]).magnitude;
             if (distance > sidelenght)
             {
                 slaveElementPosition.Insert(0, leader.localPosition);
                 slaveElementPosition.RemoveAt(slaveElementPosition.Count - 1);
             }

            for (int i = 0; i < slaveElement.Count; i++)
            {
                slaveElement[i].transform.localPosition =
                    Vector3.Lerp(slaveElementPosition[i + 1], slaveElementPosition[i], distance / sidelenght);
            }

            AddBodyAndEat();
            MovingEdgesScene();
        }
        
        private void Restart()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
        
        private void MovingEdgesScene()
        {
            if (leader.localPosition.y > 620)
            {
                leader.localPosition = new Vector3(leader.localPosition.x, -600, 0);
            }

            if (leader.localPosition.y < -620)
            {
                leader.localPosition = new Vector3(leader.localPosition.x, 600, 0);
            }

            if (leader.localPosition.x < -820)
            {
                leader.localPosition = new Vector3(1300, leader.localPosition.y, 0);
            }
            
            if (leader.localPosition.x > 1320)
            {
                leader.localPosition = new Vector3(-800, leader.localPosition.y, 0);
            }
        }
        
        private void Transmitter(int id)
        {
            _id = id;
        }
    }
}
