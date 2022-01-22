//-----------------------------------------------------------------------
// <copyright file="Functions.cs" company="Andy Young">
// Copyright (c) Andy Young. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SmartTagGpsNotifier
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Azure function to send an email when a student with a SmartTag is near to their drop-off location.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Sends an email when a student with a SmartTag is near to their drop-off location.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="log">The logging mechanism.</param>
        /// <param name="context">The execution context.</param>
        /// <returns>The result.</returns>
        [FunctionName("Notify")]
        public static async Task Run(
            [TimerTrigger("%Schedule%")] TimerInfo timer,
            ILogger log,
            ExecutionContext context)
        {
            Contract.Requires(timer != null);
            Contract.Requires(log != null);
            Contract.Requires(context != null);

            Config config = GetAppConfig(context);

            try
            {
                using HttpClient httpClient = new();
                string requestUri = $"https://parent.smart-tag.net/Activity/GetLastKnownLocationByStudentId?studentId={config.SmartTagStudentId}";

                HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                string contents = await response.Content.ReadAsStringAsync();
                StudentStatus[] statuses = JsonConvert.DeserializeObject<StudentStatus[]>(contents);

                if (statuses.Length == 0)
                {
                    log.LogError("Not statuses found");
                    return;
                }

                StudentStatus status = statuses[0];

                if (!status.IsStudentOnBus)
                {
                    log.LogInformation("Student not on bus");
                    return;
                }

                double distanceFromHome = Math.Round(GeoCoordinate.GetDistance(status.Longitude, status.Latitude, config.HomeLongitude, config.HomeLatitude));
                if (distanceFromHome <= config.MetersToNotify)
                {
                    using SmtpClient smtpClient = new(config.SmtpServer, config.SmtpServerPort)
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        EnableSsl = true,
                        Credentials = new NetworkCredential(config.EmailFrom, config.EmailFromPassword),
                    };

                    MailMessage message = new(config.EmailFrom, config.EmailTo)
                    {
                        Subject = "Student arriving shortly",
                        Body = $"Student with ID {config.SmartTagStudentId} is {distanceFromHome} meters from home and will arrive shortly.",
                    };

                    smtpClient.Send(message);

                    log.LogInformation("Student arriving shortly. Email sent.");
                    return;
                }
                else
                {
                    log.LogInformation($"Student still {distanceFromHome} meters from home.");
                    return;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Gets the application configuration settings.
        /// </summary>
        /// <param name="context">Execution context.</param>
        /// <returns>The application configuration settings.</returns>
        private static Config GetAppConfig(ExecutionContext context)
        {
            IConfigurationRoot root = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return new Config(root);
        }
    }
}
