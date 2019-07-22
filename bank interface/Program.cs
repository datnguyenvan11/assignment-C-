using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace bank_interface
{
    class Program
    {
        public static NganhangAccount currentLoggedInAccount;
        public static BlockchainAccount currentLoggedInAccountblock;

        static void Main(string[] args)
        {

            while (true)
            {
                Console.Clear();
                GiaoDich giaoDich = null;
                Console.WriteLine("Vui lòng lựa chọn phương thức giao dịch: ");
                Console.WriteLine("============================================");
                Console.WriteLine("1. Giao dịch trên ngân hàng SHB - Spring Hero Bank.");
                Console.WriteLine("2. Giao dịch blockchain.");
                Console.WriteLine("============================================");
                Console.WriteLine("Nhập lựa chọn của bạn: ");
                var choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        giaoDich = new GiaodichNganhang();
                        break;
                    case 2:
                        giaoDich = new GiaodichBlockchain();
                        break;
                    default:
                        Console.WriteLine("Sai phương thức đăng nhập.");
                        break;
                }

                // yêu cầu người dùng đăng nhập.
                giaoDich.login();
                if (currentLoggedInAccount  != null)
                {
                    Console.WriteLine("Đăng nhập thành công với tài khoản.");
                    Console.WriteLine($"Tài khoản: {currentLoggedInAccount.Username}");
                    Console.WriteLine($"Số dư: {currentLoggedInAccount.Balance}");
                    Console.WriteLine("Ấn phím bất kỳ để tiếp tục giao dịch.");
                    Console.ReadLine();
                    GenerateTransactionMenu(giaoDich);
                }
                if (currentLoggedInAccountblock  != null)
                {
                    Console.WriteLine("Đăng nhập thành công với tài khoản.");
                    Console.WriteLine($"Tài khoản: {currentLoggedInAccountblock.Accountaddress}");
                    Console.WriteLine($"Số dư: {currentLoggedInAccountblock.Balane}");
                    Console.WriteLine("Ấn phím bất kỳ để tiếp tục giao dịch.");
                    Console.ReadLine();
                    GenerateTransactionMenu(giaoDich);
                }
            }
        }

        private static void GenerateTransactionMenu(GiaoDich giaoDich)
        {
            while (true)
            {
                Console.Clear();
                // show menu dành cho người dùng đã đăng nhập.
                Console.WriteLine("Vui lòng lựa chọn kiểu giao dịch: ");
                Console.WriteLine("============================================");
                Console.WriteLine("1. Rút tiền.");
                Console.WriteLine("2. Gửi tiền.");
                Console.WriteLine("3. Chuyển khoản.");
                Console.WriteLine("4. Thoát.");
                Console.WriteLine("============================================");
                Console.WriteLine("Nhập lựa chọn của bạn: ");
                var choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        giaoDich.ruttien();
                        break;
                    case 2:
                        giaoDich.guitien();
                        break;
                    case 3:
                        giaoDich.giaodich();
                        break;
                    case 4:
                        Console.WriteLine("Thoát giao diện giao dịch.");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn sai.");
                        break;
                }

                if (choice == 4)
                {
                    break;
                }
            }
        }
    }
}