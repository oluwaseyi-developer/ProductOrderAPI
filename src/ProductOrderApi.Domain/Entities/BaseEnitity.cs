namespace ProductOrderApi.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
