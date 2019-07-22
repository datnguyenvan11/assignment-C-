namespace bank_interface
{
    public class BlockchainAccount
    {
        public string Accountaddress { get; set; }
        public string PrivateKey { get; set; }
        public double Balane { get; set; }
        public string CreateAT { get; set; }
        public string UpdateAt { get; set; }

        public BlockchainAccount()
        {
        }

        public BlockchainAccount(string accountaddress, string privateKey, double balane, string at, string updateAt)
        {
            Accountaddress = accountaddress;
            PrivateKey = privateKey;
            Balane = balane;
            CreateAT = at;
            UpdateAt = updateAt;
        }
    }
}