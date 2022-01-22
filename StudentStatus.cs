//-----------------------------------------------------------------------
// <copyright file="StudentStatus.cs" company="Andy Young">
// Copyright (c) Andy Young. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SmartTagGpsNotifier
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the status returned by the SmartTag API.
    /// </summary>
    internal sealed class StudentStatus
    {
        /// <summary>
        /// Gets or sets the last known location.
        /// </summary>
        public string LastKnownLocation { get; set; }

        /// <summary>
        /// Gets or sets the disembarkation date.
        /// </summary>
        public object DisembarkationDate { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets the activity ID.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// Gets or sets the name of the bus.
        /// </summary>
        public string BusName { get; set; }

        /// <summary>
        /// Gets or sets the name of the bus driver.
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the bus route.
        /// </summary>
        public string FriendlyRouteDisplay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the student is on the bus.
        /// </summary>
        public int StudentOnBus { get; set; }

        /// <summary>
        /// Gets a value indicating whether the student is on the bus.
        /// </summary>
        [JsonIgnore]
        public bool IsStudentOnBus => this.StudentOnBus == 1;

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        [JsonIgnore]
        public double Longitude
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.LastKnownLocation))
                {
                    return 0;
                }

                string[] split = this.LastKnownLocation.Split(",");
                if (split.Length > 0 && double.TryParse(split[0], out double longitude))
                {
                    return longitude;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        [JsonIgnore]
        public double Latitude
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.LastKnownLocation))
                {
                    return 0;
                }

                string[] split = this.LastKnownLocation.Split(",");
                if (split.Length > 1 && double.TryParse(split[1], out double latitude))
                {
                    return latitude;
                }

                return 0;
            }
        }
    }
}
