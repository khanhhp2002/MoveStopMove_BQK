using System.Text;
using UnityEngine;

public static class RandomStringGenerator
{
    // Characters to choose from
    private static string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    // Function to generate a random string of a specified length
    public static string GetRandomString(int length)
    {
        // Create a new string builder
        StringBuilder builder = new StringBuilder();

        // Generate a random string of the specified length
        for (int i = 0; i < length; i++)
        {
            // Get a random index from the characters string
            int index = Random.Range(0, _characters.Length);

            // Get the character at the random index
            char character = _characters[index];

            // Append the character to the string builder
            builder.Append(character);
        }

        // Return the generated string
        return builder.ToString();
    }
}
