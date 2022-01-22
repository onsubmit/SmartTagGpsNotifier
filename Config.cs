//-----------------------------------------------------------------------
// <copyright file="Config.cs" company="Andy Young">
// Copyright (c) Andy Young. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SmartTagGpsNotifier
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Configuration class.
    /// </summary>
    internal class Config
    {
        private readonly IConfigurationRoot root;

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        /// <param name="root">The configuration root.</param>
        internal Config(IConfigurationRoot root)
        {
            this.root = root;

            this.SmtpServer = this.ValidateString("SMTP_SERVER");
            this.SmtpServerPort = this.GetInteger("SMTP_SERVER_PORT");

            this.EmailFrom = this.ValidateString("EMAIL_FROM");
            this.EmailFromPassword = this.ValidateString("EMAIL_FROM_PASSWORD");
            this.EmailTo = this.ValidateString("EMAIL_TO");

            this.HomeLatitude = this.GetDouble("HOME_LAT");
            this.HomeLongitude = this.GetDouble("HOME_LONG");

            this.MetersToNotify = this.GetDouble("METERS_TO_NOTIFY");

            this.SmartTagStudentId = this.GetInteger("SMART_TAG_STUDENT_ID");
        }

        /// <summary>
        /// Gets the SMTP server name.
        /// </summary>
        public string SmtpServer { get; }

        /// <summary>
        /// Gets the SMTP server port.
        /// </summary>
        public int SmtpServerPort { get; }

        /// <summary>
        /// Gets the email address to send from.
        /// </summary>
        public string EmailFrom { get; }

        /// <summary>
        /// Gets the password for the email address to send from.
        /// </summary>
        public string EmailFromPassword { get; }

        /// <summary>
        /// Gets the email address(es) to send to.
        /// </summary>
        public string EmailTo { get; }

        /// <summary>
        /// Gets the latitude of the student's home or desired drop-off location.
        /// </summary>
        public double HomeLatitude { get; }

        /// <summary>
        /// Gets the longitude of the student's home or desired drop-off location.
        /// </summary>
        public double HomeLongitude { get; }

        /// <summary>
        /// Gets the distance the student must be away from home before an email notification will be sent.
        /// </summary>
        public double MetersToNotify { get; }

        /// <summary>
        /// Gets the SMART tag student ID.
        /// </summary>
        public int SmartTagStudentId { get; }

        /// <summary>
        /// Validates a string.
        /// </summary>
        /// <param name="key">The config key.</param>
        /// <returns>The string, if it's valid.</returns>
        /// <exception cref="ArgumentException">Throw if the value is empty.</exception>
        private string ValidateString(string key)
        {
            if (string.IsNullOrWhiteSpace(this.root[key]))
            {
                throw new InvalidOperationException($"Config value with key {key} is empty");
            }

            return this.root[key];
        }

        /// <summary>
        /// Gets an integer value from a string.
        /// </summary>
        /// <param name="key">The config key.</param>
        /// <returns>An integer.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the value cannot be parsed.</exception>
        private int GetInteger(string key)
        {
            this.ValidateString(key);

            if (!int.TryParse(this.root[key], out int number))
            {
                throw new InvalidOperationException($"Could not parse {this.root[key]} as an integer.");
            }

            return number;
        }

        /// <summary>
        /// Gets a double value from a string.
        /// </summary>
        /// <param name="key">The config key.</param>
        /// <returns>A double.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the value cannot be parsed.</exception>
        private double GetDouble(string key)
        {
            this.ValidateString(key);

            if (!double.TryParse(this.root[key], out double number))
            {
                throw new InvalidOperationException($"Could not parse {this.root[key]} as a double.");
            }

            return number;
        }
    }
}
