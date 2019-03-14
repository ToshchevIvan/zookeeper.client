﻿using System;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Commons.Time;
using Vostok.Logging.Abstractions;

namespace Vostok.ZooKeeper.Client
{
    /// <summary>
    /// Represents a ZooKeeper client settings.
    /// </summary>
    [PublicAPI]
    public class ZooKeeperClientSettings
    {
        /// <summary>
        /// Creates a new instance of <see cref="ZooKeeperClientSettings"/> using given <paramref name="connectionString"/> and <paramref name="log"/>.
        /// </summary>
        public ZooKeeperClientSettings([NotNull] string connectionString, [NotNull] ILog log)
            : this(() => connectionString, log)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ZooKeeperClientSettings"/> using given <paramref name="replicas"/> and <paramref name="log"/>.
        /// </summary>
        public ZooKeeperClientSettings([NotNull] [ItemNotNull] Uri[] replicas, [NotNull] ILog log)
            : this(() => replicas, log)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ZooKeeperClientSettings"/> using given <paramref name="replicasProvider"/> and <paramref name="log"/>.
        /// </summary>
        public ZooKeeperClientSettings([NotNull] Func<Uri[]> replicasProvider, [NotNull] ILog log)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            if (replicasProvider == null)
                throw new ArgumentNullException(nameof(replicasProvider));

            ConnectionStringProvider = () => BuildConnectionString(replicasProvider());
        }

        /// <summary>
        /// Creates a new instance of <see cref="ZooKeeperClientSettings"/> using given <paramref name="connectionStringProvider"/> and <paramref name="log"/>.
        /// </summary>
        public ZooKeeperClientSettings([NotNull] Func<string> connectionStringProvider, [NotNull] ILog log)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            ConnectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        /// <summary>
        /// Client logger.
        /// </summary>
        public ILog Log { get; }

        /// <summary>
        /// Delegate for producing connection string.
        /// </summary>
        public Func<string> ConnectionStringProvider { get; }

        /// <summary>
        /// Session and connect timeout.
        /// </summary>
        public TimeSpan Timeout { get; set; } = 5.Seconds();

        /// <summary>
        /// Is allowed to go to read-only mode in case of partitioning.
        /// </summary>
        public bool CanBeReadOnly { get; set; }

        public LogLevel InnerClientLogLevel { get; set; } = LogLevel.Info;

        private static string BuildConnectionString([NotNull] [ItemNotNull] Uri[] uris)
        {
            return string.Join(",", uris.Select(u => $"{u.Host}:{u.Port}"));
        }
    }
}