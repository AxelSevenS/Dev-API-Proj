using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace ApiSevenet;

public class UserRepository : Repository<User>
{
    public static readonly string fileName = "users.json";


    public UserRepository() : base(fileName) {}

    /// <summary>
    /// Save the data to the file
    /// </summary>
    public override void SaveChanges()
    {
        string jsonString = JsonSerializer.Serialize(Data);
        File.WriteAllText(fileName, jsonString);
    }
    
    /// <summary>
    /// Get a new id for a user
    /// </summary>
    /// <returns>
    /// A new id
    /// </returns>
    public uint GetNewId() =>
        Data.Aggregate((uint)0, (max, p) => p.Id > max ? p.Id : max) + 1;


    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>
    /// All users
    /// </returns>
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await Task.Run(() => Data);
    }

    /// <summary>
    /// Get a user by id
    /// </summary>
    /// <param name="id">The id of the user</param>
    /// <returns>
    /// The user with the given id
    /// </returns>
    public async Task<User?> GetUserById(uint id)
    {
        return await Task.Run(() => Data.FirstOrDefault(u => u.Id == id));
    }

    /// <summary>
    /// Get a user by username and password
    /// </summary>
    /// <param name="username">The username of the user</param>
    /// <param name="password">The password of the user</param>
    /// <returns>
    /// The user with the given username and password
    /// </returns>
    public async Task<User?> GetUserByUsernameAndPassword(string username, string password)
    {
        return await Task.Run(() => Data.FirstOrDefault(u => u.Username == username && u.Password == password));
    }

    /// <summary>
    /// Verify a user
    /// </summary>
    /// <param name="user">The user to verify</param>
    /// <returns>
    /// Whether the user is valid
    /// </returns>
    public bool VerifyUser(User user) =>
        Data.Any(u => u.Username == user.Username && u.Password == user.Password);

    // /// <summary>
    // /// Get a user by id
    // /// </summary>
    // /// <param name="id">The id of the user</param>
    // /// <remarks>
    // /// This will not update the database, use <see cref="SaveChanges"/> to do that
    // /// </remarks>
    // /// <returns>
    // /// The user with the given id
    // /// </returns>
    // public async Task<User?> PostUser(User user)
    // {
    //     return await Task.Run(() =>
    //     {
    //         if (Data.Any(u => u.Username == user.Username))
    //         {
    //             return null;
    //         }

    //         user.Id = GetNewId();
    //         Data.Add(user);
            
    //         // SaveChanges();
    //         return user;
    //     });
    // }

    // /// <summary>
    // /// Get a user by id
    // /// </summary>
    // /// <param name="id">The id of the user</param>
    // /// <param name="image">The image to add</param>
    // /// <remarks>
    // /// This will not update the database, use <see cref="SaveChanges"/> to do that
    // /// </remarks>
    // /// <returns>
    // /// The user with the given id
    // /// </returns>
    // public async Task<User?> PutUserById(uint id, User user)
    // {
    //     if (user is null) return null;

    //     return await Task.Run(() =>
    //     {
    //         User? oldUser = Data.FirstOrDefault(u => u.Id == id);

    //         if (oldUser is not null)
    //         {
    //             // replace if new data was provided
    //             string? newUsername = user.Username ?? oldUser.Username;
    //             string? newPassword = user.Password ?? oldUser.Password;

    //             oldUser = Data[Data.IndexOf(oldUser)] = oldUser with
    //             {
    //                 Username = newUsername,
    //                 Password = newPassword,
    //             };
    //         }

    //         return oldUser;
    //     });
    // }

    // /// <summary>
    // /// Delete a user by id
    // /// </summary>
    // /// <param name="id">The id of the user</param>
    // /// <remarks>
    // /// This will not update the database, use <see cref="SaveChanges"/> to do that
    // /// </remarks>
    // /// <returns>
    // /// The deleted user
    // /// </returns>
    // public async Task<User?> DeleteUserById(uint id)
    // {
    //     return await Task.Run(() =>
    //     {
    //         User? user = Data.FirstOrDefault(u => u.Id == id);
    //         if (user is not null)
    //         {
    //             Data.Remove(user);
    //         }

    //         return user;
    //     });
    // }
}