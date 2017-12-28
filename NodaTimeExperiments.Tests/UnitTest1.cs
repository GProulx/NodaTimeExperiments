using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.TimeZones;

namespace NodaTimeExperiments.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Basic_FromWebSite()
        {
            // Instant represents time from epoch
            Instant now = SystemClock.Instance.GetCurrentInstant();

            // Convert an instant to a ZonedDateTime
            ZonedDateTime nowInIsoUtc = now.InUtc();

            // Create a duration
            Duration duration = Duration.FromMinutes(3);

            // Add it to our ZonedDateTime
            ZonedDateTime thenInIsoUtc = nowInIsoUtc + duration;

            // Time zone support (multiple providers)
            var london = DateTimeZoneProviders.Tzdb["Europe/London"];

            // Time zone conversions
            var localDate = new LocalDateTime(2012, 3, 27, 0, 45, 00);
            var before = london.AtStrictly(localDate);
        }


        [TestMethod]
        public void Get_CurrentTime()
        {
            //// Instant represents time from epoch
            //Instant now = SystemClock.Instance.GetCurrentInstant();

            //// Time zone support (multiple providers)
            //var qc = DateTimeZoneProviders.Tzdb["America/Toronto"];

            //var nowLocal = qc.AtStrictly(now.InUtc());

            //DateTimeZone zone = DateTimeZoneProviders.Tzdb["Europe/London"];
            //ZonedClock clock = SystemClock.Instance.InZone(zone);
            //LocalDate today = clock.GetCurrentLocalDateTime().Date;


        }

        [TestMethod]
        public void Get_CurrentTimeQc()
        {
            // ARRANGE
            var qcZone = DateTimeZoneProviders.Tzdb["America/Toronto"];

            ZonedClock clock = SystemClock.Instance.InZone(qcZone);

            // ACT
            var now = clock.GetCurrentLocalDateTime();

            // ASSERT
            var expectedDateTime = DateTime.Now;
            now.Hour.Should().Be(expectedDateTime.Hour);
            now.Minute.Should().Be(expectedDateTime.Minute);
            now.Second.Should().Be(expectedDateTime.Second);
            now.Day.Should().Be(expectedDateTime.Day);
        }


        [TestMethod]
        public void HowToSaveDateTimeToDB()
        {
            // ARRANGE
            const string localZoneId = "America/Toronto";
            var qcZone = DateTimeZoneProviders.Tzdb[localZoneId];

            ZonedClock clock = SystemClock.Instance.InZone(qcZone);

            // ACT
            LocalDateTime now = clock.GetCurrentLocalDateTime();

            ZonedDateTime x = qcZone.AtStrictly(now);

            // ASSERT
            x.Zone.Id.Should().Be(localZoneId);

            // What I need to save into the database:
            Console.WriteLine(x.ToInstant());
            Console.WriteLine(x.Zone.Id);
            // BASED ON : https://groups.google.com/forum/#!topic/noda-time/cfzbyYXfRyI
        }



        [TestMethod]
        public void ListAllTimezones()
        {
            TzdbDateTimeZoneSource.Default
                .ZoneLocations
                .ToList()
                .ForEach(tz =>
                {
                    Console.WriteLine("ZoneId:{0} | CountryName:{1} | Comment:{2}", tz.ZoneId, tz.CountryName, tz.Comment);
                });
        }
    }
}
