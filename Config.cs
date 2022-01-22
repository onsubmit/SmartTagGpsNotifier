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
        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        /// <param name="root">The configuration root.</param>
        internal Config(IConfigurationRoot root)
        {
            this.SmtpServer = ValidateString(root["SMTP_SERVER"]);
            this.SmtpServerPort = GetInteger(root["SMTP_SERVER_PORT"]);

            this.EmailFrom = ValidateString(root["EMAIL_FROM"]);
            this.EmailFromPassword = ValidateString(root["EMAIL_FROM_PASSWORD"]);
            this.EmailTo = ValidateString(root["EMAIL_TO"]);

            this.HomeLatitude = GetDouble(root["HOME_LAT"]);
            this.HomeLongitude = GetDouble(root["HOME_LONG"]);

            this.MetersToNotify = GetDouble(root["METERS_TO_NOTIFY"]);

            this.SmartTagStudentId = GetInteger(root["SMART_TAG_STUDENT_ID"]);
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
        /// <param name="value">The string value.</param>
        /// <returns>The string, if it's valid.</returns>
        /// <exception cref="ArgumentException">Throw if the value is empty.</exception>
        private static string ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is empty", nameof(value));
            }

            return value;
        }

        /// <summary>
        /// Gets an integer value from a string.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>An integer.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the value cannot be parsed.</exception>
        private static int GetInteger(string value)
        {
            ValidateString(value);

            if (!int.TryParse(value, out int number))
            {
                throw new InvalidOperationException($"Could not parse {value} as an integer.");
            }

            return number;
        }

        /// <summary>
        /// Gets a double value from a string.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>A double.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the value cannot be parsed.</exception>
        private static double GetDouble(string value)
        {
            ValidateString(value);

            if (!double.TryParse(value, out double number))
            {
                throw new InvalidOperationException($"Could not parse {value} as a double.");
            }

            return number;
        }
    }
}
