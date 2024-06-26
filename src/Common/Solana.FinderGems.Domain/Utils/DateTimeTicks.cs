﻿

namespace Solana.FinderGems.Domain.Utils
{
    public class DateTimeTicks
    {
        #region Variables and Properties

        private static DateTimeTicks _instance;

        public static DateTimeTicks Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DateTimeTicks();
                return _instance;
            }
        }

        #endregion

        #region Constructor

        private DateTimeTicks() { }

        #endregion

        #region Public Methods

        public long ConvertDateTimeToTicks(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, TimeSpan.Zero).ToUnixTimeSeconds();
        }

        public DateTime ConvertTicksToDateTime(long ticks)
        {
            return DateTimeOffset.FromUnixTimeSeconds(ticks).DateTime;
        }

        #endregion
    }
}
