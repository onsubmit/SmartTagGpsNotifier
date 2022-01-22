//-----------------------------------------------------------------------
// <copyright file="GeoCoordinate.cs" company="Andy Young">
// Copyright (c) Andy Young. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SmartTagGpsNotifier
{
    using System;

    /// <summary>
    /// Reimplmentation of the System.Device.Location.GeoCoordinate class.
    /// </summary>
    internal static class GeoCoordinate
    {
        /// <summary>
        /// Calculates the distance in meters between two GPS coordinates.
        /// </summary>
        /// <param name="longitude1">First longitude value.</param>
        /// <param name="latitude1">First latitude value.</param>
        /// <param name="longitude2">Second longitude value.</param>
        /// <param name="latitude2">Second latitude value.</param>
        /// <returns>The distance in meters between two GPS coordinates.</returns>
        internal static double GetDistance(double longitude1, double latitude1, double longitude2, double latitude2)
        {
            // https://stackoverflow.com/a/51839058
            double piOver180 = Math.PI / 180.0;

            var d1 = latitude1 * piOver180;
            var num1 = longitude1 * piOver180;
            var d2 = latitude2 * piOver180;
            var num2 = (longitude2 * piOver180) - num1;

            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + (Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0));

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
