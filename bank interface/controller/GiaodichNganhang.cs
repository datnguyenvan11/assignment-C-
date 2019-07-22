using System;

namespace bank_interface
{
    public class GiaodichNganhang : GiaoDich
    {
        public static NganhangAccount CurrentAccount;
        private Bankmodel bankmodel = new Bankmodel();

        public void ruttien()
        {
            Program.currentLoggedInAccount = bankmodel.FindByAccountbyUsername(Program.currentLoggedInAccount.Username);
            if (Program.currentLoggedInAccount != null)
            {
                Console.Clear();
                Console.WriteLine("Tiến hành rút tiền tại hệ thống SHB.");
                Console.WriteLine("Vui lòng nhập số tiền cần rút.");
                var amount = double.Parse(Console.ReadLine());
                if (amount <= 0)
                {
                    Console.WriteLine("Số lượng không hợp lệ, vui lòng thử lại.");
                    return;
                }

                var transaction = new TransactionNganhang()
                {
                    Id = Guid.NewGuid().ToString(),
                    SenderId = Program.currentLoggedInAccount.Accountid,
                    ReceiverId = Program.currentLoggedInAccount.Accountid,
                    Type = TransactionNganhang.TrasactionType.Withdaw,
                    Message = "Tiến hành rút tiền tại ATM với số tiền: " + amount,
                    Amount = amount,
                    CreatedAtMls = DateTime.Now.Ticks,
                    UpdatedAtMls = DateTime.Now.Ticks,
                    Status = 1
                };
                if (                bankmodel.updatebalance(Program.currentLoggedInAccount, transaction)
                )
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
                Console.WriteLine("Vui lòng đăng nhập để sử dụng chức năng này.");
            }
        }

        public void guitien()
        {            
            Program.currentLoggedInAccount =bankmodel.FindByAccountbyUsername(Program.currentLoggedInAccount.Username);            

            if (Program.currentLoggedInAccount != null)
            {
            Console.WriteLine("Nhập số tiền cần gửi: ");
            var amount = double.Parse(Console.ReadLine());
            // lấy thông tin tài khoản mới nhất trước khi kiểm tra số dư.
            var transactionHistory = new TransactionNganhang()
            {
                Id = Guid.NewGuid().ToString(),
                SenderId = Program.currentLoggedInAccount.Accountid,
                ReceiverId = Program.currentLoggedInAccount.Accountid,
                Type = TransactionNganhang.TrasactionType.Deposit,
                Message = "Tiến hành gửi tiền tại ATM với số tiền: " + amount,
                Amount = amount,
                CreatedAtMls = DateTime.Now.Ticks,
                UpdatedAtMls = DateTime.Now.Ticks,
                Status = 1
            };

            if (bankmodel.updatebalance(Program.currentLoggedInAccount, transactionHistory))
            {
                Console.WriteLine("Giao dịch thành công.");
            }
            }
            else
            {
                Console.WriteLine("Vui lòng đăng nhập để sử dụng chức năng này.");
            }
        }

        public void giaodich()
        {
            Console.WriteLine("Vui lòng nhập số tài khoản chuyển tiền: ");
            var accountNumber = Console.ReadLine();
            var receiverAccount = bankmodel.FindByAccountbyAccountid(accountNumber);
            if (receiverAccount == null)
            {
                Console.WriteLine("Tài khoản nhận tiền không tồn tại hoặc đã bị khoá.");
                return;
            }
            Console.WriteLine("Tài khoản nhận tiền: " + accountNumber);
            Console.WriteLine("Chủ tài khoản: " + receiverAccount.Username);
            Console.WriteLine("Nhập số tiền chuyển khoản: ");
            var amount = double.Parse(Console.ReadLine());
            Program.currentLoggedInAccount = bankmodel.FindByAccountbyUsername(Program.currentLoggedInAccount.Username);
            if (amount > Program.currentLoggedInAccount.Balance)
            {
                Console.WriteLine("Số dư tài khoản không đủ thực hiện giao dịch.");
                return;
            }
//            Console.WriteLine("Nhập nội dung giao dịch: ");
//            var content = Console.ReadLine();
            var transactionHistory = new TransactionNganhang()
            {
                Id = Guid.NewGuid().ToString(),
                Type = TransactionNganhang.TrasactionType.Stranfer,
                Amount = amount,    
                Message = "",
                SenderId = Program.currentLoggedInAccount.Accountid,
                ReceiverId = accountNumber
            };
            if (Program.currentLoggedInAccount.Accountid==accountNumber)
            {
                return;
            }

            if (bankmodel.Transfer(Program.currentLoggedInAccount, transactionHistory))
            {
                Console.WriteLine("Giao dịch thành công.");
            }
            else
            {
                Console.WriteLine("Giao dịch thất bại, vui lòng thử lại.");
            }
        }
        

        public void login()
        {
            Program.currentLoggedInAccount = null;
            Console.Clear();
            Console.WriteLine("Tiến hành đăng nhập hệ thống SHB.");
            Console.WriteLine("Vui lòng nhập usename: ");
            var username = Console.ReadLine();
            Console.WriteLine("Vui lòng nhập mật khẩu: ");
            var password = Console.ReadLine();
            var shbAccount = bankmodel.FindByAccountandPassword(username, password);
            if (shbAccount == null)
            {
                Console.WriteLine("Sai thông tin tài khoản, vui lòng đăng nhập lại.");
                Console.WriteLine("Ấn phím bất kỳ để tiếp tục.");
                Console.Read();
                return;
            }
            Program.currentLoggedInAccount = shbAccount;
        }

    }
}