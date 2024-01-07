namespace Shared.Settings
{
    public static class RabbitMQQueueSettings
    {
        public const string OrderInputQueueName = "order-input-queue-name";
        public const string StockInputQueueName = "stock-input-queue-name";
        public const string PaymentInputQueueName = "payment-input-queue-name";
        public const string SagaOrchestrationInputQueueName = "saga-orchestration-input-queue-name";
    }
}
