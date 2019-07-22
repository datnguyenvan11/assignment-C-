using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace bank_interface
{
    public class Bankmodel
    {
        public NganhangAccount FindByAccountandPassword(string username,string password)
        {
            try 
            {
                     var cmd = new MySqlCommand("select * from AccountNganhang where Username = @Username and Password=@Password", ConnectionHepper.GetConnection());
                     cmd.Parameters.AddWithValue("@Username", username);
                     cmd.Parameters.AddWithValue("@Password",password);
                     var dataReader = cmd.ExecuteReader();
                     if (!dataReader.Read())
                     {
                         return null;
                     }
                     var obj = new NganhangAccount()
                     {
                         Username = dataReader.GetString("Username"),
                         Balance= dataReader.GetInt64(dataReader.GetOrdinal("Balance"))                
                     };
                     ConnectionHepper.CloseConnection();
                     return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        } public NganhangAccount FindByAccountbyUsername(string username)
        {
            try 
            {
                     var cmd = new MySqlCommand("select * from AccountNganhang where Username = @Username ", ConnectionHepper.GetConnection());
                     cmd.Parameters.AddWithValue("@Username", username);
                     var dataReader = cmd.ExecuteReader();
                     if (!dataReader.Read())
                     {
                         return null;
                     }
                     var obj = new NganhangAccount()
                     {
                         Accountid = dataReader.GetString("Accountid"),
                         Username = dataReader.GetString("Username"),
                         Balance= dataReader.GetInt64(dataReader.GetOrdinal("Balance"))                
                     };
                     ConnectionHepper.CloseConnection();
                     return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public NganhangAccount FindByAccountbyAccountid(string account)
        {
            try 
            {
                var cmd = new MySqlCommand("select * from AccountNganhang where Accountid = @Accountid ", ConnectionHepper.GetConnection());
                cmd.Parameters.AddWithValue("@Accountid", account);
                var dataReader = cmd.ExecuteReader();
                if (!dataReader.Read())
                {
                    return null;
                }
                var obj = new NganhangAccount()
                {
                    Accountid = dataReader.GetString("Accountid"),
                    Username = dataReader.GetString("Username"),
                    Balance= dataReader.GetInt64(dataReader.GetOrdinal("Balance"))                
                };
                ConnectionHepper.CloseConnection();
                return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public bool updatebalance(NganhangAccount account, TransactionNganhang transactionNganhang)
        {
            var tran = ConnectionHepper.GetConnection().BeginTransaction();
            try
            {
                var cmd = new MySqlCommand("select Balance from AccountNganhang  where Accountid = @Accountid",
                ConnectionHepper.GetConnection());
                cmd.Parameters.AddWithValue("@Accountid", account.Accountid);
                var reader = cmd.ExecuteReader();
                double currentAccountBalance = 0;
                if (reader.Read())
                {
                    currentAccountBalance = reader.GetDouble("Balance");
                }
                reader.Close();
                if (currentAccountBalance < 0)
                {
                    Console.WriteLine("Không đủ tiền trong tài khoản.");
                    return false;
                }

                if (transactionNganhang.Type == (TransactionNganhang.TrasactionType) 1)
                {
                    if (currentAccountBalance < transactionNganhang.Amount)
                    {
                        Console.WriteLine("Khong du tien thuc hien giao dich");
                        return false;
                    }
                    currentAccountBalance -= transactionNganhang.Amount;
                }
                else if (transactionNganhang.Type == (TransactionNganhang.TrasactionType) 2)
                {
                    currentAccountBalance += transactionNganhang.Amount;
                }

                var updateQuery =
                    "update AccountNganhang set `Balance` = @balance where Accountid = @Accountid";
                var sqlCmd = new MySqlCommand(updateQuery, ConnectionHepper.GetConnection());
                sqlCmd.Parameters.AddWithValue("@Balance", currentAccountBalance);
                sqlCmd.Parameters.AddWithValue("@Accountid", account.Accountid);
                var updateResult = sqlCmd.ExecuteNonQuery();
                var historyTransactionQuery =
                    "insert into TransactionNganhang (Id, Type, Senderid, ReceiverId, Amount, Message) " +
                    "values (@Id, @Type, @Senderid, @ReceiverId, @Amount, @Message)";
                var historyTransactionCmd =
                    new MySqlCommand(historyTransactionQuery, ConnectionHepper.GetConnection());
                historyTransactionCmd.Parameters.AddWithValue("@Id", transactionNganhang.Id);
                historyTransactionCmd.Parameters.AddWithValue("@Amount", transactionNganhang.Amount);
                historyTransactionCmd.Parameters.AddWithValue("@Type", transactionNganhang.Type);
                historyTransactionCmd.Parameters.AddWithValue("@Message", transactionNganhang.Message);
                historyTransactionCmd.Parameters.AddWithValue("@Senderid",
                    transactionNganhang.SenderId);
                historyTransactionCmd.Parameters.AddWithValue("@Receiverid",
                    transactionNganhang.ReceiverId);
                var historyResult = historyTransactionCmd.ExecuteNonQuery();

                if (updateResult != 1 || historyResult != 1)
                {
                    throw new Exception("Không thể thêm giao dịch hoặc update tài khoản.");
                }

                tran.Commit();
            }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            tran.Rollback(); // lưu giao dịch vào.                
            return false;
        }

        ConnectionHepper.CloseConnection();
        return true;
       }
        
        
        
        
         
    public bool Transfer(NganhangAccount currentLoggedInAccount, TransactionNganhang transactionHistory)
        {
            var mySqlTransaction = ConnectionHepper.GetConnection().BeginTransaction();
            try
            {
                // Kiểm tra số dư tài khoản.
                var selectBalance =
                    "select Balance from AccountNganhang  where Accountid = @Accountid";
                var cmdSelect = new MySqlCommand(selectBalance, ConnectionHepper.GetConnection());
                cmdSelect.Parameters.AddWithValue("@Accountid", currentLoggedInAccount.Accountid);
                var reader = cmdSelect.ExecuteReader();
                double currentAccountBalance = 0;
                if (reader.Read())
                {
                    currentAccountBalance = reader.GetDouble("Balance");
                }

                reader.Close(); // important.
                if (currentAccountBalance < transactionHistory.Amount)
                {
                    throw new Exception("Không đủ tiền trong tài khoản.");
                }

                currentAccountBalance -= transactionHistory.Amount;

                // Update tài khoản.
                var updateQuery =
                    "update `AccountNganhang` set `Balance` = @Balance where Accountid = @Accountid ";
                var sqlCmd = new MySqlCommand(updateQuery,ConnectionHepper.GetConnection());
                sqlCmd.Parameters.AddWithValue("@Balance", currentAccountBalance);
                sqlCmd.Parameters.AddWithValue("@Accountid", currentLoggedInAccount.Accountid);
                var updateResult = sqlCmd.ExecuteNonQuery();


                // Kiểm tra số dư tài khoản.
                var selectBalanceReceiver =
                    "select Balance from `AccountNganhang` where Accountid = @Accountid ";
                var cmdSelectReceiver = new MySqlCommand(selectBalanceReceiver,ConnectionHepper.GetConnection());
                cmdSelectReceiver.Parameters.AddWithValue("@Accountid", transactionHistory.ReceiverId);
                var readerReceiver = cmdSelectReceiver.ExecuteReader() ?? throw new ArgumentNullException("cmdSelectReceiver.ExecuteReader()");
                double receiverBalance = 0;
                if (readerReceiver.Read())
                {
                    receiverBalance = readerReceiver.GetDouble("Balance");
                }

                readerReceiver.Close(); // important.                
                receiverBalance += transactionHistory.Amount;

                // Update tài khoản.
                var updateQueryReceiver =
                    "update `AccountNganhang` set `Balance` = @Balance where Accountid = @Accountid";
                var sqlCmdReceiver = new MySqlCommand(updateQueryReceiver, ConnectionHepper.GetConnection());
                sqlCmdReceiver.Parameters.AddWithValue("@Balance", receiverBalance);
                sqlCmdReceiver.Parameters.AddWithValue("@Accountid", transactionHistory.ReceiverId);
                var updateResultReceiver = sqlCmdReceiver.ExecuteNonQuery();

                // Lưu lịch sử giao dịch.
                var historyTransactionQuery =
                    "insert into TransactionNganhang (Id, Type, Senderid, ReceiverId, Amount, Message) " +
                "values (@Id, @Type, @Senderid, @ReceiverId, @Amount, @Message)";
                var historyTransactionCmd =
                    new MySqlCommand(historyTransactionQuery, ConnectionHepper.GetConnection());
                historyTransactionCmd.Parameters.AddWithValue("@Id", transactionHistory.Id);
                historyTransactionCmd.Parameters.AddWithValue("@Amount", transactionHistory.Amount);
                historyTransactionCmd.Parameters.AddWithValue("@Type", transactionHistory.Type);
                historyTransactionCmd.Parameters.AddWithValue("@Message", transactionHistory.Message);
                historyTransactionCmd.Parameters.AddWithValue("@Senderid",transactionHistory.SenderId);
                historyTransactionCmd.Parameters.AddWithValue("@Receiverid",transactionHistory.ReceiverId);
                var historyResult = historyTransactionCmd.ExecuteNonQuery();

                if (updateResult != 1 || historyResult != 1 || updateResultReceiver != 1)
                {
                    throw new Exception("Không thể thêm giao dịch hoặc update tài khoản.");
                }

                mySqlTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
             mySqlTransaction.Rollback();
                return false;
            }
            finally
            {                
               ConnectionHepper.CloseConnection();
            }
        }
    }
   
}