namespace auth_service.Configuration
{
    public class KafkaSettings
    {
        public string KafkaConnection { get; set; }
        public TopicNamesSettings TopicNames { get; set; }
        public string ProducerName { get; set; }
    }

    public class TopicNamesSettings
    {
        public string AccountManagement { get; set; }
    }
}
