namespace AngelusTBlog.ViewModel
{
    public class MailSettings
    {
        // Configure and use a smtp server. ie from Google

        // User Info 
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }

        // Smtp Server - gmail
        public string Host { get; set; }

        // Port number gor the server
        public int Port { get; set; }
    }
}
