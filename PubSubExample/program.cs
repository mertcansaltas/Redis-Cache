using StackExchange.Redis;

ConnectionMultiplexer redis=await ConnectionMultiplexer.ConnectAsync("localhost:1871");

ISubscriber subscriber = redis.GetSubscriber();
