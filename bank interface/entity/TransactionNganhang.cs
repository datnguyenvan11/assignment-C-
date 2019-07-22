using System;

namespace bank_interface
{
    public class TransactionNganhang
    {
        public string Id { get; set; }
        public string SenderId { get;set; }
        public string ReceiverId { get;set; }
        public double Amount { get;set; }
        public string Message { get; set;}
        public long CreatedAtMls { get;set; }
        public long UpdatedAtMls { get;set; }
        public int Status { get;set; }
        public TrasactionType Type { get;set; }

        public TransactionNganhang()
        {
        }

        public enum TrasactionType
        {
         Withdaw=1,
         Deposit=2,
         Stranfer=3
        }
        public TransactionNganhang(string id, string senderId, string receiverId, double amount, string message, long createdAtMls, long updatedAtMls, int status, TrasactionType type )
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = receiverId;
            Amount = amount;
            Message = message;
            CreatedAtMls = createdAtMls;
            UpdatedAtMls = updatedAtMls;
            Status = status;
            Type = type;
        }

        /*public enum Status {

            COMPLETED=1, 
            PENDING=2,
            CANCEL=-1, 
            UNDEFINED=-2

        }*/
/*

        public static Status findByValue(int value) {
            for (int i = 0; i < Status.values().length; i++) {
                if (Status.values()[i].getValue() == value) {
                    return Status.values()[i];
                }
            }
            return Status.UNDEFINED;
        }

        public int getValue() {
            return value;
        }

        public void setValue(int value) {
            this.value = value;
        }*/
        /*}*/
    }
}