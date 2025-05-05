using System;
using UnityEngine;

/// <summary>
/// Represents an angle saving the value internally in degrees.
/// Contains help functions to find the shortest path between two angles.pathes between angles.
/// IMPORTANT: The Unity engine has the y axis pointing up, so angles are measured counterclockwise.
/// </summary>
namespace SpaceGame.Utilities
{
    public class Angle
    {
        public float InDegrees
        {
            get => _angle;
            set
            {
                _angle = value;
                _angle = _angle % 360f;
                while (_angle < 0f)
                {
                    _angle += 360f;
                }
            }
        }

        public float InRadians
        {
            get => _angle * (float)Math.PI / 180f;
            set => InDegrees = value * 180f / (float)Math.PI;
        }

        public float InMin180Plus180
        {
            get
            {
                if (_angle > 180f)
                {
                    return _angle - 360f;
                }
                else
                {
                    return _angle;
                }
            }
        }

        private float _angle = 0f;

        public Angle() { }

        public Angle(float angle)
        {
            InDegrees = angle;
        }

        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle(a.InDegrees + b.InDegrees);
        }

        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle(a.InDegrees - b.InDegrees);
        }

        public float GetClockwiseDifference(Angle otherAngle)
        {
            var deltaAngle = (otherAngle - this).InMin180Plus180;
            if (deltaAngle <= 0f)
            {
                return -deltaAngle;
            }
            else
            {
                return 360f - deltaAngle;
            }
        }

        public float GetCounterClockwiseDifference(Angle otherAngle)
        {
            var deltaAngle = (otherAngle - this).InMin180Plus180;
            if (deltaAngle >= 0f)
            {
                return deltaAngle;
            }
            else
            {
                return 360f + deltaAngle;
            }
        }
    }
}