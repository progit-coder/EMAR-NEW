using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface ILogger
    {
        /// <summary>
        /// Log a message object with the log4net.Core.Level.Debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug( object message );

        /// <summary>
        /// Log a message object with the log4net.Core.Level.Debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug( object message, Exception ex );

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info( object message );

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Info Warning.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warning( object message );

        /// <summary>
        /// Logs a message object with the log4net.Core.Level.Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error( string message );

        /// <summary>
        /// Log a message object with the log4net.Core.Level.Error level including the
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error( object message, Exception exception );

        /// <summary>
        /// Log a message object with the log4net.Core.Level.Fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal( string message );

        /// <summary>
        /// Log a message object with the log4net.Core.Level.Fatal level including the
        // stack trace of the System.Exception passed as a parameter.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Fatal( object message, Exception exception );
    }
}
