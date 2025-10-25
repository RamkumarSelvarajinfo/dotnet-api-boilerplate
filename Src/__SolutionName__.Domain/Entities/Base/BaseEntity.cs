namespace __SolutionName__.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public bool Status { get; set; } = true;
    }
}
