using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CSharpCurses
{
    public sealed class Logger : IDisposable
    {
        private Stream _outputStream;
        private TextWriter _writer;
        private static SemaphoreSlim _writerLock = new SemaphoreSlim(1);

        /// <summary>
        /// Get or set the stream that messages are immediately written to.
        /// </summary>
        public Stream OutputStream
        {
            get { return _outputStream; }
            set
            {
                _outputStream?.Dispose();
                _outputStream = value;

                _writerLock.Wait();
                _writer?.Dispose();
                _writer = new StreamWriter(value);
                _writerLock.Release();
            }
        }

        public Level OutputLevel { get; set; } = Level.Info;

        /// <summary>
        /// A list of logged messages.
        /// </summary>
        public List<Message> Messages { get; }

        /// <summary>
        /// Create a new Logger.
        /// </summary>
        public Logger()
        {
            Messages = new List<Message>();
        }

        /// <summary>
        /// Log a message. Uses <see cref="DateTime.Now"/> for the timestamp.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="message">The content of the message.</param>
        public async void Log(Level level, string message)
        {
            var msg = new Message(level, message, DateTime.Now);
            Messages.Add(msg);
            if (level >= OutputLevel && _writer != null)
            {
                // TODO this is probably a bad idea
                await _writerLock.WaitAsync();
                await _writer?.WriteLineAsync(msg.ToString());
                _writerLock.Release();
            }
        }

        /// <summary>
        /// Delete all logged messages.
        /// </summary>
        public void Clear()
        {
            Messages.Clear();
        }

        /// <summary>
        /// Get a string dump of all messages logged so far.
        /// </summary>
        /// <returns>A string value with all logged messages concatenated, separated by new lines.</returns>
        public string Dump(Level level = Level.Info)
        {
            return Messages
                .Where(m => (int) m.Level >= (int) level)
                .Select(m => m.ToString())
                .Aggregate((s1, s2) => string.Join(Environment.NewLine, s1, s2));
        }

#if NETSTANDARD1_3
        /// <summary>
        /// Set the <see cref="OutputStream"/> to write to standard out.
        /// </summary>
        public void WriteStdOut()
        {
        }

        /// <summary>
        /// Set the <see cref="OutputStream"/> to write to the specified file.
        /// </summary>
        public void SetLogFile(string filename)
        {
            OutputStream = File.OpenWrite(filename);
        }
#endif

        /// <summary>
        /// Dispose of the Logger. Disposes of the <see cref="OutputStream"/> as well.
        /// </summary>
        public void Dispose()
        {
            _outputStream?.Dispose();
            _writer?.Dispose();
        }

        /// <summary>
        /// A Message logged by <see cref="Logger"/>.
        /// </summary>
        public class Message
        {
            /// <summary>
            /// The type of the message. Indicates severity.
            /// </summary>
            public Level Level { get; }

            /// <summary>
            /// The content of the message.
            /// </summary>
            public string Content { get; }

            /// <summary>
            /// Time at which the message was logged.
            /// </summary>
            public DateTime TimeStamp { get; }

            /// <summary>
            /// Create a <see cref="Message"/>.
            /// </summary>
            /// <param name="level">Log level of the message.</param>
            /// <param name="content">Content of the message.</param>
            /// <param name="timeStamp">The time at which the message was logged.</param>
            public Message(Level level, string content, DateTime timeStamp)
            {
                Level = level;
                Content = content;
                TimeStamp = timeStamp;
            }

            public override string ToString()
            {
                return $"[{TimeStamp:T}] {Level.ToString().ToUpperInvariant()}: {Content}";
            }
        }

        /// <summary>
        /// Log level. The semantics and requirements for these differ for every application,
        /// but these are granular enough for most applications.
        /// </summary>
        public enum Level
        {
            /// <summary>
            /// Log every method call.
            /// </summary>
            Trace,
            /// <summary>
            /// Detailed debugging information.
            /// </summary>
            Debug,
            /// <summary>
            /// Informative messages.
            /// </summary>
            Info,
            /// <summary>
            /// Unexpected behavior that does not cause an immediate issue.
            /// </summary>
            Warning,
            /// <summary>
            /// Details of unexpected behavior that causes issues with the application.
            /// </summary>
            Error,
            /// <summary>
            /// Irrecoverable errors that immediately crash the application.
            /// </summary>
            Fatal
        }
    }
}
