namespace infrastructure.Entities
{
    public class PasswordHash
    {
        #region Properties
        
        public int UserId { get; set; }
        
        public byte[] Hash { get; set; }
        
        public byte[] Salt { get; set; }
        
        public string Algorithm { get; set; }
        
        #endregion
    }
}