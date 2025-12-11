namespace PersonalFinanceTracker.Server.Infrastructure
{
    using Newtonsoft.Json;

    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; init; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; }

        public sealed override string ToString() 
            => JsonConvert.SerializeObject(this);
    }
}
