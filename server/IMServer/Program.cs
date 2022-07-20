// See https://aka.ms/new-console-template for more information


using Confluent.Kafka;

const string kafkaTopic = "chat";

var cConfig = new ConsumerConfig
{
    BootstrapServers = "localhost",
    GroupId = Guid.NewGuid().ToString(),
    AutoOffsetReset = AutoOffsetReset.Latest,
    // not needs yet:
    //SaslMechanism = SaslMechanism.Plain,
    //SecurityProtocol = SecurityProtocol.SaslSsl,
    //SaslUsername = "xxxxxxx",
    //SaslPassword = "xxxxx+",

};

using var c = new ConsumerBuilder<Ignore, string>(cConfig).Build();
c.Subscribe(kafkaTopic);

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => {
    e.Cancel = true; // prevent the process from terminating.
    cts.Cancel();
};

try
{
    while (true)
    {
        try
        {
            var cr = c.Consume(cts.Token);
            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
        }
        catch (ConsumeException e)
        {
            Console.WriteLine($"Error occured: {e.Error.Reason}");
        }
    }
}
catch (OperationCanceledException)
{
    // Close and Release all the resources held by this consumer  
    c.Close();
}