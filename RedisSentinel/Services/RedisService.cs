using StackExchange.Redis;
namespace RedisSentinel.Services
   
{
	 public class RedisService
    {
        static ConfigurationOptions sentinelOptions => new()
        {
            EndPoints =
            {
                { "localhost",1881},
                { "localhost",1882},
                { "localhost",1883}
            },
            CommandMap = CommandMap.Sentinel,
            AbortOnConnectFail = false
        };

        static ConfigurationOptions masterOptions => new()
        {
            AbortOnConnectFail = false
        };

        static public async Task<IDatabase> RedisMasterDatabase()
        {
            ConnectionMultiplexer sentinelConnection = await ConnectionMultiplexer.SentinelConnectAsync(sentinelOptions);

            System.Net.EndPoint masterEndPoint = null;

            foreach (System.Net.EndPoint endPoint in sentinelConnection.GetEndPoints())
            {
                IServer server = sentinelConnection.GetServer(endPoint);
                if (!server.IsConnected) continue;

                //Burada gelecek olan veri dockerın içerisindeki internal ip yi döndürecektir.
                masterEndPoint = await server.SentinelGetMasterAddressByNameAsync("mymaster");
                break;
            }

            var localMasterIP = masterEndPoint.ToString() switch
            {
                "172.18.0.2:6379" => "localhost:1871",
                "172.18.0.3:6379" => "localhost:1872",
                "172.18.0.4:6379" => "localhost:1873",
                "172.18.0.5:6379" => "localhost:1874"
            };

            ConnectionMultiplexer masterConnection = await ConnectionMultiplexer.ConnectAsync(localMasterIP);
            IDatabase database = masterConnection.GetDatabase();
            return database;
        }
    }
}
