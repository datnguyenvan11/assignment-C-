using System;

namespace bank_interface
{
    public class GiaodichBlockchain:GiaoDich
    {
        private ModelBlockchain ModelBlockchain=new ModelBlockchain();
        public void guitien()
        {

        }        

        public void ruttien()
        {
           
        }

        public void giaodich()
        {
            Console.WriteLine("Vui lòng nhập số tài khoản chuyển tiền: ");
            var accountNumber = Console.ReadLine();
            var receiverAccount = ModelBlockchain.FindByAccountaddress(accountNumber);
            if (receiverAccount == null)
            {
                Console.WriteLine("Tài khoản nhận tiền không tồn tại hoặc đã bị khoá.");
                return;
            }
            Console.WriteLine("Địa chỉ ài khoản nhận tiền: " + receiverAccount.Accountaddress);
            Console.WriteLine("Nhập số tiền chuyển khoản: ");
            var amount = double.Parse(Console.ReadLine());
            Console.WriteLine("Vui lòng nhập mã tài khoản.");
            string privatekey =Console.ReadLine();
            if (ModelBlockchain.FindByPrivateKey(privatekey)!=null)
            {
                Program.currentLoggedInAccountblock = ModelBlockchain.FindByAccountaddress(Program.currentLoggedInAccountblock.Accountaddress);
                if (amount > Program.currentLoggedInAccountblock.Balane)
                {
                    Console.WriteLine("Số dư tài khoản không đủ thực hiện giao dịch.");
                    return;
                }
                var transactionHistory = new TransactionBlockchain()
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = TransactionNganhang.TrasactionType.Stranfer,
                    Amount = amount,    
                    SenderId = Program.currentLoggedInAccountblock.Accountaddress,
                    ReceiverId = accountNumber
                };
                if (Program.currentLoggedInAccountblock.Accountaddress==accountNumber)
                {
                    return;
                }

                if (ModelBlockchain.Transfer(Program.currentLoggedInAccountblock, transactionHistory))
                {
                    Console.WriteLine("Giao dịch thành công.");
                }
                else
                {
                    Console.WriteLine("Giao dịch thất bại, vui lòng thử lại.");
                }        
            }
            else
            {
                Console.WriteLine(" mã tài khoản không đúng");
            }
        }
    

    

        public void login()
        {
            Program.currentLoggedInAccountblock = null;
            Console.Clear();
            Console.WriteLine("Tiến hành đăng nhập hệ thống Blockchain.");
            Console.WriteLine("Vui lòng nhập accountaddress: ");
            var accountaddress = Console.ReadLine();
            Console.WriteLine("Vui lòng mã tài khoản: ");
            var privatekey = Console.ReadLine();
            var shbAccount = ModelBlockchain.FindByAddressAndPrivateKey(accountaddress, privatekey);
            if (shbAccount == null)
            {
                Console.WriteLine("Sai thông tin tài khoản, vui lòng đăng nhập lại.");
                Console.WriteLine("Ấn phím bất kỳ để tiếp tục.");
                Console.Read();
                return;
            }
            Program.currentLoggedInAccountblock = shbAccount;
        }
    }
}