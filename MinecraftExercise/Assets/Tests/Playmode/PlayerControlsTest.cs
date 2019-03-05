using System.Collections;
using GameMechanics;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerControlsTest
    {
        private GameObject gameObject;
        private PlayerControls playerControls;


        public void PrepareGameObject()
        {
            gameObject = new GameObject();
            playerControls = gameObject.AddComponent<PlayerControls>();
            gameObject.AddComponent<CharacterController>();
        }

        [UnityTest]
        public IEnumerator MovementTest()
        {
            PrepareGameObject();
            IPlayerMovementInput input = Substitute.For<IPlayerMovementInput>();
            Vector3 movementVector = new Vector3(1, 0, 1);
            input.GetMovementVector().Returns(movementVector);
            input.IsPointerOverUi().Returns(true);

            playerControls.input = input;
            playerControls.speed = 1;

            Vector3 previousPosition = gameObject.transform.position;

            yield return null;

            Assert.True(previousPosition + movementVector * Time.deltaTime == gameObject.transform.position);

            playerControls.speed = 10;
            previousPosition = gameObject.transform.position;
            yield return null;

            Assert.True(previousPosition + movementVector * Time.deltaTime * 10 == gameObject.transform.position);

            movementVector = new Vector3(0.5f, 0, 1.75f);
            input.GetMovementVector().Returns(movementVector);
            previousPosition = gameObject.transform.position;
            playerControls.speed = 5;
            yield return null;

            Assert.True(previousPosition + movementVector * Time.deltaTime * 5 == gameObject.transform.position);
        }

        [UnityTest]
        public IEnumerator GravityTest()
        {
            PrepareGameObject();
            IPlayerMovementInput input = Substitute.For<IPlayerMovementInput>();
            input.GetMovementVector().Returns(Vector3.zero);
            input.IsPointerOverUi().Returns(true);

            playerControls.input = input;
            playerControls.gravity = 10;

            yield return null;
            float gravityY = -10 * Time.deltaTime;
            float expectedY = gravityY * Time.deltaTime;

            Assert.True(Mathf.Approximately(expectedY, gameObject.transform.position.y));

            for (int i = 0; i < 100; i++)
            {
                yield return null;

                gravityY = gravityY - 10 * Time.deltaTime;
                expectedY = expectedY + gravityY * Time.deltaTime;

                Assert.True(Mathf.Approximately(expectedY, gameObject.transform.position.y));
            }
        }
    }
}