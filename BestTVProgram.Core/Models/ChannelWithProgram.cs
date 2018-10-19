using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BestTVProgram.Core.Models
{
    public class ChannelWithProgram
    {
        public Channel Channel { get; set; }

        public List<TVProgram> TVPrograms { get; set; } = new List<TVProgram>();

        public List<TVProgram> GetTVPrograms(TvProgramViewFormatsEnum tvProgramViewFormat = TvProgramViewFormatsEnum.ForAllDay)
        {
            if (tvProgramViewFormat == TvProgramViewFormatsEnum.ForAllDay)
            {
                return TVPrograms;
            }
            List<TVProgram> resultTVPrograms = new List<TVProgram>();

            CultureInfo culture = new CultureInfo("ru-RU");
            DateTime dateTimeValue;

            TimeZoneInfo timeZoneInfoForClient = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            DateTimeOffset serverTime = DateTimeOffset.Now;
            DateTimeOffset dateTimeOffsetForClient = serverTime.ToOffset(timeZoneInfoForClient.BaseUtcOffset);
            DateTime dateTimeNow = dateTimeOffsetForClient.DateTime;
            //DateTime currentDateTimeValue = DateTime.Now;
            TVProgram currentTVP = null;
            bool stopCheckForNow = false;
            int programCount = 0;
            foreach (var tvp in TVPrograms)
            {
                try
                {
                    dateTimeValue = DateTime.Parse(tvp.Time, culture);

                    if (tvProgramViewFormat == TvProgramViewFormatsEnum.ForEndOfDay)
                    {
                        if (!stopCheckForNow && (dateTimeNow < dateTimeValue))
                        {
                            resultTVPrograms.Add(currentTVP);
                            stopCheckForNow = true;
                        }
                        if (stopCheckForNow)
                        {
                            resultTVPrograms.Add(tvp);
                        }
                    }
                    else
                    {
                        if (!stopCheckForNow && (dateTimeNow < dateTimeValue))
                        {
                            resultTVPrograms.Add(currentTVP);
                            stopCheckForNow = true;
                            programCount++;
                        }
                        if (stopCheckForNow)
                        {
                            var timeDifference = dateTimeValue - dateTimeNow;
                            if (programCount == 3 || (timeDifference > TimeSpan.FromHours(1)))
                            {
                                break;
                            }
                            resultTVPrograms.Add(tvp);
                            programCount++;
                        }
                    }

                    currentTVP = tvp;

                }
                catch (FormatException e)
                {
                    resultTVPrograms.Add(new TVProgram { Program = e.Message + "|" + tvp.Program, Time = tvp.Time });
                }

            }

            return resultTVPrograms;
        }
    }

    public class TVProgram
    {
        public string Time { get; set; }
        public string Program { get; set; }

    }
}
