using System;
using MySql.Data.MySqlClient;

namespace bank_interface
{
    public class ModelBlockchain
    {
        public BlockchainAccount FindByAddressAndPrivateKey(string address, string privateKey)
        {
            try
            {
                var cmd = new MySqlCommand(
                    "select * from Accountblockchain where 	Accountaddress = @Accountaddress and PrivateKey=@PrivateKey",
                    ConnectionHepper.GetConnection());
                cmd.Parameters.AddWithValue("@Accountaddress", address);
                cmd.Parameters.AddWithValue("@PrivateKey", privateKey);
                var dataReader = cmd.ExecuteReader();
                if (!dataReader.Read())
                {
                    return null;
                }

                var obj = new BlockchainAccount()
                {
                    Accountaddress = dataReader.GetString("Accountaddress"),
                    PrivateKey = dataReader.GetString(("PrivateKey")),
                    Balane = dataReader.GetDouble("Balance")

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
        
        
        public BlockchainAccount FindByPrivateKey( string privateKey)
        {
            try
            {
                var cmd = new MySqlCommand(
                    "select * from Accountblockchain where PrivateKey=@PrivateKey",
                    ConnectionHepper.GetConnection());
                cmd.Parameters.AddWithValue("@PrivateKey", privateKey);
                var dataReader = cmd.ExecuteReader();
                if (!dataReader.Read())
                {
                    return null;
                }

                var obj = new BlockchainAccount()
                {
                    Accountaddress = dataReader.GetString("Accountaddress"),
                    PrivateKey = dataReader.GetString(("PrivateKey")),
                    Balane = dataReader.GetDouble("Balance")

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
public BlockchainAccount FindByAccountaddress( string accountaddress)
        {
            try
            {
                var cmd = new MySqlCommand(
                    "select * from Accountblockchain where Accountaddress=@Accountaddress",
                    ConnectionHepper.GetConnection());
                cmd.Parameters.AddWithValue("@Accountaddress", accountaddress);
                var dataReader = cmd.ExecuteReader();
                if (!dataReader.Read())
                {
                    return null;
                }

                var obj = new BlockchainAccount()
                {
                    Accountaddress = dataReader.GetString("Accountaddress"),
                    PrivateKey = dataReader.GetString(("PrivateKey")),
                    Balane = dataReader.GetDouble("Balance")

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


public bool Transfer(BlockchainAccount currentLoggedInAccount, TransactionBlockchain transactionHistory)
        {
            var mySqlTransaction = ConnectionHepper.GetConnection().BeginTransaction();
            try
            {
                // Kiểm tra số dư tài khoản.
                var selectBalance =
                    "select Balance from AccountBlockchain  where Accountaddress = @Accountaddress";
                var cmdSelect = new MySqlCommand(selectBalance, ConnectionHepper.GetConnection());
                cmdSelect.Parameters.AddWithValue("@Accountaddress", currentLoggedInAccount.Accountaddress);
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
                    "update `AccountBlockchain` set `Balance` = @Balance where Accountaddress = @Accountaddress ";
                var sqlCmd = new MySqlCommand(updateQuery,ConnectionHepper.GetConnection());
                sqlCmd.Parameters.AddWithValue("@Balance", currentAccountBalance);
                sqlCmd.Parameters.AddWithValue("@Accountaddress", currentLoggedInAccount.Accountaddress);
                var updateResult = sqlCmd.ExecuteNonQuery();


                // Kiểm tra số dư tài khoản.
                var selectBalanceReceiver =
                    "select Balance from `AccountBlockchain` where Accountaddress = @Accountaddress ";
                var cmdSelectReceiver = new MySqlCommand(selectBalanceReceiver,ConnectionHepper.GetConnection());
                cmdSelectReceiver.Parameters.AddWithValue("@Accountaddress", transactionHistory.ReceiverId);
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
                    "update `AccountBlockchain` set `Balance` = @Balance where Accountaddress = @Accountaddress";
                var sqlCmdReceiver = new MySqlCommand(updateQueryReceiver, ConnectionHepper.GetConnection());
                sqlCmdReceiver.Parameters.AddWithValue("@Balance", receiverBalance);
                sqlCmdReceiver.Parameters.AddWithValue("@Accountaddress", transactionHistory.ReceiverId);
                var updateResultReceiver = sqlCmdReceiver.ExecuteNonQuery();

                // Lưu lịch sử giao dịch.
                var historyTransactionQuery =
                    "insert into TransactionBlockchain(Id, Type, Senderid, ReceiverId, Amount) " +
                "values (@Id, @Type, @Senderid, @ReceiverId, @Amount)";
                var historyTransactionCmd =
                    new MySqlCommand(historyTransactionQuery, ConnectionHepper.GetConnection());
                historyTransactionCmd.Parameters.AddWithValue("@Id", transactionHistory.Id);
                historyTransactionCmd.Parameters.AddWithValue("@Amount", transactionHistory.Amount);
                historyTransactionCmd.Parameters.AddWithValue("@Type", transactionHistory.Type);
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