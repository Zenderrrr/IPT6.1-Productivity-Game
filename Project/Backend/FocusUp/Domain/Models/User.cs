using System;

public class User
{
    public int Id;
    public string Username;
    public string Email;
    public string PasswordHash;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;

    public User()
	{
	}

    public User(string username, string email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void UpdateProfile(string username, string email)
    {
        throw new NotImplementedException();
    }

    public void ChangePassword(string newPasswordHash)
    {
        throw new NotImplementedException(); 
    }

    public bool ValidateData()
    {
        throw new NotImplementedException(); 
    }

    public void UpdateData()
    {
        throw new NotImplementedException(); 
    }
}
