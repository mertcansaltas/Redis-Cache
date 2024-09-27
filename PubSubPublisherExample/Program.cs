using StackExchange.Redis;

ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:1871");

ISubscriber subscriber = redis.GetSubscriber();

await subscriber.SubscribeAsync("benimkanalim", (channel,mesaj) =>
{
    Console.WriteLine(mesaj);
});
Console.ReadLine();
