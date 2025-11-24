namespace Azure.Local.Domain.Taxonomy
{
    public class TaxonomyItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public string? ParentId { get; set; }
    }
}
