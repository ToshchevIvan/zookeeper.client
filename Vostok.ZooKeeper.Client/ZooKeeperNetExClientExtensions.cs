﻿using System;
using ZooKeeperNetExClient = org.apache.zookeeper.ZooKeeper;

namespace Vostok.ZooKeeper.Client
{
    internal static class ZooKeeperNetExClientExtensions
    {
        public static void Dispose(this ZooKeeperNetExClient client)
        {
            try
            {
                client.closeAsync().Wait();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}