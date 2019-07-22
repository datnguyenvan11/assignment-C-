namespace bank_interface
{
    public class NganhangAccount
    {
        public string Accountid { get; set; }
        public double Balance { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CreateAt { get; set; }
        public string UpdateAT { get; set; }

        public NganhangAccount()
        {
        }

        public NganhangAccount(string accountid, double balance, string username, string password, string createAt, string updateAt)
        {
            Accountid = accountid;
            Balance = balance;
            Username = username;
            Password = password;
            CreateAt = createAt;
            UpdateAT = updateAt;
        }
        
        
    }
}