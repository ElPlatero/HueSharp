using System;
using System.Collections.Generic;
using System.Globalization;

namespace HueSharp
{
    //inspired by Geoffrey Huntley's excellent GeoCoordinate class, decided to roll my own.
    //you might want to use this: https://github.com/ghuntley/geocoordinate
    public class GeoCoordinate : IEquatable<GeoCoordinate>
    {
        #region static and constant fields
        private static GeoCoordinate _unkown;
        public static GeoCoordinate Unknown => _unkown ?? (_unkown = new GeoCoordinate());
        private const double COMPARISON_DELTA = 0.001;
        #endregion

        #region fields
        private double _course;
        private double _horizontalAccuracy;
        private double _latitude;
        private double _longitude;
        private double _speed;
        private double _verticalAccuracy;
        #endregion

        #region c'tor
        public GeoCoordinate(double latitude = double.NaN, double longitude = double.NaN, double altitude = double.NaN, double horizontalAccuracy = double.NaN,
            double verticalAccuracy = double.NaN, double speed = double.NaN, double course = double.NaN)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            HorizontalAccuracy = horizontalAccuracy;
            VerticalAccuracy = verticalAccuracy;
            Speed = speed;
            Course = course;
        }
        #endregion

        #region Properties
        public double Latitude
        {
            get => _latitude;
            set
            {
                Ensure.InRange(-90.0, 90.0, value, nameof(Latitude));
                _latitude = value;
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                Ensure.InRange(-180, 180, value, nameof(Longitude));
                _longitude = value;
            }
        }

        public double HorizontalAccuracy
        {
            get => _horizontalAccuracy;
            set
            {
                Ensure.InRange(0.0, double.NaN, value, nameof(HorizontalAccuracy));
                _horizontalAccuracy = Math.Abs(value) < COMPARISON_DELTA ? double.NaN : value;
            }
        }

        public double VerticalAccuracy
        {
            get => _verticalAccuracy;
            set
            {
                Ensure.InRange(0, double.NaN, value, nameof(VerticalAccuracy));
                _verticalAccuracy = Math.Abs(value) < COMPARISON_DELTA ? double.NaN : value;
            }
        }

        public double Speed
        {
            get => _speed;
            set
            {
                Ensure.InRange(0.0, double.NaN, value, nameof(Speed));
                _speed = value;
            }
        }

        public double Course
        {
            get => _course;
            set
            {
                Ensure.InRange(0.0, 360.0, value, nameof(Course));
                _course = value;
            }
        }

        public bool IsUnknown => Equals(Unknown);

        public double Altitude { get; set; }

        public bool Equals(GeoCoordinate other)
        {
            return !ReferenceEquals(other, null) && _latitude.Equals(other.Latitude) && _longitude.Equals(other.Longitude);
        }
        #endregion

        #region operators
        public static bool operator ==(GeoCoordinate left, GeoCoordinate right) 
            => left?.Equals(right) ?? ReferenceEquals(right, null);

        public static bool operator !=(GeoCoordinate left, GeoCoordinate right) 
            => !(left == right);

        public static double operator -(GeoCoordinate minuend, GeoCoordinate subtrahend)
            => minuend.GetDistanceTo(subtrahend);
        #endregion

        #region methods, helper classes and equality members
        private bool IsValid() 
            => !double.IsNaN(Longitude) && !double.IsNaN(Latitude);

        private double GetDistanceTo(GeoCoordinate other)
        {
            if (!IsValid() || !other.IsValid())
            {
                throw new ArgumentException("Cannot calculcate distance of not all longitudes and latitudes are set.");
            }

            var d1 = Latitude * (Math.PI / 180.0);

            var num1 = Longitude * (Math.PI / 180.0);

            var d2 = other.Latitude * (Math.PI / 180.0);
            var num2 = other.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GeoCoordinate);
        }

        public override string ToString()
        {
            return Equals(Unknown) ? "Unknown" : $"{Latitude:G}, {Longitude:G}";
        }

        private static class Ensure
        {
            public static void InRange(double lowerLimit, double upperLimit, double value, string argumentName)
            {
                if (!double.IsNaN(lowerLimit) && !double.IsNaN(upperLimit) && lowerLimit > upperLimit) throw new InvalidOperationException("Invalid arguments for in range check. Lower limit must be smaller than or equal to upper limit.");

                if (!double.IsNaN(lowerLimit) && lowerLimit > value) throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must not be smaller than {lowerLimit}!");
                if (!double.IsNaN(upperLimit) && value > upperLimit) throw new ArgumentOutOfRangeException(argumentName, $"Argument {argumentName} must not be larger than {upperLimit}!");

            }
        }
        #endregion
    }
}