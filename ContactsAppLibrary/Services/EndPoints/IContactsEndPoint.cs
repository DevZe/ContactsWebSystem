using ContactsAppLibrary.Services.Models;

namespace ContactsAppLibrary.Services.EndPoints
{
    public interface IContactsEndPoint
    {
        Task<bool> DeleteContact(int id);
        Task<ContactsModel> GetContact(int id);
        Task<List<ContactsModel>> GetContacts();
        Task<bool> InsertContact(ContactsModel contact);
        Task<bool> UpdateContact(ContactsModel contact);
    }
}