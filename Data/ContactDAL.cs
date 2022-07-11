using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contacts_V2.Models;

namespace Contacts_V2.Data
{
    public static class ContactDAL
    {
        private static DataContext _context = new DataContext();
        internal static bool Create(Contact contact)
        {
            if (!checkByNickname(contact.Nickname))
            {
                _context.Contacts.Add(contact);
                var result = _context.SaveChanges();
                return result >= 1 ? true : false;
            }
            else
            {
                return false;
            }
        }

        internal static Contact? GetById(int id)
        {
            try
            {
                return _context.Contacts.Single(x => x.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        internal static bool ChangeFavoriteStatus(int id, int userId)
        {
            var entity = GetById(id);
            if (entity == null || entity.UserId != userId)
            {
                return false;
            }
            else
            {
                entity.isFavorite = !entity.isFavorite;
                _context.Update(entity);
                var result = _context.SaveChanges();
                return result >= 1 ? true : false;
            }
        }
        internal static bool Edit(Contact contact, int userId)
        {
            var entity = GetById(contact.Id);
            if (entity == null || entity.UserId != userId)
            {
                return false;
            }
            else
            {
                entity.Firstname = contact.Firstname;
                entity.Surname = contact.Surname;
                entity.Email = contact.Email;
                entity.Phone = contact.Phone;
                entity.Birthday = contact.Birthday;
                _context.Update(entity);
                var result = _context.SaveChanges();
                return result >= 1 ? true : false;
            }
        }
        internal static bool DeleteById(int id, int userId)
        {
            var entity = GetById(id);
            if (entity == null || entity.UserId != userId)
            {
                return false;
            }
            else
            {

                _context.Remove(entity);
                var result = _context.SaveChanges();
                return result >= 1 ? true : false;
            }
        }

        internal static bool checkByNickname(string Nickname)
        {
            return _context.Contacts.Where(x => x.Nickname == Nickname).Any();
        }

        internal static List<Contact> GetAll(int userId)
        {
            return _context.Contacts.Where(x => x.UserId == userId).ToList();
        }

        internal static List<Contact> Search(string sentence, int userId)
        {
            return _context.Contacts.Where(X => X.Firstname.Contains(sentence) ||
                                                     X.Surname.Contains(sentence) ||
                                                     X.Birthday.Value.ToString().Contains(sentence) ||
                                                     X.Email.Contains(sentence) ||
                                                     X.Phone.Contains(sentence)).Where(x => x.UserId == userId).ToList();
        }
    }
}
