using BusinessObjectsLayer.Models;
using DTOs.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDAO
    {
        private readonly HistoryEventDBContext context;
        public UserDAO(HistoryEventDBContext context) {  this.context = context; }
        public string Register(User user)
        {
            try
            {
                if (EmailExists(user.Email))
                {
                    return (user.Email + " was existed.");
                }
                CreatePassword(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                byte[] byteArray = passwordHash;
                string passHash = Convert.ToBase64String(byteArray);
                byte[] byteArray2 = passwordSalt;
                string passSalt = Convert.ToBase64String(byteArray2);
                

                user.Password = passSalt + " " + passHash;
                context.Users.Add(user);
                context.SaveChanges();
                return "OK";
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public User Login(string email, string password)
        {
            try
            {
                if (EmailExists(email))
                {
                    var user = context.Users.FirstOrDefault(u => u.Email == email);

                    var passParts = user.Password.Split(' ');
                    string passSalt = passParts[0];
                    string passHash = passParts[1];

                    string base64String = passSalt;
                    byte[] byteSalt = Convert.FromBase64String(base64String);
                    string base64String2 = passHash;
                    byte[] byteHash = Convert.FromBase64String(base64String2);
                    if (!VerifyPassword(password, byteHash, byteSalt))
                    {
                        throw new CustomException("Password is not valid");
                    }
                    return (user);
                }
                else
                {
                    throw new CustomException("Email is not valid.");
                }
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        private void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private bool EmailExists(string email)
        {
            return context.Users.Any(u => u.Email == email);
        }

        public User GetUserByUserId(int id)
        {
            try
            {
                return context.Users.FirstOrDefault(u => u.UserId == id);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return await context.Users.ToListAsync();
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if(user == null) {
                    throw new CustomException("User not found.");
                }
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                var userUpdate = await context.Users.FirstOrDefaultAsync(u=> u.UserId == user.UserId);
                if(userUpdate == null)
                {
                    throw new CustomException("User not found.");
                }
                if(user.Email != userUpdate.Email)
                {
                    if(EmailExists(user.Email)) {
                        throw new CustomException("Email was existed");
                    }
                }
                userUpdate.Name = user.Name;
                userUpdate.Birthday = user.Birthday;
                await context.SaveChangesAsync();
                return userUpdate;

            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task UpdateRole(int id, string role)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if(user == null)
                {
                    throw new CustomException("User not found");
                }
                if(Role.Admin.ToString().Contains(role)) {
                    user.Role = Role.Admin;
                } else if (Role.Member.ToString().Contains(role))
                {
                    user.Role = Role.Member;
                } else if (Role.Editor.ToString().Contains(role))
                {
                    user.Role = Role.Editor;
                }
                else
                {
                    throw new CustomException("Role not found");
                }

                await context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
