using log4net;
using System;
using System.Reflection;

namespace LTCPro.ServiceLayer
{
    public class Log4net : ILogger
    {
        public Log4net()
        {
            Log = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );
        }

        private ILog Log { get; }

        #region Public Methods

        /// <summary>
        ///     Log a message object with the log4net.Core.Level.Debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug( object message )
        {
            Log.Debug( message );
        }

        /// <summary>
        ///     Logs a message object with the log4net.Core.Level.Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info( object message )
        {
            Log.Info( message );
        }

        /// <summary>
        ///     Logs a message object with the log4net.Core.Level.Info Warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warning( object message )
        {
            Log.Warn( message );
        }

        /// <summary>
        ///     Logs a message object with the log4net.Core.Level.Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error( string message )
        {
            Log.Error( message );
        }


        /// <summary>
        ///     Log a message object with the log4net.Core.Level.Error level including the
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error( object message, Exception exception )
        {
            Log.Error( message, exception );
        }

        /// <summary>
        ///     Log a message object with the log4net.Core.Level.Fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal( string message )
        {
            Log.Fatal( message );
        }

        /// <summary>
        ///     Log a message object with the log4net.Core.Level.Fatal level including the
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal( object message, Exception exception )
        {
            Log.Fatal( message, exception );
        }

        public void Debug( object message, Exception ex )
        {
            Log.Debug( message + " --- " + ex.StackTrace );
        }

        #endregion
    }
}
