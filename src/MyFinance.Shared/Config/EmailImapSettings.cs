namespace MyFinance.Shared.Config
{

    public class EmailImapSettings
    {
        public required string ImapServer { get; set; }
        public int ImapPort { get; set; }
        public bool UseSsl { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}