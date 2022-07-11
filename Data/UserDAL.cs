using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contacts_V2.Models;

namespace Contacts_V2.Data
{
    public static class UserDAL
    {
        private static DataContext _context = new DataContext();
        /// <summary>
        /// Check if the email submitted already is in use
        /// </summary>
        /// <param name="Email"></param>
        /// <returns>True if the email is in use</returns>
        public static bool CheckEmailUsed(string Email)
        {
            return _context.Users.Where(x => x.Email == Email).Any();
        }
        /// <summary>
        /// Store a new User object in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True in case of success</returns>
        internal static bool Register(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            var result = _context.SaveChanges();
            return result >= 1 ? true : false;
        }

        /// <summary>
        /// Compare the email and password provided with the credentials stored in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User's ID, if the email isn't registred return -2, if the password doesn't correspond returns -1</returns>
        internal static int Login(User user)
        {

            try
            {
                var entity = _context.Users.Single(x => x.Email == user.Email);
                if (BCrypt.Net.BCrypt.Verify(user.Password, entity.Password))
                {
                    return entity.Id;
                }
                else
                {
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                return -2;
            }


        }
    }
}
