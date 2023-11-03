﻿using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class User : Entity
{
	public string Username { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; set; }
    public List<Club> Clubs { get; set; }
    public List<User> Followers { get; } = new List<User>();
    public List<Message> Messages { get; set; }

    public User(string username, string password, UserRole role, bool isActive)
	{
		Username = username;
		Password = password;
		this.Role = role;
		this.IsActive = isActive;
	}

	private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username)) throw new ArgumentException("Invalid Name");
        if (string.IsNullOrWhiteSpace(Password)) throw new ArgumentException("Invalid Surname");
    }

    public string GetPrimaryRoleName()
    {
        return Role.ToString().ToLower();
    }
}

public enum UserRole
{
    Administrator,
    Author,
    Tourist
}