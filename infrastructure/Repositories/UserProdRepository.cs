using infrastructure.DatabaseManager.Interface;
using infrastructure.Entities;
using infrastructure.Repositories.Interface;
using Npgsql;

namespace infrastructure.Repositories
{
    public class UserProdRepository : IUserProdMapper
    {
        private readonly IDBConnection _dbConnection;
        
        public UserProdRepository(IDBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public List<Products> GetUserLikes(int UserId)
        {
            using (var con =_dbConnection.GetConnection())
            {
                con.Open();
                
                const string query =
                    "SELECT product.productid, product.productname, product.typeid, product.price FROM product " +
                    "INNER JOIN userlikes ul on product.productid = ul.productid " +
                    "INNER JOIN users u on ul.userid = u.userid " +
                    "WHERE u.userid = @userid";

                using (var command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@userid", UserId);

                    using (var reader = command.ExecuteReader())
                    {
                        var products = new List<Products>();

                        while (reader.Read())
                        {
                            products.Add(new Products
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                TypeId = reader.GetInt32(reader.GetOrdinal("TypeId")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                            });
                        }

                        return products;
                    }
                }
                
            }
        }

        public List<Products> GetUserCartProducts(int UserId)
        {
            using (var con =_dbConnection.GetConnection())
            {
                con.Open();
        
                const string query =
                    "SELECT p.productid, p.productname, p.typeid, p.price " +
                    "FROM usercart u " +
                    "JOIN product p ON u.productid = p.productid " +
                    "WHERE u.userid = @userid";

                using (var command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@userid", UserId);

                    using (var reader = command.ExecuteReader())
                    {
                        var products = new List<Products>();

                        while (reader.Read())
                        {
                            products.Add(new Products
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                TypeId = reader.GetInt32(reader.GetOrdinal("TypeId")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                            });
                        }

                        return products;
                    }
                }
            }
        }

        public List<UserCartItems> GetUserCartDetails(int userId)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();

                const string query =
                    "SELECT uc.productid, uc.colorid, uc.sizeid, uc.quantity, p.productname, p.price " +
                    "FROM usercart uc " +
                    "JOIN product p ON uc.productid = p.productid " +
                    "WHERE uc.userid = @userId";

                using (var command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        var userCartDetails = new List<UserCartItems>();

                        while (reader.Read())
                        {
                            userCartDetails.Add(new UserCartItems
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ColorId = reader.GetInt32(reader.GetOrdinal("ColorId")),
                                SizeId = reader.GetInt32(reader.GetOrdinal("SizeId")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                            });
                        }

                        return userCartDetails;
                    }
                }
            }
        }

        public void AddProductToUserCart(int userId, int productId, int colorId, int sizeId, int quantity)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();
                const string sql = "INSERT INTO usercart (UserId, ProductId, colorId, sizeId, quantity) VALUES (@userId, @productId, @colorId, @sizeId, @quantity)";

                using (var commmand = new NpgsqlCommand(sql, con))
                {
                    commmand.Parameters.AddWithValue("@userId", userId);
                    commmand.Parameters.AddWithValue("@productId", productId);
                    commmand.Parameters.AddWithValue("@colorId", colorId);
                    commmand.Parameters.AddWithValue("@sizeId", sizeId);
                    commmand.Parameters.AddWithValue("@quantity", quantity);

                    commmand.ExecuteNonQuery();
                }
            }
        }

        public void RemoveProductFromCart(int userId, int productId, int colorId, int sizeId, int quantity)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();
                const string sql =
                    "DELETE FROM usercart WHERE userid = @userId " +
                    "AND productid = @productId " +
                    "AND colorid = @colorId " +
                    "AND sizeid = @sizeId";

                using (var command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@colorId", colorId);
                    command.Parameters.AddWithValue("@sizeId", sizeId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProductQuantity(int userId, int productId, int colorId, int sizeId, int quantity)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();
                const string sql =
                    "UPDATE usercart SET quantity = @quantity " +
                    "WHERE userid = @userId AND productid = @productId " +
                    "AND colorid = @colorId AND sizeid = @sizeId";

                using (var command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@colorId", colorId);
                    command.Parameters.AddWithValue("@sizeId", sizeId);
                    command.Parameters.AddWithValue("@quantity", quantity);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddProductToUserLikes(int userId, int productId)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();

                const string query = "INSERT INTO userlikes (UserId, ProductId) VALUES (@userId, @productId)";

                using (var command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@productId", productId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void RemoveProductFromLikes(int userId, int productId)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();

                const string query = "DELETE FROM userlikes WHERE userid = @userId AND productid = @productId";

                using (var command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@productId", productId);

                    command.ExecuteNonQuery();
                }
            } 
        }
    }
}