using StackExchange.Redis;

ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:1871");

ISubscriber subscriber = redis.GetSubscriber();

while (true)
{
	Console.WriteLine("Mesaj ");
	var message=Console.ReadLine();
	await subscriber.PublishAsync("benimkanalim",message);
}
