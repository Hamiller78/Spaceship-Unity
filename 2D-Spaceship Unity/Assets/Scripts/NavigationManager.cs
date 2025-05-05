using System;
using UnityEngine;

namespace SpaceGame.Utilities
{
    public class NavigationManager
    {
        public enum TurnDirection
        {
            None = 0,
            Clockwise = -1,
            CounterClockwise = 1
        }

        // public static Angle GetGlobalAngleToTarget(Vector2 source, Vector2 target, float deltaGlobalRotation)
        // {
        //     // Calculate the direction vector from source to target
        //     Vector2 direction = target - source;

        //     // Calculate the angle in radians using Mathf.Atan2
        //     float angleInRads = Mathf.Atan2(direction.y, direction.x);

        //     // Convert the angle to your custom Angle class and adjust for global rotation
        //     var angle = new Angle() { InRadians = angleInRads } - new Angle(deltaGlobalRotation);

        //     return angle;
        // }

        public static Angle GetAngleToTarget(Vector2 source, Vector2 target)
        {
            // Calculate the direction vector from source to target
            Vector2 direction = target - source;

            // Calculate the angle in radians using Mathf.Atan2
            float angleInRads = Mathf.Atan2(direction.y, direction.x);

            // Return the angle wrapped in your custom Angle class
            return new Angle() { InRadians = angleInRads };
        }

        public static float GetNewRotation(
            float currentRotationDegrees,
            float targetRotationDegrees,
            float rotationSpeed,
            float minRotation,
            float maxRotation,
            double delta)
        {
            // var currentAngle = new Angle(currentRotationDegrees);
            // var targetAngle = new Angle(targetRotationDegrees);

            // var maxDeltaDegrees = rotationSpeed * (float)delta;

            // // Calculates shortest allowed path to target rotation
            // var turnDirection = GetTurnDirection(currentAngle, targetAngle, minRotation, maxRotation);
            // if (turnDirection == TurnDirection.None)
            // {
            //     return currentRotationDegrees;
            // }

            // float absDegreesToTarget = 0f;
            // if (turnDirection == TurnDirection.Clockwise)
            // {
            //     absDegreesToTarget = Math.Abs(currentAngle.GetClockwiseDifference(targetAngle) - 180f);
            // }
            // else if (turnDirection == TurnDirection.CounterClockwise)
            // {
            //     absDegreesToTarget = Math.Abs(currentAngle.GetCounterClockwiseDifference(targetAngle) - 180f);
            // }

            // if (absDegreesToTarget < maxDeltaDegrees)
            // {
            //     return targetRotationDegrees;
            // }

            // var targetAngle = new Angle(targetRotationDegrees);

            return targetRotationDegrees;

            // return currentRotationDegrees + (maxDeltaDegrees * (int)turnDirection);
        }

        private static TurnDirection GetTurnDirection(Angle angle, Angle targetAngle, float minRotation, float maxRoation)
        {
            var shortPathDirection = GetShortestPathDirection(angle, targetAngle);
            var longPathDirection
                = shortPathDirection == TurnDirection.Clockwise
                    ? TurnDirection.CounterClockwise
                    : TurnDirection.Clockwise;

            if (IsPathFree(angle, targetAngle, shortPathDirection, minRotation, maxRoation))
            {
                return shortPathDirection;
            }
            else if (IsPathFree(angle, targetAngle, longPathDirection, minRotation, maxRoation))
            {
                return longPathDirection;
            }
            else
            {
                return TurnDirection.None;
            }
        }

        private static bool IsPathFree(
            Angle angle,
            Angle targetAngle,
            TurnDirection turnDirection,
            float minAngle,
            float maxAngle)
        {
            if (minAngle == 0f && maxAngle == 360f)
            {
                return true;
            }

            switch (turnDirection)
            {
                case TurnDirection.None:
                    return true;
                case TurnDirection.Clockwise:
                    return IsBetween(targetAngle, angle.InDegrees, maxAngle);
                case TurnDirection.CounterClockwise:
                    return IsBetween(targetAngle, minAngle, angle.InDegrees);
                default:
                    return false;
            }
        }

        private static bool IsBetween(Angle angle, float minRoation, float maxRotation)
        {
            var minAngle = new Angle(minRoation);
            var maxAngle = new Angle(maxRotation);

            var angleMinToThis = angle.GetCounterClockwiseDifference(minAngle);
            var angleThisToMax = angle.GetClockwiseDifference(maxAngle);

            return angleMinToThis + angleThisToMax <= 360f;
        }

        private static TurnDirection GetShortestPathDirection(Angle angle, Angle targetAngle)
        {
            var clockwiseDifference = angle.GetClockwiseDifference(targetAngle);
            var counterClockwiseDifference = angle.GetCounterClockwiseDifference(targetAngle);

            if (Math.Abs(clockwiseDifference - 180f) <= Math.Abs(counterClockwiseDifference - 180f))
            {
                return TurnDirection.Clockwise;
            }
            else
            {
                return TurnDirection.CounterClockwise;
            }
        }
    }
}