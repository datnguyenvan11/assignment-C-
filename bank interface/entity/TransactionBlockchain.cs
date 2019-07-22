namespace bank_interface
{
    public class TransactionBlockchain
    {
   

        public TransactionBlockchain()
        {
        }

        public string Id { get; set; }
        public string SenderId{ get; set; }
        public string ReceiverId{ get; set; }
        public double Amount{ get; set; }
        public string createdAtMls{ get; set; }
        public string updatedAtMls{ get; set; }

        public TransactionNganhang.TrasactionType Type { get; set; }
//        private int type{ get; set; }
        public TransactionBlockchain(string id, string senderId, string receiverId, double amount, string createdAtMls, string updatedAtMls, TransactionNganhang.TrasactionType type)
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = receiverId;
            Amount = amount;
            this.createdAtMls = createdAtMls;
            this.updatedAtMls = updatedAtMls;
            Type = type;
        }
    }
}