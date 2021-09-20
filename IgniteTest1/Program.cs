// See https://aka.ms/new-console-template for more information
using Apache.Ignite.Core;
using Apache.Ignite.Core.Configuration;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Discovery.Tcp.Multicast;

Console.WriteLine("Hello, World!");

var ignite = Ignition.Start(new IgniteConfiguration
{
    DataStorageConfiguration = new DataStorageConfiguration
    {
        DefaultDataRegionConfiguration = new DataRegionConfiguration
        {
            Name = "Default_Region",
            PersistenceEnabled = true
        }
    },
    IsActiveOnStart = true,
    DiscoverySpi = new TcpDiscoverySpi
    {
        IpFinder = new TcpDiscoveryMulticastIpFinder
        {
            MulticastGroup = "228.10.10.157"
        }
    }
});
ignite.GetCluster().SetActive(true);
var cache = ignite.GetOrCreateCache<int, string>("my-cache");
cache.Put(1, "Hello, World");
Console.WriteLine(cache.Get(1));