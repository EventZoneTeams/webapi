namespace EventZone.Services.Interface
{
    public interface IRedisService
    {
        /// <summary>
        /// Sets a string value in Redis with an expiration time.
        /// </summary>
        /// <param name="key">The key to be set.</param>
        /// <param name="value">The value to be set.</param>
        /// <param name="expiry">The expiration time for the key.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SetStringAsync(string key, string value, TimeSpan expiry);

        /// <summary>
        /// Gets a string value from Redis by key.
        /// </summary>
        /// <param name="key">The key to retrieve the value for.</param>
        /// <returns>A Task containing the value associated with the key.</returns>
        Task<string> GetStringAsync(string key);

        /// <summary>
        /// Deletes a key from Redis.
        /// </summary>
        /// <param name="key">The key to be deleted.</param>
        /// <returns>A Task representing the asynchronous operation, containing a boolean indicating whether the key was deleted.</returns>
        Task<bool> DeleteKeyAsync(string key);
    }
}
