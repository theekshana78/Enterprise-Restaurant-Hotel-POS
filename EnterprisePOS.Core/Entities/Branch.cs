namespace EnterprisePOS.Core.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
